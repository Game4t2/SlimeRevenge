using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class TearDrop : MonoBehaviour {
    public static TearDrop Instance { get { return instance; } }
    private static TearDrop instance;
    private Text mytext;
    public int teardrop;
    void Awake()
    {
        instance = this;

    }
	// Use this for initialization
	void Start () {
        mytext = this.transform.GetChild(0).GetComponent<Text>();
        mytext.text = "0";
        teardrop = 0;
	}
	public void incresing(){
        teardrop++;

}
	// Update is called once per frame
	void Update () {
        mytext.text = teardrop.ToString();
	}
}
