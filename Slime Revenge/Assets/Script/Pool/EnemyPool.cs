using UnityEngine;
using System.Collections.Generic;


public static class EnemyPool
{
    private static List<EnemyUnit> enemyPool = new List<EnemyUnit>();
    private static int poolSize;

    public static void InitPool(int size)
    {
        poolSize = size;
        while (enemyPool.Count < poolSize)
        {
            GameObject go = GameObject.Instantiate(GameDatabase.Instance.EnemyDatabase.baseEnemyPrefab);
            go.SetActive(false);
            EnemyPool.GetPool().Add(go.GetComponent<EnemyUnit>());
        }

    }

    public static EnemyUnit PoolRequest()
    {
        for (int i = 0; i < enemyPool.Count; i++)
        {
            if (!enemyPool[i].gameObject.activeInHierarchy)
                return enemyPool[i].GetComponent<EnemyUnit>();
        }
        poolSize++;
        EnemyUnit newEnemy = GameObject.Instantiate(GameDatabase.Instance.EnemyDatabase.baseEnemyPrefab).GetComponent<EnemyUnit>();
        enemyPool.Add(newEnemy);
        return newEnemy;
    }

    public static List<EnemyUnit> GetPool()
    {
        return enemyPool;
    }

}
