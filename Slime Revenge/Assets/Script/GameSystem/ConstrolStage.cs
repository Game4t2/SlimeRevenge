using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
public class ConstrolStage : MonoBehaviour {
    public static ConstrolStage Instance { get { return instance; } }
    public GameObject Canvas;
    public GameObject FollowMouse;
    private static ConstrolStage instance;
    private SkillID Buffer;
    private RaycastHit2D hit;
    public SkillID[] Skill=new SkillID[3];
    private bool clicked=false;
    private int clickBox = -1;
    public GameObject[] GoSkill = new GameObject[3];
    public int stageId = 0;
    public Sprite sp;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            {
                GoSkill[i] = GameObject.Find("Box" + (i + 1).ToString());
              
                Skill[i] = new SkillID();
                Skill[i].ID = 0;


            }
        }
    }
    public void Init()
    {
        for (int i = 0; i < 3; i++)
        {
            {
                GoSkill[i] = GameObject.Find("Box" + (i + 1).ToString());
                Skill[i] = new SkillID();
                Skill[i].ID = 0;


            }
        } 
    }
    void Update()
    {

        if (!Canvas.activeSelf)
        {
  
            Cameramove.Instanc.stopMove= false;
            if (Input.GetMouseButtonDown(0))
            {
                hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 1f, 1 << LayerMask.NameToLayer("Button"));
                if (hit.collider != null)
                {
                 
                    string c = hit.transform.name;
                    stageId = int.Parse(c);
                    Canvas.SetActive(true); Init();
                    Debug.Log(GoSkill[0]);
                }

            }
        }   
        else
        {
            Cameramove.Instanc.stopMove = true;
            if (Input.GetMouseButtonDown(0))
            {
                clicked = true;
                hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 5f, 1 << LayerMask.NameToLayer("Invis"));
                if (hit.collider != null)
                {
                    if (hit.transform.name == "LoadGame")
                    {
                        Sprite[] sprites = new Sprite[3];
                        for (int i = 0; i < 3; i++)
                        {
                            sprites[i] = GoSkill[i].GetComponent<SpriteRenderer>().sprite;
                        }
                        ParsingData.Instnce.Load(Skill, sprites,stageId);
                        clicked = false;
                    }
                   else
                    //Debug.Log("sss");
                    FollowMouse.GetComponent<SpriteRenderer>().sprite = hit.transform.GetComponent<SpriteRenderer>().sprite;
                    Buffer =hit.transform.GetComponent<SkillID>();
                }
                else
                {
                    hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 5f, 1 << LayerMask.NameToLayer("Normal"));
                     if (hit.collider != null)
                         {

                           FollowMouse.GetComponent<SpriteRenderer>().sprite = hit.transform.GetComponent<SpriteRenderer>().sprite;
                           
                         
                          switch (hit.transform.name)
                                {
                                    case ("Box1"): Buffer = Skill[0]; clickBox = 0; break;
                                    case ("Box2"): Buffer = Skill[1]; clickBox = 1; break;
                                    case ("Box3"): Buffer = Skill[2]; clickBox = 2; break;
                                //    case ("Box4"): Buffer = Skill[3]; clickBox = 3; break;
                                }
                          Debug.Log(hit.transform.name + "This:" + Buffer.ID);
                         }

                }
            }
            if (Input.GetMouseButtonUp(0) && FollowMouse.GetComponent<SpriteRenderer>().sprite != null && clicked)
            {
                clicked = false;
                hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 5f, 1 << LayerMask.NameToLayer("Normal"));
                if (hit.collider != null)
                {
                 //   Debug.Log("sss");
                    switch (hit.transform.name)
                  {
                      case ("Box1"): check_skill(0, Buffer.ID, Buffer.IsType, FollowMouse.GetComponent<SpriteRenderer>().sprite); break;
                      case ("Box2"): check_skill(1, Buffer.ID, Buffer.IsType, FollowMouse.GetComponent<SpriteRenderer>().sprite); break;
                      case ("Box3"): check_skill(2, Buffer.ID, Buffer.IsType, FollowMouse.GetComponent<SpriteRenderer>().sprite); break;
                    //  case ("Box4"): check_skill(3, Buffer.ID, Buffer.IsType, FollowMouse.GetComponent<SpriteRenderer>().sprite); break;
                         
                  }

                }
                else if (clickBox>-1)
                {

                    Skill[clickBox].ID = 0;
                    Skill[clickBox].IsType = false;
                    GoSkill[clickBox].GetComponent<SpriteRenderer>().sprite = sp;

                }

                FollowMouse.GetComponent<SpriteRenderer>().sprite = null;
                Buffer = null;
                clickBox = -1;
            }
        }
        

    }

    private void check_skill(int mybox,int id,bool type,Sprite sprite)
    {
        for (int i = 0; i < 3; i++)
        {
            if (i == mybox)continue ;
            else if (Skill[i].ID == id) {
                int oldid = Skill[mybox].ID;
                Skill[i].ID = oldid;
           //     Debug.Log(Skill[i].ID+","+Skill[mybox].ID);
                bool oldtype = Skill[mybox].IsType;
                Skill[i].IsType = oldtype;
                Sprite s=GoSkill[mybox].GetComponent<SpriteRenderer>().sprite;
                GoSkill[i].GetComponent<SpriteRenderer>().sprite = s;

             //   Debug.Log(GoSkill[i].GetComponent<SpriteRenderer>().sprite + "," + GoSkill[mybox].GetComponent<SpriteRenderer>().sprite);
              
            }
        }

        Debug.Log(Skill[0].ID + "," + Skill[1].ID + "," + Skill[2].ID );
        Debug.Log("--------------------------------");
        Skill[mybox].ID=id;
        Skill[mybox].IsType =type;
        GoSkill[mybox].GetComponent<SpriteRenderer>().sprite=sprite;
        Debug.Log(Skill[0].ID + "," + Skill[1].ID + "," + Skill[2].ID );
    }

}
