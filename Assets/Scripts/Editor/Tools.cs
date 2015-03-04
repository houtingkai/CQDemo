using UnityEngine;
using UnityEditor;

public class Tools : EditorWindow
{
    [MenuItem("Tools/Create Skill Asset")]
    static void CreateSkillAsset()
    {
        SkillInfo skillInfo = ScriptableObject.CreateInstance<SkillInfo>();
        string path = "Assets/Resources/Prefabs/SkillInfo/tmp.asset";
        AssetDatabase.CreateAsset(skillInfo, path);
    }
}
