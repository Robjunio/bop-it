using System;
using System.Linq;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    public static Action OnRoundStarted;
    public static Action OnRoundEnded;
    public static Action<int> OnInstructionDefined;
    public static Action OnInstructionPassed;
    public static Action OnInstructionFailed;

    private void Awake()
    {
        var instancesCount = FindObjectsByType<EventManager>(FindObjectsSortMode.None).Length; 

        if (instancesCount == 1)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else 
        {
            Destroy(this);
        }
    }
}
