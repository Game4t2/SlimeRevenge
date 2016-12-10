using UnityEngine;
using System.Collections;

public class EnemyUnit : MonoBehaviour
{
    // public Vector3 MyPosition;
    //   public GameObject MyGameObject;
    public Animator anim;

    private bool die = false;
    public int maxHp;
    public int curHp;
    public GameObject blood;
    public int def;
    public int atp;
    public float range;
    public float speed;
    public float attackspeed;
    private float finalPosition;
    public int level;
    private SpriteRenderer sprite;
    private bool fireCurse = false;
    private int burnlevel = 0;
    private bool electricCurse = false;
    private bool beattack = false;

    public EnemyUnitType mytype;
    public Element myelement;
    //public bool Onstage; use for object pooling **** if have to optimize*****


    public Element CheckwhichElement(int x)
    {
        return (Element)x;
    }

    public void Set(Element e, EnemyUnitType t, int level)
    {
        blood = this.transform.FindChild("Canvas").GetChild(0).GetChild(0).gameObject;
        range = 1f;
        electricCurse = false;
        fireCurse = false;
        mytype = t;
        // if (Mytype == UnitType.Gunner) Debug.Log("Mygun");
        finalPosition = TouchDeploy.Instance().transform.position.x;
        this.level = level;
        myelement = e;
        anim = this.GetComponent<Animator>();
        anim.SetInteger("Level", this.level - 1);
        checkLevel(this.level);
        //    this.GetComponent<BoxCollider2D>().que
        StartCoroutine("Walk");
    }

    void Awake()
    {

        sprite = this.GetComponent<SpriteRenderer>();

    }

    private WinLose checkwinlos(Element another)
    {
        if (myelement == Element.Fire)
        {
            switch (another)
            {
                case (Element.Water):
                case (Element.Soil): return WinLose.lose;
                case (Element.Grass):
                case (Element.Electric): return WinLose.win;
                default: return WinLose.equal;
            }
        }

        else if (myelement == Element.Electric)
        {
            switch (another)
            {
                case (Element.Fire):
                case (Element.Soil): return WinLose.lose;
                case (Element.Water):
                case (Element.Grass): return WinLose.win;
                default: return WinLose.equal;
            }

        }
        else if (myelement == Element.Grass)
        {
            switch (another)
            {
                case (Element.Fire):
                case (Element.Electric): return WinLose.lose;
                case (Element.Water):
                case (Element.Soil): return WinLose.win;
                default: return WinLose.equal;
            }

        }
        else if (myelement == Element.Soil)
        {
            switch (another)
            {
                case (Element.Water):
                case (Element.Grass): return WinLose.lose;
                case (Element.Fire):
                case (Element.Electric): return WinLose.win;
                default: return WinLose.equal;
            }

        }
        else if (myelement == Element.Water)
        {
            switch (another)
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

    public void Attacked(Unit slime)
    {
        if (!die)
        {
            if (!beattack)
                StartCoroutine("BeAttacked");
            int Damage = (this.checkwinlos(slime.myElement) == WinLose.win) ? slime.atp / 2 : slime.atp;
            Damage = (this.checkwinlos(slime.myElement) == WinLose.lose) ? slime.atp * 2 : slime.atp;
            if (slime.level > 2)
            {
                switch (slime.myElement)
                {
                    case (Element.Water):
                        this.curHp = this.curHp - Damage;
                        if (this.curHp <= 0)
                        {
                            die = true;
                            TearDrop.Instance.incresing();
                            StopAllCoroutines();
                            StartCoroutine("Die");
                        }
                        return;
                    case (Element.Fire):
                        if (fireCurse)
                        {
                            burnlevel = (burnlevel > slime.level) ? burnlevel : slime.level;
                        }
                        else
                        {
                            fireCurse = true; burnlevel = slime.level; StartCoroutine("Burning");
                        }
                        break;
                    default: break;
                }
                this.curHp = this.curHp - ((Damage - this.def < 0) ? 0 : Damage - this.def);
            }
            else this.curHp = this.curHp - ((Damage - this.def < 0) ? 0 : Damage - this.def);
            if (this.curHp <= 0)
            {
                die = true;
                TearDrop.Instance.incresing();
                this.gameObject.layer = 0;
                StopAllCoroutines();
                StopCoroutine("walk");

                anim.SetBool("Die", true);
                StartCoroutine("Die");

            }
            blood.transform.localPosition = blood.transform.localPosition - new Vector3((this.maxHp - this.curHp) * 2f / this.maxHp, 0f, 0f);
        }

    }
    IEnumerator Die()
    {

        float tt = anim.GetCurrentAnimatorStateInfo(0).length;
        Debug.Log(tt + "aass");
        yield return new WaitForSeconds(tt + 0.1f);
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
    IEnumerator Walk()
    {
        RaycastHit2D hit = new RaycastHit2D();
        Unit slime = new Unit();
        bool mageFoundSlime = false;
        while (this.transform.position.x > finalPosition && !die)
        {
            yield return null;
            if (!electricCurse)
            {



                if (mytype != EnemyUnitType.Mage && mytype != EnemyUnitType.Cannon && mytype != EnemyUnitType.Gunner)
                {

                    hit = Physics2D.Raycast(this.transform.position, Vector2.left, range, 1 << LayerMask.NameToLayer("Soil"));
                    if (hit.collider != null)
                    {
                        anim.SetBool("Attack", true);
                        if (this.gameObject == null) break;
                        else if (hit.transform.gameObject != null)
                        {
                            slime = hit.transform.gameObject.GetComponent<Unit>();
                            slime.Attacked(this.atp, slime.checkwinlos(myelement));

                        }
                        yield return new WaitForSeconds(attackspeed);

                    }
                    else
                    {
                        hit = Physics2D.Raycast(this.transform.position, Vector2.left, range, 1 << LayerMask.NameToLayer("Water") | 1 << LayerMask.NameToLayer("Fire") | 1 << LayerMask.NameToLayer("Electric") | 1 << LayerMask.NameToLayer("Grass") | 1 << LayerMask.NameToLayer("Normal"));
                        if (hit.collider != null)
                        {
                            anim.SetBool("Attack", true);
                            if (this.gameObject == null) break;
                            else if (hit.transform.gameObject != null)
                            {
                                slime = hit.transform.gameObject.GetComponent<Unit>();
                                slime.Attacked(this.atp, slime.checkwinlos(myelement));
                                if (slime.myElement == Element.Electric && slime.level >= 3)
                                {
                                    if (!electricCurse) { electricCurse = true; StartCoroutine("Shock"); }
                                    continue;
                                }
                            }
                            yield return new WaitForSeconds(attackspeed);
                        }
                        else
                        {


                            hit = Physics2D.Raycast(this.transform.position, Vector2.left, range, 1 << LayerMask.NameToLayer("Wall"));
                            if (hit.collider != null)
                            {

                                anim.SetBool("Attack", true);

                                Wall.Instance.HP = ((this.atp / 3 - Wall.Instance.def) <= 0) ? Wall.Instance.HP - 1 : Wall.Instance.HP - (this.atp / 3 - Wall.Instance.def);

                                Destroy(gameObject);

                                yield return new WaitForSeconds(attackspeed);
                            }
                            else
                            {
                                if (this.gameObject == null) break;
                                this.transform.position += Vector3.left * Time.deltaTime * speed;

                            }
                        }

                    }
                }
                else if (mytype == EnemyUnitType.Gunner)
                {
                    GameObject Bullet = this.transform.FindChild("Bullet").gameObject;
                    hit = Physics2D.Raycast(this.transform.position, Vector2.left, 1f, 1 << LayerMask.NameToLayer("Wall"));
                    if (hit.collider != null)
                    {

                        anim.SetBool("Attack", true);

                        Wall.Instance.HP = ((this.atp / 3 - Wall.Instance.def) <= 0) ? Wall.Instance.HP - 1 : Wall.Instance.HP - (this.atp / 3 - Wall.Instance.def);

                        Destroy(gameObject);

                        //   yield return new WaitForSeconds(Attackspeed);
                    }

                    hit = Physics2D.Raycast(this.transform.position, Vector2.left, range, 1 << LayerMask.NameToLayer("Soil") | 1 << LayerMask.NameToLayer("Water") | 1 << LayerMask.NameToLayer("Fire") | 1 << LayerMask.NameToLayer("Electric") | 1 << LayerMask.NameToLayer("Grass") | 1 << LayerMask.NameToLayer("Normal"));

                    if (hit.collider != null)
                    {
                        Bullet.transform.position = this.transform.position;
                        anim.SetBool("Attack", true);

                        Bullet.SetActive(true);

                        while (Bullet.transform.position.x > -1f)
                        {
                            yield return null;
                            Bullet.transform.position += Vector3.left * 10f * Time.deltaTime;

                            anim.SetBool("Attack", false);
                            hit = Physics2D.Raycast(Bullet.transform.position, Vector2.zero, 5f, 1 << LayerMask.NameToLayer("Soil") | 1 << LayerMask.NameToLayer("Water") | 1 << LayerMask.NameToLayer("Fire") | 1 << LayerMask.NameToLayer("Electric") | 1 << LayerMask.NameToLayer("Grass") | 1 << LayerMask.NameToLayer("Normal"));

                            //  Debug.DrawRay(new Vector3(this.transform.position.x - 1f, this.transform.position.y - 3f, this.transform.position.z), Vector2.up * 6f, Color.yellow, 5f);
                            if (this.gameObject == null) break;
                            if (hit.collider != null)
                            {
                                slime = hit.transform.gameObject.GetComponent<Unit>();
                                Bullet.SetActive(false);

                                slime.Attacked(this.atp, slime.checkwinlos(myelement));
                                break;


                            }
                        }
                        Bullet.SetActive(false);
                        yield return new WaitForSeconds(attackspeed);


                        if (this.gameObject == null) break;
                    }
                    else
                    {
                        this.transform.position += Vector3.left * Time.deltaTime * speed;
                    }


                }

                else if (mytype == EnemyUnitType.Mage)
                {
                    mageFoundSlime = false;
                    hit = Physics2D.Raycast(this.transform.position, Vector2.left, range, 1 << LayerMask.NameToLayer("Wall"));
                    if (hit.collider != null)
                    {
                        anim.SetBool("Attack", true);
                        Wall.Instance.HP = ((this.atp * 3 - Wall.Instance.def) <= 0) ? Wall.Instance.HP - 3 : Wall.Instance.HP - (this.atp * 3 - Wall.Instance.def);
                        Destroy(gameObject);
                    }
                    hit = Physics2D.Raycast(this.transform.position, Vector2.left, 1f, 1 << LayerMask.NameToLayer("Soil") | 1 << LayerMask.NameToLayer("Water") | 1 << LayerMask.NameToLayer("Fire") | 1 << LayerMask.NameToLayer("Electric") | 1 << LayerMask.NameToLayer("Grass") | 1 << LayerMask.NameToLayer("Normal"));
                    if (hit.collider != null)
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
                                slime = Hits[i].transform.gameObject.GetComponent<Unit>();
                                slime.Attacked(this.atp, slime.checkwinlos(myelement));

                            }
                        }

                        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
                        anim.SetBool("Attack", false);
                        this.gameObject.transform.FindChild("Power").gameObject.SetActive(false);
                        Debug.Log("sdasdasd" + anim.GetCurrentAnimatorStateInfo(0).length);
                        yield return new WaitForSeconds(attackspeed - anim.GetCurrentAnimatorStateInfo(0).length);
                        mageFoundSlime = false;
                        if (this.gameObject == null) break;
                        continue;
                    }
                    else
                    {
                        if (this.gameObject == null) break;
                        this.transform.position += Vector3.left * Time.deltaTime * speed;
                        yield return null;
                    }
                }
                else if (mytype == EnemyUnitType.Cannon)
                {
                    GameObject Bullet = this.transform.FindChild("Bullet").gameObject;
                    RaycastHit2D[] Hits;
                    if (this.transform.position.x >= (Ewall.Instance.transform.position.x-7f))
                        this.transform.position += Vector3.left * Time.deltaTime * speed;
                    else
                    {
                        Bullet.transform.position = this.transform.position;
                        anim.SetBool("Attack", true);

                        Bullet.SetActive(true);
                        while (Bullet.transform.position.x > Wall.Instance.transform.position.x+1f)
                        {
                            yield return null;
                            Bullet.transform.position += Vector3.left * 10f * Time.deltaTime;
                            /*hit = Physics2D.Raycast(Bullet.transform.position, Vector2.zero, 5f, 1 << LayerMask.NameToLayer("Wall"));
                            if (hit.collider != null)
                            {
                                Wall.Instance.HP = (((this.atp / 3) - Wall.Instance.def) <= 0) ? Wall.Instance.HP - 3 : Wall.Instance.HP - ((this.atp / 3) - Wall.Instance.def);
                                break;
                                // Destroy(gameObject);
                            }*/

                            anim.SetBool("Attack", false);
                            Hits = Physics2D.RaycastAll(Bullet.transform.position, Vector2.zero, 5f, 1 << LayerMask.NameToLayer("Soil") | 1 << LayerMask.NameToLayer("Water") | 1 << LayerMask.NameToLayer("Fire") | 1 << LayerMask.NameToLayer("Electric") | 1 << LayerMask.NameToLayer("Grass") | 1 << LayerMask.NameToLayer("Normal"));

                            for (int i = 0; i < Hits.Length; i++)
                            {
                                //  Debug.DrawRay(new Vector3(this.transform.position.x - 1f, this.transform.position.y - 3f, this.transform.position.z), Vector2.up * 6f, Color.yellow, 5f);
                                if (this.gameObject == null) break;
                                if (Hits[i].collider != null)
                                {
                                    slime = Hits[i].transform.gameObject.GetComponent<Unit>();

                                    if (!slime.hited)
                                    {
                                        slime.Attacked(this.atp, slime.checkwinlos(myelement), true);
                                        if (slime.myElement == Element.Normal && slime.level == 3)
                                        {
                                            Bullet.SetActive(false);
                                            break;
                                        }
                                    }
                                }

                            }
                        }
                        Bullet.SetActive(false);
                        yield return new WaitForSeconds(attackspeed);


                        if (this.gameObject == null) break;
                    }


                }
            }


            anim.SetBool("Attack", false);
        }///End While
         ///Move To Camp of enemy(infont of enemy)
        if (this.gameObject != null)
            this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector2(finalPosition, this.transform.position.y), this.speed * Time.deltaTime);

        EndGame.Instance.LoseEnd();

        yield return null;
        Destroy(gameObject);
    }
    IEnumerator Shock()
    {
        int i = 0;
        while (i < 20)
        {
            // Debug.Log("imin");
            this.transform.Translate(Vector3.right * Time.deltaTime * 8f);
            yield return new WaitForSeconds(0.01f);
            i++;
        }
        yield return new WaitForSeconds(0.5f);
        electricCurse = false;

    }
    IEnumerator Burning()
    {
        for (int i = 0; i < 5; i++)
        {
            this.curHp = this.curHp - burnlevel;
            if (this.curHp <= 0)
            {
                TearDrop.Instance.incresing(); Destroy(gameObject);
            }
            yield return new WaitForSeconds(1f);

        }
        fireCurse = false; burnlevel = 0;
    }
    private void checkLevel(int newlevel)
    {/*
        switch (newlevel)
        {
            case (1): maxHp = 10; curHp = 10; ATP = 3; Def = 1; Speed = 5; Attackspeed = 1.5f; break;
            case (2): maxHp = 11; curHp = (curHp > maxHp) ? maxHp : curHp; ATP = 4; Def = 3; Attackspeed = 1.3f; break;
            default: break;
        }
        switch (Myelement)
        {
            case (Element.Fire): switch (newlevel)
                {
                    case (3): maxHp = 6; curHp = (curHp > maxHp) ? maxHp : curHp; ATP = 6; Def = 3; Speed = 5; Attackspeed = 1.5f; break;
                    case (4): maxHp = 8; curHp = (curHp > maxHp) ? maxHp : curHp; ATP = 9; Def = 4; Speed = 6; Attackspeed = 1.3f; break;
                    case (5): maxHp = 10; curHp = (curHp > maxHp) ? maxHp : curHp; ATP = 10; Def = 5; Speed = 7; Attackspeed = 0.9f; break;
                } break;
            case (Element.Soil): switch (newlevel)
                {
                    case (3): maxHp = 24; curHp = (curHp > maxHp) ? maxHp : curHp; ATP = 4; Def = 7; Speed = 3; Attackspeed = 1.5f; break;
                    case (4): maxHp = 30; curHp = (curHp > maxHp) ? maxHp : curHp; ATP = 6; Def = 10; Speed = 4; Attackspeed = 1.4f; break;
                    case (5): maxHp = 36; curHp = (curHp > maxHp) ? maxHp : curHp; ATP = 8; Def = 14; Speed = 5; Attackspeed = 1.3f; break;

                } break;
            case (Element.Electric): switch (newlevel)
                {
                    case (3): maxHp = 12; curHp = (curHp > maxHp) ? maxHp : curHp; ATP = 1; Def = 3; Speed = 7; Attackspeed = 1.5f; break;
                    case (4): maxHp = 16; curHp = (curHp > maxHp) ? maxHp : curHp; ATP = 2; Def = 4; Speed = 9; Attackspeed = 1.3f; break;
                    case (5): maxHp = 20; curHp = (curHp > maxHp) ? maxHp : curHp; ATP = 4; Def = 5; Speed = 12; Attackspeed = 0.9f; break;

                } break;
            case (Element.Grass): switch (newlevel)
                {
                    case (3): maxHp = 12; curHp = (curHp > maxHp) ? maxHp : curHp; ATP = 4; Def = 3; Speed = 5; Attackspeed = 1.5f; break;
                    case (4): maxHp = 14; curHp = (curHp > maxHp) ? maxHp : curHp; ATP = 4; Def = 3; Speed = 6; Attackspeed = 1.3f; break;
                    case (5): maxHp = 16; curHp = (curHp > maxHp) ? maxHp : curHp; ATP = 4; Def = 3; Speed = 7; Attackspeed = 0.9f; break;

                } break;
            case (Element.Water): switch (newlevel)
                {
                    case (3): maxHp = 12; curHp = (curHp > maxHp) ? maxHp : curHp; ATP = 5; Def = 3; Speed = 5; Attackspeed = 1.5f; Range = 1.5f; break;
                    case (4): maxHp = 16; curHp = (curHp > maxHp) ? maxHp : curHp; ATP = 7; Def = 5; Speed = 6; Attackspeed = 1.4f; Range = 1.5f; break;
                    case (5): maxHp = 20; curHp = (curHp > maxHp) ? maxHp : curHp; ATP = 9; Def = 7; Speed = 7; Attackspeed = 1.3f; Range = 1.5f; break;

                } break;
            default: maxHp = 20; curHp = (curHp > maxHp) ? maxHp : curHp; ATP = 9; Def = 5; Speed = 5; Attackspeed = 1f; break;
                
        }*/
        switch (mytype)
        {
            case (EnemyUnitType.Sword): curHp = maxHp = 360 + WaveControl.Instance.ADDHP; atp = 70 + WaveControl.Instance.ADDamage; def = 10; speed = 5; attackspeed = 4f; range = 1f; break;
            case (EnemyUnitType.Pike): curHp = maxHp = 300 + WaveControl.Instance.ADDHP; atp = 80 + WaveControl.Instance.ADDamage; def = 20; speed = 3; attackspeed = 2.5f; range = 2f; break;
            case (EnemyUnitType.Mualer): curHp = maxHp = 200 + WaveControl.Instance.ADDHP; atp = 100 + WaveControl.Instance.ADDamage; def = 10; speed = 2; attackspeed = 1f; range = 1f; break;
            case (EnemyUnitType.Mage): curHp = maxHp = 160 + WaveControl.Instance.ADDHP; atp = 80 + WaveControl.Instance.ADDamage; def = 10; speed = 2; attackspeed = 2f; range = 1f; break;
            case (EnemyUnitType.Gunner): curHp = maxHp = 500 + WaveControl.Instance.ADDHP; atp = 40 + WaveControl.Instance.ADDamage; def = 20; speed = 1; attackspeed = 1f; range = 60f; break;
            case (EnemyUnitType.Cannon): curHp = maxHp = 100 + WaveControl.Instance.ADDHP; atp = 150 + WaveControl.Instance.ADDamage; def = 0; speed = 3; attackspeed = -20f; range = 60f; break;

        }
        attackspeed = 1.5f - (attackspeed * 0.12f);


    }

    // Update is called once per frame



}
