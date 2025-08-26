#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace DamageNumbersPro
{
    [CustomEditor(typeof(DamageNumberSprite), true), CanEditMultipleObjects]
    public class DamageNumberSpriteEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Color previousColor = GUI.color;
            GUIStyle descriptionTextStyle = new GUIStyle(GUI.skin.label);
            descriptionTextStyle.richText = true;
            descriptionTextStyle.wordWrap = true;
            descriptionTextStyle.stretchHeight = true;
            descriptionTextStyle.fixedHeight = 0;

            DamageNumberSprite targetSprite = (DamageNumberSprite)target;
            DamageNumber damageNumber = targetSprite.GetComponentInParent<DamageNumber>();

            foreach (Object target in targets)
            {
                DamageNumberSprite dnpSprite = (DamageNumberSprite)target;

                // Preview size
                if (dnpSprite.matchTextSize)
                {
                    SpriteRenderer spriteR = dnpSprite.GetComponent<SpriteRenderer>();
                    if (spriteR != null)
                    {
                        dnpSprite.UpdateSize(dnpSprite.GetComponentInParent<DamageNumber>(), spriteR);
                    }

                    RectTransform rectTransform = dnpSprite.GetComponent<RectTransform>();
                    if (rectTransform != null)
                    {
                        dnpSprite.UpdateSize(dnpSprite.GetComponentInParent<DamageNumber>(), rectTransform);
                    }
                }
            }

            // Check material
            SpriteRenderer spriteRenderer = targetSprite.GetComponent<SpriteRenderer>();
            if (damageNumber.enable3DGame && spriteRenderer != null && (spriteRenderer.sharedMaterial == null || spriteRenderer.sharedMaterial.shader.name.EndsWith("Overlay") == false))
            {
                GUI.color = new Color(1, 0.7f, 0.7f, 1);
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical("Helpbox");
                EditorGUILayout.LabelField("Use the <b>Sprite Overlay</b> material if you want your sprite renderer to render in front of other objects.", descriptionTextStyle);

                if(GUILayout.Button("Use Sprite Overlay Material"))
                {
                    string[] guids = AssetDatabase.FindAssets($"t:Material DNP Sprite Overlay");

                    foreach (string guid in guids)
                    {
                        string path = AssetDatabase.GUIDToAssetPath(guid);
                        Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);

                        Undo.RecordObject(spriteRenderer, "Changed sprite renderer's material.");
                        spriteRenderer.sharedMaterial = mat;
                        break;
                    }
                }

                EditorGUILayout.EndVertical();
                GUI.color = previousColor;
            }

            GUI.color = new Color(1f, 1f, 1f, 0.7f);
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical("Helpbox");
            EditorGUILayout.LabelField("You can attach this component to <b>images</b> and <b>sprite renderes</b> inside your damage number prefab. It will handle <b>fading</b> them in and out with the damage number.", descriptionTextStyle);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("<b>Match Text Size</b> will resize the image or sprite renderer to match the text's size. For <b>images</b> it will always resize the <b>rect transform</b>. For <b>sprite renderers</b> it will either resize the <b>transform</b> or if possible the sprite's <b>width</b> and <b>height</b>.", descriptionTextStyle);
            EditorGUILayout.EndVertical();
            GUI.color = previousColor;

        }
    }
}
#endif