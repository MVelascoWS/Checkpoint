using Nethereum.ABI.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Numerics;
using Thirdweb;
using Thirdweb.Api;
using Thirdweb.Unity;
using UnityEngine;
using static UnityEditor.Progress;

public class InAppWallet : MonoBehaviour
{
    public UserData userData;
    public CheckpointManager checkpointManager;
    private ThirdwebContract contract;
    public string contractAddress;
    public string chainID;
    public string ABI;
    private BigInteger[] resources;

    private PlayerResources currentResources;
    private IThirdwebWallet userWallet;
    [System.Serializable]
    public struct PlayerResources
    {
        public BigInteger food;   // Token ID 1
        public BigInteger wood;   // Token ID 2
        public BigInteger gold;   // Token ID 3
        public BigInteger stone;  // Token ID 4

        public override string ToString()
        {
            return $"Food: {food}, Wood: {wood}, Gold: {gold}, Stone: {stone}";
        }
    }
   
    public async void ConnectWithEmail()
    {
        var inAppWalletOptions = new InAppWalletOptions(email: userData.Email);
        var options = new WalletOptions(
            provider: WalletProvider.InAppWallet,
            chainId: 2763834965749000,
            inAppWalletOptions: inAppWalletOptions
        );
        var wallet = await ThirdwebManager.Instance.ConnectWallet(options);
        if (wallet != null)
        {
            HandleSuccessfulConnection(wallet);
        }
    }

    private async void HandleSuccessfulConnection(IThirdwebWallet wallet)
    {
        try 
        {
            userWallet = wallet;
            userData.Address = await wallet.GetAddress();
            checkpointManager.ShowGameplayPanel();
        }
        catch (System.Exception e){ }
    }

    public void GetResources()
    {
        GetResourcesContract();
    }
    public async void GetResourcesContract()
    {
        BigInteger chain = BigInteger.Parse(chainID);
        contract = await ThirdwebManager.Instance.GetContract(
        address: contractAddress,
        chainId: chain,
        abi: ABI
        );

        //var data = await contract.ERC1155_BalanceOf(userData.Address, 0);
        var balances = await contract.Read<BigInteger[]>(
                "getPlayerBalances",
                userData.Address
            );

        
        currentResources = new PlayerResources
        {
            food = balances[0],
            wood = balances[1],
            gold = balances[2],
            stone = balances[3]
        };

        userData.Food = currentResources.food;
        userData.Wood = currentResources.wood;
        userData.Gold = currentResources.gold;
        userData.Stone = currentResources.stone;
        Debug.Log($" Balances cargados exitosamente: {currentResources.food}");
    }

    public async void UseResources(BigInteger itemID, BigInteger amount)
    {
        BigInteger chain = BigInteger.Parse(chainID);
        contract = await ThirdwebManager.Instance.GetContract(
        address: contractAddress,
        chainId: chain,
        abi: ABI
        );
        BigInteger weiValue = BigInteger.Zero;
        //var receipt = await contract.Write(userWallet, contract, "sendResources", weiValue,userData.Address, contractAddress, itemID, amount);
        //Console.WriteLine($"Transaction receipt: {receipt}");
    }

    public async void SendResources(string recipientAddress, int tokenId, BigInteger amount)
    {
        try
        {
            

            // Validar wallet conectada
            var wallet = ThirdwebManager.Instance.ActiveWallet;
            if (wallet == null)
            {
                Debug.LogError("No hay wallet conectada");
                return;
            }

            string senderAddress = await wallet.GetAddress();
            BigInteger chain = BigInteger.Parse(chainID);
            contract = await ThirdwebManager.Instance.GetContract(
            address: contractAddress,
            chainId: chain,
            abi: ABI
            );


            BigInteger weiValue = 0;

            //var result = await contract.Write(
            //    wallet,
            //    contract,
            //    "sendResources", weiValue, userData.Address, contractAddress, tokenId, amount       // amount
            //);

            
            //var receipt = await result.WaitForTransactionReceipt();


        }
        catch (Exception ex)
        {
            Debug.LogError($"Error al enviar recursos: {ex.Message}");
            Debug.LogError($"Stack trace: {ex.StackTrace}");
        }
    }
}
