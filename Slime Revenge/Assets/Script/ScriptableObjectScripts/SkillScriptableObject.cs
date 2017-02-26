using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SkillScriptableObject", menuName = "Database/Skills", order = 1)]
public class SkillScriptableObject : ScriptableObject
{
    public List<SkillData> list = new List<SkillData>();

    public SkillData GetSkill(string id)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].skillID == id)
                return list[i];
        }
        Debug.LogError("Invalid skill id requested," + id);
        return null;
    }

}


[System.Serializable]
public class SkillData
{
    public string skillID;
    public string displayName;
    public Sprite skillSprite;
    public GameObject skillPrefab;
}
