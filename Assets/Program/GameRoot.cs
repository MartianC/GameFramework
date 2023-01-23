using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;

public class GameRoot : TMonoSingleton<GameRoot>
{
    public override void Awake()
    {
        Instance = this;
        gameObject.AddComponent<GameLogicManager>();
    }
    
    public void StartGame()
    {
        GameManager.Instance.Startup();
    }
}
