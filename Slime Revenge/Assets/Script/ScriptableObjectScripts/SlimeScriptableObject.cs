using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SlimeScriptableObject", menuName = "Unit/Slime", order = 1)]
public class SlimeScriptableObject : ScriptableObject {
    public List<SlimeUnit> list;
}

[System.Serializable]
public class SlimeUnit
{
    public string displayName;
    public string id;
    public GameObject prefab;
    public GameObject bullet;
    public int level;
    public int maxHp;
    public int atp;
    public int def;
    public float attackspeed;
    public float range;
    public float speed;
    public Element element;
    
}