using UnityEngine;
using System.Collections.Generic;


public static class SlimePool
{
    private static List<Unit> slimePool = new List<Unit>();
    private static int poolSize;

    public static void InitPool(int size)
    {
        poolSize = size;
        while (slimePool.Count < poolSize)
        {
            GameObject go = GameObject.Instantiate(GameDatabase.Instance.SlimeDatabase.baseSlimePrefab);
            go.SetActive(false);
            SlimePool.GetPool().Add(go.GetComponent<Unit>());
        }

    }

    public static Unit PoolRequest()
    {
        for (int i = 0; i < slimePool.Count; i++)
        {
            if (!slimePool[i].gameObject.activeInHierarchy)
                return slimePool[i].GetComponent<Unit>();
        }
        poolSize++;
        Unit newSlime = GameObject.Instantiate(GameDatabase.Instance.SlimeDatabase.baseSlimePrefab).GetComponent<Unit>();
        slimePool.Add(newSlime);
        return newSlime;
    }

    public static List<Unit> GetPool()
    {
        return slimePool;
    }

}
