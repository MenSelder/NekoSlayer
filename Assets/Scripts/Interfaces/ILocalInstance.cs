using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILocalInstance<T> where T : class
{
    public static T LocalInstance { get; private set; }

    public static  void ClearStatic()
    {

    }
}