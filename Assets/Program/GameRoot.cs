using System.Collections;
using System.Collections.Generic;
using GameCore;
using Platform;
using UnityEngine;

public class GameRoot : TMonoSingleton<GameRoot>
{
    public override void Awake()
    {
        Instance = this;
        gameObject.AddComponent<GameLogicManager>();
        gameObject.AddComponent<BestWebConnection>();
    }
    
    public void StartGame()
    {
        GameManager.Instance.Startup();
    }
}
