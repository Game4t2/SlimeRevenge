using UnityEngine;
using System.Collections.Generic;

public class JellyExplosion : MonoBehaviour
{
    public float range;
    public float damageDealt;
    public bool allowMultihit;

    // Use this for initialization
    void Start()
    {
        List<EnemyUnit> exploded = new List<EnemyUnit>();
        List<Unit> list = SlimePool.GetActiveSlimeList();
        for (int i = 0; i < list.Count; i++)
        {
            Collider[] col = Physics.OverlapBox(list[i].transform.position, new Vector3(range / 2, 1, 1));
            for (int enemyIndex = 0; enemyIndex < col.Length; enemyIndex++)
            {
                EnemyUnit e = col[enemyIndex].GetComponent<EnemyUnit>();
                if (e != null)
                {
                    if (allowMultihit)
                    {
                        exploded.Add(e);
                    }
                    else
                    {
                        if (!exploded.Contains(e))
                            exploded.Add(e);
                    }
                }
            }
        }
    }

    private void ExplodeEnemyList(EnemyUnit[] list)
    {
        for (int i = 0; i < list.Length; i++)
        {
            list[i].TakeDamage(damageDealt);
        }
    }


}
