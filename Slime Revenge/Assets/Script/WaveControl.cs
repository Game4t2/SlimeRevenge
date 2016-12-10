using UnityEngine;
using System.Collections;

public class WaveControl : MonoBehaviour {
    public static WaveControl Instance { get { return Instan; } }
    private static WaveControl Instan;
    public Vector2[] Len = new Vector2[3];
    public float timetodeploy=1f;
    public bool pause = false;

    public int ADDHP = 0;
    public int ADDamage=0;
    private int stage;
    private int limit;
    private int[] Ranslot;
    private int EndRan;
    private GameObject HumanDen;
    int rancout;
    int ranType;
    bool[] canon = { false, false, false };
    // Use this for initialization
    void Awake()
    {
        Instan = this;
    }
    void Start()
    {
        stage = ParsingData.Instnce.GetStage();
        float startPos = Ewall.Instance.transform.position.x;
        pause = false;
        HumanDen = GameObject.Find("HumanDen");
        Len[0] = new Vector2(startPos, GameObject.Find("L1").transform.position.y);
        Len[1] = new Vector2(startPos, GameObject.Find("L2").transform.position.y);
        Len[2] = new Vector2(startPos, GameObject.Find("L3").transform.position.y);
       
        StartCoroutine("Waving");
	}
    IEnumerator Waving()
    { switch (stage)
        {
            case (1): timetodeploy = 1f; limit = 5 + 7; Ranslot = new int[] {0}; EndRan = 1; break;
            case (2): timetodeploy = 1f; limit = 7 + 7; Ranslot = new int[] { 0, 1 }; EndRan = 2; break;
            case (3): timetodeploy = 1f; limit = 10 + 7; Ranslot = new int[] { 0,2 }; EndRan = 2; break;
            case (4): timetodeploy = 0.85f; limit = 15 + 7;Ranslot = new int[] { 0,1,2 }; EndRan = 3; break;
            case (5): timetodeploy = 0.85f; limit = 27; Ranslot = new int[] { 0, 1,2 }; EndRan = 3; break;
            case (6): timetodeploy = 0.85f; limit = 37; Ranslot = new int[] { 0,1,3 }; EndRan = 3; break;///LEVEL UP PLEASE
            case (7): timetodeploy = 0.65f; limit = 42; Ranslot = new int[] { 1,2,3 }; EndRan = 3;  break;
            case (8): timetodeploy = 0.65f; limit = 52;Ranslot = new int[] { 0,2,4 }; EndRan = 3; break;
            case (9): timetodeploy = 0.65f; limit = 76; Ranslot = new int[] { 0,3,4 }; EndRan = 3; break;
            case (10): timetodeploy = 0.65f; limit = 100; Ranslot = new int[] { 0, 3,4  }; EndRan = 3; break;
            case (11): timetodeploy = 0.5f; limit = 100; Ranslot = new int[] { 0,1,2,3 }; EndRan = 4; break;
            case (12): timetodeploy = 0.5f; limit = 117; Ranslot = new int[] { 0,2,3,4 }; EndRan = 4; break;
            case (13): timetodeploy = 0.5f; limit = 117; Ranslot = new int[] { 1,2,3,4 }; EndRan = 4; break;
            case (14): timetodeploy = 0.5f; limit = 150; Ranslot = new int[] { 0,1,5 }; EndRan = 3; break;
            case (15): timetodeploy = 0.5f; limit = 150; Ranslot = new int[] { 1, 2,5 }; EndRan = 3; break;
            case (16): timetodeploy = 0.5f; limit = 165; Ranslot = new int[] { 0, 4,5  }; EndRan = 3; break;///LEVEL UP PLEASE
            case (17): timetodeploy = 0.5f; limit = 175; Ranslot = new int[] { 0, 1, 4,5 }; EndRan = 4; break;
            case (18): timetodeploy = 0.5f; limit = 175; Ranslot = new int[] { 0,1, 2,3,5 }; EndRan = 4; break;
            case (19): timetodeploy = 0.5f; limit = 175; Ranslot = new int[] { 2,3,4,5 }; EndRan = 4; break;
            case (20): timetodeploy = 0.3f; limit = -1; Ranslot = new int[] { 0,1,2,3,4,5 }; EndRan = 6; break;
        }
      
     while (true)
        {
            if (!pause)
            {
                if (limit > 0 && GameObject.FindGameObjectsWithTag("Human").GetLength(0) > limit)
                {
                    yield return null; continue;
                }
                rancout = Random.Range(0, 3);//random Len
                ranType = Ranslot[ Random.Range(0, EndRan)];
             //   ranType = 1;
                if (ranType == 5 && canon[rancout]) {
                    yield return null; 
                    continue; }

                yield return new WaitForSeconds(timetodeploy);
                GameObject Human = Instantiate<GameObject>(this.transform.GetChild(ranType).gameObject);
                Human.transform.parent = HumanDen.transform;
                Human.transform.position = Len[rancout];
               
                Human.SetActive(true);
                switch (ranType)
                {
                    case (0): Human.AddComponent<EnemyUnit>().Set(Element.Normal, EnemyUnitType.Sword, 1); break;
                    case (1): Human.AddComponent<EnemyUnit>().Set(Element.Normal, EnemyUnitType.Pike, 1); break;
                    case (2): Human.AddComponent<EnemyUnit>().Set(Element.Normal, EnemyUnitType.Mualer, 1); break;
                    case (3): Human.AddComponent<EnemyUnit>().Set(Element.Normal, EnemyUnitType.Gunner, 1);  break;
                    case (4): Human.AddComponent<EnemyUnit>().Set(Element.Normal, EnemyUnitType.Mage, 1); break;
                    case (5): Human.AddComponent<EnemyUnit>().Set(Element.Normal, EnemyUnitType.Cannon, 1); canon[rancout] = true; break;
                       
                  //  default: Human.AddComponent<EnemyUnit>().Set(Element.Normal, UnitType.Sword, 1); break;
                }


           
            }
            yield return null;
        }
    }
	// Update is called once per frame
	void Update () {
	
	}


    public void StopAllWave()
    {
        pause = true;
    }
}
