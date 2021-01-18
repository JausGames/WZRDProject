using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellThunderCharge : Spell
{
    [SerializeField] private Player player;
    private bool charged = false;
    private bool canBeReturn = false;
    private bool canBeDestroy = false;
    private List<Player> touched = new List<Player>();
    private float awakeTime;
    private float speed = 50f;
    [SerializeField] ParticleSystem thunder;
    // Start is called before the first frame update
    void Awake()
    {
        awakeTime = Time.time;
        thunder = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {   
        if (!charged)
        {
            //Property changes over time while charging  
            transform.position = player.transform.position + player.transform.forward * 2f;
            transform.rotation = player.transform.rotation;
            gameObject.transform.localScale = new Vector3((Time.time - awakeTime) * 0.9f + 0.2f, (Time.time - awakeTime) * 0.9f +0.2f, (Time.time - awakeTime) * 0.9f +0.2f);

            //Particles changes over time while charging 
            ParticleSystem.MainModule mm = thunder.GetComponent<ParticleSystem>().main;
            mm.startSize = gameObject.transform.localScale.x * 0.4f;

            ParticleSystem.EmissionModule em = thunder.GetComponent<ParticleSystem>().emission;
            em.rateOverTime = gameObject.transform.localScale.x * 300f;

            ParticleSystem.ShapeModule ps = thunder.GetComponent<ParticleSystem>().shape;
            ps.radius = gameObject.transform.localScale.x / 2.2f;

            //Stop charging after 2 seconds

            if (Time.time - awakeTime >= 1.9f)
            {
                player.GetComponent<ThunderManager>().PlayChargeAnim();
            }
            return; 
        }
        //Detect opponent to hit and stun. 
        //Only once per player
        Collider[] colliders = Physics.OverlapSphere(transform.position, transform.localScale.x);
        foreach(Collider collider in colliders)
        {
            if (collider.gameObject.GetComponentInParent<Player>() != null && collider.gameObject.GetComponentInParent<Player>() != player && !FindPlayer(collider.gameObject.GetComponentInParent<Player>()))
            {
                collider.gameObject.GetComponentInParent<Player>().GetHit(gameObject.transform.localScale.x * 50f, player);
                    collider.gameObject.GetComponentInParent<Player>().SetParalyse();
                touched.Add(collider.gameObject.GetComponentInParent<Player>());
                
            }
        }
        transform.position += transform.forward * speed * Time.deltaTime;
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
    public void StopCharging()
    {
        charged = true;
    }
    override public bool GetDestroyable()
    {
        return canBeDestroy;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, transform.localScale.x);
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
