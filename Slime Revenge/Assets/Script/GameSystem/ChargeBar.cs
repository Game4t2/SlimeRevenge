using UnityEngine;
using System.Collections;

public class ChargeBar : MonoBehaviour {
    private static ChargeBar CH=new ChargeBar();
    public static ChargeBar Instance { get { return CH; } set{Instance=CH;} }
    private GameObject Bar;
    private float startpoint;
    private float maxpoint;

    private float Distance;
	// Use this for initialization
    
    void Awake()
    {
        CH = this;
     
    }
    void Start()
    {
        Bar = this.transform.GetChild(0).gameObject;
        startpoint = Bar.transform.localPosition.x;
        maxpoint = this.transform.localPosition.x;
        Distance = maxpoint - startpoint;
        Distance = (Distance > 0f) ? Distance : -Distance;
        Debug.Log(maxpoint + "," + startpoint);
	}
    public void Increseing()
    {
        if (!SkillUse.Instance.HeroOnStage)
        {
            if (Bar.transform.localPosition.x + (Distance / 100f) >= maxpoint) Bar.transform.localPosition = new Vector2(maxpoint, Bar.transform.localPosition.y);
            else
                Bar.transform.localPosition = new Vector2(Bar.transform.localPosition.x + (Distance / 100f), Bar.transform.localPosition.y);
        }
    }
    public void Reset()
    {

        Bar.transform.localPosition = new Vector2(startpoint, Bar.transform.localPosition.y);
    }
    public bool Isfull()
    {
        if (Bar.transform.localPosition.x >= maxpoint) return true;
        else return false;

    }
	// Update is called once per frame
  
}
