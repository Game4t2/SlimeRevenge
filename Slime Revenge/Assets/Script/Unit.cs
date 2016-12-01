using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
///Class For create Normal Unit
/// </summary>
 
 
public class Unit : MonoBehaviour {
   // public Vector3 MyPosition;
  // public GameObject MyGameObject;
    public Animator anim;
    private bool bekilled=false;
    private bool ended;
    private bool canonEmpty=true;
    public bool hited=false;
    private GameObject bullet;
    public List<GameObject> active;
    private SpriteRenderer sprite;
    public List<GameObject> inActive;
    public int maxHp;
    public int curHp;
    public int def;
    public int atp;
    public float range;
    public float speed;
    public float attackSpeed;
    public float finalPosition;
    public int level;
    private bool beattack=false;
   // public TouchDeploy Mycontrol;
  
    public Element myElement;


    public Element CheckwhichElement(int x)
    {
        switch (x)
        {
            case (1): return Element.Fire;
            case (2): return Element.Water;
            case (3): return Element.Grass;
            case (4): return Element.Electric;
            case (5): return Element.Soil;
            default: return Element.Normal;
        }

    }

    public void Set(Element e,int level=1)
    {
        ended = false;
        if(active==null)
        active = TouchDeploy.Instance().Actives[(int)e];

        if (inActive == null)
        inActive = TouchDeploy.Instance().DisActives[(int)e];
       
        finalPosition = 60f;
        this.level = level;
        myElement = e;
        anim = this.GetComponent<Animator>();
        checkLevel(this.level);
    //    this.GetComponent<BoxCollider2D>().que
        StartCoroutine("walk");
           }

   void Awake()
    {
        sprite = this.GetComponent<SpriteRenderer>();

   }
   void start()
   {
     
   }
   public void End()
   {
       if (!ended)
       {
           ended = true;

           if (this.myElement == Element.Normal && level >= 5)
           {
               SkillUse.Instance.HeroOnStage = false;
           }
           StopAllCoroutines();
           StartCoroutine("Death");
       }
   }
    IEnumerator Death(){
        //int i = 0;
        //i=this.gameObject.layer;
        yield return null;
        //this.gameObject.layer = LayerMask.NameToLayer("Invis");

        anim.SetBool("Attack", false);
        anim.SetBool("Death", true);
        if (bekilled)
            yield return new WaitForSeconds(0.7f);
        else yield return null;
     //   this.gameObject.layer = i;
      //  Debug.Log(LayerMask.LayerToName(this.gameObject.layer));
        sprite.color = Color.white;
        beattack = false;
        active.Remove(gameObject);
        inActive.Add(gameObject);
        anim.SetBool("Death", false);
        anim.SetBool("Attack", false);
        gameObject.SetActive(false); anim.SetInteger("State", 0);
        if(bekilled)
        ChargeBar.Instance.Increseing();
        bekilled = false;

    }


    public WinLose checkwinlos(Element Another)
    {
        if (myElement == Element.Fire)
        {
            switch(Another){
                case(Element.Water):
                case(Element.Soil):return WinLose.lose;
                case (Element.Grass):
                case (Element.Electric): return WinLose.win;
                default: return WinLose.equal;
                }
        }
        
        else if (myElement == Element.Electric)
        {
            switch(Another){ case(Element.Fire):
                             case(Element.Soil):return WinLose.lose;
                             case (Element.Water):
                             case (Element.Grass): return WinLose.win;
                    default: return WinLose.equal;
           }

        }
        else if (myElement == Element.Grass)
        {
            switch (Another)
            {
                case (Element.Fire):
                case (Element.Electric): return WinLose.lose;
                case (Element.Water):
                case (Element.Soil): return WinLose.win;
                default: return WinLose.equal;
            }

        }
        else if (myElement == Element.Soil)
        {
            switch (Another)
            {
                case (Element.Water):
                case (Element.Grass): return WinLose.lose;
                case (Element.Fire):
                case (Element.Electric): return WinLose.win;
                default: return WinLose.equal;
            }

        }
        else if (myElement == Element.Water)
        {
            switch (Another)
            {
                case (Element.Grass):
                case (Element.Electric): return WinLose.lose;
                case (Element.Fire):
                case (Element.Soil): return WinLose.win;
                default: return WinLose.equal;
            }

        }else
        return WinLose.equal;

    } 
   
    public void Attacked(int attack,WinLose enemyWinLose,bool canonShot=false)
    {
        if (canonShot)  
            StartCoroutine("BecanonShot");
        if (!beattack)
        {
            beattack = true;
            StartCoroutine("BeAttacked");
        }
        int Damage = attack;
        if (enemyWinLose == WinLose.win) { Damage *= 2; }
        else if ((enemyWinLose == WinLose.lose)) { Damage =Damage/2; }
        
        Damage=Damage<def?0:Damage-def; 
        this.curHp = this.curHp -Damage;
        if (curHp <= 0)
        {
            ///Ver Optimize by using Object pooling
            /// 
           /// enable = false;
            /// 
            /// Ver Not Optimize using Destroy
            bekilled = true;
            End();
            
         //   gameObject.SetActive(false);
      //      Destroy(gameObject);
           
            ///
        }

    }
    IEnumerator BecanonShot()
    {


        hited = true;
        yield return new WaitForSeconds(0.5f);

        hited = false;
    }
    IEnumerator BeAttacked()
    {
        for (int i = 0; i < 5; i++)
        {
            sprite.color = new Color(1f, 0.4f, 0.4f, 0.9f);
            yield return new WaitForSeconds(0.15f);

            sprite.color = new Color(1f, 1, 1f, 0.9f);

            yield return new WaitForSeconds(0.15f);
        }
        beattack = false;

    }
   
    public bool CheckSameElement(out GameObject touse)
    {
        if (this.myElement == Element.Normal) { touse = null; return false; }
        foreach (GameObject x in active)
        {
            if (x != this.gameObject)
            {
             //   Debug.Log("INCheck1");
                
                if (x.transform.position.x >= this.transform.position.x && x.transform.position.x <= this.transform.position.x + 2.5f&&x.transform.position.y==this.transform.position.y)
                {
                    touse = x;
                    return true;
               //     Debug.Log("INCheck2");
                 
                   
                }
            }

        } touse = gameObject;
        return false;

    }
    IEnumerator walk(){
        RaycastHit2D Hit=new RaycastHit2D();
        bool Found = false;
       GameObject another;
        while (this.transform.position.x < finalPosition)
        {
            yield return null;
         //EatAnother
            if (CheckSameElement(out another))
            {
                bool desDW = false;//destroy during walk to
                while (true)
                {
                    if (another.gameObject.activeSelf != false)
                    {
                        this.transform.position = Vector3.MoveTowards(this.transform.position, another.transform.position, this.speed * 3f * Time.deltaTime);
                        yield return null;
                        if (another.gameObject.activeSelf == false)
                        {
                            desDW = true; break;
                        }

                        else if (this.transform.position.x > another.transform.position.x - 0.2f) break;
                    }
                    else { desDW = true; break; }

                } if (!desDW)
                {
                    Unit Unitanother = another.GetComponent<Unit>();
                    this.level = (Unitanother.level > this.level) ? Unitanother.level + 1 : this.level + 1;
                    this.level = (this.level >= 5) ? 5 : this.level;
                    this.curHp = Unitanother.curHp + this.curHp;
                    checkLevel(level);
                    if (this.level >= 3)
                    {
                        anim.SetInteger("State", 2);

                    }

                    this.transform.position = Unitanother.transform.position;

                    Unitanother.End();
                }
            }
            /////

            /////Check Attacked Enemy
            if (!(this.myElement == Element.Normal && level >=4))/// CheckEnemy (checkthis is maleenunit(Element+MaleeType))
            {
                Hit = Physics2D.Raycast(this.transform.position, Vector2.right, range, 1 << LayerMask.NameToLayer("EUnit"));
                if (Hit.collider != null)
                {

                    anim.SetBool("Attack", true);
                    if (this.gameObject == null) break;
                    else if (Hit.transform.gameObject != null)
                    {
                        EnemyUnit Human = Hit.transform.gameObject.GetComponent<EnemyUnit>();
                        if (Human.transform.gameObject != null)
                            Human.Attacked(this);
                    }
                    yield return new WaitForSeconds(attackSpeed);
                    anim.SetBool("Attack", false);
                }

                else { this.transform.position += Vector3.right * Time.deltaTime * speed; }
            }

            else if(myElement==Element.Normal&&level==4)////Attack of canon
            {
                bullet = this.transform.GetChild(0).transform.gameObject;
                if (canonEmpty)
                {
                    Hit = Physics2D.Raycast(this.transform.position, Vector2.left, 1f, 1 << LayerMask.NameToLayer("Water") | 1 << LayerMask.NameToLayer("Fire") | 1 << LayerMask.NameToLayer("Electric") | 1 << LayerMask.NameToLayer("Grass") | 1 << LayerMask.NameToLayer("Soil"));
                    if (Hit.collider != null)
                    {
                        bullet.transform.position = this.transform.position;
                        another = Hit.transform.gameObject;
                        bullet.GetComponent<SpriteRenderer>().sprite = bullet.GetComponent<Bullet>().Getsprite(another.GetComponent<Unit>().myElement);
                    
                        bullet.SetActive(true);
                     
                       
                         another.GetComponent<Unit>().End();
                        canonEmpty = false;

                    }
                    else if (this.transform.position.x < 3f)
                    {
                        this.transform.position += Vector3.right * Time.deltaTime * speed;
                    }
                }
                if (!canonEmpty)
                {
                    Hit = Physics2D.Raycast(this.transform.position, Vector2.right, range, 1 << LayerMask.NameToLayer("EUnit"));
                    if (Hit.collider != null)
                    {
                        Vector3 Position = Hit.transform.position;
                       
                        anim.SetBool("Attack", true);
                        yield return new WaitForSeconds(0.1f);
                        anim.SetBool("Attack", false);
                        
                        while (bullet.transform.position.x < Position.x)
                        {
                           
                            bullet.transform.position += Vector3.right*(Time.deltaTime*10f);
                            yield return null;
                        }
                        bullet.GetComponent<Animator>().SetInteger("Element", bullet.GetComponent<Bullet>().elementvalue);
                     
                       
                        RaycastHit2D[] Hits = Physics2D.RaycastAll(Position - new Vector3(1f, 0f, 0f), Vector2.right, 6f, 1 << LayerMask.NameToLayer("EUnit"));
                      //  Debug.Log(Position - new Vector3(1f, 0f, 0f) + "to" + Position + new Vector3(5f, 0f, 0f));
                        foreach (RaycastHit2D hit in Hits)
                        {
                            if (hit.collider != null)
                            {
                                if (hit.transform.gameObject != null)
                                {
                                    EnemyUnit Human = hit.transform.gameObject.GetComponent<EnemyUnit>();
                                    if (Human.transform.gameObject != null)
                                        Human.Attacked(this);
                                    Debug.Log(Human.name);
                                }
                            }
                        }//Debug.Log(Bullet.GetComponent<Animator>().playbackTime);

                        yield return new WaitForSeconds(bullet.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length); bullet.SetActive(false);
                        bullet.SetActive(false); bullet.GetComponent<Animator>().SetInteger("Element", 7);
                       // yield return new WaitForSeconds(Attackspeed);
                     
                        canonEmpty = true;
                    }
                    else if (this.transform.position.x < 3f)
                    {
                        this.transform.position += Vector3.right * Time.deltaTime * speed;
                    }
                }
            }


            else if (myElement == Element.Normal && level == 5)////Attack of KingGuard
            {
                Found = false;
                for (int i = 0; i < 3; i++)
                {
                    RaycastHit2D[] Hits = Physics2D.RaycastAll(new Vector2(this.transform.position.x, GameObject.Find("Len").transform.GetChild(i).transform.position.y), Vector2.right, range, 1 << LayerMask.NameToLayer("EUnit"));
                    if (Hits.Length < 1) continue;
                    this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                    foreach (RaycastHit2D hit in Hits)
                    {
                        anim.SetBool("Attack", true);
                        if (hit.collider != null)
                        {
                            Found = true;
                            if (hit.transform.gameObject != null)
                            {
                                EnemyUnit Human = hit.transform.gameObject.GetComponent<EnemyUnit>();
                                if (Human.transform.gameObject != null)
                                    Human.Attacked(this);
                            }
                        }
                  
                    }
                } if (!Found)
                {
                    this.transform.position += Vector3.right * Time.deltaTime * speed;
                }
                else
                {
                    yield return new WaitForSeconds(attackSpeed);

                    this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    anim.SetBool("Attack", false);
                }
            }
           

              Hit = Physics2D.Raycast(this.transform.position, Vector2.right, 1f, 1 << LayerMask.NameToLayer("Ewall"));
              if (Hit.collider != null)
              {
                  Ewall.Instance.HP = (Ewall.Instance.def >= this.atp) ? Ewall.Instance.HP - 1 : Ewall.Instance.HP - (this.atp - Ewall.Instance.def);
                  End();
              }

        }///End While
        ///Move To Camp of enemy(infont of enemy)
        this.transform.position = Vector3.MoveTowards(this.transform.position,new Vector2(finalPosition,this.transform.position.y), this.speed* Time.deltaTime);
        End();

        yield return null;
       // gameObject.SetActive(false);
       // Destroy(gameObject);        
    }
    private void checkLevel(int newlevel)
    {
        if (myElement!=Element.Normal)
        switch (newlevel)
        {
            case (1): maxHp = 140; curHp = 140; atp = 35; def = 10; speed = 5; attackSpeed = 0f; range = 1; attackSpeed = 1.5f - (attackSpeed * 0.12f); return;
            case (2): maxHp = 160; curHp = (curHp > maxHp) ? maxHp : curHp; atp = 40; def = 20; attackSpeed = 1f; range = 1; attackSpeed = 1.5f - (attackSpeed * 0.12f); return;
              default: break;
        }
        switch (myElement)
        {
            case (Element.Fire): switch (newlevel)
                {
                    case (3): maxHp = 160; curHp = (curHp > maxHp) ? maxHp:curHp ; atp = 60; def = 20; speed = 5f; attackSpeed = 0f; break;
                    case (4): maxHp = 170; curHp = (curHp > maxHp) ? maxHp : curHp; atp = 70; def = 25; speed = 6f; attackSpeed = 2f; break;
                    case (5): maxHp = 180; curHp = (curHp > maxHp) ? maxHp : curHp; atp = 80; def = 30; speed = 7f; attackSpeed =4f; break;
                }break;
            case (Element.Soil): switch (newlevel)
                {
                    case (3): maxHp = 160; curHp = (curHp > maxHp) ? maxHp : curHp; atp = 40; def = 30; speed = 3f; attackSpeed = 0f; break;
                    case (4): maxHp = 200; curHp = (curHp > maxHp) ? maxHp : curHp; atp = 50; def = 35; speed = 4f; attackSpeed = 0f; break;
                    case (5): maxHp = 240; curHp = (curHp > maxHp) ? maxHp : curHp; atp = 60; def = 40; speed = 5f; attackSpeed = 1f; break;

                } break;
            case (Element.Electric): switch (newlevel)
                {
                    case (3): maxHp = 160; curHp = (curHp > maxHp) ? maxHp : curHp; atp = 40; def = 20; speed = 7f; attackSpeed = 0f; break;
                    case (4): maxHp = 180; curHp = (curHp > maxHp) ? maxHp : curHp; atp = 45; def = 25; speed = 9f; attackSpeed = 1f; break;
                    case (5): maxHp = 200; curHp = (curHp > maxHp) ? maxHp : curHp; atp = 50; def = 30; speed = 12f; attackSpeed = 2f; break;
     
                } break;
            case (Element.Grass): switch (newlevel)
                {         
                    case (3): maxHp = 160; curHp = (curHp > maxHp) ? maxHp : curHp; atp = 40; def = 20; speed = 5f; attackSpeed = 1f; break;
                    case (4): maxHp = 180; curHp = (curHp > maxHp) ? maxHp : curHp; atp = 50; def = 25; speed = 6f; attackSpeed = 2f; break;
                    case (5): maxHp = 210; curHp = (curHp > maxHp) ? maxHp : curHp; atp = 60; def = 30; speed = 7f; attackSpeed = 3f; break;

                }StartCoroutine("GrassHealing"); break;
            case (Element.Water): switch (newlevel)
                {
                    case (3): maxHp = 160; curHp = (curHp > maxHp) ? maxHp : curHp; atp = 50; def = 20; speed = 5f; attackSpeed = 0f; range = 1.5f; break;
                    case (4): maxHp = 180; curHp = (curHp > maxHp) ? maxHp : curHp; atp = 60; def = 25; speed = 6f; attackSpeed = 1f; range = 1.5f; break;
                    case (5): maxHp = 210; curHp = (curHp > maxHp) ? maxHp : curHp; atp = 70; def = 30; speed = 7f; attackSpeed = 2f; range = 1.5f; break;
     
                } break;
            case (Element.Normal): switch (newlevel)
                {
                    /*Sword*/   case (1): curHp = maxHp = 300; atp = 200; def = 28; speed = 4f; attackSpeed = 7f; range = 1.5f; break;
                    /*Pike*/    case (2): curHp = maxHp = 240; atp = 180; def = 34; speed = 4f; attackSpeed = 5f; range = 2f; break;
                    /*Shield*/  case (3): curHp = maxHp = 500; atp = 0;   def = 70; speed = 2f; attackSpeed = 0f; range = 0f; break;
                    /*Cannon*/  case (4): curHp = maxHp = 150;  atp = 200; def = 0; speed = 2f; attackSpeed = -10f; range = 60f; break;
                  //////Hero///////
                    /*KingGuard*/
                    case (5): curHp = maxHp = 5000; atp = 300; def = 50; speed = 1f; attackSpeed = 0f; range = 3f; break;
        
                // /*Other*/case (5): maxHp = 16; curHp = (curHp > maxHp) ? maxHp : curHp; ATP = 7; Def = 5; Speed = 6; Attackspeed = 1.4f; Range = 40f; break;
                   // case (6): maxHp = 20; curHp = (curHp > maxHp) ? maxHp : curHp; ATP = 9; Def = 7; Speed = 7; Attackspeed = 1.3f; Range = 40f; break;
     
               
                }break;


        } attackSpeed = 1.5f - (attackSpeed * 0.12f);
    }
    IEnumerator GrassHealing()
    {
        while (true)
        {
            foreach (List<GameObject> li in TouchDeploy.Instance().Actives)
            {
                foreach (GameObject g in li)
                {
                    if (g != this.gameObject && Mathf.Abs(g.transform.position.x - this.transform.position.x) < 2f && g.transform.position.y == this.transform.position.y)
                    {
                        g.GetComponent<Unit>().StartCoroutine("Healing");
                        g.GetComponent<Unit>().curHp = (g.GetComponent<Unit>().curHp < g.GetComponent<Unit>().maxHp) ? g.GetComponent<Unit>().curHp + 1 : g.GetComponent<Unit>().curHp;
                   
                
                    }
                }
            }


            yield return new WaitForSeconds(3f);
        }
    }
    IEnumerator Healing()
    {
        StopCoroutine("BeAttacked");
        beattack = false;
            sprite.color = new Color(0.4f,1f,0.4f,0.9f);
            yield return new WaitForSeconds(2f);

            sprite.color = new Color(1f, 1, 1f, 0.9f);

       
    }
	// Update is called once per frame



}
