using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderAnimatorEvent : AnimatorEvent
{
    [SerializeField] private Player player;
    [SerializeField] private ThunderManager thunder;
    private void Awake()
    {
        player = GetComponentInParent<Player>();
        thunder = GetComponentInParent<ThunderManager>();
    }
    public void MoveTrueEvent()
    {
        player.SetCanMove(true);
    }
    public void BigAttackEvent()
    {
        thunder.ThrowCharge();
    }
    public void CounterHitEvent()
    {
        player.SetInvicible(false);
    }
}
