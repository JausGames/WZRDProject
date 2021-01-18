using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderManager : SpellManager
{
    [SerializeField] private ThunderAnimatorController animator;
    [SerializeField] private ThunderParticleManager particles;
    [SerializeField] private GameObject attackThunder;
    [SerializeField] private Transform thunderStart;
    [SerializeField] private Transform thunderEnd;
    [SerializeField] private GameObject aimPoint;
    [SerializeField] private Player player;
    [SerializeField] private List<Player> touched;
    private Rigidbody body = null;
    [SerializeField] private GameObject teleportArc;
    [SerializeField] private GameObject charge;
    [SerializeField] private GameObject tesla;
    [SerializeField] private GameObject lastCharge;
    [SerializeField] private GameObject aimPrefab;
    [SerializeField] private Transform powerBall;
    [SerializeField] private bool dash = false;
    [SerializeField] private bool attack = false;
    [SerializeField] private GameObject arc;

    private float nextDashtime;
    private float dashCooldown = 2f;
    private float dashTime = 1.8f;
    [SerializeField] private float dashTimer;
    [SerializeField] private float time;
    private Vector3 dashDir;

    private float nextAttacktime;
    private float attackCadency = 0.05f;
    private float attackTime;

    private float nextBigAttacktime = 0f;
    private float bigCooldown = 5f;

    private float nextXAttack;
    private float xCooldown = 2f;

    private float nextBAttack;
    private float bCooldown = 2f;
    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody>();
        player = GetComponent<Player>();
        attackThunder.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        time = Time.time;

        if (dash && dashTimer < Time.time)
        {
            teleportArc.SetActive(false);
            dash = false;
            nextDashtime = Time.time + dashCooldown;
            var vect = aimPoint.transform.position;
            transform.position = vect;
        }
        if (attack)
        {
            if (attackTime <= Time.time)
            {
                attackThunder.SetActive(true);
                thunderStart.position = powerBall.position;
                thunderEnd.position = powerBall.position + transform.forward * 5f;
                Collider[] colliders1 = Physics.OverlapSphere(thunderEnd.position, 3f);
                foreach (Collider collider in colliders1)
                {
                    if (collider.gameObject.GetComponentInParent<Player>() != null && collider.gameObject.GetComponentInParent<Player>() != player && collider.gameObject.GetComponentInParent<Player>().GetAlive())
                    {
                        thunderEnd.position = collider.gameObject.GetComponentInParent<Player>().transform.position + transform.up;
                    }

                }
                foreach (Collider collider in colliders1)
                {
                    if (collider.gameObject.GetComponentInParent<SpellTesla>() != null && collider.gameObject.GetComponentInParent<SpellTesla>().GetOwner() == player)
                    {
                        thunderEnd.position = collider.gameObject.GetComponentInParent<SpellTesla>().transform.position;
                    }

                }
                Collider[] colliders2 = Physics.OverlapCapsule(thunderStart.position, thunderEnd.position, 0.2f);
                foreach (Collider collider in colliders2)
                {
                    if (collider.gameObject.GetComponentInParent<Player>() != null && collider.gameObject.GetComponentInParent<Player>() != player)
                    {
                        var oppo0 = collider.gameObject.GetComponentInParent<Player>();

                        Collider[] colliders3 = Physics.OverlapSphere(oppo0.transform.position, 5f);
                        foreach (Collider col in colliders3)
                        {
                            if (col.gameObject.GetComponentInParent<Player>() != null && col.gameObject.GetComponentInParent<Player>() != oppo0 && col.gameObject.GetComponentInParent<Player>() != player && !FindPlayer(col.gameObject.GetComponentInParent<Player>()))
                            {
                                var oppo = col.gameObject.GetComponentInParent<Player>();
                                touched.Add(oppo);
                                oppo.GetHit(1f, player);
                                GameObject a = Instantiate(arc, transform.position, transform.rotation);
                                a.GetComponent<TeslaArc>().SetStartPoint(oppo0.transform.position + transform.up);
                                a.GetComponent<TeslaArc>().SetEndPoint(oppo.transform.position + transform.up);
                            }

                        }
                        touched.Clear();
                        collider.gameObject.GetComponentInParent<Player>().GetHit(2f, player);
                        attackTime = Time.time + attackCadency;
                    }
                    if (collider.gameObject.GetComponentInParent<SpellTesla>() != null)
                    {
                        collider.gameObject.GetComponentInParent<SpellTesla>().SetActive(true);
                        attackTime = Time.time + attackCadency;
                    }

                }
            
            }
        }
    }
    public override void Dash(bool value)
    {
        if (Time.time <= nextDashtime) return;
        if (dash && !value)
        {
            teleportArc.SetActive(false);
            dash = false;
            nextDashtime = Time.time + dashCooldown;
            var vect = aimPoint.transform.position;
            transform.position = vect;
            return;
        }
        if (value && dashTimer < Time.time)
        {
            teleportArc.SetActive(true);
            particles.PlayDashParticle();
            dash = true;
            dashTimer = Time.time + dashTime;
            dashDir = transform.forward;
            aimPoint.transform.position = transform.position + (transform.forward * 2f - transform.up * 1f);
            aimPoint.transform.rotation = transform.rotation;

        }
        animator.PlayDash();
    }
    public override void Attack(bool value, bool value2)
    {
        Debug.Log("OnAttack, bool = " + value);
        attack = value;
        attackThunder.SetActive(value);
        animator.PlayAttack(value);
    }
    public override void BigAttack(bool perf, bool canc)
    {
        if (perf && Time.time > nextBigAttacktime)
        {
            animator.PlayBigAttack();
            GameObject rockAttack = Instantiate(charge, transform.position + (transform.forward * 1.5f), transform.rotation); 
            nextBigAttacktime = Time.time + bigCooldown;
            lastCharge = rockAttack;
            lastCharge.GetComponent<SpellThunderCharge>().SetOwner(player);
        }
        if (canc && lastCharge != null) animator.PlayBigThrowAttack();
    }

    public void PlayChargeAnim()
    {
        animator.PlayBigThrowAttack();
    }
    public void ThrowCharge()
    {
        lastCharge.GetComponent<SpellThunderCharge>().StopCharging();
    }
    public override void Defense()
    {
        if (Time.time <= nextXAttack) return;
        animator.PlayWall();
        player.SetCounter(true);
        nextXAttack = Time.time + xCooldown;
        GameObject teslaAttack = Instantiate(tesla,transform.position + transform.forward * 2f + transform.up, transform.rotation);
        teslaAttack.GetComponent<SpellTesla>().SetOwner(player);
    }

    public override void Zone()
    {
        if (Time.time <= nextBAttack) return;
        animator.PlayCounterAttack();
        nextBAttack = Time.time + bCooldown;
        particles.PlayZoneParticle();
        player.SetCounter(true);
    }
    public override void Counter(Player oppo)
    {
        animator.PlayCounterSucessAttack();
        player.SetCounter(false);
        player.SetInvicible(true);
        GameObject a = Instantiate(arc, transform.position, transform.rotation);
        a.GetComponent<TeslaArc>().SetStartPoint(oppo.transform.position + Vector3.up * 10f);
        a.GetComponent<TeslaArc>().SetEndPoint(oppo.transform.position);
    }
    private bool FindPlayer(Player player)
    {
        if (touched == null) return false;
        foreach (Player p in touched)
        {
            if (p == player) return true;
        }
        return false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(powerBall.position, 0.2f);
        Gizmos.DrawWireSphere(powerBall.position + transform.forward * 5f, 0.2f);
        Gizmos.DrawLine(powerBall.position, powerBall.position + transform.forward * 5f);
        Gizmos.color = Color.yellow;
        if (aimPoint != null) Gizmos.DrawWireSphere(aimPoint.transform.position, 0.2f);
    }
}
