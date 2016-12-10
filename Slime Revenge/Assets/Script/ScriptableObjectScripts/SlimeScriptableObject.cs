using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SlimeScriptableObject", menuName = "Unit/Slime", order = 1)]
public class SlimeScriptableObject : ScriptableObject
{
    public List<SlimeUnit> list;

    public SlimeUnit GetSlimeData(Element elem, int level, SlimeUnitType type = SlimeUnitType.NONE)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].element == elem && list[i].level == level && list[i].type == type)
                return list[i];
        }
        return null;
    }
}

[System.Serializable]
public class SlimeUnit
{
    public string displayName;
    public string id;
    public GameObject prefab;
    public int level;
    public int maxHp;
    public int atp;
    public int def;
    public float attackspeed;
    public float range;
    public float speed;
    public Element element;
    public SlimeUnitType type;

    public GameObject CreateInstance()
    {
        GameObject go = GameObject.Instantiate(prefab);
        Unit slimeUnit = go.AddComponent<Unit>();
        slimeUnit.maxHp = maxHp;
        slimeUnit.atp = atp;
        slimeUnit.def = def;
        slimeUnit.attackSpeed = attackspeed;
        slimeUnit.range = range;
        slimeUnit.speed = speed;
        slimeUnit.myElement = element;
        return go;
    }

}