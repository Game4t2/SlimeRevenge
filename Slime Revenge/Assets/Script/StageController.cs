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
                GameObject enemy = _SpawnEnemy(GameDatabase.Instance.EnemyDatabase.FindEnemyByName(wave.enemyName), wave.spawnLane);
                //if wait till wave end, will wait for this to die before start a new one
                if (wave.waitTillWaveEnd)
                    m_currentUnitGroup.Add(enemy);
            }
            yield return null;
        }
        //non group
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
        //set null will force _ReadNextWave() to run next one
        currentCoroutine = null;
    }

    private GameObject _SpawnEnemy(Enemy enemy, int lane)
    {
        GameObject enemyUnit = enemy.CreateInstance();
        Vector3 spawnPosition = Vector3.zero;
        if (m_enemyWall != null)
        {
            if (lane < 0)
                lane = Random.Range(0, m_Lane.Count - 1);
            else if (lane >= m_Lane.Count)
                lane = m_Lane.Count - 1;

            spawnPosition = new Vector3(m_enemyWall.transform.position.x, m_Lane[lane].transform.position.y, 0);
        }
        else
            Debug.LogError("Enemy wall is missing recheck at " + this.name);
        enemyUnit.transform.position = spawnPosition;
        enemyUnit.SetActive(true);
        return enemyUnit;
    }



    private bool _CheckWaveStatus()
    {
        for (int i = 0; i < m_currentUnitGroup.Count; i++)
        {
            if (m_currentUnitGroup[i].activeInHierarchy)
                return false;
        }
        return true;
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Start Wave"))
        {
            StartCoroutine(ReadNextWave());
        }
    }
}
