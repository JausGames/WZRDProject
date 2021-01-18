using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellLandMine : Spell
{
    private Rigidbody body;
    private Player player;
    private bool hasHit = false;
    private bool canBeReturn = false;
    private bool canBeDestroy = false;
    private float mineForce = 300f;
    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody>();
        Destroy(gameObject, 4f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<Player>() != null)
        {
            if (!hasHit) Destroy(gameObject, 0.1f);
            hasHit = true;
            collision.gameObject.GetComponentInParent<Rigidbody>().AddForce(transform.up * mineForce, ForceMode.Impulse);
        }
        if (collision.gameObject.GetComponentInParent<Player>() != null
            && collision.gameObject.GetComponentInParent<Player>() != player)
        {
            collision.gameObject.GetComponentInParent<Player>().GetHit(75f, player);
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
}
