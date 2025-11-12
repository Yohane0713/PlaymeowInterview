using System.IO;
using UnityEngine;

public class FileLogger : MonoBehaviour
{
    string path;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        path = Path.Combine(Application.persistentDataPath, "unity_log.txt");
        Application.logMessageReceived += OnLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= OnLog;
    }

    void OnLog(string condition, string stackTrace, LogType type)
    {
        File.AppendAllText(path, $"[{type}] {condition}\n{stackTrace}\n");
    }
}
