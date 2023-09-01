using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ParryingDaggerVisual : MonoBehaviour
{

    [SerializeField] private ParryingDaggerSO parryingDaggerSO_Default;

    public ParryingDaggerSO ParryingDaggerSO_Current { get; private set; }

    public ParticleSystem Particle { get; private set; }
    
    public void SetParryingDaggerSO(ParryingDaggerSO parryingDaggerSO)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = parryingDaggerSO.BladeSprite ?? parryingDaggerSO_Default.BladeSprite;
        Particle = parryingDaggerSO.ParringParticle ?? parryingDaggerSO_Default.ParringParticle;

        ParryingDaggerSO_Current = parryingDaggerSO;
    }
}
