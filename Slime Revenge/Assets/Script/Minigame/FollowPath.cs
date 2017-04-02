using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour {

   public List<GameObject> pathPoint=new List<GameObject>();
    int index=0;
  public bool Finished=false;
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (index == pathPoint.Count)
            {
                Finished = true;
                clearPath();
            }
            if (Mathf.Abs(Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), pathPoint[index].transform.position)) < 0.2f)
            {
                pathPoint[index].GetComponent<PathPoint>().SetPass(true);
                index++;
            }


        }
        else if (Input.GetMouseButtonUp(0))
        {

            clearPath();
        }
    }
        public void clearPath()
    {
        for(int i=0;i < pathPoint.Count; i++)
        {
            pathPoint[i].GetComponent<PathPoint>().SetPass(false);
        } index = 0;
    }
}
public class PathPoint
{
    bool passed=false;

       public void SetPass(bool pass)
        {
            passed = pass;

        }
}