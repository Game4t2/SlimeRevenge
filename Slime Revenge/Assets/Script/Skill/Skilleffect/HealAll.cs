using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAll : MonoBehaviour {
    public static HealAll Instance {
        get { return instance; }
    }
    private static HealAll instance;
	// Use this for initialization
	void Start () {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Heal(float totalHeal)
    {
        List<Unit> list = SlimePool.GetActiveSlimeList();
        for (int i = 0; i < list.Count; i++)
        {
            list[i].currentHp=(list[i].currentHp+totalHeal>list[i].maxHp)? list[i].maxHp : list[i].currentHp + totalHeal;
            

        }



    }
}
