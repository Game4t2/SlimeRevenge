using UnityEngine;
using System.Collections;

public static class GameDatabase
{
    private static SlimeScriptableObject m_slimeDatabase;
    public static SlimeScriptableObject SlimeDatabase
    {
        get
        {
            if (m_slimeDatabase == null)
                m_slimeDatabase = Resources.Load("/ScriptableObject/SlimeScriptableObject") as SlimeScriptableObject;
            return m_slimeDatabase;
        }
    }
    private static EnemyScriptableObject m_enemyDatabase;
    public static EnemyScriptableObject EnemyDatabase
    {
        get
        {
            if (m_enemyDatabase == null)
                m_enemyDatabase = Resources.Load("/ScriptableObject/EnemyScriptableObject")as EnemyScriptableObject;
                return m_enemyDatabase;
        }
    }

}
