using UnityEngine;
using System.Collections;

/// <summary>
/// Spawn slime from slime king and throw to a position
/// slime element are randomed from the panel
/// </summary>
public class SpawnSlimeSkill : MonoBehaviour
{
    public int slimeCount;
    public float spawnPosition;
    [Tooltip("This should not be higher than 0.5")]
    public float randomRange;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < slimeCount; i++)
        {
            float realSpawnPosition = spawnPosition + Random.Range(-randomRange, randomRange);
            Vector3 spwanPosition = Vector3.Lerp(StageController.Instance.slimeWall.transform.position, StageController.Instance.enemyWall.transform.position, realSpawnPosition);
            Element elem = TouchDeploy.Instance.GetRandomElementInQueue();
            TouchDeploy.Instance.CreateSlime(elem, 1, spwanPosition);
        }
    }

}
