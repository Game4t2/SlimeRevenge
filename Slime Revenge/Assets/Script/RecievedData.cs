using UnityEngine;
using System.Collections;

public class RecievedData : MonoBehaviour {
    private Color c=new Color(0.5f,0.5f,0.5f);
    private bool iswhite = false;
	// Use this for initialization
    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            ParsingData.Instnce.GetData(i, out this.transform.GetChild(i).GetComponent<SkillID>().ID, out this.transform.GetChild(i).GetComponent<SkillID>().IsType);

            this.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite= ParsingData.Instnce.Getsprite(i);
       }
    }
	
	// Update is called once per frame
	void Update () {
        
        if (TearDrop.Instance.teardrop < 5)
        {
            for (int i = 0; i < 3; i++)
            {
               
                this.transform.GetChild(i).GetComponent<SpriteRenderer>().color =c;
            }
            iswhite = false;

        }
        else if (!iswhite)
        {
            iswhite = true;

            for (int i = 0; i < 3; i++)
            this.transform.GetChild(i).GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                if (this.transform.GetChild(i).GetComponent<SkillID>().BecoolDown)
                    this.transform.GetChild(i).GetComponent<SpriteRenderer>().color = c;
                else this.transform.GetChild(i).GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
	}
}
