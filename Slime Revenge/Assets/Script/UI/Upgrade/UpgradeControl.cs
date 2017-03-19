using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UpgradeControl : MonoBehaviour {
    public Button skillListButton;
    public GameObject skillListContent;
    public Text materailLabel;
    
    int materails;
    string choosedSkill="";
    GameObject skillImage;
    Text skillDescription;
    // Use this for initialization
	void Start () {
        materails = 0;
        skillImage = this.transform.FindChild("SkillImage").gameObject;
        skillDescription = skillImage.transform.GetChild(0).GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChooseSkill(string id)
    {
        choosedSkill = id;
           materails = 0;

        materailLabel.text = (materails).ToString();
           SkillData skill = GameDatabase.Instance.MySkillDatabase.GetSkill(id);
        skillImage.GetComponent<RawImage>().texture = skill.skillSprite.texture;
        skillDescription.text = skill.displayName;
    }
    public void IncreaseMaterails()
    {
        SkillData skill = GameDatabase.Instance.MySkillDatabase.GetSkill(choosedSkill);
        if (materails < (skill.total - 1))
            materails++;
        materailLabel.text = (materails).ToString() ;
    }

    public void DecreaseMaterail()
    {
        if (materails > 0)
            materails--;
        materailLabel.text = (materails).ToString();
    }
    public void ClickUpgrade()
    {
        SkillData skill = GameDatabase.Instance.MySkillDatabase.GetSkill(choosedSkill);

        if ( materails <= (skill.maxlevel - skill.level))
            skill.total = skill.total-materails;
        else
        {
            materails = (skill.maxlevel - skill.level);
            skill.total = skill.total - materails;
        }
        skill.level = skill.level + materails;
        materails = 0;
        materailLabel.text = (materails).ToString();

    }
    public void ClickDestroy()
    {

    }
    public void SetSkillList()
    {
      
        for (int i = 0; i < GameDatabase.Instance.MySkillDatabase.list.Count; i++) {
            Button B = Instantiate(skillListButton);
            B.transform.SetParent(skillListContent.transform);
            B.onClick.AddListener(delegate { ChooseSkill(GameDatabase.Instance.MySkillDatabase.list[i].skillID); });
            B.transform.FindChild("RawImage").GetComponent<RawImage>().texture = GameDatabase.Instance.MySkillDatabase.list[i].skillSprite.texture;
            B.transform.FindChild("Text").GetComponent<Text>().text = GameDatabase.Instance.MySkillDatabase.list[i].displayName;

        }
    }

}
