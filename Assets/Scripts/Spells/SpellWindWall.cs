using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellWindWall : Spell
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
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position + transform.up * 2f, new Vector3(5f, 3f, 1f), Quaternion.identity);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.GetComponent<Spell>() != null && hitCollider.gameObject.GetComponent<Spell>().GetReturn())
            {
                hitCollider.gameObject.GetComponent<Rigidbody>().AddForce(-hitCollider.gameObject.GetComponent<Rigidbody>().velocity * hitCollider.gameObject.GetComponent<Rigidbody>().mass, ForceMode.Impulse);
            }
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
    override public bool GetDestroyable()
    {
        return canBeDestroy;
    }
    public void SetOwner(Player value)
    {
        player = value;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(gameObject.transform.position + transform.up * 2f, new Vector3(5f, 3f, 1f));
    }
}
