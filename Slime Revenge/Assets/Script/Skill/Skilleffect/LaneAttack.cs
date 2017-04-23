using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneAttack : MonoBehaviour {
    public float damage = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
  public  void SetDamage(float dmg)
    {
        damage = dmg;

    }

    public bool EnemyAttacked(float laneY)
    {
     GameObject[] Enemy=  GameObject.FindGameObjectsWithTag("Human");
        for(int i = 0; i < Enemy.Length; i++)
        {

            if (Enemy[i].transform.position.y> laneY - 0.5f|| Enemy[i].transform.position.y < laneY + 0.5f)
            {
                Enemy[i].GetComponent<EnemyUnit>().currentHp -= damage;

            }

        }
        return true;

    }
}
