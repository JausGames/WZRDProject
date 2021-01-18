using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthManager : SpellManager
{
    [SerializeField] private EarthAnimatorController animator;
    [SerializeField] private EarthParticleManager particles;
    private Rigidbody body = null;
    [SerializeField] private Player player;
    [SerializeField] private GameObject rock;
    [SerializeField] private GameObject meteor;
    [SerializeField] private GameObject wall;
    [SerializeField] private Transform powerBall;
    [SerializeField] private bool shield = false;
    [SerializeField] private bool invicible = false;
    private float nextDashtime;
    private float dashCooldown = 2f;
    private float dashForce = 70f;
    private float nextAttacktime;
    private float cooldown = .8f;
    private float rockForce = 50f;
    private float nextBigAttacktime;
    private float bigCooldown = 5f;
    private float nextXAttack;
    private float xCooldown = 2f;
    private float nextBAttack;
    private float bCooldown = 2f;
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
                    hitCollider.gameObject.GetComponent<Rigidbody>().velocity = -0.7f * hitCollider.gameObject.GetComponent<Rigidbody>().velocity;
                }
            }
            if (nextBAttack <= Time.time) shield = false;
        }
    }
    public override void Dash(bool value)
    {
        if (Time.time <= nextDashtime) return;
        animator.PlayDash();
        nextDashtime = Time.time + dashCooldown;
        particles.PlayDashParticle();
        body.AddForce((transform.forward + transform.up * 3f) * dashForce, ForceMode.Impulse);
    }
    public override void Attack(bool value, bool value2)
    {
        if (Time.time <= nextAttacktime) return;
        animator.PlayAttack();
        player.SetCanMove(false);

    }
    public void ThrowRock()
    {
        nextAttacktime = Time.time + cooldown;
        GameObject rockAttack = Instantiate(rock, powerBall.position, transform.rotation);
        particles.PlayThrowParticle();
        rockAttack.GetComponent<SpellRock>().SetOwner(player);
        rockAttack.GetComponent<Rigidbody>().AddForce((transform.forward * 10f + Vector3.up) * rockForce, ForceMode.Impulse);
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
}
