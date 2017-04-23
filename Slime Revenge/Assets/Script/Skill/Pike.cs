using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pike : MonoBehaviour {
    public GameObject minigame;
    public GameObject skillResult;
    public float minigameTime;
    private float lanePositionY;
    private int myPoint;
    private bool clicked=false;
 	// Use this for initialization
	void Start () {
        clear();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0) && !clicked)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 1f, 1 << LayerMask.NameToLayer("Len"));
            if (hit.collider != null)
            {
                if (hit.transform.tag == "Len")
                {
                    clicked = true;
                       lanePositionY = hit.transform.position.y;
                    StartCoroutine("CountDown");
                }

            }
        }
    }
    IEnumerator CountDown()
    {
        minigame.SetActive(true);
        minigame.GetComponent<Rolling>().point = 0;
        yield return new WaitForSeconds(minigameTime);
        myPoint = minigame.GetComponent<Rolling>().point;
        minigame.SetActive(false);
        skillResult.SetActive(true);
        skillResult.GetComponent<LaneAttack>().SetDamage(myPoint);
        yield return new WaitUntil(() => skillResult.GetComponent<LaneAttack>().EnemyAttacked(lanePositionY));
        clear();
        this.gameObject.SetActive(false);
    }
    private void clear()
    {
        minigame.GetComponent<Rolling>().point = 0;
        minigame.SetActive(false);
        skillResult.SetActive(false);
        myPoint = 0;
        lanePositionY = 0f; clicked = false;
    }

}
