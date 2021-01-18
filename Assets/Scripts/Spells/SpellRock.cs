using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellRock : Spell
{
    private Rigidbody body;
    private Player player;
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
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return;
        if (collision.gameObject.GetComponentInParent<Player>() != null && collision.gameObject.GetComponentInParent<Player>() == player) return;

        if (!hasHit) Destroy(gameObject); 
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
}
