using UnityEngine;
using System.Collections;

public class EndGame : MonoBehaviour
{
    public static EndGame Instance { get { return Ins; } }
    private static EndGame Ins;
    // Use this for initialization
    void Start()
    {
        Ins = this;
    }
    public void LoseEnd()
    {
        StopAll();
        this.transform.FindChild("YourLose").gameObject.SetActive(true);

        Time.timeScale = 0f;

    }
    public void WinEnd()
    {
        StopAll();
        this.transform.FindChild("Win").gameObject.SetActive(true);

        Time.timeScale = 0f;

    }
    public void StopAll()
    {
        Cameramove.Instanc.stopMove = true;
        TouchDeploy.Instance.controlOn = false;
        WaveControl.Instance.pause = true;

    }
    public void ReRun()
    {
        Cameramove.Instanc.stopMove = false;
        TouchDeploy.Instance.controlOn = true;
        WaveControl.Instance.pause = false;

    }
    // Update is called once per frame
    void Update()
    {

    }
}
