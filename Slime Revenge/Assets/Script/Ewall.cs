using UnityEngine;
using System.Collections;
using UnityEngine.Sprites;
public class Ewall : MonoBehaviour
{
    public static Ewall Instance { get { return wall; } }
    private static Ewall wall;
    public int HP = 1000;
    public int max_Hp = 1000;
    public int def = 50;
    private int stage;
    public Sprite[] sp=new Sprite[2];
    // Use this for initialization
    void Awake()
    {
        wall = this;
    }
    void Start()
    {
        stage = ParsingData.Instnce.GetStage();
        int s = stage / 10;
        if (stage%10==0)
        this.GetComponent<SpriteRenderer>().sprite = sp[s-1];
        else
        {
            Debug.Log("spriteon");
            this.GetComponent<SpriteRenderer>().sprite = sp[s];
        }
      
        wall = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.HP < 0f)
        {
            EndGame.Instance.WinEnd();
            gameObject.SetActive(false);
        }
    }
}
