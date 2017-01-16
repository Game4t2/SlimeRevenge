using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScreenRoll : MonoBehaviour
{
    private float speed=1f;
    public float sensitive=7f;
    private int maxscene = 4;
    private bool mouseTouch = false;
    private int movePattern=0;
    private Vector2 startPos;
    private Vector2 mousePos;
    private Vector2 prevMousePos;
    private Vector3[] screenPos = new Vector3[4];
    // Use this for initialization
    void Start()
    {   for (int i = 0; i < maxscene; i++)
        {
            screenPos[i] = this.transform.GetChild(i).transform.position;
        }
    }
    public void RotateScreen(bool moveRight)
    {
        
        if (moveRight)
            movePattern++;
        else
            movePattern--;

        if (movePattern > maxscene-1) movePattern = 0;
            else if (movePattern < 0) movePattern = maxscene - 1;
        ResetPos();

    }

    public void ResetPos()
    {
        for (int i = 0; i < maxscene; i++)
        {
            this.transform.GetChild(i).transform.position = screenPos[(i + movePattern) % maxscene];
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 5f);
            if (hit.collider != null && hit.transform.name == this.name)
            {
                mouseTouch = true;
                startPos = mousePos;
                prevMousePos = mousePos;
            }
        }
        if (Input.GetMouseButton(0) && mouseTouch)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            for (int i = 0; i < maxscene; i++) {
                Vector3 pos = this.transform.GetChild(i).transform.position;
                this.transform.GetChild(i).Translate((mousePos.x - prevMousePos.x)*speed,0,0);
            }
          
            prevMousePos = mousePos;
        }
        if (Input.GetMouseButtonUp(0) && mouseTouch)
        {
            if (mousePos.x > startPos.x + sensitive)
            {

                RotateScreen(true);
                startPos = mousePos;
            }
            if (mousePos.x < startPos.x - sensitive)
            {

                RotateScreen(false);
                startPos = mousePos;
            }
            ResetPos();
            mouseTouch = false;
        }
    }
}
