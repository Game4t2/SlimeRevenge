using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class Taping : MonoBehaviour {
    public int point=0;
    public bool multiplePoint;

    public List<Vector3> tapPosition = new List<Vector3>();
   // public bool enableTouch;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Tap()
    {
            point++;
            Debug.Log(point);
            if (multiplePoint)
            {
                this.gameObject.transform.position = tapPosition[point%tapPosition.Count];
            }
        
    }
    void ClearPoint()
    {
        point = 0;
    }
   
}
