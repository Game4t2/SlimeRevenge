using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "Unit/Enemy", order = 2)]
public class EnemyScriptableObject : ScriptableObject
{
    public GameObject baseEnemyPrefab;

    public List<EnemyData> list;

    public EnemyData FindEnemyByName(string name)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (name.Equals(list[i].displayName))
                return list[i];
        }
        Debug.LogError("Can't find enemy with name \"" + name + "\" in Database");
        return null;
    }

}

[System.Serializable]
public class EnemyData
{
    public string displayName;
    public string id;
    public GameObject prefab;
    public GameObject blood;
    public int level;
    public int maxHp;
    public int atp;
    public int def;
    public float attackspeed;
    public float range;
    public float speed;
    private bool fireCurse = false;
    private bool electricCurse = false;
    public EnemyUnitType type;
    public Element element;

    public GameObject CreateInstance()
    {
        GameObject go = GameObject.Instantiate(prefab);

        EnemyUnit eu = go.GetComponent<EnemyUnit>();
        if (eu == null)
            eu = go.AddComponent<EnemyUnit>();
        eu.level = level;
        eu.maxHp = maxHp;
        eu.atp = atp;
        eu.def = def;
        eu.attackspeed = attackspeed;
        eu.range = range;
        eu.speed = speed;
        eu.type = type;
        eu.element = element;

        return go;
    }

}
