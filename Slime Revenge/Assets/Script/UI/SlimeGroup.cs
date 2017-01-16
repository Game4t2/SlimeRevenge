using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeGroup : MonoBehaviour {
 //   public GameObject[] slime = new GameObject[4];//0=upgrade 1=title 2=memo
    private int direction = 1;
    private int maxscene = 1;
    public float speed = 1f;
                                                  // Use this for initialization
    void Start () {
 //       slime[0].SetActive(true);
   //     for(int i=1;i< maxscene; i++)
     //   slime[i].SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {



        this.transform.localPosition += new Vector3(direction * speed, 0f, 0f);
      
                bool chk = false;
                if (direction > 0 && this.transform.localPosition.x >= (this.transform.parent.transform.localPosition.x + 1000f)) chk = true;
                else if(direction < 0 && this.transform.localPosition.x <= (this.transform.parent.transform.localPosition.x - 1000f))chk = true;

                if (chk )
                {  direction *= -1;    }


	}
}
