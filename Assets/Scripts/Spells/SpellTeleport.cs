using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellTeleport : Spell
{
    [SerializeField] private Player player;
    private bool hasHit = false;
    private bool canBeReturn = false;
    private bool canBeDestroy = false;
    private Vector3 direction;
    // Start is called before the first frame update
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * 5f * Time.deltaTime;
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
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 2f);
    }
}
