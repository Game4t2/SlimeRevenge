using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class ParsingData : MonoBehaviour {
    public static ParsingData Instnce { get { return instan; } }
    private static ParsingData instan;
    public static int stage;
    public static Sprite[] sprites = new Sprite[4];
     public static SkillID[] ski=new SkillID[4];
	// Use this for initialization
    void Awake()
    {


        instan = this;
        DontDestroyOnLoad(this);
    }
    void Start()
    {
	}
    public void GetData(int index,out int ID,out bool Istype)
    {
        ID = ski[index].ID;
        Istype = ski[index].IsType;
    }

    public Sprite Getsprite(int index){

        return sprites[index];
    }
    public int GetStage()
    {
        return stage;
    }
    public void Load(SkillID[] ss,Sprite[] sps,int stageID)
    {
        for (int i = 0; i < 3; i++)
        {
            sprites[i] = sps[i];
            ski[i] = ss[i];
        }
        stage = stageID;
        Debug.Log(ss[0].ID);
        SceneManager.LoadScene("GamePlay");

    }
    public void DestroyMe()
    {

        Destroy(gameObject);
    }
	
}
