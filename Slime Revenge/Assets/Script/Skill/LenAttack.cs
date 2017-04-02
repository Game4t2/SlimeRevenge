using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LenAttack : MonoBehaviour {
    public float damage = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void SetDamage(float dmg)
    {
        damage = dmg;

    }

    void EnemyAttacked(float lenY)
    {
     GameObject[] Enemy=  GameObject.FindGameObjectsWithTag("Human");
        for(int i = 0; i < Enemy.Length; i++)
        {

            if (Enemy[i].transform.position.y>lenY+0.1f|| Enemy[i].transform.position.y < lenY - 0.1f)
            {
                Enemy[i].GetComponent<EnemyUnit>().currentHp -= damage;

            }

        }

    }
}
