using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeGroup : MonoBehaviour {
    public GameObject[] slime = new GameObject[3];//0=upgrade 1=title 2=memo
    private int direction = 1;
    public float speed = 1f;
                                                  // Use this for initialization
    void Start () {
        slime[0].SetActive(true);
        for(int i=1;i<3;i++)
        slime[i].SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < 3; i++)
        {
            if (slime[i].activeSelf)
            {
                bool chk = false;
                slime[i].transform.localPosition += new Vector3(direction* speed, 0f, 0f);
                if (direction > 0 && slime[i].transform.localPosition.x >= (slime[i].transform.parent.transform.localPosition.x + 644f)) chk = true;
                else if(direction < 0 && slime[i].transform.localPosition.x <= (slime[i].transform.parent.transform.localPosition.x - 644f))chk = true;

                if (chk )
                {
                    if ((i < 2 && direction == 1)|| (i >0 && direction == -1))
                    {

                        slime[i + direction].SetActive(true);
                        slime[i].SetActive(false);
                        slime[i].transform.localPosition = slime[i].transform.parent.transform.localPosition + Vector3.right * direction * 644f;
                        slime[i + direction].transform.localPosition = slime[i + direction].transform.parent.transform.localPosition + Vector3.right * -direction * 644f;
                    }
                    else
                    {
                        for (int j = 0; j < 3; j++)
                            slime[j].transform.Rotate(Vector3.up,180f);

                        direction *= -1;
                    }
                    break;
                }
            }
        }


	}
}
