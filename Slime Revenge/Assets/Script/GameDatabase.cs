using UnityEngine;
using System.Collections;

public class GameDatabase : MonoBehaviour
{
    private static GameDatabase _instance;
    public static GameDatabase Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<GameDatabase>();
            return _instance;
        }

    }

    [SerializeField]
    private SlimeScriptableObject m_slimeDatabase;
    public SlimeScriptableObject SlimeDatabase
    {
        get
        {
            if (m_slimeDatabase == null)
                m_slimeDatabase = new SlimeScriptableObject();
            return m_slimeDatabase;
        }
    }
    [SerializeField]
    private EnemyScriptableObject m_enemyDatabase;
    public EnemyScriptableObject EnemyDatabase
    {
        get
        {
            if (m_enemyDatabase == null)
                m_enemyDatabase = new EnemyScriptableObject();
                return m_enemyDatabase;
        }
    }

}
