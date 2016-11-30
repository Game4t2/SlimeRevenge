using UnityEngine;
using System.Collections;

public class Stage : MonoBehaviour {
    private Touch T;
    RaycastHit2D hit;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void Update()
    {
        T = Input.GetTouch(0);
        if (T.phase.Equals(TouchPhase.Began))
        {
            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(T.position), Vector2.zero, 1f, 1 << LayerMask.NameToLayer("Base"));
            if (hit.collider != null && hit.transform.name == "Deploy") ///Raycast on layer base name deploy(KIng slime len)
            {
            }
        }
    }
}
