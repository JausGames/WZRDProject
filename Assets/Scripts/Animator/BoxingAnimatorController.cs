using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxingAnimatorController : AnimatorController
{
    [SerializeField] private Animator animator;
    public void PlayAttack(int nb)
    {
        animator.SetTrigger("Attack" + nb.ToString());
    }
    public void PlayBigAttack()
    {
        animator.SetTrigger("BigAttack");
    }
    public void PlayDashAttack()
    {
        animator.SetTrigger("DashAttack");
    }
    public void PlayCounterAttack()
    {
        animator.SetTrigger("Counter");
    }
    public void PlayCounterSucessAttack()
    {
        animator.SetTrigger("CounterHit");
    }
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
    public void DashForward()
    {
        animator.SetTrigger("Dash");
        animator.SetBool("GoForward", true);
        animator.SetBool("GoBackward", false);
        animator.SetBool("GoLeft", false);
        animator.SetBool("GoRight", false);
    }
    public void DashBackward()
    {
        animator.SetTrigger("Dash");
        animator.SetBool("GoForward", false);
        animator.SetBool("GoBackward", true);
        animator.SetBool("GoLeft", false);
        animator.SetBool("GoRight", false);
    }
    public void DashLeft()
    {
        animator.SetTrigger("Dash");
        animator.SetBool("GoForward", false);
        animator.SetBool("GoBackward", false);
        animator.SetBool("GoLeft", true);
        animator.SetBool("GoRight", false);
    }
    public void DashRight()
    {
        animator.SetTrigger("Dash");
        animator.SetBool("GoForward", false);
        animator.SetBool("GoBackward", false);
        animator.SetBool("GoLeft", false);
        animator.SetBool("GoRight", true);
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
