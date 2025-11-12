using Firebase;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.UI;

public class EmailAuthController : MonoBehaviour
{
    [Header("登入介面")]
    [SerializeField] InputField loginEmailInput;
    [SerializeField] InputField loginPasswordInput;
    [SerializeField] Button loginButton;

    [Header("開啟按鈕")]
    [SerializeField] public Button openRegisterButton;
    [SerializeField] public Button openResetButton;

    [Header("註冊面板")]
    [SerializeField] public GameObject registerPanel;
    [SerializeField] public InputField registerEmailInput;
    [SerializeField] public InputField registerPasswordInput;
    [SerializeField] public Button registerConfirmButton;
    [SerializeField] public Button registerCloseButton;
    [SerializeField] public Text registerConfirmText;
    [SerializeField] public Text textErrorRegister;

    [Header("忘記密碼面板")]
    [SerializeField] public GameObject resetPanel;
    [SerializeField] public InputField resetEmailInput;
    [SerializeField] public Button resetConfirmButton;
    [SerializeField] public Button resetCloseButton;
    [SerializeField] public Text resetConfirmText;
    [SerializeField] public Text textErrorReset;

    [Header("狀態文字")]
    [SerializeField] public Text textInputError;

    private FirebaseAuth auth;

    private void Awake()
    {
        if (registerPanel) registerPanel.SetActive(false);
        if (resetPanel) resetPanel.SetActive(false);
        if (loginButton) loginButton.onClick.AddListener(OnLogin);
        if (openRegisterButton) openRegisterButton.onClick.AddListener(() => ToggleRegister(true));
        if (openResetButton) openResetButton.onClick.AddListener(() => ToggleReset(true));
        if (registerConfirmButton) registerConfirmButton.onClick.AddListener(OnRegisterConfirm);
        if (registerCloseButton) registerCloseButton.onClick.AddListener(() => ToggleRegister(false));
        if (resetConfirmButton) resetConfirmButton.onClick.AddListener(OnResetConfirm);
        if (resetCloseButton) resetCloseButton.onClick.AddListener(() => ToggleReset(false));      
    }

    private async void Start()
    {
        await FirebaseBootstrap.EnsureReady();
        auth = FirebaseAuth.DefaultInstance;
    }

    public void OpenRegisterPanel() => ToggleRegister(true);
    public void OpenResetPanel() => ToggleReset(true);

    private void ToggleRegister(bool show)
    {
        if (!registerPanel) return;
        registerPanel.SetActive(show);
        ClearRegisterError();
        if (textInputError) textInputError.text = "";
    }

    private void ToggleReset(bool show)
    {
        if (!resetPanel) return;
        resetPanel.SetActive(show);

        if (textErrorReset) textErrorReset.text = "";
    }

    private async void OnLogin()
    {
        string email = loginEmailInput ? loginEmailInput.text.Trim() : "";
        string password = loginPasswordInput ? loginPasswordInput.text.Trim() : "";

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ShowMessage("帳號或密碼錯誤");
            return;
        }

        loginButton.interactable = false;

        try
        {
            await auth.SignInWithEmailAndPasswordAsync(email, password);
            ShowMessage("登入成功！");
            Debug.Log("登入成功：" + email);
        }
        catch
        {
            ShowMessage("帳號或密碼錯誤");
        }
        finally
        {
            loginButton.interactable = true;
        }
    }

    private async void OnRegisterConfirm()
    {
        string email = registerEmailInput ? registerEmailInput.text.Trim() : "";
        string password = registerPasswordInput ? registerPasswordInput.text.Trim() : "";

        registerConfirmButton.interactable = false;
        ClearRegisterError();

        try
        {
            await auth.CreateUserWithEmailAndPasswordAsync(email, password);
            ShowMessage("已成功註冊");
            registerPanel.SetActive(false);
        }
        catch (FirebaseException e)
        {
            var msg = MapRegisterError((AuthError)e.ErrorCode);
            ShowErrorRegister(msg);
            Debug.LogError($"[Firebase Register Error] {(AuthError)e.ErrorCode}");
        }
        catch
        {
            ShowErrorRegister("註冊失敗，請重試");
        }
        finally
        {
            registerConfirmButton.interactable = true;
        }
    }

    private string MapRegisterError(AuthError code)
    {
        switch (code)
        {
            case AuthError.InvalidEmail: return "請輸入正確Email格式";
            case AuthError.MissingEmail: return "請輸入Email";
            case AuthError.MissingPassword: return "請輸入密碼";
            case AuthError.WeakPassword: return "密碼長度不足";
            case AuthError.EmailAlreadyInUse: return "此Email已被使用";
            default: return "註冊失敗，請重試";
        }
    }

    private void ClearRegisterError()
    {
        if (textErrorRegister)
        {
            textErrorRegister.text = "";
            textErrorRegister.gameObject.SetActive(false);
        }
    }

    private async void OnResetConfirm()
    {
        string email = resetEmailInput ? resetEmailInput.text.Trim() : "";
        resetConfirmButton.interactable = false;

        try
        {
            await auth.SendPasswordResetEmailAsync(email);
            ShowMessage("已寄出重置密碼郵件，請確認");
            resetPanel.SetActive(false);
        }
        catch
        {
            ShowErrorReset("寄送失敗，請輸入正確信箱");
        }
        finally
        {
            resetConfirmButton.interactable = true;
        }
    }

    private void ShowMessage(string message)
    {
        if (textInputError)
        {
            textInputError.text = message;
            textInputError.gameObject.SetActive(true);
        }
    }

    private void ShowErrorRegister(string message)
    {
        if (textErrorRegister)
        {
            textErrorRegister.text = message;
            textErrorRegister.gameObject.SetActive(true);
        }
    }

    private void ShowErrorReset(string message)
    {
        if (textErrorReset)
        {
            textErrorReset.text = message;
            textErrorReset.gameObject.SetActive(true);
        }
    }
}
