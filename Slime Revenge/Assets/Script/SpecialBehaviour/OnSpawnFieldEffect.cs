using UnityEngine;
using System.Collections;

public class OnSpawnFieldEffect : OnSpawnBehaviour {

    public FieldEffect fieldEffect;


	// Use this for initialization
	void Start () {
        FieldEffectController.AddFieldEffect(fieldEffect);
	}
	
}
