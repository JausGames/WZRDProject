using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterAnimatorController : AnimatorController
{
    [SerializeField] private Animator animator;
    public void PlayAttack(bool value)
    {
        animator.SetBool("Attack", value);
    }
    public void PlayBigAttack()
    {
        animator.SetTrigger("BigAttack");
    }
    public void PlaySelf()
    {
        animator.SetTrigger("SelfAttack");
    }
    /*public void PlayCounterSucessAttack()
    {
        animator.SetTrigger("CounterHit");
    }*/
    override public void ToIdle()
    {
        animator.SetBool("GoForward", false);
        animator.SetBool("GoBackward", false);
        animator.SetBool("GoLeft", false);
        animator.SetBool("GoRight", false);
    }
    override public void WalkForward()
    {
        animator.SetBool("GoForward", true);
        animator.SetBool("GoBackward", false);
        animator.SetBool("GoLeft", false);
        animator.SetBool("GoRight", false);
    }
    override public void WalkBackward()
    {
        animator.SetBool("GoForward", false);
        animator.SetBool("GoBackward", true);
        animator.SetBool("GoLeft", false);
        animator.SetBool("GoRight", false);
    }
    override public void WalkLeft()
    {
        animator.SetBool("GoForward", false);
        animator.SetBool("GoBackward", false);
        animator.SetBool("GoLeft", true);
        animator.SetBool("GoRight", false);
    }
    override public void WalkRight()
    {
        animator.SetBool("GoForward", false);
        animator.SetBool("GoBackward", false);
        animator.SetBool("GoLeft", false);
        animator.SetBool("GoRight", true);
    }
    public void PlayDash()
    {
        animator.SetTrigger("Dash");
    }
    public void PlayDefense()
    {
        animator.SetTrigger("Defense");
    }


    public override void Die()
    {
        animator.enabled = false;
    }
    public override void Revive()
    {
        animator.enabled = true;
    }
    public override void Spawn()
    {
        animator.CrossFade("Idle", 0);
    }
}
