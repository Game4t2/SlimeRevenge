using UnityEngine;
using System.Collections;

public class Taping : MonoBehaviour {
    public int point=0;
    public bool destroyAftertouch;
    public bool enableTouch;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Tap()
    {
        if (enableTouch)
        {
            point++;
            if (destroyAftertouch)
            {
                Destroy(this.gameObject);
            }
        }
    }
    public void Tap(int pointPlus)
    {
        if (enableTouch)
        {
            point=point+pointPlus;
            if (destroyAftertouch)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
