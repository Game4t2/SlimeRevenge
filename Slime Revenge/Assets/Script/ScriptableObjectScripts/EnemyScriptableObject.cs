using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "Unit/Enemy", order = 2)]
public class EnemyScriptableObject : ScriptableObject {
    public List<Enemy> list;
}

[System.Serializable]
public class Enemy
{
    public string displayName;
    public string id;
    private SpriteRenderer sprite;
    public GameObject blood;
    public Animator anim;
    public int level;
    public int maxHp;
    public int atp;
    public int def;
    public float attackspeed;
    public float range;
    public float speed;
    private bool fireCurse = false;
    private bool electricCurse = false;
    public UnitType type;
    public Element element;

}
