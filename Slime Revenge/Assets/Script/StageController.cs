using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageController : MonoBehaviour
{
    private static StageController _instance;
    public static StageController Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<StageController>();
            return _instance;
        }
    }

    [SerializeField]
    private EnemyWave m_stageData;
    [SerializeField]
    private Ewall m_enemyWall;
    public Ewall enemyWall
    {
        get { return m_enemyWall; }
    }
    [SerializeField]
    private Wall m_slimeWall;
    public Wall slimeWall
    {
        get { return m_slimeWall; }
    }

    [SerializeField]
    private List<GameObject> m_Lane;

    private int currentIndex;
    private Coroutine currentCoroutine;

    private List<GameObject> m_currentUnitGroup;

    // Use this for initialization
    void Start()
    {
        currentIndex = -1;
        m_currentUnitGroup = new List<GameObject>();
        InitWall();
    }

    private void InitWall()
    {
        if (m_enemyWall != null)
        {
            m_enemyWall.max_Hp = m_stageData.wall.max_Hp;
            m_enemyWall.def = m_stageData.wall.def;
        }
    }


    IEnumerator ReadNextWave()
    {
        while (currentIndex < m_Lane.Count - 1)
        {
            if (currentCoroutine == null)
            {
                currentIndex++;
                currentCoroutine = StartCoroutine(WaveProcessing(m_stageData.waves[currentIndex]));
            }
            yield return null;
        }

    }

    IEnumerator WaveProcessing(Wave wave)
    {
        if (!wave.groupWithNextWave)
            currentCoroutine = null;
        yield return null;
        float time = 0f;
        int unitCount = 0;
        while (unitCount < wave.amount)
        {
            time += Time.deltaTime;
            if (time > wave.spawnDelay)
            {
                time = 0;
                unitCount++;
                GameObject enemyUnit = GameDatabase.Instance.EnemyDatabase.FindEnemyByName(wave.enemyName).CreateInstance();
                Vector3 spawnPosition = Vector3.zero;
                if (m_enemyWall != null)
                {
                    int lane = wave.spawnLane;
                    if (wave.spawnLane < 0)
                        lane = Random.Range(0, m_Lane.Count - 1);
                    else if (wave.spawnLane >= m_Lane.Count)
                        lane = m_Lane.Count - 1;

                    spawnPosition = new Vector3(m_enemyWall.transform.position.x, m_Lane[lane].transform.position.y, 0);
                }
                else
                    Debug.LogError("Enemy wall is missing recheck at " + this.name);
                enemyUnit.transform.position = spawnPosition;
                enemyUnit.SetActive(true);
                m_currentUnitGroup.Add(enemyUnit);
            }
            yield return null;
        }
        if (!wave.groupWithNextWave && wave.waitTillWaveEnd)
        {
            if (wave.waitTillWaveEnd)
            {
                while (_CheckWaveStatus())
                {
                    yield return null;
                }
            }
            time = 0;
            while (time < wave.waveDelay)
            {
                time += Time.deltaTime;
                yield return null;
            }
        }




        currentCoroutine = null;
    }

    private bool _CheckWaveStatus()
    {
        return false;
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Start Wave"))
        {
            StartCoroutine(ReadNextWave());
        }
    }
}
