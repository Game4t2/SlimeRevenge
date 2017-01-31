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
    private bool killed = false;
    private bool dead;
    private bool canonEmpty = true;
    public bool hited = false;
    private GameObject bullet;
    private SpriteRenderer sprite;
    public float maxHp;
    public float currentHp;
    public float def;
    public float atp;
    public float range;
    public float speed;
    public float attackSpeed;
    private float finalPosition;
    public int level;
    private bool beattack = false;
    // public TouchDeploy Mycontrol;

    public Element element;

    public void StartWalk()
    {
        StartCoroutine("walk");
    }
    void Awake()
    {
        sprite = this.GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

    }
    void start()
    {
        StageController.Instance.onGamePaused += _OnPaused;
        StageController.Instance.onGameUnpaused += _OnUnpaused;
    }

    private void OnEnable()
    {
        currentHp = maxHp;
    }

    private void _OnPaused()
    {
        anim.enabled = false;
    }

    private void _OnUnpaused()
    {
        anim.enabled = true;
    }

    private void _Die()
    {
        if (!dead)
        {
            dead = true;

            if (this.element == Element.Normal && level >= 5)
            {
                SkillUse.Instance.HeroOnStage = false;
            }
            StopAllCoroutines();
            StartCoroutine("OnDyingCoroutine");
        }
    }

    IEnumerator OnDyingCoroutine()
    {
        yield return null;

        anim.SetBool("Attack", false);
        anim.SetBool("Death", true);
        if (killed)
            yield return new WaitForSeconds(0.7f);
        else yield return null;
        sprite.color = Color.white;
        beattack = false;
        anim.SetBool("Death", false);
        anim.SetBool("Attack", false);
        gameObject.SetActive(false);
        anim.SetInteger("State", 0);
        if (killed)
            ChargeBar.Instance.Increseing();
        killed = false;
    }


    public void Attacked(float attack, WinLose enemyWinLose, bool canonShot = false)
    {
        if (canonShot)
            StartCoroutine("BecanonShot");
        if (!beattack)
        {
            beattack = true;
            StartCoroutine("AnimationAttacked");
        }
        float damage = attack;
        if (enemyWinLose == WinLose.win) { damage *= 2; }
        else if ((enemyWinLose == WinLose.lose)) { damage = damage / 2; }

        damage = damage < def ? 0 : damage - def;
        this.currentHp = this.currentHp - damage;
        if (currentHp <= 0)
        {
            killed = true;
            _Die();
        }

    }

    public float GetTotalAttack()
    {
        float totalAttack = atp;
        List<FieldEffect> list = FieldEffectController.GetSlimeFieldEffect();
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].targetStatus == "atp" && list[i].effect == FieldEffect.EffectType.Buff)
                totalAttack = totalAttack + (atp * list[i].effectPower);
            else if (list[i].targetStatus == "atp" && list[i].effect == FieldEffect.EffectType.Debuff)
                totalAttack = totalAttack - (atp * list[i].effectPower);
        }
        return totalAttack;
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


    public Unit GetClosestSameElementSlime()
    {
        if (this.element == Element.Normal)
            return null;
        Unit[] sameElemUnits = SlimePool.GetPool().FindAll(unit => unit.element == this.element).ToArray();
        Unit cloestUnit = null;
        for (int i = 0; i < sameElemUnits.Length; i++)
        {
            Vector3 direction = (sameElemUnits[i].transform.position - this.transform.position);
            if (direction.x > 0 && direction.y == 0 && sameElemUnits[i].transform.position.x <= this.transform.position.x + 2.5f)
            {
                cloestUnit = sameElemUnits[i];
                return cloestUnit;
            }
        }
        return cloestUnit;

    }

    public void EatToEvol(Unit otherUnit)
    {
        if (this.dead)
            return;
        //if my level is higher will absorb smaller and gain new stat
        if (level > otherUnit.level)
        {
            //level was capped at 5
            GameDatabase.Instance.SlimeDatabase.GetSlimeData(element, Mathf.Min(level + 1, 5)).CreateInstance(this);
            this.currentHp = otherUnit.currentHp + this.currentHp;
        }
        else
            _Die();
        //level won't go over 5
        level = Mathf.Min(level, 5);
        this.transform.position = otherUnit.transform.position;

        otherUnit._Die();

    }

    private void Attack(RaycastHit2D hit)
    {

        if (this.gameObject == null) return;
        else if (hit.transform.gameObject != null)
        {
            EnemyUnit Human = hit.transform.gameObject.GetComponent<EnemyUnit>();
            if (Human.transform.gameObject != null)
                Human.Attacked(this);
        }
    }


    IEnumerator walk()
    {
        RaycastHit2D hit = new RaycastHit2D();
        bool found = false;
        Unit another;
        while (true)
        {
            yield return null;
            //EatAnother
            another = GetClosestSameElementSlime();
            if (another != null)
            {
                while (another.gameObject.activeInHierarchy)///walkTotarget
                {
                    this.transform.position = Vector3.MoveTowards(this.transform.position, another.transform.position, this.speed * 3f * Time.deltaTime);
                    yield return null;
                    if (this.transform.position.x > another.transform.position.x - 0.2f) break;
                }

                EatToEvol(another);
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
                            Attack(inhit);
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
                _Die();
            }
            yield return null;
        }///End While

    }

    /*private void checkLevel(int newlevel)
    {
        foreach (Unit slimeUnit in SlimePool.GetPool())
        {
            if (slimeUnit.level == newlevel)
            {
                element = slimeUnit.element;
                maxHp = currentHp = slimeUnit.maxHp;
                atp = slimeUnit.atp;
                def = slimeUnit.def;
                speed = slimeUnit.speed;
                attackSpeed = slimeUnit.attackSpeed;
                range = slimeUnit.range;
                attackSpeed = 1.5f - (attackSpeed * 0.12f);
            }

        }
        if (level >= 3 && element == Element.Grass)
            StartCoroutine("GrassHealing");


    }*/


    IEnumerator GrassHealing()
    {
        float radius = 2f;
        while (true)
        {
            /*
            foreach (List<GameObject> li in TouchDeploy.Instance.GetPool())
            {
                foreach (GameObject g in li)
                {
                    if (g != this.gameObject && Mathf.Abs(g.transform.position.x - this.transform.position.x) < 2f && g.transform.position.y == this.transform.position.y)
                    {
                        g.GetComponent<Unit>().StartCoroutine("Healing");
                        g.GetComponent<Unit>().currentHp = (g.GetComponent<Unit>().currentHp < g.GetComponent<Unit>().maxHp) ? g.GetComponent<Unit>().currentHp + 1 : g.GetComponent<Unit>().currentHp;
                        g.GetComponent<Unit>().StartCoroutine("AnimationHealing");
                        g.GetComponent<Unit>().curHp = (g.GetComponent<Unit>().curHp < g.GetComponent<Unit>().maxHp) ? g.GetComponent<Unit>().curHp + 1 : g.GetComponent<Unit>().curHp;


                    }
                }
            }
            */
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, radius);
            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i].gameObject != this.gameObject)
                {
                    Unit unit = cols[i].GetComponent<Unit>();
                    unit.StartCoroutine("Healing");
                    unit.currentHp = (unit.currentHp < unit.maxHp) ? unit.currentHp + 1 : unit.currentHp;
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



}
