using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterAnimatorEvent : AnimatorEvent
{
    [SerializeField] private Player player;
    [SerializeField] private WaterManager water;
    private void Awake()
    {
        player = GetComponentInParent<Player>();
        water = GetComponentInParent<WaterManager>();
    }
    public void MoveTrueEvent()
    {
        player.SetCanMove(true);
    }
    public void BigAttackEvent()
    {
        water.SendIce();
    }
}
