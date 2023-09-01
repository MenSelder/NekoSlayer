using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SparkSystemScript : MonoBehaviour //delete this
{
    public static SparkSystemScript Instance { get; private set; }

    [SerializeField] private ParticleSystem sparks;

    private void Awake()
    {
        Instance = this;
    }

    public void OnParry(Vector2 position)
    {
        //var parrySegment = ParryingDaggerScript.LocalInstance.ParryingPointPair; //not avaliable
        var slicerSegment = Slicer2D.Slicer2DController.Get().linearControllerObject.GetPair(0);

        //var atDirectorn = Quaternion.LookRotation(ReflectionFromSegment(slicerSegment.b - slicerSegment.a, parrySegment.Item1, parrySegment.Item2));
        //Debug.Log("U: " + atDirectorn);

        //atDirectorn = Quaternion.LookRotation(Test(slicerSegment.b - slicerSegment.a, parrySegment.Item1, parrySegment.Item2));
        //Debug.Log("U: " + atDirectorn);

        var particle = Instantiate(sparks, position, Quaternion.identity);
        //var particle = Instantiate(sparks, position
        //    , Quaternion.LookRotation(ReflectionFromSegment(slicerSegment.b - slicerSegment.a, parrySegment.Item1, parrySegment.Item2))); //base 

        //var particle = Instantiate(sparks, position, atDirectorn); //test

        //Debug.Log("#reflect dir:" + 
        //    ReflectionFromSegment(slicerSegment.b - slicerSegment.a, parrySegment.Item1, parrySegment.Item2).normalized);

        particle.Play();
    }

    public Vector2 ReflectionFromSegment(Vector2 origin, Vector2 pointA, Vector2 pointB) //trash?
    {
        //Vector2 direction = pointA - pointB;

        //Vector2 direction = pointB - pointA;
        // rotate 90° Clockwise
        //Vector2 normal1 = new Vector2(direction.y, -direction.x);

        Vector2 normal = Vector2.Perpendicular(pointB - pointA);

        return Vector2.Reflect(origin, normal);
    }

    public Vector2 Test(Vector2 origin, Vector2 pointA, Vector2 pointB) //trash
    {
        var a = origin.normalized;
        var b = (pointB - pointA).normalized;

        Vector2 res = a + b;
        return res;
    }
}
