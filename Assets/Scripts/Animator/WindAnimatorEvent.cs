using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindAnimatorEvent : AnimatorEvent
{
    [SerializeField] private Player player;
    [SerializeField] private WindManager wind;
    private void Awake()
    {
        player = GetComponentInParent<Player>();
        wind = GetComponentInParent<WindManager>();
    }
    public void MoveTrueEvent()
    {
        player.SetCanMove(true);
    }
    public void AttackEvent()
    {
        wind.ThrowPlane();
    }

    public void BigAttackEvent()
    {
        //wind.PlayBigAttackParticle();
        //wind.ThrowTornado();
    }
    public void SelfAttackEvent()
    {
        player.SetCounter(true);
    }
    public void StopSelfAttackEvent()
    {
        player.SetCounter(false);
    }
    public void CounterHitAttackEvent()
    {
        player.SetCounter(false);
        //Attack

    }
    public void ResetBallEvent()
    {
        wind.ResetPowerBall();
    }
}
