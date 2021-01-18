using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellWave : Spell
{
    [SerializeField] private Player player;
    private Rigidbody body;
    private bool hasHit = false;
    private bool canBeReturn = true;
    private bool canBeDestroy = true;
    private float waveSpeed = 15f;
    private Vector3 direction;
    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody>(); 
        Destroy(gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        body.velocity = direction * waveSpeed;
        //Physics.OverlapCapsule(transform.position - 1f * transform.right, transform.position + 1f * transform.right, 2f); 
        Collider[] hitColliders = Physics.OverlapCapsule(transform.position - 2f * transform.right, transform.position + 2f * transform.right, 1f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.GetComponent<Spell>() != null && hitCollider.gameObject != this)
            {
                hitCollider.gameObject.GetComponent<Spell>().DestroySpell();

            }
            if (hitCollider.gameObject.GetComponentInParent<Player>() != null && hitCollider.gameObject.GetComponentInParent<Player>() != player)
            {
                Debug.Log("hitdude, " + hitCollider.gameObject.GetComponentInParent<Player>());
                hitCollider.gameObject.GetComponentInParent<Rigidbody>().velocity = direction * waveSpeed;

            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        
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
    public void SetDirection(Vector3 vect)
    {
        direction = vect;
    }
    override public bool GetDestroyable()
    {
        return canBeDestroy;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position - 2f * transform.right, 1f);
        Gizmos.DrawWireSphere(transform.position + 2f *transform.right, 1f);
        Gizmos.DrawLine(transform.position - 2f * transform.right, transform.position + 2f * transform.right);
    }
}
