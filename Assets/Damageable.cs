using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Damageable : MonoBehaviour
{

    public event EventHandler OnDamage;
    public event EventHandler OnHeal;

    public float Health {  get; private set; }

    private float healthMax = 3f;

    public bool IsAlive => Health > 0;
    public bool IsDead => Health <= 0;

    public void Damage(float damage)
    {
        Health -= damage;
        if (Health < 0) Health = 0;

        OnDamage?.Invoke(this, EventArgs.Empty);
    }
    public void Heal(float heal)
    {
        Health += heal;
        if (Health > healthMax) Health = healthMax;

        OnHeal?.Invoke(this, EventArgs.Empty);
    }

    public void Start()
    {
        Health = healthMax;
    }

}
