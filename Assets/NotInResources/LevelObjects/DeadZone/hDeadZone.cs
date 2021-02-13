using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hDeadZone : hEffectObject
{
    public override void DoEffect(GameObject inTarget) => inTarget.GetComponent<hGameBall>()?.Die();
}
