using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindParticleManager : PlayerParticleManager
{
    [SerializeField] private ParticleSystem dashSystem;
    [SerializeField] private ParticleSystem healthSystem;
    [SerializeField] private ParticleSystem zoneSystem;
    [SerializeField] private ParticleSystem rockDashSystem;
    // Start is called before the first frame update
    void Start()
    {
        dashSystem.Stop();
        healthSystem.Stop();
        zoneSystem.Stop();
        rockDashSystem.Stop();
    }
    private void Update()
    {
        
    }

    public void PlayDashParticle()
    {
        dashSystem.Play();
        rockDashSystem.Play();

    }
    public override void PlayHealthParticle()
    {
        healthSystem.Play();
    }
    public void PlayZoneParticle()
    {
        zoneSystem.Play();
    }
}
