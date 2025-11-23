using Newtonsoft.Json;
using Privy;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using UnityEngine;

public class Login : MonoBehaviour
{
    PrivyConfig config = new PrivyConfig
    {
        AppId = "cmiaqfo6u0045l40cmhfchlmx",
        ClientId = "client-WY6TLA5pJ7TU9EC4fhdnW7jfwxKteBFEdK8mjkECjruEG"
    };
    PrivyUser privyUser;
    public UserData userData;
    public CheckpointManager uiManager;
    [SerializeField] private CustomNetworkManager networkManager;

    [Header("Contract Configuration")]
    [SerializeField] private string contractAddress = "0x..."; // Dirección de tu contrato
    [SerializeField] private string chainId = "2021"; // Ronin Saigon Testnet

    [Header("ABI Configuration")]
    [TextArea(5, 10)]
    [SerializeField] private string contractABI = ""; // JSON del ABI del contrato

    public void TryLogin()
    {
        StartPrivy();
    }
    async void StartPrivy()
    {
        PrivyManager.Initialize(config);
        var authState = await PrivyManager.Instance.GetAuthState();

        switch (authState)
        {
            case AuthState.Authenticated:
                // User is authenticated. Grab the user's linked accounts
                var privyUser = await PrivyManager.Instance.GetUser();
                var linkedAccounts = privyUser.LinkedAccounts;
                if (privyUser != null)
                {
                    // Grab the embedded wallet from the embedded wallet list
                    // For demonstration purposes we're just grabbing the first one.
                    if (privyUser.EmbeddedWallets.Length > 0)
                    {
                        IEmbeddedEthereumWallet embeddedWallet = privyUser.EmbeddedWallets[0];
                        //Ensure the Wallet is not null
                        if (embeddedWallet != null)
                        {
                            Debug.Log("Address " + embeddedWallet.Address);
                            //////
                            ///
                            string data = await ReadPlayerScoreAsync(embeddedWallet.Address);
                            Debug.Log("smart contract data: " + data);
                        }
                        else
                            Debug.Log("Null wallet");
                    }
                    else
                    {
                        IEmbeddedEthereumWallet embeddedWallet = await privyUser.CreateWallet();
                        Debug.Log("New wallet created with address: " + embeddedWallet.Address);
                    }                    
                }
                else
                {
                    Debug.Log("Null user");
                }
                uiManager.ShowGameplayPanel();
                break;
            case AuthState.Unauthenticated:
                // User is not authenticated.
                bool success = await PrivyManager.Instance.Email.SendCode(userData.Email);

                if (success)
                {
                    uiManager.ShowPasswordPanel();
                }
                else
                {
                    uiManager.ShowErrorPanel();
                }
                break;
            default:
                break;
        }
    }

    public void TryLoginWithEmailCode()
    {
        TryToLoginWithCode();
    }
    public async void TryToLoginWithCode()
    {
        try
        {
            // User will be authenticated if this call is successful
            var authState = await PrivyManager.Instance.Email.LoginWithCode(userData.Email, userData.Code);
            var privyUser = await PrivyManager.Instance.GetUser();
            var linkedAccounts = privyUser.LinkedAccounts;
            uiManager.ShowGameplayPanel();
            
        }
        catch
        {
            uiManager.ShowErrorPanel();
            Debug.Log("Error logging user in.");
        }
    }


    public async Task<string> ReadPlayerScoreAsync(string encodedCallData)
    {
        // encodedCallData = "0x" + selector + params
        // ej: getPlayerScore(playerAddress)

        IEmbeddedEthereumWallet wallet = privyUser.EmbeddedWallets[0];
        if (wallet == null) return null;

        // Objeto de llamada eth_call como JSON (primer parámetro del RPC)
        string callObjectJson =
            $"{{\"to\":\"{contractAddress}\",\"data\":\"{encodedCallData}\"}}";

        // Construimos el RpcRequest de Privy
        var rpcRequest = new Privy.RpcRequest
        {
            Method = "eth_call",
            // params[0] = tx object, params[1] = bloque ("latest")
            Params = new[]
            {
            callObjectJson,
            "latest"
        }
        };

        try
        {
            Privy.RpcResponse response = await wallet.RpcProvider.Request(rpcRequest);
            Debug.Log($"eth_call result: {response.Data}");

            // response.Data debería ser algo tipo "0x000000...score"
            return response.Data;
        }
        catch (PrivyException.EmbeddedWalletException ex)
        {
            Debug.LogError($"Error en eth_call: {ex.Error} {ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error genérico en eth_call: {ex.Message}");
            return null;
        }
    }



}
public class RpcRequest
{
    public string Method { get; set; }
    public string[] Params { get; set; }

}

public class RpcResponse
{
    public string Method { get; set; }
    public string Data { get; set; }

}
