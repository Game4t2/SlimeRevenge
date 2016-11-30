using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
///Class For create Normal Unit
/// </summary>
 
 public enum Element { Normal=0,Fire=1, Water=2,Grass=3,Electric=4,Soil=5 }
public class Unit : MonoBehaviour {
   // public Vector3 MyPosition;
  // public GameObject MyGameObject;
    public Animator anim;
    private bool bekilled=false;
    private bool Ended;
    private bool CanonEmpty=true;
    public bool hited=false;
    private GameObject Bullet;
    public List<GameObject> Active;
    private SpriteRenderer sprite;
    public List<GameObject> DisActive;
    public int MaxHp;
    public int CurHp;
    public int Def;
    public int ATP;
    public float Range;
    public float Speed;
    public float Attackspeed;
    public float FinalPosition;
    public int Level;
    private bool beattack=false;
   // public TouchDeploy Mycontrol;
  
    public Element Myelement;


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

    public void Set(Element E,int level=1)
    {
        Ended = false;
        if(Active==null)
        Active = TouchDeploy.Instance().Actives[(int)E];

        if (DisActive == null)
        DisActive = TouchDeploy.Instance().DisActives[(int)E];
       
        FinalPosition = 60f;
        Level = level;
        Myelement = E;
        anim = this.GetComponent<Animator>(); 
        checkLevel(Level);
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
       if (!Ended)
       {
           Ended = true;

           if (this.Myelement == Element.Normal && Level >= 5)
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
        Active.Remove(gameObject);
        DisActive.Add(gameObject);
        anim.SetBool("Death", false);
        anim.SetBool("Attack", false);
        gameObject.SetActive(false); anim.SetInteger("State", 0);
        if(bekilled)
        ChargeBar.Instance.Increseing();
        bekilled = false;

    }

    public enum WinLose {win,lose,equal}

    public WinLose checkwinlos(Element Another)
    {
        Element My = Myelement;
        if (My == Element.Fire)
        {
            switch(Another){
                case(Element.Water):
                case(Element.Soil):return WinLose.lose;
                case (Element.Grass):
                case (Element.Electric): return WinLose.win;
                default: return WinLose.equal;
                }
        }
        
        else if (My == Element.Electric)
        {
            switch(Another){ case(Element.Fire):
                             case(Element.Soil):return WinLose.lose;
                             case (Element.Water):
                             case (Element.Grass): return WinLose.win;
                    default: return WinLose.equal;
           }

        }
        else if (My == Element.Grass)
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
        else if (My == Element.Soil)
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
        else if (My == Element.Water)
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
   
    public void Attacked(int Atack,WinLose EnemyWinLose,bool CanonShot=false)
    {
        if (CanonShot)  
            StartCoroutine("BecanonShot");
        if (!beattack)
        {
            beattack = true;
            StartCoroutine("BeAttacked");
        }
        int Damage = Atack;
        if (EnemyWinLose == WinLose.win) { Damage *= 2; }
        else if ((EnemyWinLose == WinLose.lose)) { Damage =Damage/2; }
        
        Damage=Damage<Def?0:Damage-Def; 
        this.CurHp = this.CurHp -Damage;
        if (CurHp <= 0)
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
        if (this.Myelement == Element.Normal) { touse = null; return false; }
        foreach (GameObject x in Active)
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
        while (this.transform.position.x < FinalPosition)
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
                        this.transform.position = Vector3.MoveTowards(this.transform.position, another.transform.position, this.Speed * 3f * Time.deltaTime);
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
                    this.Level = (Unitanother.Level > this.Level) ? Unitanother.Level + 1 : this.Level + 1;
                    this.Level = (this.Level >= 5) ? 5 : this.Level;
                    this.CurHp = Unitanother.CurHp + this.CurHp;
                    checkLevel(Level);
                    if (this.Level >= 3)
                    {
                        anim.SetInteger("State", 2);

                    }

                    this.transform.position = Unitanother.transform.position;

                    Unitanother.End();
                }
            }
            /////

            /////Check Attacked Enemy
            if (!(this.Myelement == Element.Normal && Level >=4))/// CheckEnemy (checkthis is maleenunit(Element+MaleeType))
            {
                Hit = Physics2D.Raycast(this.transform.position, Vector2.right, Range, 1 << LayerMask.NameToLayer("EUnit"));
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
                    yield return new WaitForSeconds(Attackspeed);
                    anim.SetBool("Attack", false);
                }

                else { this.transform.position += Vector3.right * Time.deltaTime * Speed; }
            }

            else if(Myelement==Element.Normal&&Level==4)////Attack of canon
            {
                Bullet = this.transform.GetChild(0).transform.gameObject;
                if (CanonEmpty)
                {
                    Hit = Physics2D.Raycast(this.transform.position, Vector2.left, 1f, 1 << LayerMask.NameToLayer("Water") | 1 << LayerMask.NameToLayer("Fire") | 1 << LayerMask.NameToLayer("Electric") | 1 << LayerMask.NameToLayer("Grass") | 1 << LayerMask.NameToLayer("Soil"));
                    if (Hit.collider != null)
                    {
                        Bullet.transform.position = this.transform.position;
                        another = Hit.transform.gameObject;
                        Bullet.GetComponent<SpriteRenderer>().sprite = Bullet.GetComponent<Bullet>().Getsprite(another.GetComponent<Unit>().Myelement);
                    
                        Bullet.SetActive(true);
                     
                       
                         another.GetComponent<Unit>().End();
                        CanonEmpty = false;

                    }
                    else if (this.transform.position.x < 3f)
                    {
                        this.transform.position += Vector3.right * Time.deltaTime * Speed;
                    }
                }
                if (!CanonEmpty)
                {
                    Hit = Physics2D.Raycast(this.transform.position, Vector2.right, Range, 1 << LayerMask.NameToLayer("EUnit"));
                    if (Hit.collider != null)
                    {
                        Vector3 Position = Hit.transform.position;
                       
                        anim.SetBool("Attack", true);
                        yield return new WaitForSeconds(0.1f);
                        anim.SetBool("Attack", false);
                        
                        while (Bullet.transform.position.x < Position.x)
                        {
                           
                            Bullet.transform.position += Vector3.right*(Time.deltaTime*10f);
                            yield return null;
                        }
                        Bullet.GetComponent<Animator>().SetInteger("Element", Bullet.GetComponent<Bullet>().elementvalue);
                     
                       
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

                        yield return new WaitForSeconds(Bullet.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length); Bullet.SetActive(false);
                        Bullet.SetActive(false); Bullet.GetComponent<Animator>().SetInteger("Element", 7);
                       // yield return new WaitForSeconds(Attackspeed);
                     
                        CanonEmpty = true;
                    }
                    else if (this.transform.position.x < 3f)
                    {
                        this.transform.position += Vector3.right * Time.deltaTime * Speed;
                    }
                }
            }


            else if (Myelement == Element.Normal && Level == 5)////Attack of KingGuard
            {
                Found = false;
                for (int i = 0; i < 3; i++)
                {
                    RaycastHit2D[] Hits = Physics2D.RaycastAll(new Vector2(this.transform.position.x, GameObject.Find("Len").transform.GetChild(i).transform.position.y), Vector2.right, Range, 1 << LayerMask.NameToLayer("EUnit"));
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
                    this.transform.position += Vector3.right * Time.deltaTime * Speed;
                }
                else
                {
                    yield return new WaitForSeconds(Attackspeed);

                    this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    anim.SetBool("Attack", false);
                }
            }
           

              Hit = Physics2D.Raycast(this.transform.position, Vector2.right, 1f, 1 << LayerMask.NameToLayer("Ewall"));
              if (Hit.collider != null)
              {
                  Ewall.Instance.HP = (Ewall.Instance.def >= this.ATP) ? Ewall.Instance.HP - 1 : Ewall.Instance.HP - (this.ATP - Ewall.Instance.def);
                  End();
              }

        }///End While
        ///Move To Camp of enemy(infont of enemy)
        this.transform.position = Vector3.MoveTowards(this.transform.position,new Vector2(FinalPosition,this.transform.position.y), this.Speed* Time.deltaTime);
        End();

        yield return null;
       // gameObject.SetActive(false);
       // Destroy(gameObject);        
    }
    private void checkLevel(int newlevel)
    {
        if (Myelement!=Element.Normal)
        switch (newlevel)
        {
            case (1): MaxHp = 140; CurHp = 140; ATP = 35; Def = 10; Speed = 5; Attackspeed = 0f; Range = 1; Attackspeed = 1.5f - (Attackspeed * 0.12f); return;
            case (2): MaxHp = 160; CurHp = (CurHp > MaxHp) ? MaxHp : CurHp; ATP = 40; Def = 20; Attackspeed = 1f; Range = 1; Attackspeed = 1.5f - (Attackspeed * 0.12f); return;
              default: break;
        }
        switch (Myelement)
        {
            case (Element.Fire): switch (newlevel)
                {
                    case (3): MaxHp = 160; CurHp = (CurHp > MaxHp) ? MaxHp:CurHp ; ATP = 60; Def = 20; Speed = 5f; Attackspeed = 0f; break;
                    case (4): MaxHp = 170; CurHp = (CurHp > MaxHp) ? MaxHp : CurHp; ATP = 70; Def = 25; Speed = 6f; Attackspeed = 2f; break;
                    case (5): MaxHp = 180; CurHp = (CurHp > MaxHp) ? MaxHp : CurHp; ATP = 80; Def = 30; Speed = 7f; Attackspeed =4f; break;
                }break;
            case (Element.Soil): switch (newlevel)
                {
                    case (3): MaxHp = 160; CurHp = (CurHp > MaxHp) ? MaxHp : CurHp; ATP = 40; Def = 30; Speed = 3f; Attackspeed = 0f; break;
                    case (4): MaxHp = 200; CurHp = (CurHp > MaxHp) ? MaxHp : CurHp; ATP = 50; Def = 35; Speed = 4f; Attackspeed = 0f; break;
                    case (5): MaxHp = 240; CurHp = (CurHp > MaxHp) ? MaxHp : CurHp; ATP = 60; Def = 40; Speed = 5f; Attackspeed = 1f; break;

                } break;
            case (Element.Electric): switch (newlevel)
                {
                    case (3): MaxHp = 160; CurHp = (CurHp > MaxHp) ? MaxHp : CurHp; ATP = 40; Def = 20; Speed = 7f; Attackspeed = 0f; break;
                    case (4): MaxHp = 180; CurHp = (CurHp > MaxHp) ? MaxHp : CurHp; ATP = 45; Def = 25; Speed = 9f; Attackspeed = 1f; break;
                    case (5): MaxHp = 200; CurHp = (CurHp > MaxHp) ? MaxHp : CurHp; ATP = 50; Def = 30; Speed = 12f; Attackspeed = 2f; break;
     
                } break;
            case (Element.Grass): switch (newlevel)
                {         
                    case (3): MaxHp = 160; CurHp = (CurHp > MaxHp) ? MaxHp : CurHp; ATP = 40; Def = 20; Speed = 5f; Attackspeed = 1f; break;
                    case (4): MaxHp = 180; CurHp = (CurHp > MaxHp) ? MaxHp : CurHp; ATP = 50; Def = 25; Speed = 6f; Attackspeed = 2f; break;
                    case (5): MaxHp = 210; CurHp = (CurHp > MaxHp) ? MaxHp : CurHp; ATP = 60; Def = 30; Speed = 7f; Attackspeed = 3f; break;

                }StartCoroutine("GrassHealing"); break;
            case (Element.Water): switch (newlevel)
                {
                    case (3): MaxHp = 160; CurHp = (CurHp > MaxHp) ? MaxHp : CurHp; ATP = 50; Def = 20; Speed = 5f; Attackspeed = 0f; Range = 1.5f; break;
                    case (4): MaxHp = 180; CurHp = (CurHp > MaxHp) ? MaxHp : CurHp; ATP = 60; Def = 25; Speed = 6f; Attackspeed = 1f; Range = 1.5f; break;
                    case (5): MaxHp = 210; CurHp = (CurHp > MaxHp) ? MaxHp : CurHp; ATP = 70; Def = 30; Speed = 7f; Attackspeed = 2f; Range = 1.5f; break;
     
                } break;
            case (Element.Normal): switch (newlevel)
                {
                    /*Sword*/   case (1): CurHp = MaxHp = 300; ATP = 200; Def = 28; Speed = 4f; Attackspeed = 7f; Range = 1.5f; break;
                    /*Pike*/    case (2): CurHp = MaxHp = 240; ATP = 180; Def = 34; Speed = 4f; Attackspeed = 5f; Range = 2f; break;
                    /*Shield*/  case (3): CurHp = MaxHp = 500; ATP = 0;   Def = 70; Speed = 2f; Attackspeed = 0f; Range = 0f; break;
                    /*Cannon*/  case (4): CurHp = MaxHp = 150;  ATP = 200; Def = 0; Speed = 2f; Attackspeed = -10f; Range = 60f; break;
                  //////Hero///////
                    /*KingGuard*/
                    case (5): CurHp = MaxHp = 5000; ATP = 300; Def = 50; Speed = 1f; Attackspeed = 0f; Range = 3f; break;
        
                // /*Other*/case (5): MaxHp = 16; CurHp = (CurHp > MaxHp) ? MaxHp : CurHp; ATP = 7; Def = 5; Speed = 6; Attackspeed = 1.4f; Range = 40f; break;
                   // case (6): MaxHp = 20; CurHp = (CurHp > MaxHp) ? MaxHp : CurHp; ATP = 9; Def = 7; Speed = 7; Attackspeed = 1.3f; Range = 40f; break;
     
               
                }break;


        } Attackspeed = 1.5f - (Attackspeed * 0.12f);
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
                        g.GetComponent<Unit>().CurHp = (g.GetComponent<Unit>().CurHp < g.GetComponent<Unit>().MaxHp) ? g.GetComponent<Unit>().CurHp + 1 : g.GetComponent<Unit>().CurHp;
                   
                
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
