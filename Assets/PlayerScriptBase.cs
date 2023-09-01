using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerScriptBase : MonoBehaviour
{
    //used only at Starting..UI
    //public static PlayerScriptBase Instance { get; private set; } //make local

    //remake interact with dagger (use dagger States) !!


    [SerializeField] protected GameObject character;
    public GameObject Character => character;

    [SerializeField] protected GameObject parryDagger;
    public GameObject ParryDagger => parryDagger;

    [SerializeField] protected GameObject blade;
    public GameObject Blade => blade;

    [SerializeField] protected CinemachineVirtualCamera virtualCamera;

    public event EventHandler OnWait;
    public event EventHandler OnDefence;
    public event EventHandler OnAttack;
    public event EventHandler OnEndTurn;

    public enum PlayerState
    {
        Wait,
        Defence,
        Attack
    }

    public PlayerState CurrentPlayerState { get; private set; }

    private void Awake()
    {
        //Instance = this;

        CurrentPlayerState = PlayerState.Wait;
    }

    public void SetPlayerState(PlayerState playerState)
    {
        switch (playerState)
        {
            case PlayerState.Wait:
                Wait();
                break;
            case PlayerState.Defence:
                Defence();
                break;
            case PlayerState.Attack:
                Attack();
                break;
            default: 
                Debug.LogError("playerState not implement");
                break;
        }

        CurrentPlayerState = playerState;
    }

    private void Wait()
    {
        // All disabled (blade, dagger, endTurnButton)

        // ! server will manage it:
        //if Enemy Attack -> look at yourself
        //if Enemy Defence -> look at yourself

        // look at enemy possible only when you Attack!!!

        virtualCamera.gameObject.SetActive(true);

        blade.SetActive(false);
        parryDagger.SetActive(false);

        OnWait?.Invoke(this, EventArgs.Empty);
    }

    private void Defence()
    {
        // blade - disable
        // dagger - enable

        virtualCamera.gameObject.SetActive(true);

        blade.SetActive(false);
        parryDagger.SetActive(true);

        OnDefence?.Invoke(this, EventArgs.Empty);
    }

    private void Attack()
    {
        //camera: look at enemy (but enemy dagger have no Mesh)
        //dagger 
        virtualCamera.gameObject.SetActive(false);

        blade.SetActive(true);
        parryDagger.SetActive(false);

        OnAttack?.Invoke(this, EventArgs.Empty);
    }

    public void EndTurn()
    {
        OnEndTurn?.Invoke(this, EventArgs.Empty);
    }
}
