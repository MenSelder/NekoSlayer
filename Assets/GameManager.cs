using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /*
    GameStates:
    waitingToStart
    ChoiceOfStartPlayer

    > loop: {
        Defence turn of chosen player(1)
        attack of player2
        player2 defence 
        player1 attack
    }
    */
    public enum GameState
    {
        WaitStart,
        StartingPlayerDefinition,
        GamePlay,
        GameEnd
    }

    public static GameManager Instance { get; private set; }

    // ?? watch multiplayer course..
    //[SerializeField] private GameObject Player1; 
    //[SerializeField] private GameObject Player2;

    [SerializeField] private List<PlayerScriptBase> playersList;
    public List<PlayerScriptBase> PlayersList => playersList;


    //[SerializeField] private GameObject blade;
    //[SerializeField] private GameObject obsticle;

    public GameState CurrentGameState { get; private set; }

    public event EventHandler OnStateChange; //remake with EventHandler (done)

    public event EventHandler OnGameLoad;
    public event EventHandler OnWaitStart;
    public event EventHandler<OnStartingPlayerDefinitionEventArgs> OnStartingPlayerDefinition;
    public class OnStartingPlayerDefinitionEventArgs : EventArgs
    {
        public PlayerScriptBase StartingPlayer;
    }

    public event EventHandler OnGamePlay;
    public event EventHandler OnGameEnd;

    private float waitStartTimer = 2f;
    public float WaitStartTimer => waitStartTimer;

    private float startingPlayerDefinitionTimer = 2f;
    public float StartingPlayerDefinitionTimer => startingPlayerDefinitionTimer;


    private void Awake()
    {
        Instance = this;

        CurrentGameState = GameState.WaitStart;
    }

    private void Start()
    {
        foreach (PlayerScriptBase player in playersList)
        {
            player.OnEndTurn += Player_OnEndTurn;
            player.GetComponentInChildren<CutEvent2D>().OnStab += CutEvent2D_OnStab;
        }

        OnGameLoad?.Invoke(this, EventArgs.Empty);

        // wait until all players connected and ready
        WaitStart();
    }

    private void CutEvent2D_OnStab(object sender, EventArgs e)
    {
        if (CurrentGameState != GameState.GamePlay) return;

        foreach (PlayerScriptBase player in playersList)
        {
            if (player.CurrentPlayerState == PlayerScriptBase.PlayerState.Attack)
            {
                player.SetPlayerState(PlayerScriptBase.PlayerState.Defence); // Attack -> Defence
                continue;
            }

            player.SetPlayerState(PlayerScriptBase.PlayerState.Wait); // other -> Wait
        }
    }

    private void Player_OnEndTurn(object sender, EventArgs e)
    {
        if (CurrentGameState != GameState.GamePlay) return;

        var playerSender = sender as PlayerScriptBase;

        switch (playerSender.CurrentPlayerState) //remake?
        {
            case PlayerScriptBase.PlayerState.Defence:
                // (me) sender -> wait; other player -> attack
                playerSender.SetPlayerState(PlayerScriptBase.PlayerState.Wait);
                playersList.Where(i => i != playerSender).ToList().ForEach((i) =>
                {
                    i.SetPlayerState(PlayerScriptBase.PlayerState.Attack);
                });
                break;
            default: break;
        }
    }


    private void Update()
    {
        switch (CurrentGameState)
        {
            case GameState.WaitStart:
                waitStartTimer -= Time.deltaTime;

                if (waitStartTimer > 0) return;
                
                SetGameState(GameState.StartingPlayerDefinition);                
                break;
            case GameState.StartingPlayerDefinition:
                startingPlayerDefinitionTimer -= Time.deltaTime;

                if (startingPlayerDefinitionTimer > 0) return;

                SetGameState(GameState.GamePlay);
                break;
            case GameState.GamePlay:

                //exchange this on event like "Damagable.OnDamage if anyone dead"...
                var deadList = playersList
                    .Where(i => { return i.GetComponent<Damageable>().IsDead; })
                    .ToList();
                if (deadList.Count <= 0) return;
                
                SetGameState(GameState.GameEnd);
                break;
            case GameState.GameEnd:
                break;
            default:
                break;
        }
    }

    public void SetGameState(GameState gameState)
    {
        if (gameState == CurrentGameState) return;

        switch (gameState)
        {
            case GameState.WaitStart:
                WaitStart();
                break;
            case GameState.StartingPlayerDefinition:
                StartingPlayerDefinition();
                break;
            case GameState.GamePlay:
                GamePlay();
                break;
            case GameState.GameEnd:
                GameEnd();
                break;
            default:
                throw new NotImplementedException();
        }

        CurrentGameState = gameState;
        OnStateChange?.Invoke(this, EventArgs.Empty);
    }

    private void WaitStart()
    {
        //set all players wait mode
        foreach (PlayerScriptBase player in playersList)
        {
            player.SetPlayerState(PlayerScriptBase.PlayerState.Wait);
        }

        OnWaitStart?.Invoke(this, EventArgs.Empty);
    }

    private void StartingPlayerDefinition()
    {
        PlayerScriptBase startingPlayer = GetRandomFromList(playersList);
        //startingPlayer = PlayerMain.Instance; //TEST, delete later (playerMain always first)

        startingPlayer.SetPlayerState(PlayerScriptBase.PlayerState.Defence);

        OnStartingPlayerDefinition?.Invoke(this, new OnStartingPlayerDefinitionEventArgs { StartingPlayer = startingPlayer });
    }

    public T GetRandomFromList<T>(List<T> list)
    {
        int randomIndex = UnityEngine.Random.Range(0, list.Count);

        return list[randomIndex];
    }

    private void GamePlay()
    {
        OnGamePlay?.Invoke(this, EventArgs.Empty);
    }

    private void GameEnd()
    {
        foreach(PlayerScriptBase player in playersList)
        {
            if (player.GetComponent<Damageable>().IsAlive)
            {
                player.SetPlayerState(PlayerScriptBase.PlayerState.Attack);
                continue;
            }

            player.SetPlayerState(PlayerScriptBase.PlayerState.Wait);
        }


        OnGameEnd?.Invoke(this, EventArgs.Empty);
    }
}
