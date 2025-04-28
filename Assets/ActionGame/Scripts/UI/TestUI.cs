

using UnityEngine;
using UnityEngine.UI;

public class TestUI : MonoBehaviour
{
    [SerializeField] private Button btnSkill1;

    private void Awake()
    {
        btnSkill1.onClick.AddListener(OnSkill1Click);
    }

    private void OnDestroy()
    {
        btnSkill1.onClick.RemoveAllListeners();
    }

    private void OnSkill1Click()
    {
        //SkillCommand skillCommand = new SkillCommand();
        //skillCommand.SkillSlot = 1;
        //Game.Instance.GetGameSystem<CommandInvoker>().AddCommand(skillCommand);
    }
}
