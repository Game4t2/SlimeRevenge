using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SlimeScriptableObject", menuName = "Unit/Slime", order = 1)]
public class SlimeScriptableObject : ScriptableObject
{
    public GameObject baseSlimePrefab;
    public List<SlimeData> list;

    public SlimeData GetSlimeData(Element elem, int level, SlimeUnitType type = SlimeUnitType.NONE)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].element == elem && list[i].level == level && list[i].type == type)
                return list[i];
        }
        return null;
    }

    public SlimeData FindSlimeByName(string name)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (name.Equals(list[i].displayName))
                return list[i];
        }
        Debug.LogError("Can't find slime with name \"" + name + "\" in Database");
        return null;
    }
}

[System.Serializable]
public class SlimeData
{
    public string displayName;
    public string id;
    public GameObject prefab;
    public int level;
    public float maxHp;
    public float atp;
    public float def;
    public float attackspeed;
    public float range;
    public float speed;
    public Element element;
    public SlimeUnitType type;

    public Unit CreateInstance(Unit slimeUnit)
    {
        slimeUnit.maxHp = maxHp;
        slimeUnit.atp = atp;
        slimeUnit.def = def;
        slimeUnit.attackSpeed = attackspeed;
        slimeUnit.range = range;
        slimeUnit.speed = speed;
        slimeUnit.element = element;
        Animator anim = slimeUnit.GetComponent<Animator>();
        if (anim == null)
            anim = slimeUnit.gameObject.AddComponent<Animator>();
        anim.runtimeAnimatorController = prefab.GetComponent<Animator>().runtimeAnimatorController;
        anim.SetInteger("Level", level - 1);
        if (level >= 3)
            anim.SetInteger("State", 2);
        SpriteRenderer spr = slimeUnit.GetComponent<SpriteRenderer>();
        if (spr == null)
            spr = slimeUnit.gameObject.AddComponent<SpriteRenderer>();
        spr.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
        BoxCollider2D col = slimeUnit.GetComponent<BoxCollider2D>();
        if (col == null)
            col = slimeUnit.gameObject.AddComponent<BoxCollider2D>();
        col.size = prefab.GetComponent<BoxCollider2D>().size;
        slimeUnit.gameObject.name = "Slime_" + element.ToString() + "_" + level;
        return slimeUnit;
    }

}