using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBoomerang : Spell
{
    private Rigidbody body;
    private Player player;
    private List<Player> touched = new List<Player>();
    private WindManager spells;
    private bool hasHit = false;
    private bool canBeReturn = true;
    private bool canBeDestroy = true;
    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(Vector3.up, Mathf.Clamp( Mathf.Abs(body.velocity.magnitude) * 0.08f, 1f , 3f));
        //Detect opponent to hit and stun. 
        //Only once per player
        //Collider[] colliders = Physics.OverlapSphere(transform.position, transform.localScale.x);
        //Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);
        Collider[] colliders = Physics.OverlapCapsule(transform.position - transform.up, transform.position + transform.up, 1f);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.GetComponentInParent<Player>() != null && collider.gameObject.GetComponentInParent<Player>() != player && !FindPlayer(collider.gameObject.GetComponentInParent<Player>()))
            {
                collider.gameObject.GetComponentInParent<Player>().GetHit(body.velocity.magnitude * 2f, player);
                collider.gameObject.GetComponentInParent<Rigidbody>().AddForce(Vector3.up * body.velocity.magnitude * 8f, ForceMode.Impulse);
                touched.Add(collider.gameObject.GetComponentInParent<Player>());

            }
        }
        Collider[] colliders2 = Physics.OverlapSphere(transform.position, 0.1f);
        foreach (Collider collider in colliders2)
        {
            if (collider.gameObject.GetComponentInParent<Player>() != null && collider.gameObject.GetComponentInParent<Player>() == player )
            {
                spells.SetLastBoomerang(null);
                Destroy(gameObject);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position - transform.up, 1f);
        Gizmos.DrawWireSphere(transform.position + transform.up, 1f);
        Gizmos.DrawLine(transform.position - 2f * transform.up, transform.position + 2f * transform.up);
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
    public void SetWindManager(WindManager value)
    {
        spells = value;
    }
    override public bool GetDestroyable()
    {
        return canBeDestroy;
    }
    public void DeleteTouched()
    {
        touched.Clear();
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
}
