using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private Camera cam;

    float baseTime;
    [SerializeField] Image baseImg;

    float bigTime;
    [SerializeField] Image bigImg;

    float dashTime;
    [SerializeField] Image dashImg;

    float zoneTime;
    [SerializeField] Image zoneImg;

    float defTime;
    [SerializeField] Image defImg;

    private void Start()
    {
        cam = FindObjectOfType<Camera>();
    }
    private void FixedUpdate()
    {
        if (cam) transform.rotation = Quaternion.Euler(-45f, 180f, 0f) ;
        else cam = FindObjectOfType<Camera>();


    }

    public void SetBaseTime(float time)
    {
        Debug.Log("PlayerUI, SetBaseTime : time = " + time);
        baseTime = time;
        if (time == 0f) baseImg.color = new Color(0.55f, 0.8f, 0.8f, 0.7f);
        else
        {
            var color = Color.HSVToRGB((1 - time) * 0.32f, 0.8f, 0.8f);
            color = new Color(color.r, color.g, color.b, 0.6f);
            baseImg.color = color;
        }
    }
    public void SetBigTime(float time)
    {
        Debug.Log("PlayerUI, SetBigTime : time = " + time);
        bigTime = time;
        if (time == 0f) bigImg.color = new Color(0.55f, 0.8f, 0.8f, 0.7f);
        else
        {
            var color = Color.HSVToRGB((1 - time) * 0.32f, 0.8f, 0.8f);
            color = new Color(color.r, color.g, color.b, (1 - time) * 0.3f + 0.3f); 
            bigImg.color = color;
        }
    }
    public void SetDashTime(float time)
    {
        dashTime = time;
        if (time == 0f) dashImg.color = new Color(0.55f, 0.8f, 0.8f, 0.7f);
        else
        {
            var color = Color.HSVToRGB((1 - time) * 0.32f, 0.8f, 0.8f);
            color = new Color(color.r, color.g, color.b, (1 - time) * 0.3f + 0.3f); 
            dashImg.color = color;
        }
    }
    public void SetZoneTime(float time)
    {
        zoneTime = time;
        if (time == 0f) zoneImg.color = new Color(0.55f, 0.8f, 0.8f, 0.7f);
        else
        {
            var color = Color.HSVToRGB((1 - time) * 0.32f, 0.8f, 0.8f);
            color = new Color(color.r, color.g, color.b, (1 - time) * 0.3f + 0.3f); 
            zoneImg.color = color;
        }
    }
    public void SetDefTime(float time)
    {
        defTime = time;
        if (time == 0f) defImg.color = new Color(0.55f, 0.8f, 0.8f, 0.7f);
        else
        {
            var color = Color.HSVToRGB((1 - time) * 0.32f, 0.8f, 0.8f);
            color = new Color(color.r, color.g, color.b, (1 - time) * 0.3f + 0.3f); 
            defImg.color = color;
        }
    }
}
