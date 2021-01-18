using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindManager : SpellManager
{
    [SerializeField] private WindParticleManager particles;
    [SerializeField] private Player player;
    [SerializeField] private WindAnimatorController animator;
    private Rigidbody body = null;
    [SerializeField] private GameObject plane;
    [SerializeField] private GameObject tornado;
    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject lastTornado;
    [SerializeField] private Transform powerBall;
    [SerializeField] private bool dash = false;
    const int DASH_NUMBER = 2;
    [SerializeField] private int dashNumber = 2;
    [SerializeField] private bool shield = false;
    [SerializeField] private bool invicible = false;
    [SerializeField] private bool attack = false;
    [SerializeField] private bool tornadoReturn = false;

    private float nextDashtime;
    private float dashCooldown = 1.4f;
    private float closedDash;
    private float dashCadency = 0.4f;
    private float dashForce = 300f;
    private Vector3 dashDir;

    private float nextAttacktime;
    private float attackCadency = 0.4f;
    private float attackTime;
    private float cooldown = .8f;
    private float jetForce = 1f;

    private float nextBigAttacktime;
    private bool throwing;
    private float bigCooldown = 5f;
    [SerializeField]  private float inChargeTime;
    private float bigChargeTime = 2f;
    private float bigTime;
    private float returnTime = 1f;
    private float backTime;

    private float nextXAttack;
    private float xCooldown = 2f;

    private float nextBAttack;
    private float bCooldown = 2f;
    private float invicibleTime = 1f;
    private float invicibleTimer;
    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody>();
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inChargeTime != 0f) powerBall.localScale = new Vector3((Time.time - inChargeTime) * 1.8f + 1f, (Time.time - inChargeTime) * 1.8f + 1f, (Time.time - inChargeTime) * 1.8f + 1f);
        if (invicible && Time.time > invicibleTimer)
        {
            invicible = false;
            player.SetInvicible(false);
        }
        if (Time.time >= bigTime && lastTornado == null && throwing)
        {
            ThrowTornado(0f);
        }
        if (Time.time >= backTime && lastTornado != null && !tornadoReturn)
        {
            lastTornado.GetComponent<SpellBoomerang>().DeleteTouched();
            tornadoReturn = true;
        }
        if (tornadoReturn && lastTornado != null)
        {
            lastTornado.GetComponent<Rigidbody>().velocity = (transform.position - lastTornado.transform.position).normalized * 12f;
        }

        if (Time.time >= nextDashtime)
        {
            if (dashNumber == DASH_NUMBER) return;
            dashNumber++;
            nextDashtime = Time.time + dashCooldown;
        }
    }
    public override void Dash(bool value)
    {
        if (dashNumber <= 0 || closedDash >= Time.time) return;
        if (dashNumber == 2) nextDashtime = Time.time + dashCooldown;
        animator.PlayDash();
        closedDash = Time.time + dashCadency;
        dashDir = transform.forward;
        body.AddForce(transform.forward * dashForce, ForceMode.Impulse);
        dashNumber --;


        particles.PlayDashParticle();
    }
    public override void Attack(bool value, bool value2)
    {
        if (Time.time <= attackTime) return;
        tornadoReturn = false;
        attackTime = Time.time + attackCadency;
        animator.PlayAttack();
        Debug.Log("OnAttack, bool = " + value);
    }
    public void ThrowPlane()
    {
        GameObject rockAttack = Instantiate(plane, powerBall.position + 0.3F * transform.forward, transform.rotation);
        rockAttack.GetComponent<SpellPlane>().SetOwner(player);

        rockAttack.GetComponent<Rigidbody>().AddForce((transform.forward * 2f + 0.3f * transform.up ) * jetForce, ForceMode.Impulse);
    }
    public override void BigAttack(bool perf, bool canc)
    {
        if (perf)
        {
            if (Time.time <= nextBigAttacktime) return;
            bigTime = bigChargeTime + Time.time;
            throwing = true;
            animator.PlayBigAttack();
            inChargeTime = Time.time;
        }
        if (canc && lastTornado == null && throwing)
        {
            ThrowTornado(bigTime - Time.time);
        }
    }
    public void ThrowTornado(float force)
    {
        ResetPowerBall();
        throwing = false;
        animator.PlayThrowBigAttack();
        nextBigAttacktime = Time.time + bigCooldown;
        GameObject rockAttack = Instantiate(tornado, powerBall.position - transform.up * 0.8f, transform.rotation);
        rockAttack.GetComponent<Rigidbody>().AddForce((transform.forward * (19f - 9 * force)) * jetForce, ForceMode.Impulse);
        backTime = Time.time + returnTime - force/5f;
        rockAttack.GetComponent<SpellBoomerang>().SetOwner(player);
        rockAttack.GetComponent<SpellBoomerang>().SetWindManager(this);
        lastTornado = rockAttack;
    }
    public override void Defense()
    {
        if (Time.time <= nextXAttack) return;
        animator.PlayWall();
        nextXAttack = Time.time + xCooldown;
        GameObject mineAttack = Instantiate(wall, powerBall.position - Vector3.up * 3.2f + transform.forward, transform.rotation);
        mineAttack.GetComponent<SpellWindWall>().SetOwner(player);
    }

    public override void Zone()
    {
        if (Time.time <= nextBAttack) return;
        animator.PlayCounterAttack();
        nextBAttack = Time.time + bCooldown;
        particles.PlayZoneParticle();
        player.SetCounter(true);
        inChargeTime = Time.time;
    }
    public override void Counter(Player oppo)
    {
        animator.PlayCounterSucessAttack();
        ResetPowerBall();
        oppo.GetHit(10f, player);
    }
    public void SetLastBoomerang(GameObject obj)
    {
        lastTornado = obj;
        if (obj == null) tornadoReturn = false;
    }
    public void ResetPowerBall()
    {
        inChargeTime = 0f;
        powerBall.localScale = Vector3.one;
    }
}
