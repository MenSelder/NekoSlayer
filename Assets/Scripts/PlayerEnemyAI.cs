using Slicer2D;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Utilities2D;
using Random = UnityEngine.Random;

public class PlayerEnemyAI : PlayerScriptBase
{
    public static PlayerScriptBase Instance { get; private set; } //make local

    [SerializeField] private Vector2 pointA;
    [SerializeField] private Vector2 pointB;

    [SerializeField] private Vector2 daggerPosition;
    [SerializeField] private Quaternion daggerRotation;

    //get players character sprites
    // min max by sprites bounds + error? = ranges

    [SerializeField] private GameObject debugMark;

    private GameObject markA;
    private GameObject markB;

    private Vector2 prevPointA;
    private Vector2 prevPointB;

    private void Awake()
    {
        Instance = this;
    }

    private void LogicAI()
    {
        if (GameManager.Instance.CurrentGameState != GameManager.GameState.GamePlay) 
            return;

        switch (CurrentPlayerState)
        {
            case PlayerState.Wait:
                return;

            case PlayerState.Defence:
                RndTransferDagger();
                EndTurn();

                break;
            case PlayerState.Attack:
                RndAttackPoints();
                DoStab();

                break;
        }
    }

    private void Update()
    {
        LogicAI();

        if (pointA == null || pointB == null) return;
        if (pointA.Equals(prevPointA) && pointB.Equals(prevPointB)) return;

        prevPointA = pointA; 
        prevPointB = pointB;

        DrawPoints();
    }

    [ContextMenu("RndPoints")]
    private void RndAttackPoints()
    {
        Bounds attackBounds = PlayerMain.Instance.Character.GetComponentInChildren<SpriteRenderer>().bounds;

        pointA = GetRndPointOnBound(attackBounds.min, attackBounds.max);
        pointB = GetRndPointOnBound(attackBounds.max, attackBounds.min); //reversed

    }

    private Vector2 GetRndPointOnBound(Vector2 min, Vector2 max)
    {
        if (Random.Range(0, 2) == 0) // 50/50
        {
            //a
            float x = min.x;
            float y = math.lerp(min.y, max.y, Random.Range(0f, 1f));

            return new Vector2(x, y);
        }
        else
        {
            //b 
            float x = math.lerp(min.x, max.x, Random.Range(0f, 1f));
            float y = min.y;

            return new Vector2(x, y);
        }     
    }

    private Vector2 GetRndPointWithinBounds(Vector2 min, Vector2 max)
    {
        float x = Random.Range(min.x, max.x);
        float y = Random.Range(min.y, max.y);
        
        return new Vector2(x, y);
    }

    [ContextMenu("DrawPoints")]
    private void DrawPoints()
    {
        DeletePoints();

        markA = Instantiate(debugMark, pointA, Quaternion.identity);
        markA.GetComponent<MeshRenderer>().material.color = Color.red;

        markB = Instantiate(debugMark, pointB, Quaternion.identity);
        markB.GetComponent<MeshRenderer>().material.color = Color.green;

    }

    [ContextMenu("DeletePoints")]
    private void DeletePoints()
    {
        if (markA != null && markB != null)
        {
            Destroy(markA);
            Destroy(markB);
        }
    }

    [ContextMenu("RndTransferDagger")]
    private void RndTransferDagger()
    {
        float rndDegree = Random.Range(0f, 360f);
        var rotation = Quaternion.Euler(0.0f, 0.0f, rndDegree);

        daggerRotation = rotation;

        Bounds defenceBounds = character.GetComponentInChildren<SpriteRenderer>().bounds;
        daggerPosition = GetRndPointWithinBounds(defenceBounds.min, defenceBounds.max);

        parryDagger.transform.rotation = daggerRotation;
        parryDagger.transform.position = daggerPosition;
    }

    [ContextMenu("Do Stab")]
    public void DoStab()
    {
        Pair2D pair2D = new Pair2D(pointA, pointB);
        List<Slice2D> results = Slicing.LinearSliceAll(pair2D, null);
    }

    [ContextMenu("EndTurn")]
    public void EndTurnMenu()
    {
        EndTurn();
    }
}
