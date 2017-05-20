using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffAll : MonoBehaviour {
    public float buffPrecent=1f;
    public float time;
    string[] buffStat= { "atp", "def","range", "speed","attackspeed" };
    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Buff(int i=5)/// 0-4 buff only one stat 5 buffall >5buff except1 stat: 0=atp 1=def 2=range 3=speed 4=attackspeed
    {if (i <= 5)
            for (int j = 0; j < 5; j++)
            {
                if (j == i - 6) continue;
                FieldEffect f = new FieldEffect(FieldEffect.EffectType.Buff, "slime", buffStat[j], buffPrecent, time);
                FieldEffectController.AddFieldEffect(f);
            }
        else
        {
            FieldEffect f = new FieldEffect(FieldEffect.EffectType.Buff, "slime", buffStat[i], buffPrecent, time);
            FieldEffectController.AddFieldEffect(f);
        }
    }
}

