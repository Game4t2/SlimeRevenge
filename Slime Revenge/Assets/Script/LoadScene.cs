using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class LoadScene : MonoBehaviour {
    public static LoadScene INs { get {return In; } }
    private static LoadScene In;
	
	// Use this for initialization
	void Start () {
        In = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LoadS(bool destroy=false){
        if (!destroy||  ParsingData.Instnce==null)
        {
            SceneManager.LoadScene("Stage");
            Time.timeScale = 1f;
        }
        else
        {
            ParsingData.Instnce.DestroyMe();
            SceneManager.LoadScene("Stage");
            Time.timeScale = 1f;
        }
    }

}
