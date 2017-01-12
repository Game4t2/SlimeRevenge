using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
public class SkillRoll : MonoBehaviour {
    public List<GameObject> skillList = new List<GameObject>();/// DATA changelater


    private bool active=true;
    public Vector2 startpos;
    private Touch tuse;
    private bool mouseTouch=false;
    private float speed=5f;
    private Vector3 mousePos;
    private Vector3[] pos =new Vector3[5];
	// Use this for initialization
	void Start () {
        ////initial
        mouseTouch = false;
        this.transform.GetChild(0).transform.RotateAround(this.transform.position, Vector3.back, 120f);
        pos[0] = this.transform.GetChild(0).transform.position;
        this.transform.GetChild(1).transform.RotateAround(this.transform.position, Vector3.back, 60f);
        pos[1] = this.transform.GetChild(1).transform.position;
        this.transform.GetChild(2).transform.RotateAround(this.transform.position, Vector3.back, -120f);
        pos[2] = this.transform.GetChild(2).transform.position;
        this.transform.GetChild(3).transform.RotateAround(this.transform.position, Vector3.back, -60f);
        pos[3] = this.transform.GetChild(3).transform.position;
        this.transform.GetChild(4).transform.RotateAround(this.transform.position, Vector3.back, 0f);
        pos[4] = this.transform.GetChild(4).transform.position;
       // SkillListChecking();
        SetSprite();
    }
    public void SkillListChecking()
    {
        int totalSkill = skillList.Count;
        if (totalSkill == 0)
        {
            active = false;return;
        }
        if(totalSkill < 5)
        {
            for(int i= 0; i < 5- totalSkill; i++)
            {
                skillList.Add(skillList[i]);
            }
        }
    }
    public void SetSprite()
    {
        this.transform.GetChild(4).GetComponent<RawImage>().texture = skillList[2].GetComponent<SpriteRenderer>().sprite.texture;
        for (int i = 0; i < 2; i++) {
           
                this.transform.GetChild(i).GetComponent<RawImage>().texture = skillList[i%skillList.Count].GetComponent<SpriteRenderer>().sprite.texture;
        }
        for (int i = 3,j=3; i >= 2; i--,j++)
            {
            
                this.transform.GetChild(i).GetComponent<RawImage>().texture = skillList[j % skillList.Count].GetComponent<SpriteRenderer>().sprite.texture;
        }

    }
    public void ActiveSkill()
    {

    }
    public void RollRullet()
    {
         bool movedown = false;

                float angle = Mathf.Abs(this.transform.GetChild(1).transform.rotation.eulerAngles.z);
                if (angle > 180)
                {
                    movedown = true;
                    angle = Mathf.Abs(angle - 360);
                }
                if (angle <= 30f || angle >= 90f)
                {
                    DataSwap(movedown);
                    ResetPos();
                }
    }
    public void DataSwap(bool movedown)
    {
        int totalSkill = skillList.Count;
        GameObject gameobjold;
        GameObject gameobjnew;
        if (!movedown)
        {
            gameobjnew = skillList[0];
            gameobjold = skillList[0];
            for (int i = 0; i < totalSkill-1; i++)
            {
                gameobjnew = skillList[i+1];
                skillList[i+1] = gameobjold;
                gameobjold = gameobjnew;
            }
            skillList[0] = gameobjnew;
        }
        else
        {
            gameobjnew = skillList[totalSkill - 1];
            gameobjold = skillList[totalSkill - 1];
            for (int i = totalSkill-1; i >0;i--)
            {

                gameobjnew = skillList[i - 1];
                skillList[i- 1] = gameobjold;
                gameobjold = gameobjnew;

            }
            skillList[totalSkill - 1] = gameobjnew;

        }
        SetSprite();
    }
    public void ResetPos()
    {

        this.transform.GetChild(0).rotation = Quaternion.Euler(0f, 0f, -120f);
        this.transform.GetChild(1).rotation = Quaternion.Euler(0f, 0f, -60f);
        this.transform.GetChild(2).rotation = Quaternion.Euler(0f, 0f, 120f);
        this.transform.GetChild(3).rotation = Quaternion.Euler(0f, 0f, 60f);
        this.transform.GetChild(4).rotation = Quaternion.Euler(0f, 0f, 0f);
        for(int i = 0; i < 5; i++)
        {
            this.transform.GetChild(i).transform.position = pos[i] ;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 5f);
                if (hit.collider != null && hit.transform.name == this.name)
                {
                    mouseTouch = true;
                    startpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                }
            }
            if (Input.GetMouseButton(0) && mouseTouch)
            {
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                if (mousePos.x > pos[4].x + 0.5f)
                    this.transform.GetChild(4).transform.position = new Vector2(mousePos.x, this.transform.GetChild(4).transform.position.y);
                else
                {
                    this.transform.GetChild(4).transform.RotateAround(this.transform.position, Vector3.back, (mousePos.y - startpos.y) * -speed);
                }

                for (int i = 0; i < 4; i++)
                    this.transform.GetChild(i).transform.RotateAround(this.transform.position, Vector3.back, (mousePos.y - startpos.y) * -speed);

                RollRullet();
                startpos = mousePos;
            }
            if (Input.GetMouseButtonUp(0) && mouseTouch)
            {
                mouseTouch = false;
                if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > pos[4].x + 2f)
                {

                    ActiveSkill();
                    Debug.Log("skillActive");
                }
                ResetPos();

            }



        }
    }
}
