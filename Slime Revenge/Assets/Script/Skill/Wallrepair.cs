using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallrepair : MonoBehaviour {
    public float healingPoint;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Heal()
    {
        Wall.Instance.hp =( Wall.Instance.hp+ (Wall.Instance.max_Hp * healingPoint)> Wall.Instance.max_Hp) ? Wall.Instance.max_Hp : Wall.Instance.hp + (Wall.Instance.max_Hp * healingPoint) ;

    }
}
