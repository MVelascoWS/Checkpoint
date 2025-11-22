using Privy;
using UnityEngine;

public class Login : MonoBehaviour
{
    PrivyConfig config = new PrivyConfig
    {
        AppId = "",
        ClientId = ""
    };
    PrivyUser privyUser;

    async void Start()
    {
        PrivyManager.Initialize(config);
        var authState = await PrivyManager.Instance.GetAuthState();

        switch (authState)
        {
            case AuthState.Authenticated:
                // User is authenticated. Grab the user's linked accounts
                var privyUser = await PrivyManager.Instance.GetUser();
                var linkedAccounts = privyUser.LinkedAccounts;
                break;
            case AuthState.Unauthenticated:
                // User is not authenticated.
                bool success = await PrivyManager.Instance.Email.SendCode("test@gmail.com");

                if (success)
                {
                    // Prompt user to enter the OTP they received at their email address through your UI
                }
                else
                {
                    // There was an error sending an OTP to your user's email
                }
                break;
            default:
                break;
        }
    }

    public async void TryToLoginWithCode()
    {
        try
        {
            // User will be authenticated if this call is successful
            //await PrivyManager.Instance.Email.LoginWithCode(email, code);
        }
        catch
        {
            // If "LoginWithCode" throws an exception, user login was unsuccessful.
            Debug.Log("Error logging user in.");
        }
    }
}
