using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities2D;

public class ParryingDaggerScript : MonoBehaviour
{
    public enum DaggerState { Movable, Immovable, Hidden }
        
    //public static ParryingDaggerScript LocalInstance { get; private set; } // ?? for multiplayer


    [SerializeField] private ParryingDaggerVisual parryingDaggerVisual;
    [SerializeField] private ParryingDaggerSO parryingDaggerSO;
    public ParryingDaggerSO ParryingDaggerSO => parryingDaggerSO;

    DaggerState currentDaggerState;

    private GameObject startPoint;
    private GameObject endPoint;

    private DraggableScript draggableScript;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        //LocalInstance = this;

        draggableScript = GetComponent<DraggableScript>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        currentDaggerState = DaggerState.Movable;
    }

    private void Start()
    {
        parryingDaggerVisual.SetParryingDaggerSO(parryingDaggerSO);

        //Create points of parrying segment
        startPoint = new GameObject("startPoint");
        startPoint.transform.SetParent(transform);
        startPoint.transform.position = spriteRenderer.bounds.min;

        endPoint = new GameObject("endPoint");
        endPoint.transform.SetParent(transform);
        endPoint.transform.position = spriteRenderer.bounds.max;
    }

    public (Vector2, Vector2) ParryingPointPair => (startPoint.transform.position, endPoint.transform.position);

    public void SetMode(DaggerState state) //?? need remake (done)
    {
        if (currentDaggerState == state) return;

        switch (state)
        {
            case DaggerState.Movable:
                draggableScript.enabled = true;
                spriteRenderer.enabled = true;
                break;
            case DaggerState.Immovable:
                draggableScript.enabled = false;
                spriteRenderer.enabled = true;
                break;
            case DaggerState.Hidden:
                draggableScript.enabled = false;
                spriteRenderer.enabled = false;
                break;
            default:
                break;
        }

        currentDaggerState = state;
    }
}
