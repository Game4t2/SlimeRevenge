using UnityEngine;
using System;
using System.Collections;

public class FieldEffect
{
    public EffectType effect;
    public float duration;
    public string targetUnitType;
    public string targetStatus;


    public enum EffectType
    {
        Buff = 1,
        Debuff = 2,
        Healing = 3,
    }

  



}
