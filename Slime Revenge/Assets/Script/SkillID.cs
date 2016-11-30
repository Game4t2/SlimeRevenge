using UnityEngine;
using System.Collections;

public class SkillID : MonoBehaviour
{
    public int ID;
    public bool IsType;
    public bool BecoolDown = false;
    private int numberofused = 0;
    private int maxused = 4;
	// Use this for initialization
    public SkillID() { ID = 0; IsType = false; }
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void set(int I,bool t)
    {
        BecoolDown = false;
        ID = I;
        IsType = t;

    }
    public void StartcoolDown()
    {
        if (!BecoolDown)
            StartCoroutine("coolodown");
    }
    public IEnumerator coolodown(){

        BecoolDown = true;
        yield return new WaitForSeconds(3f);
        if(IsType)
        numberofused++;
        if (numberofused <= maxused)
        BecoolDown = false;
    }
}
