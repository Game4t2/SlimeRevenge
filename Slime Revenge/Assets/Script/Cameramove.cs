using UnityEngine;
using System.Collections;

public class Cameramove : MonoBehaviour {
    public static Cameramove Instanc { get { return Ins; } }
    private static Cameramove Ins;
    public bool StopMove=false;
    public bool MoveNormal=true;
    private Camera cam;
    private Vector3 StartPos;
    private float count = 0f;
    private Vector3 FinalPos;
    public float EndPos;
	// Use this for initialization
    void Start()
    {
        StopMove = false;
        Ins = this;
        cam = this.GetComponent<Camera>();
        StartPos = cam.transform.position;
       
	}
	
	// Update is called once per frame
    void Update()
    {

        if (!StopMove)
        {
            if (Input.GetMouseButtonDown(0))
            {
                count = 0f;
                StartPos = cam.ScreenToViewportPoint(Input.mousePosition);
                //   Debug.Log(StartPos);
            }
            if (Input.GetMouseButton(0) && MoveNormal)
            {
                count = 0f;
                FinalPos = cam.ScreenToViewportPoint(Input.mousePosition);
                if (cam.transform.position.x >= 0 && cam.transform.position.x <= EndPos)
                    cam.transform.Translate((StartPos.x - FinalPos.x) * 3f, 0f, 0f);
                // Debug.Log(FinalPos.x);

            }
            else if (Input.GetMouseButtonUp(0))
            {
                FinalPos = cam.ScreenToViewportPoint(Input.mousePosition);
                //Debug.Log((cam.ScreenToViewportPoint(Input.mousePosition).x - StartPos.x));

                count = 5f;
            }
            if (cam.transform.position.x > EndPos)
            {
                cam.transform.position = new Vector3(EndPos, 0f, cam.transform.position.z);
                count = 0f;
            }
            else if (cam.transform.position.x < 0f)
            {
                cam.transform.position = new Vector3(0f, 0f, cam.transform.position.z);
                count = 0f;
            }
            else if (count > 0f && cam.transform.position.x > 0 && cam.transform.position.x < EndPos)
                cam.transform.Translate((StartPos.x - FinalPos.x) * 1f * count, 0f, 0f);
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
