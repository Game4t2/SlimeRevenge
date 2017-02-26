using UnityEngine;
using System.Collections;
using UnityEngine.Sprites;
public class Bullet : MonoBehaviour {
 public Sprite[] Sp=new Sprite[5];
 public int elementvalue;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {	
	}

    public Sprite Getsprite(Element e)
    {
        switch (e)
        {
            case (Element.Electric): elementvalue = 3; return Sp[3];
            case (Element.Fire): elementvalue = 0; return Sp[0];
            case (Element.Grass): elementvalue =2; return Sp[2];
            case (Element.Soil): elementvalue = 4; return Sp[4];
            case (Element.Water): elementvalue =1; return Sp[1];
        
        }return Sp[0];

    }
}
