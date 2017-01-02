using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour
{
    private Animator anim;
    private bool destroyed = false;
    public int hp = 100;
    public int max_Hp = 100;
    public int def = 50;

    // Use this for initialization
    void Start()
    {
        anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.hp < 0f && !destroyed)
        {
            destroyed = true;
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
