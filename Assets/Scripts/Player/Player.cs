using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private bool isAlive = true;
    [SerializeField] private bool invicible = false;
    [SerializeField] private bool counter = false;

    [SerializeField] public AnimatorController animator;
    [SerializeField] private bool paralysed = false;
    [SerializeField] private float paralysedTime = 0.7f;
    [SerializeField] private float paralysedTimer;
    [SerializeField] private float counterTime = 0.7f;
    [SerializeField] private float counterTimer;
    [SerializeField] private float counterDamage = 20f;
    [SerializeField] private float health = 200f;
    [SerializeField] private float playerIndex = 0;
    [SerializeField] public PlayerController controller = null;
    [SerializeField] public PlayerCombat combat = null;
    [SerializeField] public Healthometer healthometer = null;
    [SerializeField] public Transform visual;
    [SerializeField] public PlayerParticleManager particles;
    [SerializeField] public Renderer hat;
    [SerializeField] public Rigidbody body;
    [SerializeField] private CapsuleCollider inGameCollider;
    [SerializeField] private Collider[] ragdollColliders;
    [SerializeField] private Rigidbody[] ragdollRigidbodies;
    [SerializeField] private List<Vector3> ragdollPosition= new List<Vector3>();
    [SerializeField] private List<Quaternion> ragdollRotation = new List<Quaternion>();
    // Start is called before the first frame update
    void Awake()
    {
        visual = transform.Find("Visual");
        particles = visual.GetComponentInChildren<PlayerParticleManager>();
        controller = GetComponent<PlayerController>();
        combat = GetComponent<PlayerCombat>();
        body = GetComponent<Rigidbody>();
        inGameCollider = GetComponent<CapsuleCollider>();
        ragdollColliders = visual.GetComponentsInChildren<Collider>();
        ragdollRigidbodies = visual.GetComponentsInChildren<Rigidbody>();
        if (inGameCollider) inGameCollider.enabled = true;
        if (ragdollColliders.Length > 1)
        {
            foreach (Collider col in ragdollColliders) col.enabled = false;
        }
        if (ragdollRigidbodies.Length > 1)
        {
            foreach (Rigidbody bod in ragdollRigidbodies) { bod.isKinematic = true; ragdollPosition.Add(bod.transform.localPosition); ragdollRotation.Add(bod.transform.localRotation); }
        }
        
       /*  ActiveRagdoll(true);
         
        if (animator != null) animator.Die();*/
         

    }
    public void SetHatColor(Material mat)
    {
        hat.material = mat;
    }
    private void Update()
    {
        if (health == 0) Die();
        if (counter && Time.time > counterTimer)
        {
            counter = false;
            SetInvicible(false);
        }
        if (transform.position.y < -5) Die();
        if(paralysed && Time.time <= paralysedTimer)
        {
            combat.SetCanMove(false);
            controller.SetCanMove(false);
        }
        if (paralysed && Time.time > paralysedTimer)
        {
            combat.SetCanMove(true);
            controller.SetCanMove(true);
            paralysed = false;
        }
    }

    public bool GetAlive()
    {
        return isAlive;
    }
    public void SetAlive(bool value)
    {
        isAlive = value;
    }
    public void SetCanMove(bool value)
    {
        combat.SetCanMove(value);
        controller.SetCanMove(value);
    }
    public void SetInvicible(bool value)
    {
        invicible = value;
        body.isKinematic = value;
        controller.SetCanMove(!value);
        combat.SetCanMove(!value);


    }
    public float GetHealth()
    {
        return health;
    }
    public void ResetHealth()
    {
        health = 200f;
    }
    public bool GetHit(float damage, Player opponent)
    {
        if (counter)
        {
            if (animator != null) opponent.GetHit(counterDamage, this);
            combat.Counter(opponent);
        }
        if (invicible) return false;
        if (health > damage) { health -= damage; return true; }
        
        Die();
        return false;
    }
    private void ActiveRagdoll(bool value)
    {
        if (ragdollColliders.Length > 1)
        {
            foreach (Collider col in ragdollColliders) col.enabled = value;
        }
        if (ragdollRigidbodies.Length > 1)
        {
            foreach (Rigidbody bod in ragdollRigidbodies) bod.isKinematic = !value;
        }
        if (inGameCollider != null) { inGameCollider.enabled = !value; body.isKinematic = value; }
    }
    private void Die()
    {
        /*if (inGameCollider != null) inGameCollider.enabled = false;
        ActiveRagdoll(true);
        if (animator != null) animator.Die();*/
        if (animator != null) animator.Die();
        ActiveRagdoll(true);
        health = 0f;
        isAlive = false;
        controller.SetCanMove(false);
        combat.SetCanMove(false);
        controller.SetFreeze(false);
    }
    public void Revive()
    {
        if (animator != null) animator.Revive();
        for (int i = 0; i < ragdollPosition.Count; i++)
        {
            ragdollRigidbodies[i].gameObject.transform.localPosition = ragdollPosition[i];
            ragdollRigidbodies[i].gameObject.transform.localRotation = ragdollRotation[i];
        }
        ActiveRagdoll(false);
    }
    public void PlaySpawnAnim()
    {
        if (animator != null) animator.Spawn();
    }
    public void SetPlayerIndex(int value)
    {
        playerIndex = value;
        controller.SetPlayerIndex(value);
        combat.SetPlayerIndex(value);
    }
    public void StopCounter()
    {
        counter = false;
        SetInvicible(false);
    }
    public void SetCounter(bool boo)
    {
        SetInvicible(boo);
        counter = boo;
        counterTimer = Time.time + counterTime;
    }
    public void SetCounter(float time)
    {
        SetInvicible(true);
        counter = true;
        counterTimer = Time.time + time;
    }
    public void SetParalyse()
    {
        paralysed = true;
        paralysedTimer = Time.time + paralysedTime;
    }
    public void StopMotion()
    {
        controller.StopMotion();
        controller.SetFreeze(true);
    }
    public void AddHealth(float value)
    {
        health += value;
        particles.PlayHealthParticle();
    }

}
