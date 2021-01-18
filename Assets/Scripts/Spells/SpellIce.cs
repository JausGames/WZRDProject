using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellIce : Spell
{
    private Rigidbody body;
    private Player player;
    private bool canBeReturn = false;
    private bool canBeDestroy = false;
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
