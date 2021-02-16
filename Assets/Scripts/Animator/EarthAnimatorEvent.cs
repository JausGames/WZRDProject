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
        Debug.Log("EarthAnimatorEven, MoveTrueEvent");
        player.SetCanMove(true);
    }
    public void AttackEvent()
    {
        earth.ThrowRock();
    }
    public void JumpEvent()
    {
        earth.Jump();
    }
    public void StartLandingEvent()
    {
        earth.StartLanding();
    }
    public void LandingEvent()
    {
        earth.Landing();
    }
}
