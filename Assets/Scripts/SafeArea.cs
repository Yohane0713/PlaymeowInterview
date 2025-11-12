using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeArea : MonoBehaviour
{
    RectTransform rt;
    Rect lastSafe;

    void Awake() { rt = GetComponent<RectTransform>(); Apply(); }
    void Update() { if (Screen.safeArea != lastSafe) Apply(); }

    void Apply()
    {
        lastSafe = Screen.safeArea;
        var anchorMin = lastSafe.position;
        var anchorMax = lastSafe.position + lastSafe.size;
        anchorMin.x /= Screen.width; anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width; anchorMax.y /= Screen.height;
        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
    }
}
