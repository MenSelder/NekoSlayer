using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Slicer2D;
using System;
using Utilities2D;
using Debug = UnityEngine.Debug;
using UnityEngine.Rendering.Universal;

// rename to like Character Cut ...??
public class CutEvent2D : MonoBehaviour
{

    public event EventHandler OnStab;

    public event EventHandler<OnStabSuccessEventArgs> OnStabSuccess;
    public class OnStabSuccessEventArgs : EventArgs
    {
        //public Vector2 SliceDirectionVector; //useless (Slice.slice)
        public Slice2D Slice;
    }

    public event EventHandler<OnStabParriedEventArgs> OnStabParried;
    public class OnStabParriedEventArgs : EventArgs
    {
        public Vector2 IntersectionPoint;
        public Slice2D Slice;
    }

    public event EventHandler<OnCutEventArgs> OnCut; //can be removed???
    public class OnCutEventArgs : EventArgs
    {
        public Slice2D Slice;
    }    

    private Sliceable2D sliceable2D;
    private Damageable damageable;

    private void Awake()
    {
        sliceable2D = GetComponent<Sliceable2D>();
        //damageable = GetComponent<Damageable>();
        damageable = GetComponentInParent<Damageable>();
    }

    private void Start()
    {
        sliceable2D.AddEvent(TestEventSlice);
        sliceable2D.AddResultEvent(TestResultEvent);
    }

    private bool TestEventSlice(Slice2D slice)
    {
        OnStab?.Invoke(this, EventArgs.Empty);

        PlayerScriptBase playerScript = GetComponentInParent<PlayerScriptBase>();
        ParryingDaggerScript parryingDaggerScript = playerScript.ParryDagger.GetComponent<ParryingDaggerScript>();
        var parrySegment = parryingDaggerScript.ParryingPointPair;

        var bladeSegment = slice.slice;

        //Blade Strike Parried
        if (LineUtil.IntersectLineSegments2D(parrySegment.Item1, parrySegment.Item2
            , bladeSegment[0].ToVector2(), bladeSegment[1].ToVector2(), out Vector2 intersectionPoint))
        {
            OnStabParried?.Invoke(this, new OnStabParriedEventArgs
            {
                IntersectionPoint = intersectionPoint,
                Slice = slice
            });
            
            return false;
        }

        OnStabSuccess?.Invoke(this, new OnStabSuccessEventArgs
        {
            Slice = slice
        });
        BloodmarkSystemScript.Instance.OnSlicing(slice); //same; 
                                                         // FaceUI -> Make Drips
                                                         // Player -> blood and...

        float damage = 1f;
        //test
        if (damageable.IsAlive) damage = CalculateDamage(slice);

        //float damage = CalculateDamage(slice);
        damageable.Damage(damage);

        return damageable.IsDead;
    }

    private float CalculateDamage(Slice2D slice)
    {
        var spriteRenderer = slice.originGameObject.GetComponent<SpriteRenderer>();
        var bounds = spriteRenderer.bounds;

        var diagonalSize = Vector2.Distance(bounds.min, bounds.max);

        float sliceLenghtSum = 0;

        foreach (var s in slice.slices)
        {
            var pointA = s[0];
            var pointB = s[1];

            sliceLenghtSum += Vector2.Distance(pointA.ToVector2(), pointB.ToVector2());
        }

        return sliceLenghtSum / diagonalSize;
    }

    private void TestResultEvent(Slice2D slice)
    {
        var bladeSegment = slice.slice;
        Vector2 sliceDirectionVector = bladeSegment[1].ToVector2() - bladeSegment[0].ToVector2();

        foreach (var newGameObjects in slice.GetGameObjects())
        {
            float forceValue = 1000;
            newGameObjects.GetComponent<Rigidbody2D>().AddForce(sliceDirectionVector * forceValue);
        }

        OnCut?.Invoke(this, new OnCutEventArgs { Slice = slice });//?
    }

    //delete this
    private Vector2 GetSliceDirectionVector() //remake with "var bladeSegment = slice.slice;"
    {
        int indexId = 0;
        Pair2 pair = Slicer2DController.Get().linearControllerObject.GetPair(indexId);
        return pair.b - pair.a;        
    }
}
