using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class TouchDeploy : MonoBehaviour
{

    private bool clicked = false;
    private Element[] queuedElement = new Element[3];
    private static TouchDeploy Instances;
    [SerializeField]
    private Animator kingAnimator;
    public bool controlOn = true;
    public static TouchDeploy Instance { get { return (Instances); } }


    // public List<GameObject>[] active = new List<GameObject>[6];
    //  public List<GameObject>[] inactive = new List<GameObject>[6];
    public GameObject[] queuedSlime = new GameObject[3];
    private Cameramove cam;
    private RaycastHit2D hit;
    [SerializeField]
    private int m_poolSize;




    ///change Input getbutton=>touch 
    // Use this for initialization
    void Awake()
    {
        Instances = this;
        SlimePool.InitPool(m_poolSize);
    }
    void Start()
    {
        cam = Camera.main.GetComponent<Cameramove>();
        controlOn = true;
        RandomElementInQueue();
    }

    public void RandomElementInQueue()
    {
        queuedElement = GetUniqueElements(queuedElement.Length).ToArray();
        _WaitingQueAnimation();
    }

    private List<Element> GetUniqueElements(int length)
    {
        if (System.Enum.GetNames(typeof(Element)).Length < length)
        {
            Debug.LogError("Can't get requested array due to requested length");
            return null;
        }
        List<Element> result = new List<Element>();
        for (int i = 0; i < length; i++)
        {
            Element elem;
            do
            {
                elem = Global.GetRandomElement();
            } while (result.Contains(elem) || elem == Element.Normal);
            result.Add(elem);
        }
        return result;
    }

    private void _WaitingQueAnimation()
    {
        queuedSlime[0].GetComponent<Animator>().SetInteger("Type", (int)queuedElement[0]);
        queuedSlime[1].GetComponent<Animator>().SetInteger("Type", (int)queuedElement[1]);
        queuedSlime[2].GetComponent<Animator>().SetInteger("Type", (int)queuedElement[2]);

    }

    // Update is called once per frame
    void Update()
    {
        // Vector2 Mposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (controlOn)
        {
            if (!clicked && Input.GetMouseButtonDown(0))
            {
                hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 1f, 1 << LayerMask.NameToLayer("Base"));
                if (hit.collider != null && hit.transform == transform) ///Raycast on layer base name deploy(KIng slime len)
                {
                    cam.moveNormal = false;
                    clicked = true;
                    Camera.main.transform.position = new Vector3(cam.minPos, Camera.main.transform.position.y, Camera.main.transform.position.z);
                    // Debug.Log(Input.mousePosition.x + "" + hit.transform.name);
                }

            }
            if (clicked && Input.GetMouseButtonUp(0))
            {
                Camera.main.GetComponent<Cameramove>().moveNormal = true;
                clicked = false;
                hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 1f, 1 << LayerMask.NameToLayer("Len"));
                if (hit.collider != null)
                {
                    if (hit.transform.tag == "Len")
                    {

                        Vector2 position = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, hit.transform.position.y);
                        Element myelement;
                        myelement = (Element)queuedElement[0];

                        _ReorderQueue();

                        _WaitingQueAnimation();
                        Unit newUnit = SlimePool.PoolRequest();
                        GameDatabase.Instance.SlimeDatabase.GetSlimeData(queuedElement[0], 1).CreateInstance(newUnit);
                        foreach (SlimeUnit s in newUnit.GetComponent<Unit>().slimeUnits)
                        {
                            if (s.level == 1)
                            {
                                //newUnit.GetComponent<Unit>().Set(s);

                                newUnit.gameObject.layer = LayerMask.NameToLayer(myelement.ToString());
                            }
                        }
                        StartCoroutine(Throwing(newUnit.gameObject, myelement, position));
                    }
                }
            }
        }
    }

    private void _ReorderQueue()
    {
        Element reorder = 0;
        reorder = queuedElement[0];
        queuedElement[0] = queuedElement[1];
        queuedElement[1] = queuedElement[2];
        queuedElement[2] = reorder;
    }


    IEnumerator KingAnimation()
    {

        kingAnimator.SetBool("Sent", true);

        yield return new WaitForSeconds(0.25f);

        kingAnimator.SetBool("Sent", false);
    }


    IEnumerator Throwing(GameObject slime, Element x, Vector3 targetPosition)
    {
        LayerMask slimeLayer = slime.layer;
        slime.layer = LayerMask.NameToLayer("Invis");
        slime.transform.position = this.transform.position;

        kingAnimator.SetBool("Sent", true);
        float nv = (((targetPosition.x - this.transform.position.x) / 15.365f) / 0.46f);
        slime.GetComponent<Animator>().speed = (nv > 1f) ? 1f - nv : 1f + nv;
        if (slime.GetComponent<Animator>().speed == 0)
        {

            slime.GetComponent<Animator>().speed = 1f;
        }
        //   yield return new WaitForSeconds(0.1f);
        slime.SetActive(true);

        float t = 0f;
        float lerpTime = 1f;
        float speed = 1f;
        float projectileHeight = 3f;
        Vector3 startPos = transform.position;

        while (t < lerpTime)
        {
            t += Time.deltaTime * speed;
            float lerpValue = t / lerpTime;
            float height = Mathf.Sin(Mathf.PI * lerpValue) * projectileHeight;
            slime.transform.position = Vector3.Lerp(startPos, targetPosition, lerpValue) + Vector3.up * height;
            yield return null;
        }
        slime.GetComponent<Animator>().SetInteger("State", 1);

        slime.GetComponent<Animator>().speed = 1f;
        kingAnimator.SetBool("Sent", false);
        slime.layer = slimeLayer;
        slime.GetComponent<Unit>().StartWalk();
        //    Tui = false;
        // yield return new WaitForSeconds(1f);
        // ontrow = false;
    }


}
