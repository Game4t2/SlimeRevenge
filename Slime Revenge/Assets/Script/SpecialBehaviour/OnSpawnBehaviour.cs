using UnityEngine;
using System.Collections;

public class OnSpawnBehaviour : MonoBehaviour
{



    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    [System.Serializable]
    public class SpawnAction
    {
        public ActionType action;


        public enum ActionType
        {
            MINIGAME = 1,
            FIELDEFFECT = 2,
        }
    }



}
