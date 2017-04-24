using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyWrath : MonoBehaviour
{

    public void Start()
    {
        List<Unit> sList = SlimePool.GetActiveSlimeList();
        List<EnemyUnit> eList = EnemyPool.GetActiveEnemyList();
        for (int i = 0; i < sList.Count; i++)
        {
            if (Random.Range(0, 2) < 1)
            {
                sList[i].TakeDamage(sList[i].currentHp+1);
            }
        }
        for (int j = 0; j < eList.Count; j++)
        {
            if (Random.Range(0, 2) < 1)
            {
                eList[j].TakeDamage(eList[j].currentHp + 1);
            }
        }

    }

}
