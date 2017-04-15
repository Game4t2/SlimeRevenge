using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour {

   private List<GameObject> pathPoint=new List<GameObject>();
    public float sensitive = 0.7f;
    int index=0;
  private bool Finished=false;
	// Use this for initialization
	void Start () {
        for (int i = 0; i < this.transform.childCount; i++)
            pathPoint.Add(this.transform.GetChild(i).gameObject);
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(index);
        if (Input.GetMouseButton(0))
        {
            if (index == pathPoint.Count)
            {
                Finished = true;
                Debug.Log("pass");
                clearPath();
            }
            if (Mathf.Abs(Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), pathPoint[index].transform.position)) < sensitive)
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
