using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public class AnalyticsInitializer : MonoBehaviour
{
    private static bool initialized = false;

    private async void Awake()
    {
        if (initialized) return;
        initialized = true;

        DontDestroyOnLoad(gameObject);

        try
        {
            await UnityServices.InitializeAsync();
            AnalyticsService.Instance.StartDataCollection();
            Debug.Log("[Analytics] OK Initialized (persistent)");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[Analytics] X Init Failed: {e.Message}");
        }
    }
}

