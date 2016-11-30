using UnityEngine;
using System.Collections;

public class EnemyUnit : MonoBehaviour {
    // public Vector3 MyPosition;
    //   public GameObject MyGameObject;
    public Animator anim;
    
    private bool die = false;
    public int MaxHp;
    public int CurHp;
    public GameObject blood;
    public int Def;
    public int ATP;
    public float Range;
    public float Speed;
    public float Attackspeed;
    public float FinalPosition;
    public int Level;
    private SpriteRenderer sprite;
    private bool FireCurse=false;
    private int burnlevel=0;
    private bool ElectricCurse=false;
    private bool beattack = false;
    public enum UnitType {Sword,Pike,Mualer,Gunner, Mage, Cannon }
    public UnitType Mytype;
    public Element Myelement;
    //public bool Onstage; use for object pooling **** if have to optimize*****


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

    public void Set(Element E, UnitType T, int level)
    {
        blood = this.transform.FindChild("Canvas").GetChild(0).GetChild(0).gameObject;
        Range = 1f;
        ElectricCurse = false;
        FireCurse = false;
        Mytype = T;
       // if (Mytype == UnitType.Gunner) Debug.Log("Mygun");
        FinalPosition =-5f;
        Level = level;
        Myelement = E;
        anim = this.GetComponent<Animator>();
        anim.SetInteger("Level", Level - 1);
        checkLevel(Level);
        //    this.GetComponent<BoxCollider2D>().que
        StartCoroutine("walk");
    }

    void Awake()
    {

        sprite = this.GetComponent<SpriteRenderer>();

    }
    public enum WinLose { win, lose, equal }

    private WinLose checkwinlos( Element Another)
    {
        Element My = this.Myelement;

        if (My == Element.Fire)
        {
            switch (Another)
            {
                case (Element.Water):
                case (Element.Soil): return WinLose.lose;
                case (Element.Grass):
                case (Element.Electric): return WinLose.win;
                default: return WinLose.equal;
            }
        }

        else if (My == Element.Electric)
        {
            switch (Another)
            {
                case (Element.Fire):
                case (Element.Soil): return WinLose.lose;
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

        }
        else
            return WinLose.equal;

    }

    public void Attacked(Unit Slime)
    {
        if (!die)
        {
            if (!beattack)
                StartCoroutine("BeAttacked");
            int Damage = (this.checkwinlos(Slime.Myelement) == WinLose.win) ? Slime.ATP / 2 : Slime.ATP;
            Damage = (this.checkwinlos(Slime.Myelement) == WinLose.lose) ? Slime.ATP * 2 : Slime.ATP;
            if (Slime.Level > 2)
            {
                switch (Slime.Myelement)
                {
                    case (Element.Water): this.CurHp = this.CurHp - Damage;
                        if (this.CurHp <= 0)
                        {
                            die = true;
                            TearDrop.Instance.incresing();
                            StopAllCoroutines();
                            StartCoroutine("Die");
                        } return;
                    case (Element.Fire):
                        if (FireCurse)
                        {
                            burnlevel = (burnlevel > Slime.Level) ? burnlevel : Slime.Level;
                        }
                        else
                        {
                            FireCurse = true; burnlevel = Slime.Level; StartCoroutine("Burning");
                        }
                        break;
                    default: break;
                } this.CurHp = this.CurHp - ((Damage - this.Def < 0) ? 0 : Damage - this.Def);
            }
            else this.CurHp = this.CurHp - ((Damage - this.Def < 0) ? 0 : Damage - this.Def);
            if (this.CurHp <= 0)
            {
                die = true;
                TearDrop.Instance.incresing();
                this.gameObject.layer = 0;
                StopAllCoroutines();
                StopCoroutine("walk");

                anim.SetBool("Die", true);
                StartCoroutine("Die");

            }
            blood.transform.localPosition = blood.transform.localPosition - new Vector3((this.MaxHp - this.CurHp)*2f / this.MaxHp, 0f, 0f);
        }

    }
    IEnumerator Die()
    {

        float tt = anim.GetCurrentAnimatorStateInfo(0).length;
        Debug.Log(tt + "aass");
       yield return new WaitForSeconds(tt+0.1f);
        Destroy(gameObject);
    }
    IEnumerator BeAttacked()
    {
        Color Mycolor = sprite.color;
        beattack = true;
        for (int i = 0; i < 5; i++)
        {
            sprite.color = Color.red;
            yield return new WaitForSeconds(0.15f);

            sprite.color = Mycolor;

            yield return new WaitForSeconds(0.15f);
        }
        beattack = false;
    }
    IEnumerator walk()
    {
        RaycastHit2D Hit = new RaycastHit2D();
        Unit Slime = new Unit();
        bool mageFoundSlime = false;
        while (this.transform.position.x > FinalPosition&&!die)
        {
            yield return null;
            if (!ElectricCurse)
            { 
                
               

                if (Mytype != UnitType.Mage && Mytype != UnitType.Cannon&&Mytype!=UnitType.Gunner)
                {   
                   
                    Hit = Physics2D.Raycast(this.transform.position, Vector2.left, Range, 1 << LayerMask.NameToLayer("Soil"));
                    if (Hit.collider != null)
                    {
                        anim.SetBool("Attack", true);
                        if (this.gameObject == null) break;
                        else if (Hit.transform.gameObject != null)
                        {
                          Slime = Hit.transform.gameObject.GetComponent<Unit>();
                            Slime.Attacked(this.ATP, Slime.checkwinlos(Myelement));

                        }
                        yield return new WaitForSeconds(Attackspeed);

                    }
                    else
                    {
                        Hit = Physics2D.Raycast(this.transform.position, Vector2.left, Range, 1 << LayerMask.NameToLayer("Water") | 1 << LayerMask.NameToLayer("Fire") | 1 << LayerMask.NameToLayer("Electric") | 1 << LayerMask.NameToLayer("Grass") | 1 << LayerMask.NameToLayer("Normal"));
                        if (Hit.collider != null)
                        {
                            anim.SetBool("Attack", true);
                            if (this.gameObject == null) break;
                            else if (Hit.transform.gameObject != null)
                            {
                               Slime = Hit.transform.gameObject.GetComponent<Unit>();
                                Slime.Attacked(this.ATP, Slime.checkwinlos(Myelement));
                                if (Slime.Myelement == Element.Electric && Slime.Level >= 3)
                                        {
                                         if (!ElectricCurse) { ElectricCurse = true; StartCoroutine("Shock"); } continue;
                                        }
                            }
                            yield return new WaitForSeconds(Attackspeed);
                        }
                        else
                        {


                             Hit = Physics2D.Raycast(this.transform.position, Vector2.left, Range, 1 << LayerMask.NameToLayer("Wall"));
                             if (Hit.collider != null)
                             {

                                 anim.SetBool("Attack", true);
                             
                                 Wall.Instance.HP = ((this.ATP / 3 - Wall.Instance.def) <= 0) ? Wall.Instance.HP - 1 : Wall.Instance.HP - (this.ATP / 3 - Wall.Instance.def);
                     
                                     Destroy(gameObject);

                                 yield return new WaitForSeconds(Attackspeed);
                            }
                              else
                                {
                                    if (this.gameObject == null) break; 
                                 this.transform.position += Vector3.left * Time.deltaTime * Speed;

                                } 
                        }
                        
                    }
                }
                else if(Mytype==UnitType.Gunner)
                {
                    GameObject Bullet = this.transform.FindChild("Bullet").gameObject;
                    Hit = Physics2D.Raycast(this.transform.position, Vector2.left, 1f, 1 << LayerMask.NameToLayer("Wall"));
                    if (Hit.collider != null)
                    {

                        anim.SetBool("Attack", true);

                        Wall.Instance.HP = ((this.ATP / 3 - Wall.Instance.def) <= 0) ? Wall.Instance.HP - 1 : Wall.Instance.HP - (this.ATP / 3 - Wall.Instance.def);

                        Destroy(gameObject);

                     //   yield return new WaitForSeconds(Attackspeed);
                    }
                  
                      Hit = Physics2D.Raycast(this.transform.position, Vector2.left, Range, 1 << LayerMask.NameToLayer("Soil") | 1 << LayerMask.NameToLayer("Water") | 1 << LayerMask.NameToLayer("Fire") | 1 << LayerMask.NameToLayer("Electric") | 1 << LayerMask.NameToLayer("Grass") | 1 << LayerMask.NameToLayer("Normal"));

                   if (Hit.collider != null)
                    {
                        Bullet.transform.position = this.transform.position;
                        anim.SetBool("Attack", true);

                        Bullet.SetActive(true);

                        while (Bullet.transform.position.x > -1f)
                        {
                            yield return null;
                            Bullet.transform.position += Vector3.left * 10f * Time.deltaTime;
                            
                            anim.SetBool("Attack", false);
                            Hit = Physics2D.Raycast(Bullet.transform.position, Vector2.zero, 5f, 1 << LayerMask.NameToLayer("Soil") | 1 << LayerMask.NameToLayer("Water") | 1 << LayerMask.NameToLayer("Fire") | 1 << LayerMask.NameToLayer("Electric") | 1 << LayerMask.NameToLayer("Grass") | 1 << LayerMask.NameToLayer("Normal"));
                         
                                //  Debug.DrawRay(new Vector3(this.transform.position.x - 1f, this.transform.position.y - 3f, this.transform.position.z), Vector2.up * 6f, Color.yellow, 5f);
                                if (this.gameObject == null) break;
                                if (Hit.collider != null)
                                {
                                    Slime = Hit.transform.gameObject.GetComponent<Unit>();
                                    Bullet.SetActive(false);
                                   
                                     Slime.Attacked(this.ATP, Slime.checkwinlos(Myelement));
                                     break;
                                

                               } 
                        } Bullet.SetActive(false);
                        yield return new WaitForSeconds(Attackspeed);


                        if (this.gameObject == null) break;
                    }
                   else
                   {
                       this.transform.position += Vector3.left * Time.deltaTime * Speed;
                   }


                }

                else if(Mytype == UnitType.Mage)
                {
                    mageFoundSlime = false;
                    Hit = Physics2D.Raycast(this.transform.position, Vector2.left, Range, 1 << LayerMask.NameToLayer("Wall"));
                    if (Hit.collider != null)
                    {
                        anim.SetBool("Attack", true);
                        Wall.Instance.HP = ((this.ATP*3 - Wall.Instance.def) <= 0) ? Wall.Instance.HP - 3 : Wall.Instance.HP - (this.ATP*3 - Wall.Instance.def);
                        Destroy(gameObject);
                    }
                    Hit = Physics2D.Raycast(this.transform.position, Vector2.left, 1f,1<<LayerMask.NameToLayer("Soil")| 1 << LayerMask.NameToLayer("Water") | 1 << LayerMask.NameToLayer("Fire") | 1 << LayerMask.NameToLayer("Electric") | 1 << LayerMask.NameToLayer("Grass") | 1 << LayerMask.NameToLayer("Normal"));
                    if (Hit.collider != null)
                    {
                      
                        anim.SetBool("Attack", true);
                        this.gameObject.transform.FindChild("Power").gameObject.SetActive(true);
                        mageFoundSlime = true;
                    }
                    if (mageFoundSlime)
                    {
                        RaycastHit2D[] Hits = Physics2D.RaycastAll(new Vector2(this.transform.position.x - 1f, this.transform.position.y - 3f), Vector2.up, 6f, 1 << LayerMask.NameToLayer("Soil") | 1 << LayerMask.NameToLayer("Water") | 1 << LayerMask.NameToLayer("Fire") | 1 << LayerMask.NameToLayer("Electric") | 1 << LayerMask.NameToLayer("Grass") | 1 << LayerMask.NameToLayer("Normal"));

                        for (int i = 0; i < Hits.Length; i++)
                        {
                            Debug.DrawRay(new Vector3(this.transform.position.x - 1f, this.transform.position.y - 3f, this.transform.position.z), Vector2.up * 6f, Color.yellow, 5f);
                            if (this.gameObject == null) break;
                            if (Hits[i].collider != null)
                            {
                                mageFoundSlime = true;
                                Slime = Hits[i].transform.gameObject.GetComponent<Unit>();
                                Slime.Attacked(this.ATP, Slime.checkwinlos(Myelement));

                            }
                        }
                      
                        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
                        anim.SetBool("Attack", false);
                        this.gameObject.transform.FindChild("Power").gameObject.SetActive(false);
                        Debug.Log("sdasdasd"+anim.GetCurrentAnimatorStateInfo(0).length);
                        yield return new WaitForSeconds(Attackspeed - anim.GetCurrentAnimatorStateInfo(0).length);
                        mageFoundSlime = false;
                        if (this.gameObject == null) break;
                        continue;
                    }
                     else
                     {
                         if (this.gameObject == null) break;
                         this.transform.position += Vector3.left * Time.deltaTime * Speed;
                         yield return null;
                     }
                }
                else
                {
                    GameObject Bullet = this.transform.FindChild("Bullet").gameObject;
                    RaycastHit2D[] Hits;
                    if (this.transform.position.x >= 14f)
                        this.transform.position += Vector3.left * Time.deltaTime * Speed;
                    else
                    {
                        Bullet.transform.position = this.transform.position;
                        anim.SetBool("Attack", true);

                        Bullet.SetActive(true);
                        while (Bullet.transform.position.x > -1f)
                        {
                            yield return null;
                            Bullet.transform.position += Vector3.left * 10f * Time.deltaTime;
                            Hit = Physics2D.Raycast(Bullet.transform.position, Vector2.zero, 5f, 1 << LayerMask.NameToLayer("Wall"));
                            if (Hit.collider != null)
                            {
                                Wall.Instance.HP = (((this.ATP / 3) - Wall.Instance.def) <= 0) ? Wall.Instance.HP - 3 : Wall.Instance.HP - ((this.ATP / 3) - Wall.Instance.def);
                                break;
                                // Destroy(gameObject);
                            }

                            anim.SetBool("Attack", false);
                            Hits = Physics2D.RaycastAll(Bullet.transform.position, Vector2.zero, 5f, 1 << LayerMask.NameToLayer("Soil") | 1 << LayerMask.NameToLayer("Water") | 1 << LayerMask.NameToLayer("Fire") | 1 << LayerMask.NameToLayer("Electric") | 1 << LayerMask.NameToLayer("Grass") | 1 << LayerMask.NameToLayer("Normal"));
                          
                            for (int i = 0; i < Hits.Length; i++)
                            {
                                //  Debug.DrawRay(new Vector3(this.transform.position.x - 1f, this.transform.position.y - 3f, this.transform.position.z), Vector2.up * 6f, Color.yellow, 5f);
                                if (this.gameObject == null) break;
                                if (Hits[i].collider != null)
                                {
                                    Slime = Hits[i].transform.gameObject.GetComponent<Unit>();

                                    if (!Slime.hited)
                                    {
                                        Slime.Attacked(this.ATP, Slime.checkwinlos(Myelement), true);
                                        if (Slime.Myelement == Element.Normal && Slime.Level == 3)
                                        {
                                            Bullet.SetActive(false);
                                            break;
                                        }
                                    }
                                }

                            } 
                        } Bullet.SetActive(false);
                        yield return new WaitForSeconds(Attackspeed);


                        if (this.gameObject == null) break;
                    }


                }
            }


            anim.SetBool("Attack", false);
        }///End While
        ///Move To Camp of enemy(infont of enemy)
        if (this.gameObject != null) 
        this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector2(FinalPosition, this.transform.position.y), this.Speed * Time.deltaTime);

        EndGame.Instance.LoseEnd();

        yield return null;
        Destroy(gameObject);
    }
    IEnumerator Shock()
    {
        int i = 0;
        while (i<20)
        {
           // Debug.Log("imin");
            this.transform.Translate(Vector3.right * Time.deltaTime * 8f); 
            yield return new WaitForSeconds(0.01f);
            i++;
        } yield return new WaitForSeconds(0.5f);
        ElectricCurse = false;

    }
    IEnumerator Burning()
    {
        for (int i = 0; i < 5; i++)
        {
            this.CurHp=this.CurHp-burnlevel;
            if (this.CurHp <= 0)
            {
                TearDrop.Instance.incresing(); Destroy(gameObject);
            }
            yield return new WaitForSeconds(1f);

        } FireCurse = false; burnlevel = 0;
    }
    private void checkLevel(int newlevel)
    {/*
        switch (newlevel)
        {
            case (1): MaxHp = 10; CurHp = 10; ATP = 3; Def = 1; Speed = 5; Attackspeed = 1.5f; break;
            case (2): MaxHp = 11; CurHp = (CurHp > MaxHp) ? MaxHp : CurHp; ATP = 4; Def = 3; Attackspeed = 1.3f; break;
            default: break;
        }
        switch (Myelement)
        {
            case (Element.Fire): switch (newlevel)
                {
                    case (3): MaxHp = 6; CurHp = (CurHp > MaxHp) ? MaxHp : CurHp; ATP = 6; Def = 3; Speed = 5; Attackspeed = 1.5f; break;
                    case (4): MaxHp = 8; CurHp = (CurHp > MaxHp) ? MaxHp : CurHp; ATP = 9; Def = 4; Speed = 6; Attackspeed = 1.3f; break;
                    case (5): MaxHp = 10; CurHp = (CurHp > MaxHp) ? MaxHp : CurHp; ATP = 10; Def = 5; Speed = 7; Attackspeed = 0.9f; break;
                } break;
            case (Element.Soil): switch (newlevel)
                {
                    case (3): MaxHp = 24; CurHp = (CurHp > MaxHp) ? MaxHp : CurHp; ATP = 4; Def = 7; Speed = 3; Attackspeed = 1.5f; break;
                    case (4): MaxHp = 30; CurHp = (CurHp > MaxHp) ? MaxHp : CurHp; ATP = 6; Def = 10; Speed = 4; Attackspeed = 1.4f; break;
                    case (5): MaxHp = 36; CurHp = (CurHp > MaxHp) ? MaxHp : CurHp; ATP = 8; Def = 14; Speed = 5; Attackspeed = 1.3f; break;

                } break;
            case (Element.Electric): switch (newlevel)
                {
                    case (3): MaxHp = 12; CurHp = (CurHp > MaxHp) ? MaxHp : CurHp; ATP = 1; Def = 3; Speed = 7; Attackspeed = 1.5f; break;
                    case (4): MaxHp = 16; CurHp = (CurHp > MaxHp) ? MaxHp : CurHp; ATP = 2; Def = 4; Speed = 9; Attackspeed = 1.3f; break;
                    case (5): MaxHp = 20; CurHp = (CurHp > MaxHp) ? MaxHp : CurHp; ATP = 4; Def = 5; Speed = 12; Attackspeed = 0.9f; break;

                } break;
            case (Element.Grass): switch (newlevel)
                {
                    case (3): MaxHp = 12; CurHp = (CurHp > MaxHp) ? MaxHp : CurHp; ATP = 4; Def = 3; Speed = 5; Attackspeed = 1.5f; break;
                    case (4): MaxHp = 14; CurHp = (CurHp > MaxHp) ? MaxHp : CurHp; ATP = 4; Def = 3; Speed = 6; Attackspeed = 1.3f; break;
                    case (5): MaxHp = 16; CurHp = (CurHp > MaxHp) ? MaxHp : CurHp; ATP = 4; Def = 3; Speed = 7; Attackspeed = 0.9f; break;

                } break;
            case (Element.Water): switch (newlevel)
                {
                    case (3): MaxHp = 12; CurHp = (CurHp > MaxHp) ? MaxHp : CurHp; ATP = 5; Def = 3; Speed = 5; Attackspeed = 1.5f; Range = 1.5f; break;
                    case (4): MaxHp = 16; CurHp = (CurHp > MaxHp) ? MaxHp : CurHp; ATP = 7; Def = 5; Speed = 6; Attackspeed = 1.4f; Range = 1.5f; break;
                    case (5): MaxHp = 20; CurHp = (CurHp > MaxHp) ? MaxHp : CurHp; ATP = 9; Def = 7; Speed = 7; Attackspeed = 1.3f; Range = 1.5f; break;

                } break;
            default: MaxHp = 20; CurHp = (CurHp > MaxHp) ? MaxHp : CurHp; ATP = 9; Def = 5; Speed = 5; Attackspeed = 1f; break;
                
        }*/
        switch (Mytype)
        {
            case (UnitType.Sword): CurHp = MaxHp = 360+WaveControl.Instance.ADDHP; ATP = 70 + WaveControl.Instance.ADDamage ; Def = 10; Speed = 5; Attackspeed = 4f; Range = 1f; break;
            case (UnitType.Pike): CurHp = MaxHp = 300 + WaveControl.Instance.ADDHP; ATP = 80 + WaveControl.Instance.ADDamage; Def = 20; Speed = 3; Attackspeed = 2.5f; Range = 2f; break;
            case (UnitType.Mualer): CurHp = MaxHp = 200 + WaveControl.Instance.ADDHP; ATP = 100 + WaveControl.Instance.ADDamage; Def = 10; Speed = 2; Attackspeed = 1f; Range = 1f; break;
            case (UnitType.Mage): CurHp = MaxHp = 160 + WaveControl.Instance.ADDHP; ATP = 80 + WaveControl.Instance.ADDamage; Def = 10; Speed = 2; Attackspeed = 2f; Range = 1f; break;
            case (UnitType.Gunner): CurHp = MaxHp = 500 + WaveControl.Instance.ADDHP; ATP = 40 + WaveControl.Instance.ADDamage; Def = 20; Speed = 1; Attackspeed = 1f; Range = 60f; break;
            case (UnitType.Cannon): CurHp = MaxHp = 100 + WaveControl.Instance.ADDHP; ATP = 150 + WaveControl.Instance.ADDamage; Def = 0; Speed = 3; Attackspeed = -20f; Range = 60f; break;

        } Attackspeed = 1.5f - (Attackspeed * 0.12f);


    }

    // Update is called once per frame



}
