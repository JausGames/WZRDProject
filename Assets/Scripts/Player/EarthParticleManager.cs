using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthParticleManager : PlayerParticleManager
{
    [SerializeField] private ParticleSystem dashSystem;
    [SerializeField] private ParticleSystem healthSystem;
    [SerializeField] private ParticleSystem zoneSystem;
    [SerializeField] private ParticleSystem landSystem;
    [SerializeField] private ParticleSystem throwTrailsSystem;
    // Start is called before the first frame update
    void Start()
    {
        dashSystem.Stop();
        healthSystem.Stop();
        zoneSystem.Stop();
        landSystem.Stop();
        throwTrailsSystem.Stop();
    }


    public void PlayDashParticle()
    {
        dashSystem.Play();

    }
    public void PlayLandParticle()
    {
        landSystem.Play();
    }
    public override void PlayHealthParticle()
    {
        healthSystem.Play();
    }
    public void PlayZoneParticle()
    {
        zoneSystem.Play();
    }
    public void PlayThrowParticle()
    {
        throwTrailsSystem.Play();
    }
}
