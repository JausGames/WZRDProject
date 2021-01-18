using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellMeteor : Spell
{
    private Rigidbody body;
    private Player player;
    private bool hasHit = false;
    private bool canBeReturn = false;
    private bool canBeDestroy = false;
    private float awakeTime = 0f;
    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody>();
        awakeTime = Time.time;
    }
    private void Update()
    {
        if (!hasHit) gameObject.transform.localScale = new Vector3((Time.time - awakeTime) * 3f, (Time.time - awakeTime ) * 3f, (Time.time - awakeTime) * 3f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        hasHit = true;
        if (collision.gameObject.GetComponentInParent<Player>() != null) collision.gameObject.GetComponentInParent<Player>().GetHit(200f, player);
        Destroy(gameObject, 1f);
    }
    public void SetOwner(Player value)
    {
        player = value;
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
}
