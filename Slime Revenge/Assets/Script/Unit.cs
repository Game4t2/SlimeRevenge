using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
///Class For create Normal Unit
/// </summary>


public class Unit : MonoBehaviour
{
    // public Vector3 MyPosition;
    // public GameObject MyGameObject;
    public Animator anim;
    private bool bekilled = false;
    private bool ended;
    public List<SlimeUnit> slimeUnits;
    private bool canonEmpty = true;
    public bool hited = false;
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
    private bool beattack = false;
    // public TouchDeploy Mycontrol;

    public Element element;



    public void Set(SlimeUnit sunit, List<SlimeUnit> slimegroup)
    {
        slimeUnits = slimegroup;
        Set(sunit);
        //    this.GetComponent<BoxCollider2D>().que

    }
    public void Set(SlimeUnit sunit)
    {
        ended = false;
        if (active == null)
            active = TouchDeploy.Instance().Actives[(int)sunit.element];

        if (inActive == null)
            inActive = TouchDeploy.Instance().DisActives[(int)sunit.element];

        finalPosition = 60f;

        this.level = sunit.level;
        anim = this.GetComponent<Animator>();
        checkLevel(this.level);
        //    this.GetComponent<BoxCollider2D>().que

    }
    public void StartWalk()
    {
        StartCoroutine("walk");
    }
    void Awake()
    {
        sprite = this.GetComponent<SpriteRenderer>();

    }
    public void End()
    {
        if (!ended)
        {
            ended = true;

            if (this.element == Element.Normal && level >= 5)
            {
                SkillUse.Instance.HeroOnStage = false;
            }
            StopAllCoroutines();
            StartCoroutine("Death");
        }
    }
    IEnumerator Death()
    {
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
        if (bekilled)
            ChargeBar.Instance.Increseing();
        bekilled = false;

    }



    public void Attacked(int attack, WinLose enemyWinLose, bool canonShot = false)
    {
        if (canonShot)
            StartCoroutine("BecanonShot");
        if (!beattack)
        {
            beattack = true;
            StartCoroutine("AnimationAttacked");
        }
        int Damage = attack;
        if (enemyWinLose == WinLose.win) { Damage *= 2; }
        else if ((enemyWinLose == WinLose.lose)) { Damage = Damage / 2; }

        Damage = Damage < def ? 0 : Damage - def;
        this.curHp = this.curHp - Damage;
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
    IEnumerator AnimationAttacked()
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
        if (this.element == Element.Normal) { touse = null; return false; }
        foreach (GameObject x in active)
        {
            if (x != this.gameObject)
            {
                //   Debug.Log("INCheck1");

                if (x.transform.position.x >= this.transform.position.x && x.transform.position.x <= this.transform.position.x + 2.5f && x.transform.position.y == this.transform.position.y)
                {
                    touse = x;
                    return true;
                    //     Debug.Log("INCheck2");
                }
            }

        }
        touse = gameObject;
        return false;

    }
    public void EatToEvol(Unit Unitanother)
    {
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
    public void Attack(RaycastHit2D hit)
    {

        if (this.gameObject == null) return;
        else if (hit.transform.gameObject != null)
        {
            EnemyUnit Human = hit.transform.gameObject.GetComponent<EnemyUnit>();
            if (Human.transform.gameObject != null)
                Human.Attacked(this);
        }
    }
    public void Attack(RaycastHit2D hit,bool foundenemy)
    {
        if (hit.transform.gameObject != null)
        {
            Attack(hit);
            foundenemy = true;
        }
    }

 IEnumerator walk()
    {
        RaycastHit2D hit = new RaycastHit2D();
        bool found = false;
        GameObject another;
        while (this.transform.position.x < finalPosition)
        {
            yield return null;
            //EatAnother
            if (CheckSameElement(out another))
            {
                while (true)///walkTotarget
                {
                    if (another.gameObject.activeSelf != false)
                    {
                        this.transform.position = Vector3.MoveTowards(this.transform.position, another.transform.position, this.speed * 3f * Time.deltaTime);
                        yield return null;

                        if (this.transform.position.x > another.transform.position.x - 0.2f) break;
                    }

                }
                EatToEvol(another.GetComponent<Unit>());
            }
            /////

            /////Check Attacked Enemy
            if (this.element != Element.Normal)/// CheckEnemy (checkthis is maleenunit(Element+MaleeType))
            {
                hit = Physics2D.Raycast(this.transform.position, Vector2.right, range, 1 << LayerMask.NameToLayer("EUnit"));
                if (hit.collider != null)
                {

                    anim.SetBool("Attack", true);
                    Attack(hit);
                    yield return new WaitForSeconds(attackSpeed);
                    anim.SetBool("Attack", false);
                }

                else { this.transform.position += Vector3.right * Time.deltaTime * speed; }
            }
            
            else if (element == Element.Normal && level == 5)////Attack of KingGuard
            {
                found = false;
                for (int i = 0; i < 3; i++)
                {
                    RaycastHit2D[] hits = Physics2D.RaycastAll(new Vector2(this.transform.position.x, GameObject.Find("Len").transform.GetChild(i).transform.position.y), Vector2.right, range, 1 << LayerMask.NameToLayer("EUnit"));
                    if (hits.Length < 1) continue;
                    this.gameObject.transform.GetChild(0).gameObject.SetActive(true);///effectof KING on
                    foreach (RaycastHit2D inhit in hits)
                    {
                        anim.SetBool("Attack", true);
                        if (!found)
                             Attack(inhit, found);
                        else Attack(inhit);
                    }
                }
                if (!found)//walk
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


            hit = Physics2D.Raycast(this.transform.position, Vector2.right, 1f, 1 << LayerMask.NameToLayer("Ewall"));
            if (hit.collider != null)
            {
                StageController.Instance.slimeWall.hp = (StageController.Instance.slimeWall.def >= this.atp) ? StageController.Instance.slimeWall.hp - 1 : StageController.Instance.slimeWall.hp - (this.atp - StageController.Instance.slimeWall.def);
                End();
            }

        }///End While
         ///Move To Camp of enemy(infont of enemy)
        this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector2(finalPosition, this.transform.position.y), this.speed * Time.deltaTime);
        End();

        yield return null;
        // gameObject.SetActive(false);
        // Destroy(gameObject);        
    }
    private void checkLevel(int newlevel)
    {
        foreach (SlimeUnit slimeUnit in slimeUnits)
        {
            if (slimeUnit.level == newlevel)
            {
                element = slimeUnit.element;
                maxHp = curHp = slimeUnit.maxHp;
                atp = slimeUnit.atp;
                def = slimeUnit.def;
                speed = slimeUnit.speed;
                attackSpeed = slimeUnit.attackspeed;
                range = slimeUnit.range;
                attackSpeed = 1.5f - (attackSpeed * 0.12f);
            }

        }
        if (level >= 3 && element == Element.Grass)
            StartCoroutine("GrassHealing");


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
                        g.GetComponent<Unit>().StartCoroutine("AnimationHealing");
                        g.GetComponent<Unit>().curHp = (g.GetComponent<Unit>().curHp < g.GetComponent<Unit>().maxHp) ? g.GetComponent<Unit>().curHp + 1 : g.GetComponent<Unit>().curHp;


                    }
                }
            }


            yield return new WaitForSeconds(3f);
        }
    }
    IEnumerator AnimationHealing()
    {
        StopCoroutine("BeAttacked");
        beattack = false;
        sprite.color = new Color(0.4f, 1f, 0.4f, 0.9f);
        yield return new WaitForSeconds(2f);

        sprite.color = new Color(1f, 1, 1f, 0.9f);


    }
    // Update is called once per frame



}
