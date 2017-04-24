using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldBombardment : MonoBehaviour
{

    // Use this for initialization
    public void Start()
    {
        List<EnemyUnit> enemy = EnemyPool.GetPool();
        foreach (EnemyUnit e in enemy)
        {
            if (e.currentHp > 0)
                e.TakeDamage(e.currentHp + 1);
        }
    }

  
}
