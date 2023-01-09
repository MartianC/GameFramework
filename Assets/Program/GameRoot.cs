using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using xasset;

public class GameRoot : TMonoSingleton<GameRoot>
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<GameLogicManager>();
        GameManager.Instance.Startup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
