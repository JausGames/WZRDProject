using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class AnimatorController : MonoBehaviour
{

    abstract public void ToIdle();
    abstract public void WalkForward();
    abstract public void WalkBackward();
    abstract public void WalkLeft();
    abstract public void WalkRight();


    abstract public void Die();
    abstract public void Revive();
    abstract public void Spawn();
}
