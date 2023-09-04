using System.Collections;
using System.Collections.Generic;
using GameCore;
using HotLogic;
using Platform;
using UnityEngine;

public class GameRoot : TMonoSingleton<GameRoot>
{
    public void Awake()
    {
        gameObject.AddComponent<GameLogicManager>();
        gameObject.AddComponent<BestWebConnection>();
        //gameObject.AddComponent<TcpNetManager>();
    }
    
    public void StartGame()
    {
        GameManager.Instance.Startup();
    }
}
