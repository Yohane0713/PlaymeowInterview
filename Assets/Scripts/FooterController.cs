using UnityEngine;
using UnityEngine.UI;

public class FooterController : MonoBehaviour
{
    [Header("面板物件")]
    public GameObject panelPrivacy;
    public GameObject panelTerms;

    [Header("按鈕物件")]
    public Button buttonOpenPrivacy;
    public Button buttonClosePrivacy;
    public Button buttonOpenTerms;
    public Button buttonCloseTerms;

    void Start()
    {
        if (panelPrivacy) panelPrivacy.SetActive(false);
        if (panelTerms) panelTerms.SetActive(false);

        if (buttonOpenPrivacy) buttonOpenPrivacy.onClick.AddListener(OpenPrivacy);
        if (buttonClosePrivacy) buttonClosePrivacy.onClick.AddListener(ClosePrivacy);
        if (buttonOpenTerms) buttonOpenTerms.onClick.AddListener(OpenTerms);
        if (buttonCloseTerms) buttonCloseTerms.onClick.AddListener(CloseTerms);
    }

    public void OpenPrivacy()
    {
        if (panelPrivacy) panelPrivacy.SetActive(true);
    }

    public void ClosePrivacy()
    {
        if (panelPrivacy) panelPrivacy.SetActive(false);
    }

    public void OpenTerms()
    {
        if (panelTerms) panelTerms.SetActive(true);
    }

    public void CloseTerms()
    {
        if (panelTerms) panelTerms.SetActive(false);
    }
}
