using UnityEngine;
using System.Collections.Generic;

public class FieldEffectController : MonoBehaviour
{
    //field effect and its start time
    private static Dictionary<FieldEffect, float> m_slimeFieldEffect;
    private static Dictionary<FieldEffect, float> m_enemyFieldEffect;
    private static float m_time;

    // Use this for initialization
    void Start()
    {
        m_slimeFieldEffect = new Dictionary<FieldEffect, float>();
        m_enemyFieldEffect = new Dictionary<FieldEffect, float>();
    }

    // Update is called once per frame
    void Update()
    {
        m_time += Time.deltaTime;
    }

    public static List<FieldEffect> GetSlimeFieldEffect()
    {
        List<FieldEffect> fe = new List<FieldEffect>();
        foreach (KeyValuePair<FieldEffect, float> pair in m_slimeFieldEffect)
        {
            //current time < start time + duration
            if (m_time < (pair.Value + pair.Key.duration))
            {
                fe.Add(pair.Key);
            }
            else
            {
                m_slimeFieldEffect.Remove(pair.Key);
            }
        }
        return fe;

    }

    public static List<FieldEffect> GetEnemyFieldEffect()
    {
        List<FieldEffect> fe = new List<FieldEffect>();
        foreach (KeyValuePair<FieldEffect, float> pair in m_enemyFieldEffect)
        {
            //current time < start time + duration
            if (m_time < (pair.Value + pair.Key.duration))
            {
                fe.Add(pair.Key);
            }
            else
            {
                m_enemyFieldEffect.Remove(pair.Key);
            }
        }
        return fe;

    }
}
