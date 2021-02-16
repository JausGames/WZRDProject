using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthManager : SpellManager
{
    
    [SerializeField] private EarthAnimatorController animator;
    [SerializeField] private EarthParticleManager particles;
    [SerializeField] LayerMask playerMask;
    private Rigidbody body = null;
    [SerializeField] private Player player;
    [SerializeField] private GameObject rock;
    [SerializeField] private GameObject meteor;
    [SerializeField] private GameObject wall;
    [SerializeField] private Transform powerBall;
    [SerializeField] private bool shield = false;
    [SerializeField] private bool invicible = false;
    [SerializeField] private bool landing = false;

    private float nextAttacktime;
    private float cooldown = .6f;
    private float rockForce = 50f;

    private float nextBigAttacktime;
    private float bigCooldown = 7f;

    private float nextDashtime;
    private float dashCooldown = 4f;
    private float dashForce = 200f;
    private float landingForce = 40f;

    private float nextXAttack;
    private float xCooldown = 3f;

    private float nextBAttack;
    private float bCooldown = 3f;
    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shield)
        {
            Debug.Log("PlayerCombat, Update : shield = true");

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.GetComponent<Spell>() != null && hitCollider.gameObject.GetComponent<Spell>().GetReturn())
                {
                    hitCollider.gameObject.GetComponent<Rigidbody>().velocity = -0.8f * hitCollider.gameObject.GetComponent<Rigidbody>().velocity;
                }
            }
            if (nextBAttack <= Time.time + 1f) shield = false;
        }
    }
    private void FixedUpdate()
    {

        if (landing)
        {
            body.AddForce(Vector3.down * 0.05f * dashForce, ForceMode.Acceleration);
            if (player.GetOnFloor())
            {
                animator.PlayLand();
                landing = false;
                Landing();
            }
        }
    }
    public override void Dash(bool perfomed, bool canceled)
    {
        Debug.Log("EarthManager, Dash : perf & canc = " + perfomed + " & " + canceled);
        if (Time.time <= nextDashtime || !perfomed) return;
        Debug.Log("EarthManager, Dash : perf = " + perfomed);
        nextDashtime = Time.time + dashCooldown;
        player.SetCanMove(false);
        animator.PlayDash();
    }
    public void Jump()
    {
            Debug.Log("EarthManager, Jump");
            particles.PlayDashParticle();
            body.AddForce((transform.forward * 0.9f + transform.up * 1.2f) * dashForce, ForceMode.Impulse);


    }
    public void StartLanding()
    {
        Debug.Log("EarthManager, StartLanding");
        landing = true;
    }
    public void Landing()
    {
        
            Debug.Log("EarthManager, Landing");
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 4f, playerMask);
            particles.PlayLandParticle();
        if (hitColliders.Length == 0) return;
            foreach (Collider col in hitColliders)
            {
                if (col.GetComponentInParent<Player>() != player)
                {
                //col.GetComponent<Rigidbody>().AddExplosionForce(landingForce, transform.position, 15f, 2f);
                //col.GetComponentInParent<Rigidbody>().AddExplosionForce(landingForce, transform.position, 15f, 2f);

                col.GetComponent<Rigidbody>().AddForce(Vector3.up * landingForce, ForceMode.Impulse);
                col.GetComponentInParent<Rigidbody>().AddForce(Vector3.up * landingForce, ForceMode.Impulse);
                col.GetComponentInParent<Player>().GetHit(15f, player);
                }
            }
        
    }
    public override void Attack(bool perf, bool canc)
    {
        if (Time.time <= nextAttacktime || !perf) return;
        nextAttacktime = Time.time + cooldown;
        animator.PlayAttack();
        //player.SetCanMove(false);

    }
    public void ThrowRock()
    {
        GameObject rockAttack = Instantiate(rock, powerBall.position + transform.forward * 2f, transform.rotation);
        //Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), GetComponent<Collider>());
        particles.PlayThrowParticle();
        rockAttack.GetComponent<SpellRock>().SetOwner(player);
        rockAttack.GetComponent<Rigidbody>().AddForce((transform.forward * 10f + Vector3.up * 2.5f) * rockForce, ForceMode.Impulse);
    }
    public override void BigAttack(bool value, bool value2)
    {
        if (Time.time <= nextBigAttacktime) return;
        animator.PlayBigAttack();
        player.SetCanMove(false);
        nextBigAttacktime = Time.time + bigCooldown;
        GameObject rockAttack = Instantiate(meteor, transform.position + (transform.forward * 7f + transform.up * 60f), transform.rotation);

        rockAttack.GetComponent<Rigidbody>().AddForce((-300f * Vector3.up), ForceMode.Impulse);
        rockAttack.GetComponent<SpellMeteor>().SetOwner(player);
    }
    public override void Defense()
    {
        if (Time.time <= nextXAttack) return;
        animator.PlayWall();
        nextXAttack = Time.time + xCooldown;
        GameObject rockAttack = Instantiate(wall, transform.position + (transform.forward * 4f), transform.rotation);
    }

    public override void Zone()
    {
        if (Time.time <= nextBAttack) return;
        animator.PlayCounterAttack();
        player.SetCanMove(false);
        nextBAttack = Time.time + bCooldown;
        particles.PlayZoneParticle();
        shield = true;
    }
    public override void Counter(Player oppo)
    {

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 4f);
    }
}
