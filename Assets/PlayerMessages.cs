using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MessageSystem))]
public class PlayerMessages : MonoBehaviour
{
    [SerializeField] private Damageable damageable;

    private const string DEFAULT_PHRASE = "СБЭУ!";
    /*
     * Make separated method like:
     * InvokePlayerMessage(PlayerScriptBase player, PhraseType...) ?
     */

    void Start()
    {
        foreach (var player in GameManager.Instance.PlayersList)
        {
            player.GetComponent<Damageable>().OnDamage += Damageable_OnDamage;
            player.GetComponentInChildren<CutEvent2D>().OnStabParried += CutEvent2D_OnStabParried;
            player.GetComponentInChildren<CutEvent2D>().OnStabSuccess += CutEvent2D_OnStabSuccess;
        }

        GameManager.Instance.OnGameLoad += GameManager_OnGameLoad;
        GameManager.Instance.OnGameEnd += GameManager_OnGameEnd;
    }
        
    private void GameManager_OnGameLoad(object sender, System.EventArgs e)
    {
        foreach (var player in GameManager.Instance.PlayersList)
        {
            var playerCharacterSO = player.Character.GetComponent<CharacterScript>().CharacterSO;
            MessageSystem.Instance.PublishMessage(playerCharacterSO.CharacterName, GetRandomPhraseFromListOrDefault(playerCharacterSO.GreetPhrases), playerCharacterSO);
        }
    }

    private void GameManager_OnGameEnd(object sender, System.EventArgs e)
    {
        foreach (var player in GameManager.Instance.PlayersList)
        {
            if (player.GetComponent<Damageable>().IsAlive)
            {
                var playerCharacterSO = player.Character.GetComponent<CharacterScript>().CharacterSO;
                MessageSystem.Instance.PublishMessage(playerCharacterSO.CharacterName, GetRandomPhraseFromListOrDefault(playerCharacterSO.WinPhrases), playerCharacterSO);
            }
        }
    }

    private void CutEvent2D_OnStabSuccess(object sender, CutEvent2D.OnStabSuccessEventArgs e)
    {
        var cutEvent2D = sender as CutEvent2D;
        var playerDamageTaker = cutEvent2D.GetComponentInParent<PlayerScriptBase>();
        var playerDamager = GameManager.Instance.PlayersList.Where(i => i != playerDamageTaker).FirstOrDefault();
        var playerDamagerCharacterSO = playerDamager.Character.GetComponent<CharacterScript>().CharacterSO;

        MessageSystem.Instance.PublishMessage(playerDamagerCharacterSO.CharacterName
            , GetRandomPhraseFromListOrDefault(playerDamagerCharacterSO.MakeDamagePhrases), playerDamagerCharacterSO);
    }

    private void CutEvent2D_OnStabParried(object sender, CutEvent2D.OnStabParriedEventArgs e)
    {
        var cutEvent2D = sender as CutEvent2D;
        var player = cutEvent2D.GetComponentInParent<PlayerScriptBase>();
        var playerCharacterSO = player.Character.GetComponent<CharacterScript>().CharacterSO;

        MessageSystem.Instance.PublishMessage(playerCharacterSO.CharacterName, GetRandomPhraseFromListOrDefault(playerCharacterSO.ParryPhrases), playerCharacterSO);
    }

    private void Damageable_OnDamage(object sender, System.EventArgs e)
    {
        var damageable = sender as Damageable;

        var player = damageable.GetComponent<PlayerScriptBase>();
        var playerCharacterSO = player.Character.GetComponent<CharacterScript>().CharacterSO;


        if (damageable.IsAlive)
        {
            MessageSystem.Instance.PublishMessage(playerCharacterSO.CharacterName
                , $"{GetRandomPhraseFromListOrDefault(playerCharacterSO.TakeDamagePhrases)} У меня осталось {damageable.Health} hp!"
                , playerCharacterSO);
        }
        else
        {
            MessageSystem.Instance.PublishMessage(playerCharacterSO.CharacterName, GetRandomPhraseFromListOrDefault(playerCharacterSO.LosePhrases), playerCharacterSO);

            damageable.OnDamage -= Damageable_OnDamage;
        }
    }

    public string GetRandomPhraseFromListOrDefault(List<string> list) //from GameManager (move to utilities class?)
    {
        if (list.Count <= 0) return DEFAULT_PHRASE;

        int randomIndex = Random.Range(0, list.Count);

        return list[randomIndex];
    }
}
