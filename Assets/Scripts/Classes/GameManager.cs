using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] float worldEventPerSec = 120f;
    private NetworkVariable<float> timer = new NetworkVariable<float>();
    private NetworkVariable<bool> wEventTrigger = new NetworkVariable<bool>();
   
    public float minutes
    {       
        get
        {
            
            return Mathf.Floor(timer.Value / 60);
        }
    }
    public float seconds
    {
        get
        {
            return Mathf.RoundToInt(timer.Value % 60);
        }
    }
    public bool worldEvent
    {
        get
        {
            return wEventTrigger.Value;
        }
    }

    private void Start()
    {
        StartCoroutine(worldEvents());
    }
    private void Update()
    {
        timer.Value = Time.time;
        //Debug.Log((int)timer.Value);
        
    }
    IEnumerator worldEvents()
    {
        while (true)
        {
            Debug.Log("routine working");
            yield return new WaitForSeconds(1);
            yield return new WaitUntil(() => (int)(timer.Value) % worldEventPerSec == 0);
            {
                wEventTrigger.Value = true;
                Debug.Log("world event");                             
            }
        }
    }
}

