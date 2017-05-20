using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jellysacrifice : MonoBehaviour {
    public Element slimeElementtodestroy;
    public float totalHeal;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void DestroyAllslimeelement()
    {
        List<Unit> list= SlimePool.GetActiveSlimeList();
        for (int i = 0; i < list.Count; i++)
        {
            if(list[i].element== slimeElementtodestroy)
            {
                list[i]._Die();
            }

        }
        Heal();
    }
    public void Heal()
    {
        HealAll.Instance.Heal(totalHeal);
    }
}
