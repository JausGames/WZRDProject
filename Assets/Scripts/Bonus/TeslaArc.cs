using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaArc : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    private void Awake()
    {
        Destroy(gameObject, 0.3f);
    }
    public void SetEndPoint(Vector3 vect)
    {
        endPoint.position = vect;
    }
    public void SetStartPoint(Vector3 vect)
    {
        startPoint.position = vect;
    }
}
