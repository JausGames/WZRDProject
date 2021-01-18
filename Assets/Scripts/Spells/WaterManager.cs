using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterManager : SpellManager
{
    [SerializeField] private WaterAnimatorController animator;
    [SerializeField] private WaterParticleManager particles;
    [SerializeField] private Player player;
    [SerializeField] private Transform powerball;
    private Rigidbody body = null;
    [SerializeField] private GameObject jet;
    [SerializeField] private GameObject ice;
    [SerializeField] private GameObject mine;
    [SerializeField] private GameObject wave;
    [SerializeField] private GameObject visual;
    [SerializeField] private List<Material> materials;
    [SerializeField] private Material iceMaterial;
    [SerializeField] private bool dash = false;
    [SerializeField] private bool shield = false;
    [SerializeField] private bool invicible = false;
    [SerializeField] private bool attack = false;

    private float nextDashtime;
    private float dashCooldown = 2f;
    private float dashForce = 15f;
    private float dashTime = 1f;
    [SerializeField] private float dashTimer;
    private Vector3 dashDir;

    private float nextAttacktime;
    private float attackCadency = 0.1f;
    private float attackTime;
    private float cooldown = .8f;
    private float jetForce = 1.2f;

    private float nextBigAttacktime;
    private float bigCooldown = 5f;

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
        //materials.AddRange(visual.GetComponentsInChildren<Material>());
    }

    // Update is called once per frame
    void Update()
    {
        if (invicible && Time.time > invicibleTimer)
        {
            invicible = false;
            player.SetInvicible(false);
        }
        if (attack)
        {
            if (attackTime <= Time.time)
            {
                GameObject rockAttack = Instantiate(jet, powerball.position, transform.rotation);
                rockAttack.GetComponent<SpellJet>().SetOwner(player);

                rockAttack.GetComponent<Rigidbody>().AddForce((transform.forward * 10f + Vector3.up) * jetForce, ForceMode.Impulse);
                attackTime = Time.time + attackCadency;
            }
        }
        if (dash)
        {
            body.velocity = dashDir * dashForce;
            if (Time.time >= dashTimer) dash = false; 
        }
    }
    public override void Dash(bool value)
    {
        if (dash && !value) dash = false;
        if (Time.time <= nextDashtime) return;
        animator.PlayDash();
        nextDashtime = Time.time + dashCooldown;
        dash = true;
        dashTimer = Time.time + dashTime;
        dashDir = transform.forward;
        GameObject rockAttack = Instantiate(wave, transform.position + transform.forward * 2f, transform.rotation);
        rockAttack.GetComponent<SpellWave>().SetOwner(player);
        rockAttack.GetComponent<SpellWave>().SetDirection(transform.forward);

        particles.PlayDashParticle();
    }
    public override void Attack(bool value, bool value2)
    {
        Debug.Log("OnAttack, bool = " + value);
        animator.PlayAttack(value);
        attack = value;
    }
    public override void BigAttack(bool value, bool value2)
    {
        if (Time.time <= nextBigAttacktime) return;
        nextBigAttacktime = Time.time + bigCooldown;
        animator.PlayBigAttack();
    }
    public void SendIce()
    {
        GameObject rockAttack = Instantiate(ice, transform.position + (transform.forward - transform.up), transform.rotation);
    }
    public override void Defense()
    {
        if (Time.time <= nextXAttack) return;
        animator.PlayDefense();
        nextXAttack = Time.time + xCooldown;
        GameObject mineAttack = Instantiate(mine, transform.position - 2f * transform.forward, transform.rotation);
        mineAttack.GetComponent<SpellLandMine>().SetOwner(player);
    }

    public override void Zone()
    {
        if (Time.time <= nextBAttack) return;
        nextBAttack = Time.time + bCooldown;
        particles.PlayZoneParticle();
        animator.PlaySelf();
        player.SetInvicible(true);
        /*for (int i = 0; i < materials.Count; i++)
        {
            materials[i] = iceMaterial;
        }*/
        invicibleTimer = Time.time + invicibleTime;
        invicible = true;
    }
    public override void Counter(Player oppo)
    {

    }
}
