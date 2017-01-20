﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaRandom : MonoBehaviour {
    private bool clicked = false;
    public List<GameObject> groupNormal;
    public List<GameObject> groupRare;
    public List<GameObject> groupSRare;
    public int salt;
    private GameObject result;
    private Animator slimeHammer;
    private Button onetimebutton;
    private Button tentimebutton;
    private Text saltDisplay;
    private Text tentimeText;
    
    // Use this for initialization
    void Start () {
        slimeHammer = this.transform.FindChild("SlimeHammer").gameObject.GetComponent<Animator>();
        tentimebutton = this.transform.FindChild("10 time").gameObject.GetComponent<Button>();
        tentimeText = this.transform.FindChild("10 time").GetChild(0).gameObject.GetComponent<Text>();
        result = this.transform.FindChild("Result").gameObject;
        onetimebutton = this.transform.FindChild("1 time").gameObject.GetComponent<Button>();
        saltDisplay= this.transform.FindChild("Salt").GetChild(0).gameObject.GetComponent<Text>();

    }
    public void ClickGacha10time()
    {
        if (!clicked)
        {
            int time = salt / 10;
            if (time > 10) time = 10;
            ClickGacha(time);
        }
    }
	public void ClickGacha(int time)
    {
        if (!clicked)
        {
            clicked = true;
            if (salt >= (time * 10) && time != 0)
            {
                salt = salt - (time * 10);
                if (time == 10)
                    time = 11;
                ResultGacha(StartRandom(time));
            }
            else
            {
                return;
            }
        }

    }

    IEnumerator runResult()
    {
        slimeHammer.SetBool("Attack", true);
      
        yield return new WaitForSeconds(0.35f);
        result.SetActive(true);
        result.transform.GetChild(0).gameObject.SetActive(true);
        slimeHammer.SetBool("Attack", false);
        clicked = false;
    }
    public void ResultGacha(List<GameObject> output)
    {
        StartCoroutine("runResult");
        for (int i = 1; i <= output.Count; i++)
        {
            result.transform.GetChild(i).gameObject.SetActive(true);
            result.transform.GetChild(i).GetComponent<RawImage>().texture = output[i - 1].GetComponent<SpriteRenderer>().sprite.texture;
            result.transform.GetChild(i).GetComponent<RawImage>().color = output[i - 1].GetComponent<SpriteRenderer>().color;
        }

    }
    public void ClosePanel()
    {
        for (int i = 0; i <=11 ; i++)
        {
            result.transform.GetChild(i).gameObject.SetActive(false);
        }
        result.SetActive(false);


    }
    private List<GameObject> StartRandom(int time)
    {
        List<GameObject> output = new List<GameObject>();

        for(int i = 0; i < time; i++)
        {
            int j=Random.Range(0, 100);
            output.Add(RandomType(j));



        }
        return output;
    }
    private GameObject RandomType(int level)
    {
        int x = 99;
        if (level < 3)
        {
           x= Random.Range(0, groupSRare.Count);
            return groupSRare[x];
        }
        else if(level<27)
        {

            x = Random.Range(0, groupRare.Count);
            return groupRare[x];
        }
        else
        {

            x = Random.Range(0, groupNormal.Count);
            return groupNormal[x];
        }

    }



	// Update is called once per frame
	void Update () {
        saltDisplay.text = salt.ToString();
        if (salt / 10 <= 0)
        {
            onetimebutton.interactable = false;
            tentimebutton.interactable = false;
            tentimeText.text= "10+1 Time";
        }
        else if (salt / 10 >= 10)
        {

            tentimebutton.interactable = true;
            tentimeText.text =  "10+1 Time";
        }
        else
        {
            onetimebutton.interactable = true;
            tentimebutton.interactable = true;
            tentimeText.text = (salt / 10).ToString() + "Time";

        }
    }
}