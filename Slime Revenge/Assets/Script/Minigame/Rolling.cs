using UnityEngine;
using System.Collections;

public class Rolling : MonoBehaviour {
    Vector3 startPos;
    Vector3 newPos;
    Vector3 center;
    bool rotateClockwise = true;
    
    bool[] checkpoint = new bool[5]{false,false,false,false,false};///0=5 1=90 2=180 3=270  4=355
   public int point = 0;
    float angle;
	// Use this for initialization
    void Start()
    {
        for (int i = 0; i < 5; i++)
            checkpoint[i] = false;
        Debug.Log(checkpoint[0] + "" + checkpoint[1] + " " + checkpoint[2] + " " + checkpoint[3] + " " + checkpoint[4]);
        center = this.transform.parent.position;
        center.z = 0f;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0)) {

            startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            startPos.z = 0f;
                
            
            }
        if (Input.GetMouseButton(0)){
            newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newPos.z = 0f;
            angle = Vector2.Angle((startPos - center), (newPos - center));
         //   Debug.Log((startPos - center) + "," + (newPos - center) + "," + center);
            if (Vector3.Cross((startPos - center), (newPos - center)).z <0)
          {
              angle = 360-angle;
          

          }
            this.transform.localRotation = Quaternion.Euler(new Vector3(0f,0f,angle));
     if (angle <= 359f && angle > 270f && !checkpoint[2])
        {
            rotateClockwise = true;
            checkpoint[4] = true;

            for (int i = 3; i >= 0; i--)
                checkpoint[i] = false;
        }
        else  if (angle >=5f && angle < 90f && !checkpoint[2])
        {

            rotateClockwise = false;
            checkpoint[0] = true;
            for (int i = 1; i <5; i++)
                checkpoint[i] = false;
        } 
     if (rotateClockwise)
     {
         checkpoint[3] = (angle <= 270f) ? true : false;
         checkpoint[2] = (angle <= 180f) ? true : false;
         checkpoint[1] = (angle <= 90f) ? true : false;
         checkpoint[0] = (angle <= 10f) ? true : false;

     }
     else if (!rotateClockwise)
     {
         checkpoint[1] = (angle >= 90f) ? true : false;
         checkpoint[2] = (angle >= 180f) ? true : false;
         checkpoint[3] = (angle >= 270f) ? true : false;
         checkpoint[4] = (angle >= 350f) ? true : false;

     }
        }
   
       // Debug.Log(checkpoint[0] +""+ checkpoint[1] +" "+checkpoint[2] +" " +checkpoint[3]+" "+ checkpoint[4]);


        if (checkpoint[0] && checkpoint[1] && checkpoint[2] && checkpoint[3] && checkpoint[4])
        {
            startPos = newPos;
            point++;
            for (int i = 0; i < 5; i++)
                checkpoint[i] = false;
            Debug.Log(point);
        }

        if (Input.GetMouseButtonUp(0))
        {
            for (int i = 0; i < 5; i++)
                checkpoint[i] = false;
        }
	}

    public void CheckPassingPoint(float angle)
    {
                
    }
}
