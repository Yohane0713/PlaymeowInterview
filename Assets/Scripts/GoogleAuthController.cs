using Firebase;
using Firebase.Auth;
using Google;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GoogleAuthController : MonoBehaviour
{
    [Header("UI")]
    public Button googleLoginButton;
    public Text statusText;

    private const string WEB_CLIENT_ID = "382873620859-nl2qpgd90j2nds8vfpktpnrleij9ma7k.apps.googleusercontent.com";

    private GoogleSignInConfiguration config;
    private FirebaseAuth auth;

    private void Awake()
    {
        config = new GoogleSignInConfiguration
        {
            WebClientId = WEB_CLIENT_ID,
            RequestIdToken = true,
            RequestEmail = true
        };

        if (googleLoginButton)
            googleLoginButton.onClick.AddListener(SignInButton);
    }

    private async void Start()
    {
        await FirebaseBootstrap.EnsureReady();
    }

    // 給UI Button onClick用
    public void SignInButton()
    {
        _ = SignInWithGoogleFlow();
    }

    public void SignOutButton()
    {
        GoogleSignIn.DefaultInstance.SignOut();
        if (auth != null) auth.SignOut();
        SetStatus("已登出");
    }

    private async Task SignInWithGoogleFlow()
    {
        try
        {
            GoogleSignIn.Configuration = config;
            GoogleSignIn.Configuration.UseGameSignIn = false;
            SetStatus("Opening Google sign-in ...");

            var gsUser = await GoogleSignIn.DefaultInstance.SignIn();

            var credential = GoogleAuthProvider.GetCredential(gsUser.IdToken, null);
            FirebaseUser user = await auth.SignInWithCredentialAsync(credential);
            SetStatus($"登入成功：{user.DisplayName} ({user.Email})");

        }
        catch (Exception e)
        {
            Debug.LogError(e);
            SetStatus("登入失敗，請重試");
        }
    }

    private void SetStatus(string msg)
    {
        if (statusText) statusText.text = msg;
        Debug.Log(msg);
    }
}