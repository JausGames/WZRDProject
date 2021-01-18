using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxingManager : SpellManager
{
    [SerializeField]public BoxingParticleManager particles;
    [SerializeField] private BoxingAnimatorController animator;
    [SerializeField] private PlayerController controller;
    [SerializeField] private Player player;
    private Rigidbody body = null;

    [SerializeField] private bool dash = false;
    [SerializeField] private bool combo = false;
    [SerializeField] private bool attack = false;
    [SerializeField] private bool inputBuff = false;
    [SerializeField] private bool inCombo = false;
    const int DASH_NUMBER = 2;
    [SerializeField] private int dashNumber = 2;
    [SerializeField] private bool shield = false;
    [SerializeField] private bool invicible = false;
    [SerializeField] private List<Vector2> directions = new List<Vector2>(); 

    private float nextDashtime;
    private float dashCooldown = 1.4f;
    private float closedDash;
    private float dashCadency = 0.2f;
    private float dashForce = 350f;
    private Vector3 dashDir;

    private float nextAttacktime;
    private float attackCadency = 2f;
    private float attackTime;
    private float cooldown = .8f;

    private float nextBigAttacktime;
    private float bigCooldown = 5f;
    private float bigCadency = 3f;
    private float bigTime;

    private float nextXAttack;
    private float xCooldown = 2f;

    private float nextBAttack;
    private float bCooldown = 2f;
    private float invicibleTime = 1f;
    private float invicibleTimer;

    private bool rightHand;
    [SerializeField] private Transform rightHandTransform;
    [SerializeField] private Transform leftHandTransform;
    private Vector3 attackdir;
    private float attackdamage;
    private float attackforce;
    private float attackRadius;
    private List<Player> touched = new List<Player>();



    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody>();
        player = GetComponent<Player>();
        controller = GetComponent<PlayerController>();
        animator = GetComponent<BoxingAnimatorController>();
        directions.Add(new Vector2(0, 1));
        directions.Add(new Vector2(1, 0));
        directions.Add(new Vector2(-1, 0));
        directions.Add(new Vector2(0, -1));
    }

    // Update is called once per frame
    void Update()
    {


        if (attack)
        {
            Collider[] colliders = null;
            if (rightHand) { colliders = Physics.OverlapSphere(rightHandTransform.position, attackRadius); }
            else { colliders = Physics.OverlapSphere(leftHandTransform.position, attackRadius); }
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.GetComponentInParent<Player>() != null && collider.gameObject.GetComponentInParent<Player>() != player && !FindPlayer(collider.gameObject.GetComponentInParent<Player>()))
                {
                    touched.Add(collider.gameObject.GetComponentInParent<Player>());
                    collider.gameObject.GetComponentInParent<Player>().GetHit(attackdamage, player);
                    collider.gameObject.GetComponentInParent<Rigidbody>().AddForce(attackdir * attackforce, ForceMode.Impulse);
                }
            }
        }

        if (invicible && Time.time > invicibleTimer)
        {
            invicible = false;
            player.SetInvicible(false);
        }

        if (Time.time >= nextDashtime)
        {
            if (dashNumber == DASH_NUMBER) return;
            dashNumber++;
            nextDashtime = Time.time + dashCooldown;
        }
    }
    private void OnDrawGizmos()
    {
        if (!attack) return;
        Gizmos.color = Color.red;
        if (rightHand )Gizmos.DrawWireSphere(rightHandTransform.position, attackRadius);
        if (!rightHand) Gizmos.DrawWireSphere(leftHandTransform.position, attackRadius);
    }
    public override void Dash(bool value)
    {
        if (dashNumber <= 0 || closedDash >= Time.time) return;
        if (dashNumber == 2) nextDashtime = Time.time + dashCooldown;
        closedDash = Time.time + dashCadency;
        dashDir = transform.forward;
        dashNumber --;
        Debug.Log("BoxingManager, Dash : move");
        var move = FindClosest(controller.GetMove(), directions);
        Debug.Log("BoxingManager, Dash : dir");
        var dir = FindClosest(controller.GetLook(), directions);
        if (move == dir || controller.GetMove().magnitude <= 0.2f || controller.GetLook().magnitude <= 0.2f)
        {
            move = transform.forward;
            Debug.Log("BoxingManager, Dash Direction : Forward");
            animator.DashForward();
        }
        if (move == -dir)
        {
            Debug.Log("BoxingManager, Dash Direction : Backward");
            animator.DashBackward();
        }
        if (controller.CheckRightDash(move, dir))
        {
            Debug.Log("BoxingManager, Dash Direction : Right");
            animator.DashRight();
        }
        if (!controller.CheckRightDash(move, dir))
        {
            Debug.Log("BoxingManager, Dash Direction : Left");
            animator.DashLeft();
        }
        particles.PlayDashParticle(new Vector3(move.x, 0.5f, move.y));
        body.AddForce(new Vector3(move.x, 0f, move.y).normalized * 200f, ForceMode.Impulse);



    }
    public void Hit(bool right, float damage, Vector3 dir, float force, float radius)
    {
        rightHand = right;
        attackdamage = damage;
        attackdir = dir;
        attackforce = force;
        attackRadius = radius;
        attack = true;
    }
    public void StopHit()
    {
        touched.Clear();
        attack = false;
    }
    public void SetCanMove(bool value)
    {
        player.SetCanMove(value);
    }
    public override void Attack(bool value, bool value2)
    {
        Debug.Log("Boxing Manager, Attack : performed = " + value2);
        //if (inputBuff && value2) { SetCombo(true); SetBuff(false); }

        if (Time.time <= attackTime || inputBuff) return;
        attackTime = Time.time + attackCadency;
        player.SetCanMove(false);
        animator.PlayAttack(1);
//inCombo = true;
    }
    public override void BigAttack(bool value, bool value2)
    {
        if (value)
        {
            if (Time.time <= nextBigAttacktime) return;
            attackTime = Time.time;
            player.SetCanMove(false);
            animator.PlayBigAttack();

        }
    }
    public override void Defense()
    {
        if (Time.time <= nextXAttack) return;
        nextXAttack = Time.time + xCooldown;
        player.SetCanMove(false);
        animator.PlayDashAttack();
    }
    public bool GetCombo()
    {
        return combo;
    }

    public void SetCombo(bool value)
    {
        combo = value;
    }
    public void SetBuff(bool value)
    {
        inputBuff = value;
    }
    public override void Zone()
    {
        if (Time.time <= nextBAttack) return;
        nextBAttack = Time.time + bCooldown;
        player.SetCanMove(false);
        particles.PlayZoneParticle();
        animator.PlayCounterAttack();
    }
    public override void Counter(Player oppo)
    {
        player.SetCanMove(false);
        transform.position = oppo.transform.position - oppo.transform.forward;
        transform.rotation = oppo.transform.rotation;
        animator.PlayCounterSucessAttack();
    }
    public void PlayBigAttackParticle()
    {
        particles.PlayBigParticle();
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
    private Vector3 FindClosest(Vector2 vect, List<Vector2> vects )
    {
        List<float> range = new List<float>();
        for (var i = 0; i < vects.Count; i++)
        {
            Debug.Log("BoxingManager, FindClosest : vetcs[i] = " + i + " " + vects[i]);
            range.Add(Vector2.Distance(vects[i], vect));
            Debug.Log("BoxingManager, FindClosest : range[i] = " + i + " " + range[i]);
        }
        float closest = Mathf.Infinity;
        int nb = 0;
        for (var i = 0; i < range.Count; i++)
        {
            if (range[i] < closest)
            {
                closest = range[i];
                nb = i;
            }
        }
        return vects[nb];
    }
}
