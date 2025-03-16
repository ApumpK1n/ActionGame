using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;


public class AutoGenerateFootIKCurve : EditorWindow
{
    public GameObject sampleGo;
    //采样帧率，可提高曲线精确度
    public int sampleRate = 30;
    //脚底距离小于此值时视为触地
    public float footGroundedHeight = 0.005f;
    //左脚曲线名
    public string leftFootIKCurveName = "LeftFootIKCurve";
    //右脚曲线名
    public string rightFootIKCurveName = "RightFootIKCurve";

    //私有
    private Animator _animator;
    private bool _isGenerating;
    private Transform _leftFootBoneTran;
    private Transform _rightFootBoneTran;
    private float _leftBottomHeight;
    private float _rightBottomHeight;

    [MenuItem("Tools/Auto Generate FootIKCurve", false)]
    public static void DoWindow()
    {
        var window = GetWindowWithRect<AutoGenerateFootIKCurve>(new Rect(0, 0, 400, 200));
        window.Show();
    }

    private void OnSelectionChange()
    {
        if (sampleGo == null)
        {
            sampleGo = Selection.activeGameObject;
            _animator = sampleGo.GetComponent<Animator>();

            if (_animator == null)
            {
                Debug.LogError("错误的GameObject：没有Animator");
                sampleGo = null;
                _animator = null;
                return;
            }

            _leftFootBoneTran = _animator.GetBoneTransform(HumanBodyBones.LeftFoot);
            _rightFootBoneTran = _animator.GetBoneTransform(HumanBodyBones.RightFoot);
            _leftBottomHeight = _animator.leftFeetBottomHeight;
            _rightBottomHeight = _animator.rightFeetBottomHeight;

            Repaint();
        }
    }

    private void OnClickResetSampleGo()
    {
        Debug.Log("重选GameObject");
        sampleGo = null;
        Repaint();
    }

    public void OnGUI()
    {
        //需要先选定一个Gameobject
        if (sampleGo == null)
        {
            EditorGUILayout.HelpBox("请选择Scene上要播放人形动画的GameObject", MessageType.Info);
            EditorGUILayout.HelpBox("GameObject应该具备人形骨骼，拥有Animator组件，\n组件拥有AnimatorController资产", MessageType.Info);
            EditorGUILayout.HelpBox("将要自动计算曲线的动画Clip都放到AnimatorController中", MessageType.Info);
            EditorGUILayout.HelpBox("点击GameObject没反应的话先点击其他物体再重新点回去试试", MessageType.Info);
            return;
        }

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField($"播放对象：{sampleGo.name}");
        if (GUILayout.Button("重选"))
        {
            this.OnClickResetSampleGo();
        }
        GUILayout.EndHorizontal();

        sampleRate = EditorGUILayout.IntField("sampleRate", sampleRate);
        footGroundedHeight = EditorGUILayout.FloatField("footGroundedHeight", footGroundedHeight);
        leftFootIKCurveName = EditorGUILayout.TextField("leftFootIKCurveName", leftFootIKCurveName);
        rightFootIKCurveName = EditorGUILayout.TextField("rightFootIKCurveName", rightFootIKCurveName);

        if (GUILayout.Button("开始生成脚步动画曲线") && !_isGenerating)
        {
            BakeAnimationFootIKCurve();
        }
    }

    /// <summary>
    /// 核心函数，采样动画并检测脚距离地面的距离
    /// </summary>
    private void BakeAnimationFootIKCurve()
    {
        BakeBegin();

        AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            string assetPath = AssetDatabase.GetAssetPath(clip);
            Debug.Log($"处理clip:{clip.name}");

            //获得importer
            var importer = AssetImporter.GetAtPath(assetPath) as ModelImporter;
            Debug.Assert(importer);
            ModelImporterClipAnimation[] anims = importer.clipAnimations;
            int animIndex = Enumerable.Range(0, anims.Length).FirstOrDefault(i => anims[i].name == clip.name);

            //清空同名曲线
            anims[animIndex].curves = anims[animIndex].curves
                .Where(c =>
                    c.name != leftFootIKCurveName &&
                    c.name != rightFootIKCurveName)
                .ToArray();

            //动画采样并生成keyFrame
            int frames = Mathf.CeilToInt(clip.length * sampleRate);
            List<Keyframe> leftFootKeyList = new List<Keyframe>();
            List<Keyframe> rightFootKeyList = new List<Keyframe>();
            float leftCurWeight = 0f;
            float rightCurWeight = 0f;
            float leftLastWeight = 0f;
            float rightLastWeight = 0f;

            AnimationMode.BeginSampling();
            float time = 0f;
            float keyFrameTime = 0f;
            bool leftGrounded = false;
            bool rightGrounded = false;
            for (int i = 0; i <= frames; i++)
            {
                time = (float)i / sampleRate;
                AnimationMode.SampleAnimationClip(sampleGo, clip, time);
                leftGrounded = IsFootGrounded(sampleGo.transform, _leftFootBoneTran, _leftBottomHeight);
                rightGrounded = IsFootGrounded(sampleGo.transform, _rightFootBoneTran, _rightBottomHeight);
                keyFrameTime = (float)i / frames;

                //仅权重发生跳变时才植入Keyframe
                leftCurWeight = leftGrounded ? 1f : 0f;
                rightCurWeight = rightGrounded ? 1f : 0f;
                if (i == 0 || i == frames)
                {
                    leftFootKeyList.Add(new Keyframe(keyFrameTime, leftCurWeight));
                    rightFootKeyList.Add(new Keyframe(keyFrameTime, rightCurWeight));
                }
                else
                {
                    //与上一帧权重不相等
                    if (Math.Abs(leftCurWeight - leftLastWeight) > 0.01f)
                    {
                        leftFootKeyList.Add(new Keyframe((float)(i - 1) / frames, leftLastWeight));
                        leftFootKeyList.Add(new Keyframe(keyFrameTime, leftCurWeight));
                    }
                    if (Math.Abs(rightCurWeight - rightLastWeight) > 0.01f)
                    {
                        rightFootKeyList.Add(new Keyframe((float)(i - 1) / frames, rightLastWeight));
                        rightFootKeyList.Add(new Keyframe(keyFrameTime, rightCurWeight));
                    }
                }
                leftLastWeight = leftCurWeight;
                rightLastWeight = rightCurWeight;
            }
            AnimationMode.EndSampling();

            //生成两条完整新曲线
            var leftInfoCurve = new ClipAnimationInfoCurve();
            var rightInfoCurve = new ClipAnimationInfoCurve();
            leftInfoCurve.name = leftFootIKCurveName;
            rightInfoCurve.name = rightFootIKCurveName;
            leftInfoCurve.curve = new AnimationCurve(leftFootKeyList.ToArray());
            rightInfoCurve.curve = new AnimationCurve(rightFootKeyList.ToArray());

            //保存
            anims[animIndex].curves = Enumerable.Concat(anims[animIndex].curves,
                new[] { leftInfoCurve, rightInfoCurve }).ToArray();
            importer.clipAnimations = anims;
            importer.SaveAndReimport();
        }

        BakeEnd();
    }

    private bool IsFootGrounded(Transform goTran, Transform boneTran, float bottomHeight)
    {
        return (boneTran.position.y - bottomHeight - goTran.position.y) <= footGroundedHeight;
    }

    private void BakeBegin()
    {
        _isGenerating = true;
        AnimationMode.StartAnimationMode();
    }
    private void BakeEnd()
    {
        _isGenerating = false;
        AnimationMode.StopAnimationMode();
    }
}
