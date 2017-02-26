using UnityEngine;
using System.Collections;

public class PauseMene : MonoBehaviour {
    private float lastTimeScale;
    private bool pause=false;
	// Use this for initialization
	void Start () {
       // pause = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void OpenPause()
    {
        if (!pause)
        {
            this.pause = true;
            lastTimeScale = Time.timeScale;
            EndGame.Instance.StopAll();
            Time.timeScale = 0f;
         //   Debug.Log(lastTimeScale);
            this.gameObject.SetActive(true);
        
        }
        else
        {
            
            ClosePause();
        }
    }
    public void ClosePause()
    {
        pause = false;
      //  Debug.Log(lastTimeScale);
        EndGame.Instance.ReRun();

        Time.timeScale = lastTimeScale;
        this.gameObject.SetActive(false);
    }
}
