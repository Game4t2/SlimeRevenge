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

    public void Attacked(Unit slime)
    {
        if (!die)
        {
            if (!beattack)
                StartCoroutine("BeAttacked");
            int Damage = (Global.ElementalWeakness(element, slime.element) == WinLose.win) ? slime.atp / 2 : slime.atp;
            Damage = (Global.ElementalWeakness(element, slime.element) == WinLose.lose) ? slime.atp * 2 : slime.atp;
            if (slime.level > 2)
            {
                switch (slime.element)
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

    IEnumerator Walk()
    {
        RaycastHit2D Hit = new RaycastHit2D();
        Unit target = null;
        bool mageFoundSlime = false;
        while (this.transform.position.x > finalPosition && !die)
        {
            yield return null;
            if (!electricCurse)
            {



                if (type != EnemyUnitType.Mage && type != EnemyUnitType.Cannon && type != EnemyUnitType.Gunner)
                {

                    Hit = Physics2D.Raycast(this.transform.position, Vector2.left, range, 1 << LayerMask.NameToLayer("Soil"));
                    if (Hit.collider != null)
                    {
                        anim.SetBool("Attack", true);
                        if (this.gameObject == null) break;
                        else if (Hit.transform.gameObject != null)
                        {
                            target = Hit.transform.gameObject.GetComponent<Unit>();
                            target.Attacked(atp, Global.ElementalWeakness(target.element, element));

                        }
                        yield return new WaitForSeconds(attackspeed);

                    }
                    else
                    {
                        Hit = Physics2D.Raycast(this.transform.position, Vector2.left, range, 1 << LayerMask.NameToLayer("Water") | 1 << LayerMask.NameToLayer("Fire") | 1 << LayerMask.NameToLayer("Electric") | 1 << LayerMask.NameToLayer("Grass") | 1 << LayerMask.NameToLayer("Normal"));
                        if (Hit.collider != null)
                        {
                            anim.SetBool("Attack", true);
                            if (this.gameObject == null) break;
                            else if (Hit.transform.gameObject != null)
                            {
                                target = Hit.transform.gameObject.GetComponent<Unit>();
                                target.Attacked(atp, Global.ElementalWeakness(target.element, element));
                                if (target.element == Element.Electric && target.level >= 3)
                                {
                                    if (!electricCurse) { electricCurse = true; StartCoroutine("Shock"); }
                                    continue;
                                }
                            }
                            yield return new WaitForSeconds(attackspeed);
                        }
                        else
                        {


                            Hit = Physics2D.Raycast(this.transform.position, Vector2.left, range, 1 << LayerMask.NameToLayer("Wall"));
                            if (Hit.collider != null)
                            {

                                anim.SetBool("Attack", true);

                                StageController.Instance.slimeWall.hp = ((atp / 3 - StageController.Instance.slimeWall.def) <= 0) ? StageController.Instance.slimeWall.hp - 1 : StageController.Instance.slimeWall.hp - (atp / 3 - StageController.Instance.slimeWall.def);

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
                else if (type == EnemyUnitType.Gunner)
                {
                    GameObject Bullet = this.transform.FindChild("Bullet").gameObject;
                    Hit = Physics2D.Raycast(this.transform.position, Vector2.left, 1f, 1 << LayerMask.NameToLayer("Wall"));
                    if (Hit.collider != null)
                    {

                        anim.SetBool("Attack", true);

                        StageController.Instance.slimeWall.hp = ((atp / 3 - StageController.Instance.slimeWall.def) <= 0) ? StageController.Instance.slimeWall.hp - 1 : StageController.Instance.slimeWall.hp - (atp / 3 - StageController.Instance.slimeWall.def);

                        Destroy(gameObject);

                        //   yield return new WaitForSeconds(Attackspeed);
                    }

                    Hit = Physics2D.Raycast(this.transform.position, Vector2.left, range, 1 << LayerMask.NameToLayer("Soil") | 1 << LayerMask.NameToLayer("Water") | 1 << LayerMask.NameToLayer("Fire") | 1 << LayerMask.NameToLayer("Electric") | 1 << LayerMask.NameToLayer("Grass") | 1 << LayerMask.NameToLayer("Normal"));

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
                                target = Hit.transform.gameObject.GetComponent<Unit>();
                                Bullet.SetActive(false);

                                target.Attacked(atp, Global.ElementalWeakness(target.element, element));
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

                else if (type == EnemyUnitType.Mage)
                {
                    mageFoundSlime = false;
                    Hit = Physics2D.Raycast(this.transform.position, Vector2.left, range, 1 << LayerMask.NameToLayer("Wall"));
                    if (Hit.collider != null)
                    {
                        anim.SetBool("Attack", true);
                        StageController.Instance.slimeWall.hp = ((atp * 3 - StageController.Instance.slimeWall.def) <= 0) ? StageController.Instance.slimeWall.hp - 3 : StageController.Instance.slimeWall.hp - (atp * 3 - StageController.Instance.slimeWall.def);
                        Destroy(gameObject);
                    }
                    Hit = Physics2D.Raycast(this.transform.position, Vector2.left, 1f, 1 << LayerMask.NameToLayer("Soil") | 1 << LayerMask.NameToLayer("Water") | 1 << LayerMask.NameToLayer("Fire") | 1 << LayerMask.NameToLayer("Electric") | 1 << LayerMask.NameToLayer("Grass") | 1 << LayerMask.NameToLayer("Normal"));
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
                                target = Hits[i].transform.gameObject.GetComponent<Unit>();
                                target.Attacked(atp, Global.ElementalWeakness(target.element, element));

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
                else
                {
                    GameObject Bullet = this.transform.FindChild("Bullet").gameObject;
                    RaycastHit2D[] Hits;
                    if (this.transform.position.x >= 14f)
                        this.transform.position += Vector3.left * Time.deltaTime * speed;
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
                                StageController.Instance.slimeWall.hp = (((atp / 3) - StageController.Instance.slimeWall.def) <= 0) ? StageController.Instance.slimeWall.hp - 3 : StageController.Instance.slimeWall.hp - ((atp / 3) - StageController.Instance.slimeWall.def);
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
                                    target = Hits[i].transform.gameObject.GetComponent<Unit>();

                                    if (!target.hited)
                                    {
                                        target.Attacked(atp, Global.ElementalWeakness(target.element, element), true);
                                        if (target.element == Element.Normal && target.level == 3)
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

    #region Status Effect

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


    #endregion

}
