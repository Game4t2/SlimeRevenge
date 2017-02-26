using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyUnit : MonoBehaviour
{
    // public Vector3 MyPosition;
    //   public GameObject MyGameObject;
    public Animator anim;

    private bool die = false;
    public float maxHp;
    public float currentHp;
    public GameObject blood;
    public float def;
    public float atp;
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

    public EnemyUnitType type;
    public Element element;
    //public bool Onstage; use for object pooling **** if have to optimize*****


    void Awake()
    {
        sprite = this.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        blood = this.transform.FindChild("Canvas").GetChild(0).GetChild(0).gameObject;
        anim = GetComponent<Animator>();
        anim.SetInteger("Level", this.level - 1);
        WalKToTarget();
        StageController.Instance.onGamePaused += _OnPaused;
        StageController.Instance.onGameUnpaused += _OnUnpaused;
    }

    //this will be called by pool
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

    private void OnDying()
    {
        die = true;
        gameObject.SetActive(false);
    }

    public void WalKToTarget(Transform t = null)
    {
        if (t == null)
            t = FindObjectOfType<Wall>().transform;
        if (t == null)
        {
            Debug.LogError("No walking target or wall in this scene");
            return;
        }
        finalPosition = t.position.x;
        StartCoroutine("Walk");
    }

    public float GetTotalAttack()
    {
        float totalAttack = atp;
        FieldEffect[] list = FieldEffectController.GetEnemyFieldEffect().ToArray();
        for (int i = 0; i < list.Length; i++)
        {
            if (list[i].targetStatus == "atp" && list[i].effect == FieldEffect.EffectType.Buff)
                totalAttack = totalAttack + (atp * list[i].effectPower);
            else if (list[i].targetStatus == "atp" && list[i].effect == FieldEffect.EffectType.Debuff)
                totalAttack = totalAttack - (atp * list[i].effectPower);
        }
        return atp;
    }

    public void TakeDamage(float dmg)
    {
        this.currentHp = this.currentHp - ((dmg - this.def < 0) ? 0 : dmg - this.def);
    }

    public void Attacked(Unit slime)
    {
        if (!die)
        {
            if (!beattack)
                StartCoroutine("BeAttacked");
            float damage = slime.GetTotalAttack();
            if (Global.ElementalWeakness(element, slime.element) == WinLose.lose)
                damage /= 2;
            if (slime.level > 2)
            {
                switch (slime.element)
                {
                    case (Element.Water):
                        this.currentHp = this.currentHp - damage;
                        if (this.currentHp <= 0)
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
                            fireCurse = true;
                            burnlevel = slime.level;
                            StartCoroutine("Burning");
                        }
                        break;
                    default: break;
                }
                TakeDamage(damage);
            }
            else
                TakeDamage(damage);
            if (this.currentHp <= 0)
            {
                die = true;
                TearDrop.Instance.incresing();
                this.gameObject.layer = 0;
                StopAllCoroutines();
                StopCoroutine("walk");

                anim.SetBool("Die", true);
                StartCoroutine("Die");

            }
            blood.transform.localPosition = blood.transform.localPosition - new Vector3((this.maxHp - this.currentHp) * 2f / this.maxHp, 0f, 0f);
        }

    }

    public int CheckHitType(out RaycastHit2D hit)
    {
        Vector2 directionRay = Vector2.left;
        Vector2 source = this.transform.position;
        if (type == EnemyUnitType.Gunner)
        {
            directionRay = Vector2.down;
            source = this.transform.FindChild("Bullet").gameObject.transform.position;

        }

        hit = Physics2D.Raycast(source, directionRay, range, 1 << LayerMask.NameToLayer("Soil"));
        if (hit.collider != null)
        {
            return 1;///hit soid
        }
        else
        {
            hit = Physics2D.Raycast(source, directionRay, range, 1 << LayerMask.NameToLayer("Water") | 1 << LayerMask.NameToLayer("Fire") | 1 << LayerMask.NameToLayer("Electric") | 1 << LayerMask.NameToLayer("Grass") | 1 << LayerMask.NameToLayer("Normal"));
            if (hit.collider != null)
            {
                if (type == EnemyUnitType.Pike || type == EnemyUnitType.Mualer || type == EnemyUnitType.Sword && hit.transform.gameObject.layer == LayerMask.NameToLayer("Electric"))
                {
                    StartCoroutine("Shock");
                }
                return 2;///hit normal slime
            }
            else
            {
                Physics2D.Raycast(source, directionRay, range, 1 << LayerMask.NameToLayer("Wall"));
                if (hit.collider != null)
                {
                    return 3;///hit normal wall
                }
            }
        }
        return 0;
    }
    public bool Aim()
    {
        return Aim(this.transform.position);
    }
    public bool Aim(Vector2 source)
    {
        RaycastHit2D hit = Physics2D.Raycast(source, Vector2.left, range, 1 << LayerMask.NameToLayer("Soil") | 1 << LayerMask.NameToLayer("Water") | 1 << LayerMask.NameToLayer("Fire") | 1 << LayerMask.NameToLayer("Electric") | 1 << LayerMask.NameToLayer("Grass") | 1 << LayerMask.NameToLayer("Normal"));

        if (hit.collider != null)
        {
            return true;
        }
        return false;
    }

    public void Attack(RaycastHit2D hit, bool cannonshot = false)
    {
        if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Wall"))
        {
            if (this.gameObject == null) return;
            else if (hit.transform.gameObject != null)
            {
                Unit target = hit.transform.gameObject.GetComponent<Unit>();
                target.Attacked(GetTotalAttack(), Global.ElementalWeakness(target.element, element), cannonshot);
            }
        }
        else
        {

            StageController.Instance.slimeWall.hp = ((GetTotalAttack() / 3 - StageController.Instance.slimeWall.def) <= 0) ? StageController.Instance.slimeWall.hp - 1 : StageController.Instance.slimeWall.hp - (GetTotalAttack() / 3 - StageController.Instance.slimeWall.def);
            if (Mathf.Abs(hit.transform.position.x - this.transform.position.x) < 1f)
            {
                Destroy(gameObject);

            }
        }
    }

    public bool Attack()
    {
        RaycastHit2D hit = new RaycastHit2D();

        int hittype = CheckHitType(out hit); ////0=not hit;1=hit soid ;2=hit other slime ;3=hit wall 
        if (hittype > 0)
        {
            if (hittype <= 2)
            {

                anim.SetBool("Attack", true);
                if (this.gameObject == null) return false;
                else if (hit.transform.gameObject != null)
                {
                    Unit target = hit.transform.gameObject.GetComponent<Unit>();
                    target.Attacked(GetTotalAttack(), Global.ElementalWeakness(target.element, element));
                }
                return true;
            }
            else
            {
                anim.SetBool("Attack", true);

                StageController.Instance.slimeWall.Attacked(GetTotalAttack());
                if (type != EnemyUnitType.Gunner)
                {
                    Destroy(gameObject);

                }
                return true;
            }
        }
        return false;
    }
    public void multipleAttack() { }


    IEnumerator Walk()
    {
        RaycastHit2D hit = new RaycastHit2D();
        Unit target = null;
        bool mageFoundSlime = false;
        while (this.transform.position.x > finalPosition && !die)
        {
            yield return null;
            if (!electricCurse)
            {

                if (type == EnemyUnitType.Gunner)
                {
                    if (Aim())
                    {
                        GameObject bullet = this.transform.FindChild("Bullet").gameObject;
                        bullet.transform.position = this.transform.position;
                        anim.SetBool("Attack", true);

                        bullet.SetActive(true);

                        while (bullet.transform.position.x > -1f)
                        {
                            yield return null;
                            bullet.transform.position += Vector3.left * 10f * Time.deltaTime;

                            Attack();
                        }
                        bullet.SetActive(false);
                        yield return new WaitForSeconds(attackspeed);
                        if (this.gameObject == null) break;
                    }
                    else
                    {
                        this.transform.position += Vector3.left * Time.deltaTime * speed;
                    }
                }

                else if (type == EnemyUnitType.Mage)
                {
                    mageFoundSlime = false;

                    for (int i = 0; i < 3; i++)
                    {
                        if (Aim(new Vector2(this.transform.position.x, GameObject.Find("Len").transform.GetChild(i).transform.position.y)))
                        {

                            anim.SetBool("Attack", true);
                            this.gameObject.transform.FindChild("Power").gameObject.SetActive(true);
                            mageFoundSlime = true;
                            break;
                        }
                    }
                    if (mageFoundSlime)
                    {
                        RaycastHit2D[] hits = Physics2D.RaycastAll(new Vector2(this.transform.position.x - 1f, this.transform.position.y - 3f), Vector2.up, 6f, 1 << LayerMask.NameToLayer("Soil") | 1 << LayerMask.NameToLayer("Wall") | 1 << LayerMask.NameToLayer("Water") | 1 << LayerMask.NameToLayer("Fire") | 1 << LayerMask.NameToLayer("Electric") | 1 << LayerMask.NameToLayer("Grass") | 1 << LayerMask.NameToLayer("Normal"));

                        foreach (RaycastHit2D inhits in hits)
                        {
                            if (inhits.collider != null)
                                Attack(inhits);
                        }

                        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
                        anim.SetBool("Attack", false);
                        this.gameObject.transform.FindChild("Power").gameObject.SetActive(false);
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
                else if (type == EnemyUnitType.Cannon)
                {
                    GameObject bullet = this.transform.FindChild("Bullet").gameObject;
                    RaycastHit2D[] hits;
                    if (this.transform.position.x >= 14f)
                        this.transform.position += Vector3.left * Time.deltaTime * speed;
                    else
                    {
                        bullet.transform.position = this.transform.position;
                        anim.SetBool("Attack", true);

                        bullet.SetActive(true);
                        while (bullet.transform.position.x > -1f)
                        {
                            yield return null;
                            bullet.transform.position += Vector3.left * 10f * Time.deltaTime;
                            hits = Physics2D.RaycastAll(bullet.transform.position, Vector2.zero, 5f, 1 << LayerMask.NameToLayer("Soil") | 1 << LayerMask.NameToLayer("Wall") | 1 << LayerMask.NameToLayer("Water") | 1 << LayerMask.NameToLayer("Fire") | 1 << LayerMask.NameToLayer("Electric") | 1 << LayerMask.NameToLayer("Grass") | 1 << LayerMask.NameToLayer("Normal"));
                            foreach (RaycastHit2D inhits in hits)
                            {
                                if (inhits.collider != null)
                                    Attack(inhits, true);
                            }
                        }
                        bullet.SetActive(false);
                        yield return new WaitForSeconds(attackspeed);


                        if (this.gameObject == null) break;
                    }
                }
                else
                {
                    if (Attack()) yield return new WaitForSeconds(attackspeed);
                    else
                    {
                        if (this.gameObject == null) break;
                        this.transform.position += Vector3.left * Time.deltaTime * speed;
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

    #region Status Effect

    IEnumerator Die()
    {

        float tt = anim.GetCurrentAnimatorStateInfo(0).length;
        Debug.Log(tt + "aass");
        yield return new WaitForSeconds(tt + 0.1f);
        OnDying();
    }

    IEnumerator BeAttacked()
    {
        Color myColor = sprite.color;
        beattack = true;
        for (int i = 0; i < 5; i++)
        {
            sprite.color = Color.red;
            yield return new WaitForSeconds(0.15f);

            sprite.color = myColor;

            yield return new WaitForSeconds(0.15f);
        }
        beattack = false;
    }
    IEnumerator Shock()
    {
        electricCurse = true;
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
            this.currentHp = this.currentHp - burnlevel;
            if (this.currentHp <= 0)
            {
                TearDrop.Instance.incresing(); Destroy(gameObject);
            }
            yield return new WaitForSeconds(1f);

        }
        fireCurse = false; burnlevel = 0;
    }


    #endregion

}
