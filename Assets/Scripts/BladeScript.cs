using Slicer2D;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BladeScript : MonoBehaviour
{
    [SerializeField] private BladeSO bladeSO;
    public BladeSO BladeSO => bladeSO;

    public event EventHandler<OnCutEventArgs> OnCut;
    public class OnCutEventArgs : EventArgs
    {
        public Slice2D Slice2D;
    }

    private void Awake()
    {
        //Instance = this;

        var slicer = GetComponent<Slicer2DController>();
        slicer.AddResultEvent((slice2D) =>
        {
            OnCut?.Invoke(this, new OnCutEventArgs { Slice2D = slice2D });
        });
    }
}
