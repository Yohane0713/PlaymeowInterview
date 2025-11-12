using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField] GameObject popup;
    [SerializeField] Button closeButton;
    [SerializeField] Button simpleChineseButton;

    void Start()
    {
        // 預設隱藏
        popup.SetActive(false);

        // 偵測是否為簡體中文
        if (Application.systemLanguage == SystemLanguage.ChineseSimplified ||
            Application.systemLanguage == SystemLanguage.Chinese)
        {
            popup.SetActive(true);
        }

        closeButton.onClick.AddListener(HidePopup);
        simpleChineseButton.onClick.AddListener(OnClickSwitchLanguage);
    }

    void HidePopup()
    {
        popup.SetActive(false);
    }

    void OnClickSwitchLanguage()
    {
        Debug.Log("切換到簡體中文");
        // TODO: 語系切換 或載入簡體中文場景 
        HidePopup();
    }
}
