using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using UnityEngine.UIElements;

public class SliceVisualEffects : MonoBehaviour
{

    private CutEvent2D cutEvent2D;
    private ParticleSystem parringParticle;

    private Damageable damageable;

    private void Awake()
    {
        cutEvent2D = GetComponent<CutEvent2D>();

        PlayerScriptBase playerScriptBase = GetComponentInParent<PlayerScriptBase>();
        ParryingDaggerScript parryingDaggerScript = playerScriptBase.ParryDagger.GetComponent<ParryingDaggerScript>();
        parringParticle = parryingDaggerScript.ParryingDaggerSO.ParringParticle;

        damageable = GetComponentInParent<Damageable>();
    }

    void Start()
    {
        cutEvent2D.OnStabParried += CutEvent2D_OnStabParried; //sparks onParrying
        //cutEvent2D.OnStabSuccess += CutEvent2D_OnStabSuccess; //blood ... save bloodSytem?

        damageable.OnDamage += Damageable_OnDamage; // unfreeze rotation on death
    }

    private void CutEvent2D_OnStabSuccess(object sender, CutEvent2D.OnStabSuccessEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void CutEvent2D_OnStabParried(object sender, CutEvent2D.OnStabParriedEventArgs e)
    {
        var particle = Instantiate(parringParticle, e.IntersectionPoint, Quaternion.identity);
        particle.Play();

    }

    private void Damageable_OnDamage(object sender, System.EventArgs e) //maybe trash! (ok)
    {
        /*
         * every new object also subscribe on this
         * enough to invoke this once
         * 
         * NEED TO FIND SOLUTION ??? (NO)
        */
        if (damageable.IsAlive) return;

        var rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.constraints = RigidbodyConstraints2D.None;

        damageable.OnDamage -= Damageable_OnDamage;
    }
}
