using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxingParticleManager : PlayerParticleManager
{
    [SerializeField] private ParticleSystem dashSystem;
    [SerializeField] private ParticleSystem healthSystem;
    [SerializeField] private ParticleSystem zoneSystem;
    [SerializeField] private ParticleSystem rockDashSystem;
    [SerializeField] private ParticleSystem bigAttackSystem;
    [SerializeField] private ParticleSystem combo1Particle;
    [SerializeField] private ParticleSystem combo2Particle;
    [SerializeField] private ParticleSystem combo3Particle;
    // Start is called before the first frame update
    void Start()
    {
        dashSystem.Stop();
        healthSystem.Stop();
        zoneSystem.Stop();
        rockDashSystem.Stop();
        bigAttackSystem.Stop();
        combo1Particle.Stop();
        combo2Particle.Stop();
        combo3Particle.Stop();
    }
    private void Update()
    {
        
    }

    public void PlayDashParticle(Vector3 dir)
    {
        dashSystem.transform.LookAt(dir);
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
    public void PlayBigParticle()
    {
        bigAttackSystem.Play();
    }
     public void PlayCombo1Particle()
    {
        combo1Particle.Play();
    }
    public void PlayCombo2Particle()
    {
        combo2Particle.Play();
    }
    public void PlayCombo3Particle()
    {
        combo3Particle.Play();
    }
}
