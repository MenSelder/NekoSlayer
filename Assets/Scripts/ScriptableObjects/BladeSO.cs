using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu()]
public class BladeSO : ScriptableObject
{
    public string Name;
    public Sprite BladeSprite;
    public ParticleSystem CutParticle;
}
