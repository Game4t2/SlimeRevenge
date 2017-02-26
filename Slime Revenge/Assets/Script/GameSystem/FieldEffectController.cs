using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This class control field effect such as buff debuff, dot
/// Note: all field effect will run based on unity game time(not support pausefunction)
/// </summary>
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
        StageController.Instance.onGamePaused += _OnPaused;
        StageController.Instance.onGameUnpaused += _OnUnpaused;
    }

    void _OnPaused()
    {

    }

    private void _OnUnpaused()
    {

    }

    // Update is called once per frame
    void Update()
    {
        m_time += Time.deltaTime;
    }

    public static void AddFieldEffect(FieldEffect effect)
    {
        if (effect.targetUnitType == "slime")
        {
            m_slimeFieldEffect.Add(effect, Time.time);
        }
        else if (effect.targetUnitType == "enemy")
        {
            m_enemyFieldEffect.Add(effect, Time.time);
        }
        else if (effect.targetUnitType == "both")
        {
            m_enemyFieldEffect.Add(effect, Time.time);
            m_slimeFieldEffect.Add(effect, Time.time);
        }
        else
            Debug.LogError("Can't add " + effect.targetUnitType + " as field effect, target type not available ");
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

    private void LateUpdate()
    {
        foreach (KeyValuePair<FieldEffect, float> pair in m_enemyFieldEffect)
        {
            //current time < start time + duration
            if (pair.Key.effect == FieldEffect.EffectType.Damage && m_time < (pair.Value + pair.Key.duration))
            {
                List<EnemyUnit> enemies = EnemyPool.GetPool();
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (enemies[i].isActiveAndEnabled)
                        enemies[i].currentHp -= pair.Key.effectPower;
                }
            }

        }
        foreach (KeyValuePair<FieldEffect, float> pair in m_slimeFieldEffect)
        {
            //current time < start time + duration
            if (pair.Key.effect == FieldEffect.EffectType.Damage && m_time < (pair.Value + pair.Key.duration))
            {
                List<Unit> slimes = SlimePool.GetPool();
                for (int i = 0; i < slimes.Count; i++)
                {
                    if (slimes[i].isActiveAndEnabled)
                        slimes[i].currentHp -= pair.Key.effectPower;
                }
            }

        }
    }
}
