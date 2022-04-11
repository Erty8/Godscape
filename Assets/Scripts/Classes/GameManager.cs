using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    private NetworkVariable<float> timer = new NetworkVariable<float>();
   
    public float time
    {
        get
        {
            return timer.Value;
        }
    }

    private void Start()
    {
        
    }
    private void Update()
    {
        timer.Value = Time.time;
    }
}

