using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxingAnimatorEvent : AnimatorEvent
{
    [SerializeField] private Player player;
    [SerializeField] private BoxingManager boxing;
    [SerializeField] private Transform leftGloves;
    [SerializeField] private Transform rightGloves;
    [SerializeField] private BoxingAnimatorController animator;
    [SerializeField] private int lastAttack;
    private void Awake()
    {
        player = GetComponentInParent<Player>();
        boxing = GetComponentInParent<BoxingManager>();
    }
    public void MoveTrueEvent()
    {
        player.SetCanMove(true);
    }
    public void MoveFalseEvent()
    {
        player.SetCanMove(false);
    }
    public void Combo1Event()
    {
        lastAttack = 1;
        boxing.Hit(false, 5f, transform.forward, 50f, 1.6f);
        boxing.particles.PlayCombo1Particle();
    }
    public void Combo2Event()
    {
        lastAttack = 2;
        boxing.Hit(true, 12f, transform.forward, 150f, 2f);
        boxing.particles.PlayCombo2Particle();
        boxing.GetComponent<Rigidbody>().AddForce(transform.forward * 150f, ForceMode.Impulse);
    }
    public void Combo3Event()
    {
        lastAttack = 3;
        boxing.Hit(true, 20f, transform.up, 140f, 2.5f);
        boxing.particles.PlayCombo3Particle();
        boxing.GetComponent<Rigidbody>().AddForce(transform.up * 120f, ForceMode.Impulse);
    }
    public void OpenBufferEvent()
    {
       /* boxing.SetBuff(true);*/
    }
    public void CheckComboEvent()
    {
        /*Debug.Log("AAAAAAAAAAAAAAAAAAAAA  ");
        if (boxing.GetCombo())
        {
            Debug.Log("AAAAAAAAAAAAAAAAAAAAA  " + (lastAttack + 1).ToString());
            animator.PlayAttack(lastAttack + 1);
            boxing.SetCombo(false);
        }

            boxing.SetBuff(false);*/
        
    }
    public void BigAttackEvent()
    {
        boxing.Hit(true, 100f, transform.forward, 500f, 3f);
        boxing.PlayBigAttackParticle();
        boxing.GetComponent<Rigidbody>().AddForce(transform.forward * 250f, ForceMode.Impulse);
    }
    public void DashAttackEvent()
    {
        boxing.Hit(true, 10f, transform.forward, 400f, 1.5f);
        boxing.particles.PlayCombo2Particle();
        boxing.GetComponent<Rigidbody>().AddForce(transform.forward * 300f, ForceMode.Impulse);
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
        boxing.Hit(false, 50f, transform.up, 200f, 2f);

    }
    public void StopHitEvent()
    {
        boxing.StopHit();
    }
}
