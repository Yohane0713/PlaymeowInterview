using System;
using System.Threading.Tasks;
using Firebase;
using Firebase.Extensions;   // for ContinueWithOnMainThread
using UnityEngine;

public class FirebaseBootstrap : MonoBehaviour
{
    private static Task _initTask;
    private static bool _ready;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _ = EnsureReady();
    }

    public static async Task EnsureReady()
    {
        if (_ready) return;

        if (_initTask != null)
        {
            await _initTask;
            return;
        }

        _initTask = FirebaseApp.CheckAndFixDependenciesAsync()
            .ContinueWithOnMainThread(task =>
            {
                var status = task.Result;
                if (status != DependencyStatus.Available)
                {
                    throw new Exception($"Firebase dependencies not available: {status}");
                }
            });

        await _initTask;
        _ready = true;
    }

    public static bool IsReady => _ready;
}
