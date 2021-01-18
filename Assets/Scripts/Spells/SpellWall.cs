using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellWall : Spell
{
    private Rigidbody body;
    private Player player;
    private bool canBeReturn = true;
    private bool canBeDestroy = false;
    private float time = 2f;
    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        Destroy(gameObject, time);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<Spell>() != null && collision.gameObject.GetComponentInParent<Spell>().GetDestroyable()) collision.gameObject.GetComponentInParent<Spell>().DestroySpell();
        
    }
    override public void DestroySpell()
    {
        Destroy(gameObject);
    }
    override public bool GetReturn()
    {
        return canBeReturn;
    }
    override public bool GetDestroyable()
    {
        return canBeDestroy;
    }
    public void SetOwner(Player value)
    {
        player = value;
    }
}
