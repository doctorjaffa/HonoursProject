using System;
using UnityEngine;

public static class EventManager
{
    public static event Action OnCommitCrime;

    public static void TriggerCrimeResponse()
    {
        OnCommitCrime?.Invoke();
    }
}
