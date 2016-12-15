using UnityEngine;
using System.IO;
using System.Collections.Generic;

[System.Serializable]
public class EnemyWave : ScriptableObject
{
    public float mapLength;
    public List<Wave> waves;

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
public class Wave
{
    public Enemy enemy;
    public int amount;
    [Tooltip("Use -1 for anylane")]
    public int spawnLane;
    public float spawnDelay;
    [Tooltip("Wait until this wave end before spawn next wave")]
    public bool waitTillWaveEnd;


}
