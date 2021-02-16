using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellRock : Spell
{
    private Rigidbody body;
    [SerializeField] ParticleSystem throwParticle;
    [SerializeField] ParticleSystem contactParticle;
    private Collider col;
    private Player player;
    [SerializeField] private Rigidbody[] dividedBody;
    private bool hasHit = false;
    private bool canBeReturn = true;
    private bool canBeDestroy = true;
    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        dividedBody = GetComponentsInChildren<Rigidbody>();

        for (int i = 1; i < dividedBody.Length; i++)
        {
            var cols = dividedBody[i].gameObject.GetComponents<Collider>();
            foreach(Collider c in cols) Physics.IgnoreCollision(col, c);

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return;
        contactParticle.Play();
        throwParticle.Stop();
        if (collision.gameObject.GetComponentInParent<Player>() == player) return;

        if (!hasHit) Dismember(); 
        hasHit = true;
        if (collision.gameObject.GetComponentInParent<Player>() != null 
            && collision.gameObject.GetComponentInParent<Player>() != player)
        {
            collision.gameObject.GetComponentInParent<Player>().GetHit(50f, player);
        }
    }
    override public void DestroySpell()
    {
        Destroy(gameObject);
    }
    override public bool GetReturn()
    {
        return canBeReturn;
    }
    public void SetOwner(Player value)
    {
        player = value;
    }
    override public bool GetDestroyable()
    {
        return canBeDestroy;
    }
    private void Dismember()
    {
        var vel = body.velocity;
        Destroy(body);
        Destroy(col);
        for (int i = 1; i < dividedBody.Length; i++)
        {
            dividedBody[i].isKinematic = false;
            dividedBody[i].velocity = vel;
        }
        Destroy(this.gameObject, 3f);
    }
}
