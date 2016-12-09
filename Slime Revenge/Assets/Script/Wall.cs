using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {
    public static Wall Instance { get { return wall; } }
    private static Wall wall;
    private Animator anim;
    private bool downed=false;
    public int HP = 100;
    public int max_Hp = 100;
    public int def = 50;

	// Use this for initialization
	void Start () {
        wall = this;
        anim = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (this.HP < 0f && !downed)
        {
            downed = true;
            StartCoroutine("WallBreak");
        }
	}
    IEnumerator WallBreak()
    {
        
        anim.SetBool("Down", true);
        Debug.Log(anim.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        gameObject.SetActive(false);

    }
}
