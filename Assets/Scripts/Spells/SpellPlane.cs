using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellPlane : Spell
{
    private Rigidbody body;
    private Player player;
    private List<Player> seen = new List<Player>();
    private bool hasHit = false;
    private bool canBeReturn = true;
    private bool canBeDestroy = true;
    private float jetForce = 2500f;
    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 10f);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.GetComponentInParent<Player>() != null && collider.gameObject.GetComponentInParent<Player>() != player && !FindPlayer(collider.gameObject.GetComponentInParent<Player>()))
            {
                seen.Add(collider.gameObject.GetComponentInParent<Player>());
            }
        }
        if (seen.Count > 0)
        {
            var dir = new Vector3(seen[0].transform.position.x - transform.position.x, 0f, seen[0].transform.position.z - transform.position.z).normalized / 6f;
            var localX = dir.z * transform.forward.x + dir.x * transform.forward.z;
            var localZ = dir.z * transform.right.x + dir.x * transform.right.z;
            var local = (dir.x + dir.z) * localX * transform.forward + (dir.x + dir.z) * localZ * transform.right;
            body.AddForce(dir, ForceMode.Impulse);
        }
        seen.Clear();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<Player>() != null && collision.gameObject.GetComponentInParent<Player>() == player) return;
        if (!hasHit) Destroy(gameObject, 0.2f);
        hasHit = true;
        if (collision.gameObject.GetComponentInParent<Player>() != null && collision.gameObject.GetComponentInParent<Player>() != player) collision.gameObject.GetComponentInParent<Player>().GetHit(20f, player);

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
    private bool FindPlayer(Player player)
    {
        if (seen == null) return false;
        foreach (Player p in seen)
        {
            if (p == player) return true;
        }
        return false;
    }
}
