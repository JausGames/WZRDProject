﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

abstract public class SpellManager : MonoBehaviour
{
    [SerializeField] protected Sprite picture;
    abstract public void Dash(bool perfomed, bool canceled);
    abstract public void Counter(Player oppo);
    abstract public void Attack(bool perfomed, bool canceled);
    abstract public void BigAttack(bool perfomed, bool canceled);
    abstract public void Defense();
    abstract public void Zone();

    public Sprite GetPicture()
    {
        return picture;
    }
}
