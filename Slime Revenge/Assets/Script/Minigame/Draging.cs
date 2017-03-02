using UnityEngine;
using System.Collections;

public class Draging : MonoBehaviour {
    enum Way { up,down,left,right}
    Way myWay;
    Vector2 startPos;

    Vector2 newPos;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
        }
        if (Input.GetMouseButtonUp(0))
        {
            newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (startPos.x -newPos.x<-1f) myWay = Way.left;
            else if (startPos.x - newPos.x > 1f) myWay = Way.right;
            else if (startPos.y - newPos.y < -1f) myWay = Way.up;
            else if (startPos.y - newPos.y > 1f) myWay = Way.down;

            Debug.Log(myWay);
        }
        
	}
}
