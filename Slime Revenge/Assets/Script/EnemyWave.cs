using UnityEngine;
using System.IO;
using System.Collections.Generic;

[System.Serializable]
[CreateAssetMenu(fileName = "New Enemy Wave", menuName = "Stage")]
public class EnemyWave : ScriptableObject
{
    public WallData wall;
    public float mapLength;
    public List<Wave> waves;

    public EnemyWave()
    {
        mapLength = 10;
        waves = new List<Wave>();
    }

    public static string WriteJson(string filePathAndName, EnemyWave content)
    {
        string json = MiniJSON.Json.Serialize(content);
        System.IO.File.WriteAllText(filePathAndName, json);
        return json;
    }

    public static EnemyWave ReadJson(string filePathAndName)
    {
        string json = System.IO.File.ReadAllText(filePathAndName);
        return MiniJSON.Json.Deserialize(json) as EnemyWave;
    }

}

[System.Serializable]
public class WallData
{
    public float max_Hp;
    public float def;
    public GameObject prefab;
}


[System.Serializable]
public class Wave
{
    public string enemyName;
    public int amount;
    [Tooltip("Use -1 for anylane")]
    public int spawnLane;
    [Tooltip("Time delay between each unit")]
    public float spawnDelay;
    [Tooltip("Wait until this wave end beforSpawn next wave")]
    public bool waitTillWaveEnd;
    [Tooltip("Time delay before next wave")]
    public float waveDelay;
    public bool groupWithNextWave;


}

