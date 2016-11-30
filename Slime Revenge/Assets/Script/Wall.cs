using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {
    public static Wall Instance { get { return wall; } }
    private static Wall wall;
    public int HP = 1000;
    public int max_Hp = 1000;
    public int def = 50;

	// Use this for initialization
	void Start () {
        wall = this;
	}
	
	// Update is called once per frame
	void Update () {
        if (this.HP < 0f)
        {
            gameObject.SetActive(false);
        }
	}
}
