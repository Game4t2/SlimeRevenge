using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPoint : MonoBehaviour {
    
        bool passed = false;

        public void SetPass(bool pass)
        {
            passed = pass;
        if (passed == true)
        {

            this.GetComponent<SpriteRenderer>().color = Color.red;

        }
        else { this.GetComponent<SpriteRenderer>().color = Color.white; }
        }
   
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
