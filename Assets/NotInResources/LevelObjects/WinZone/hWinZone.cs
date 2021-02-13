using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hWinZone : hEffectObject
{
    private static event DelVoid _loseEvent;
    public static event DelVoid loseEvent { add => _loseEvent += value; remove => _loseEvent -= value; }
    
    public void Lose()
    {
        _loseEvent?.Invoke();
    }

    public override void DoEffect(GameObject inTarget)
    {
        var ball = inTarget.GetComponent<hGameBall>();
        if(ball != null)
        {
            ball.Goal();
            gameObject.SetActive(false);
        }
    }
}
