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

    public void GetSkillRate(string rate,List<SkillData> listOfskill)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].rate == rate)
                listOfskill.Add(list[i]);
        }
        Debug.LogError("Invalid skill id requested," + rate);
    
    }


}


[System.Serializable]
public class SkillData
{
    public string skillID;
    public string displayName;
    public string rate;

    public int maxlevel;
    public int level;
    public int total;
    public Sprite skillSprite;
    public GameObject skillPrefab;
}
