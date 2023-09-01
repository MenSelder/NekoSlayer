using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : PlayerScriptBase
{
    public static PlayerScriptBase Instance { get; private set; } //make local

    private void Awake()
    {
        Instance = this;
    }

}
