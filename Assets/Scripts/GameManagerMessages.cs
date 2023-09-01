using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerMessages : MonoBehaviour
{
    private const string SYSTEM_NAME = "System";

    private void Start()
    {
        GameManager.Instance.OnGameLoad += GameManager_OnGameLoad;
        GameManager.Instance.OnStartingPlayerDefinition += Instance_OnStartingPlayerDefinition;
    }

    private void Instance_OnStartingPlayerDefinition(object sender, GameManager.OnStartingPlayerDefinitionEventArgs e)
    {
        var startingPlayerName = e.StartingPlayer.Character.GetComponent<CharacterScript>().CharacterSO.CharacterName;
        MessageSystem.Instance.PublishMessage(SYSTEM_NAME, $"Starting player {startingPlayerName}!");
    }

    private void GameManager_OnGameLoad(object sender, System.EventArgs e)
    {
        MessageSystem.Instance.PublishMessage(SYSTEM_NAME, "GameStarted");
    }
}
