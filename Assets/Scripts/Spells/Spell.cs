using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Spell : MonoBehaviour
{

    abstract public bool GetReturn();
    abstract public bool GetDestroyable();
    abstract public void DestroySpell();
}
