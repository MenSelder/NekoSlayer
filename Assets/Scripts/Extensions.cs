using UnityEngine;

public static class Extensions
{
    public static bool TryGetComponentInParent<T>(this GameObject gameObject, out T component)
    {
        component = gameObject.GetComponentInParent<T>();

        return component == null ? false : true;
    }
}