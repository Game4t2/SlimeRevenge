using UnityEngine;
using System.Collections;

public class Cameramove : MonoBehaviour {
    public static Cameramove Instanc { get { return Ins; } }
    private static Cameramove Ins;
    public bool stopMove=false;
    public bool moveNormal=true;
    private Camera cam;
    private Vector3 firstPos;
   
    private float count = 0f;
    private Vector3 finalPos;
    public float maxPos;
    public float minPos;
	// Use this for initialization
    void Start()
    {
        stopMove = false;
        Ins = this;
        cam = this.GetComponent<Camera>();
        firstPos = cam.transform.position;
       
	}
	
	// Update is called once per frame
    void Update()
    {

        if (!stopMove)
        {
            if (Input.GetMouseButtonDown(0))
            {
                count = 0f;
                firstPos = cam.ScreenToViewportPoint(Input.mousePosition);
                //   Debug.Log(StartPos);
            }
            if (Input.GetMouseButton(0) && moveNormal)
            {
                count = 0f;
                finalPos = cam.ScreenToViewportPoint(Input.mousePosition);
                if (cam.transform.position.x >= minPos && cam.transform.position.x <= maxPos)
                    cam.transform.Translate((firstPos.x - finalPos.x) * 3f, 0f, 0f);
                // Debug.Log(FinalPos.x);

            }
            else if (Input.GetMouseButtonUp(0))
            {
                finalPos = cam.ScreenToViewportPoint(Input.mousePosition);
                //Debug.Log((cam.ScreenToViewportPoint(Input.mousePosition).x - StartPos.x));

                count = 5f;
            }
            if (cam.transform.position.x >= maxPos)
            {
                cam.transform.position = new Vector3(maxPos, cam.transform.position.y, cam.transform.position.z);
                count = 0f;
            }
            else if (cam.transform.position.x <= minPos)
            {
                cam.transform.position = new Vector3(minPos, cam.transform.position.y, cam.transform.position.z);
                count = 0f;
            }
            else if (count > 0f && cam.transform.position.x > 0 && cam.transform.position.x < maxPos)
                cam.transform.Translate((firstPos.x - finalPos.x) * 1f * count, 0f, 0f);
            count = count - 0.2f;

            /*else if(Input.GetMouseButton(0)&&!MoveNormal){
              
            
                        if (cam.ScreenToViewportPoint(Input.mousePosition).x > 0.6f&&this.transform.position.x<40f)
                            {
                              cam.transform.position = cam.transform.position + (Vector3.right);

                            }
                    else if (cam.ScreenToViewportPoint(Input.mousePosition).x < 0.2f&&this.transform.position.x>0f)
                          {
                              cam.transform.position = cam.transform.position + (Vector3.left);

                          }
                    }*/

        }
    }
}
