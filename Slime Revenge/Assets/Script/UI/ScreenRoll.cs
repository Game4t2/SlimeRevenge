using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScreenRoll : MonoBehaviour
{
    private float speed=1f;
    private bool mouseTouch = false;
    private int movePattern=0;
    public Vector2 startPos;
    private Vector2 mousePos;
    private Vector2 prevMousePos;
    private Vector3[] screenPos = new Vector3[3];
    // Use this for initialization
    void Start()
    {   for (int i = 0; i < 3; i++)
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

        if (movePattern > 2) movePattern = 0;
            else if (movePattern < 0) movePattern = 2;
        ResetPos();

    }

    public void ResetPos()
    {
        for (int i = 0; i < 3; i++)
        {
            this.transform.GetChild(i).transform.position = screenPos[(i + movePattern) % 3];
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
            for (int i = 0; i < 3; i++) {
                Vector3 pos = this.transform.GetChild(i).transform.position;
                this.transform.GetChild(i).Translate((mousePos.x - prevMousePos.x)*speed,0,0);
            }
            if (mousePos.x > startPos.x + 7f)
            {
               
                RotateScreen(true);
                startPos = mousePos;
            }
            if (mousePos.x < startPos.x -7f)
            {
             
                RotateScreen(false);
                startPos = mousePos;
            }
            prevMousePos = mousePos;
        }
        if (Input.GetMouseButtonUp(0) && mouseTouch)
        {
            ResetPos(); mouseTouch = false;
        }
    }
}
