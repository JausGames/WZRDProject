using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellTesla : Spell
{
    private Rigidbody body;
    private Player player;
    private List<Player> touched = new List<Player>();
    private bool actived = false;
    private bool canBeReturn = false;
    private bool canBeDestroy = false;
    private float mineForce = 300f;
    [SerializeField] private GameObject arc;
    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody>();
        Destroy(gameObject, 4f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!actived) return;

        Collider[] colliders = Physics.OverlapSphere(transform.position, 7f);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.GetComponentInParent<Player>() != null && collider.gameObject.GetComponentInParent<Player>() != player && !FindPlayer(collider.gameObject.GetComponentInParent<Player>()))
            {
                collider.gameObject.GetComponentInParent<Player>().GetHit(4f, player);
                GameObject a = Instantiate(arc, transform.position, transform.rotation);
                a.GetComponent<TeslaArc>().SetStartPoint(transform.position);
                a.GetComponent<TeslaArc>().SetEndPoint(collider.transform.position);
                touched.Add(collider.gameObject.GetComponentInParent<Player>());

            }
        }
        touched.Clear();
        actived = false;
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
    public void SetActive(bool value)
    {
        actived = value;
    }
    public Player GetOwner()
    {
        return player;
    }
    override public bool GetDestroyable()
    {
        return canBeDestroy;
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
