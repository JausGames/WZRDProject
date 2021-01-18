using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthAnimatorEvent : AnimatorEvent
{
    [SerializeField] private Player player;
    [SerializeField] private EarthManager earth;
    private void Awake()
    {
        player = GetComponentInParent<Player>();
        earth = GetComponentInParent<EarthManager>();
    }
    public void MoveTrueEvent()
    {
        player.SetCanMove(true);
    }
    public void AttackEvent()
    {
        earth.ThrowRock();
    }
}
