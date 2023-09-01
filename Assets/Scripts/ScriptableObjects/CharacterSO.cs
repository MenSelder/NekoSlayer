using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CharacterSO : ScriptableObject
{
    public string CharacterName;

    [Header("Character Phrases")]
    public List<string> GreetPhrases;
    public List<string> TakeDamagePhrases;
    public List<string> ParryPhrases;
    public List<string> MakeDamagePhrases;
    public List<string> WinPhrases;
    public List<string> LosePhrases;

    [Header("Message Visual")]
    public Color MessageTextColor;
    public Color MessageBackgroundColor;
}
