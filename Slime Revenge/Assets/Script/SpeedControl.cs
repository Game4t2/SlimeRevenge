using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class SpeedControl : MonoBehaviour {
    private GameObject[] SpeedLevel=new GameObject[4];
    private int current;
	// Use this for initialization
	void Start () {
        for(int i=0;i<2;i++){
            SpeedLevel[i] = this.transform.GetChild(i).gameObject;


        }
        SpeedLevel[1].GetComponent<Image>().color = new Color(0.4f, 0.4f, 0.4f); 
        current = 0; 
        ThisSpeed(0);
    }
	
	// Update is called once per frame
	void Update () {

	
	}
    public void ThisSpeed(int speed)
    {

        SpeedLevel[current].GetComponent<Image>().color = new Color(0.4f, 0.4f, 0.4f);
        SpeedLevel[speed].GetComponent<Image>().color = Color.white;
        current = speed;
        switch (speed)
        {
            case (0): Time.timeScale = 1f; return;
            case (1): Time.timeScale = 2f; return;
        }

    }
}
