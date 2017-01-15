using UnityEngine;
using System.Collections;

using System.Collections.Generic;
public class SkillUse : MonoBehaviour
{
    public static SkillUse Instance { get { return instance; } }
    private static SkillUse instance;
    private RaycastHit2D hit;
    private bool ClickedType = false;
    private GameObject SlimesT;
    private GameObject ClickedButton;
    private int Type;
    // private float cooldown=3f;

    public bool HeroOnStage = false;
    private float deployPoint = -3;
    private string typename;
    public List<GameObject> inactive;
    public List<GameObject> active;
    // Use this for initialization
    void Awake()
    {
        instance = this;

    }
    void Start()
    {

        SlimesT = GameObject.Find("SlimeType");
    }

    // Update is called once per frame
    void Update()
    {
        if (ChargeBar.Instance.Isfull() && !HeroOnStage)
        {
            HeroOnStage = true;
            ChargeBar.Instance.Reset();
            Type = 5;
            DeployType(5, 2, 2);
        }


        if (inactive == null)
        {

            //DisActives = TouchDeploy.Instance().inactive[0];
            //Actives = TouchDeploy.Instance().active[0];
        }
        if (Input.GetMouseButtonDown(0) && TouchDeploy.Instance.controlOn && TearDrop.Instance.teardrop >= 5)
        {
            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 1f, 1 << LayerMask.NameToLayer("Button"));
            if (hit.collider != null)
            {

                if (hit.transform.GetComponent<SkillID>().IsType && !hit.transform.GetComponent<SkillID>().BecoolDown)
                {
                    TearDrop.Instance.teardrop -= 5;
                    Type = hit.transform.GetComponent<SkillID>().ID;
                    ClickedType = true;
                    hit.transform.GetComponent<SpriteRenderer>().color = Color.red;
                    ClickedButton = hit.transform.gameObject;
                    hit.transform.GetComponent<SkillID>().StartcoolDown();
                    DeployType(Type);
                }
            }




        }
    }
    private void CreateUnit(Vector2 Position)
    {

        GameObject mygameObject = Instantiate<GameObject>(SlimesT.transform.FindChild(typename).gameObject);
        active.Add(mygameObject);
        mygameObject.transform.position = Position;
        mygameObject.SetActive(true);
        //  mygameObject.AddComponent<Unit>().Set(Element.Normal, Type);


    }
    private void DeployType(int myType, int startLen = 1, int EndLen = 3)
    {
        switch (myType)
        {
            case (1): typename = "Sword"; break;
            case (2): typename = "Pike"; break;
            case (3): typename = "Shield"; break;
            case (4): typename = "Cannon"; break;
            case (5): typename = "KingGuard"; break;
        }
        if (inactive.Count < 1)
        {
            for (int i = startLen; i <= EndLen; i++)
            {
                CreateUnit(new Vector2(deployPoint, GameObject.Find("L" + i.ToString()).transform.position.y));

            }
            ClickedType = false;
        }
        else
        {
            int i = startLen; GameObject mygameObject;
            for (int j = 0; j < inactive.Count; j++)
            {
                if (inactive[j].GetComponent<SkillID>().ID == this.Type)
                {
                    mygameObject = inactive[j];
                    inactive.RemoveAt(j);
                    active.Add(mygameObject);
                    mygameObject.transform.position = new Vector2(deployPoint, GameObject.Find("L" + i.ToString()).transform.position.y);
                    mygameObject.SetActive(true);
                    //     mygameObject.GetComponent<Unit>().Set(Element.Normal, Type);
                    i++;
                    if (i > EndLen) break;
                }
            }
            while (i <= EndLen)
            {

                CreateUnit(new Vector2(deployPoint, GameObject.Find("L" + i.ToString()).transform.position.y));
                i++;

            }
            ClickedType = false;
        }
    }
    /*
    private void DeployTypeOnOneLen()
    {
        switch (Type)
        {
            case (1): typename = "Sword"; break;
            case (2): typename = "Pike"; break;
            case (3): typename = "Shield"; break;
            case (4): typename = "Cannon"; break;
        }
        GameObject mygameObject;
        if (DisActives.Count < 1)
        {
            mygameObject = Instantiate<GameObject>(SlimesT.transform.FindChild(typename).gameObject);
            Actives.Add(mygameObject);
            mygameObject.transform.position = new Vector2(deployPoint, hit.transform.position.y);
            mygameObject.SetActive(true);
            mygameObject.AddComponent<Unit>().Set(Element.Normal, Type);
            ClickedType = false;
        }
        else
        {
            for (int i = 0; i < DisActives.Count; i++)
            {
                if (DisActives[i].GetComponent<SkillID>().ID == this.Type)
                {
                    mygameObject = DisActives[i];
                    DisActives.RemoveAt(i);
                    Actives.Add(mygameObject);
                    mygameObject.transform.position = new Vector2(deployPoint, hit.transform.position.y);
                    mygameObject.SetActive(true);
                    mygameObject.GetComponent<Unit>().Set(Element.Normal, Type);

                    ClickedType = false; break;
                }
            }
            if (ClickedType)
            {

                mygameObject = Instantiate<GameObject>(SlimesT.transform.FindChild(typename).gameObject);
                Actives.Add(mygameObject);
                mygameObject.transform.position = new Vector2(deployPoint, hit.transform.position.y);
                mygameObject.SetActive(true);
                mygameObject.AddComponent<Unit>().Set(Element.Normal, Type);

                ClickedType = false;
            }
        }
    }*/


}
