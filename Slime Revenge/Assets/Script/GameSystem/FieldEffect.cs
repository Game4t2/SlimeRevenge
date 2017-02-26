using UnityEngine;
using System;
using System.Collections;

public class FieldEffect
{
    public EffectType effect;
    public float duration;
    /// <summary>
    /// Unit type byname 
    /// This included "slime" , "enemy"or "both"
    /// </summary>
    public string targetUnitType;
    /// this parameter sould be documented
    public string targetStatus;
    /// <summary>
    /// This will represeted as 0-1
    /// </summary>
    public float effectPower;

    public enum EffectType
    {
        Buff = 1,
        Debuff = 2,
        Healing = 3,
        Damage = 4,
    }

    ///Field effect constructor
    public FieldEffect(EffectType e,string target,string status,float power, float d )
    {
        effect = e;
        duration = d;
        targetUnitType = target;
        targetStatus = status;
        effectPower = power;
    }



}
