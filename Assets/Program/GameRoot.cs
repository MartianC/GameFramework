using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;

public class GameRoot : TMonoSingleton<GameRoot>
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<GameLogicManager>();
        GameManager.Instance.Startup();
    }
    
    public override void Awake()
    {
        Instance = this;
    }
}
