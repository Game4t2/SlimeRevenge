using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Sprites;
public class Ewall : MonoBehaviour
{
    public static Ewall Instance { get { return wall; } }
    private static Ewall wall;
    public int HP = 1000;
    public int max_Hp = 1000;
    public int def = 50;
    private Animator anim;
    private bool ended=false;
    private int stage;
    public List<Sprite> sp;
    // Use this for initialization
    void Awake()
    {
        wall = this;
    }
    void Start()
    {
        stage = ParsingData.Instnce.GetStage();

        int s = stage / 10;
        

        this.transform.GetComponent<SpriteRenderer>().sprite = sp[s];
        anim = this.GetComponent<Animator>();
    
     
        ended = false;
        wall = this;
    }

    // Update is called once per frame
    void Update()
    {
          if (this.HP < 0f && !ended)
        {
            ended = true; StartCoroutine("TotheEnd");
            
        }
    }

    IEnumerator TotheEnd()
    {
        this.GetComponent<Animator>().enabled = true;
        
        anim.SetInteger("Type", stage / 10);
        anim.SetBool("Down", true);
        yield return null;

        EndGame.Instance.WinEnd();
        gameObject.SetActive(false);
    }
}
