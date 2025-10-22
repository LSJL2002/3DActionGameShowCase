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

        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
        Debug.Log("[Analytics] Initialized (persistent)");
    }
}
