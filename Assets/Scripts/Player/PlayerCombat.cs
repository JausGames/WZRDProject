using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private bool canMove = false;
    [SerializeField] private SpellManager spells;
    private int playerIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove) return;
        
    }
    public void SetCanMove(bool value)
    {
        canMove = value;
    }
    public bool GetCanMove()
    {
        return canMove;
    }

    public int GetPlayerIndex()
    {
        return playerIndex;
    }
    public void SetPlayerIndex(int nb)
    {
        playerIndex = nb;
    }
    public void Attack(bool value, bool value2)
    {
        if (!canMove) return;
        spells.Attack(value, value2);
        //attack = true;

    }
    public void BigAttack(bool performed, bool canceled)
    {
        if (!canMove) return;
        spells.BigAttack(performed, canceled);
        //attack = true;

    }
    public void Dash(bool value)
    {
        if (!canMove) return;
        spells.Dash(value);
        //dash = true;

    }
    public void Zone()
    {
        if (!canMove) return;
        spells.Zone();

    }
    public void Defense()
    {
        if (!canMove) return;
        spells.Defense();

    }
    public void Counter(Player oppo)
    {
        Debug.Log("PlayerCombat, Counter");
        spells.Counter(oppo);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }
}
