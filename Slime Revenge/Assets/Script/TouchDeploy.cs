using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class TouchDeploy : MonoBehaviour {
  
    private bool clickCata=false;
    public int[] Pathline=new int[3];
    private static TouchDeploy Instances;
    private Animator myani;
    private GameObject SlimeDen;
    public bool ControlOn=true;
    public static TouchDeploy Instance() { return (Instances); }
    public List<GameObject>[] Actives=new List<GameObject>[6];
    public List<GameObject>[] DisActives=new List<GameObject>[6];
    public GameObject Wait1;
    public GameObject Wait2;
    private Cameramove cam;
    public SlimeScriptableObject slimeScritableObject;
    public GameObject Wait3;
    private GameObject SliemtyPE;
    private RaycastHit2D hit;
    ///change Input getbutton=>touch 
	// Use this for initialization
    void Awake()
    {
        Instances = this;
       
    }
    void Start()
    {
        cam = Camera.main.GetComponent<Cameramove>();
        ControlOn = true;
        myani = this.transform.GetChild(0).GetComponent<Animator>();
        SlimeDen = GameObject.Find("SlimeDen");
        for (int i = 0; i < 6; i++)
        {
            Actives[i] = new List<GameObject>();
            DisActives[i] = new List<GameObject>();
        }
        Wait1 = GameObject.Find("wait1");
        Wait2 = GameObject.Find("wait2");
        Wait3 = GameObject.Find("wait3");
        SliemtyPE = GameObject.Find("ElementSlime");
        RandomElement();
	}
    public void RandomElement()
    {
            int x = Random.Range(1, 6);
            int y = Random.Range(1, 6);
            int z = Random.Range(1, 6);

            Pathline[0] = x;
            while (y == x)
            {
              y = Random.Range(1, 6);
            }
            Pathline[1] = y;
            while (z == x || z == y)
            {
                z = Random.Range(1, 6);
            }
            Pathline[2] = z;
            WaitingQueAnimation();
    }
    private void WaitingQueAnimation()
    {
        Wait1.GetComponent<Animator>().SetInteger("Type", Pathline[0]);
        Wait2.GetComponent<Animator>().SetInteger("Type", Pathline[1]);
        Wait3.GetComponent<Animator>().SetInteger("Type", Pathline[2]);

    }

	// Update is called once per frame
    void Update()
    {
       /* Touch[] t = Input.touches;
        if(t.Length==1)
        if (!clickCata && t[0].phase.Equals(TouchPhase.Began))
        {
             hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(t[0].position),Vector2.zero,1f,1<<LayerMask.NameToLayer("Base")) ;
             if (hit.collider != null && hit.transform.name == "Deploy") ///Raycast on layer base name deploy(KIng slime len)
             {
                 Camera.main.GetComponent<Cameramove>().MoveNormal = false;
                 clickCata = true;
                 Camera.main.transform.position = new Vector3(0f, Camera.main.transform.position.y, Camera.main.transform.position.z);
                // Debug.Log(Input.mousePosition.x + "" + hit.transform.name);
             }
             else if(hit.collider != null)///Raycast hit on layer base other (Reroll)
             {
                 RandomElement();
             }
        }
        if (t.Length == 1)
        if (clickCata && t[0].phase.Equals(TouchPhase.Ended))
        {
            Camera.main.GetComponent<Cameramove>().MoveNormal = true;
            clickCata = false;
              hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint( t[0].position), Vector2.zero, 1f, 1 << LayerMask.NameToLayer("Len"));
            if (hit.collider != null)
            {
                if(hit.transform.tag=="Len")
                {

                    Vector2 Position = new Vector2(Camera.main.ScreenToWorldPoint(t[0].position).x, hit.transform.position.y);
                    Element x;
                    switch (Pathline[0])
                    {
                        case (1): x = Element.Fire; break;
                        case (2): x = Element.Water; break;
                        case (3): x = Element.Grass; break;
                        case (4): x = Element.Electric; break;
                        case (5): x = Element.Soil; break;
                        default: x = Element.Normal; break;
                    }
                    int reorder = 0;
                    reorder = Pathline[0];
                    Pathline[0] = Pathline[1];
                    Pathline[1] = Pathline[2];
                    Pathline[2] = reorder;

                    WaitingQueAnimation();
                    GameObject mygameobject;
                 //   x = Element.Electric; 
                    //Debug.Log(SliemtyPE);
                    if (DisActives[reorder].Count < 1)
                    {
                       mygameobject = Instantiate<GameObject>(SliemtyPE.transform.FindChild(x.ToString()).gameObject);
                      
                    }
                    else
                    {
                        mygameobject = DisActives[reorder][0];
                        DisActives[reorder].RemoveAt(0);
                    } 
                    Actives[reorder].Add(mygameobject);
                    mygameobject.transform.position = Position;
                    StartCoroutine(trowing(mygameobject, x));
//                    mygameobject.transform.position = this.transform.position;

    //                mygameobject.AddComponent<Unit>().Set(x); 
                }

//                Debug.Log(hit.transform.name);
            }


        }*/
       // Vector2 Mposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (ControlOn )
        {
            if (!clickCata && Input.GetMouseButtonDown(0))
            {
                hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 1f, 1 << LayerMask.NameToLayer("Base"));
                if (hit.collider != null && hit.transform.name == "Deploy") ///Raycast on layer base name deploy(KIng slime len)
                {
                   cam.moveNormal = false;
                    clickCata = true;
                    Camera.main.transform.position = new Vector3(cam.minPos, Camera.main.transform.position.y, Camera.main.transform.position.z);
                    // Debug.Log(Input.mousePosition.x + "" + hit.transform.name);
                }
           
            }
            if (clickCata && Input.GetMouseButtonUp(0))
            {
                Camera.main.GetComponent<Cameramove>().moveNormal = true;
                clickCata = false;
                hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 1f, 1 << LayerMask.NameToLayer("Len"));
                if (hit.collider != null)
                {
                    if (hit.transform.tag == "Len")
                    {

                        Vector2 Position = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, hit.transform.position.y);
                        Element myelement;
                        myelement=(Element)Pathline[0]; 
                     
                        int reorder = 0;
                        reorder = Pathline[0];
                        Pathline[0] = Pathline[1];
                        Pathline[1] = Pathline[2];
                        Pathline[2] = reorder;


                        WaitingQueAnimation();
                        GameObject mygameobject;
                        //   x = Element.Electric; 
                        //  Debug.Log(reorder);
                        if (DisActives[reorder].Count < 1)
                        {
                            List<SlimeUnit> slimeGroup=new List<SlimeUnit>();
                            SlimeUnit sUse=new SlimeUnit();
                            foreach (SlimeUnit s in slimeScritableObject.list)
                            {
                                if (s.element == myelement)
                                { 
                                    slimeGroup.Add(s);
                                    if(s.level==1)sUse=s;
                                }
                            }
                            mygameobject = Instantiate<GameObject>( sUse.gameObject);
                            mygameobject.transform.parent = SlimeDen.transform;
                            mygameobject.AddComponent<Unit>().Set(sUse, slimeGroup);
                            mygameobject.layer = LayerMask.NameToLayer(myelement.ToString());
                        }
                        else
                        {
                            mygameobject = DisActives[reorder][0];
                            DisActives[reorder].RemoveAt(0);
                            foreach (SlimeUnit s in mygameobject.GetComponent<Unit>().slimeUnits)
                            {
                                if (s.level==1)
                                {
                                    mygameobject.GetComponent<Unit>().Set(s);

                                    mygameobject.layer = LayerMask.NameToLayer(myelement.ToString());
                                }
                            }
                        }
                        Actives[reorder].Add(mygameobject);
                        mygameobject.transform.position = Position;

                        StartCoroutine(trowing(mygameobject, myelement));
                        //                    mygameobject.transform.position = this.transform.position;

                        //                  mygameobject.SetActive(true);
                        //                mygameobject.AddComponent<Unit>().Set(x); 
                    }

                    //                Debug.Log(hit.transform.name);
                }


            }
        }
        /*
        Touch[] T = Input.touches;
        for (int i = 0; i < T.Length; i++)
        {
            Debug.Log(T[i].position);
            this.transform.GetChild(0).GetComponent<Text>().text = T[i].position.ToString();

        }*/
	}
    IEnumerator KingAnimation()
    {

        myani.SetBool("Sent", true);

        yield return new WaitForSeconds(0.25f);

        myani.SetBool("Sent", false);
    }
    IEnumerator trowing(GameObject para,Element x)
    {
     //   Tui = true;
        Vector3 lastpo = para.transform.position;
        LayerMask ley = para.layer;
        para.layer = LayerMask.NameToLayer("Invis");
        para.transform.position = this.transform.position;
    
        //   StopCoroutine("KingAnimation");
        myani.SetBool("Sent", true);
        Debug.Log("S="+(lastpo.x - this.transform.position.x));
        float nv=((( lastpo.x - this.transform.position.x)/15.365f)/0.46f);
        para.GetComponent<Animator>().speed =(nv>1f)? 1f-nv:1f+nv;
        if (para.GetComponent<Animator>().speed == 0)
        {

            para.GetComponent<Animator>().speed = 1f;
        }
        Debug.Log(para.GetComponent<Animator>().speed);
        //   yield return new WaitForSeconds(0.1f);
        para.SetActive(true);

        float T = Time.time;
        while (para.transform.position!=lastpo)
        {
          para.transform.position = Vector2.MoveTowards(para.transform.position, lastpo, 16f * Time.deltaTime);
            yield return null;
        }
        Debug.Log("T="+(Time.time-T));
        para.transform.position = lastpo;
        para.GetComponent<Animator>().SetInteger("State", 1);

        para.GetComponent<Animator>().speed = 1f;
        myani.SetBool("Sent", false);
        para.layer = ley;
        para.GetComponent<Unit>().StartWalk();
   //    Tui = false;
       // yield return new WaitForSeconds(1f);
       // ontrow = false;
    }
}
