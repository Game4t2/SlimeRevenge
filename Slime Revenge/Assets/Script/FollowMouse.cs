using UnityEngine;
using System.Collections;

public class FollowMouse : MonoBehaviour {
    Vector2 Position;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = Position;
	}
}
