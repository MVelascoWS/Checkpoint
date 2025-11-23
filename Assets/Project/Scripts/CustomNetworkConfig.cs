using UnityEngine;
using Privy;
using System.Collections.Generic;
using System;

/// <summary>
/// Configuración de redes EVM personalizadas para Privy Unity SDK
/// Soporta múltiples redes: Ronin, Polygon, Base, Arbitrum, Optimism, etc.
/// </summary>
[CreateAssetMenu(fileName = "CustomNetworkConfig", menuName = "Privy/Custom Network Config", order = 1)]
public class CustomNetworkConfig : ScriptableObject
{
    [Header("Network Configuration")]
    public string networkName = "Custom Network";
    public string chainId;
    public string chainIdHex;
    public string rpcUrl;
    public string blockExplorerUrl;

    [Header("Native Currency")]
    public string currencyName = "ETH";
    public string currencySymbol = "ETH";
    public int currencyDecimals = 18;

    [Header("Network Details")]
    public NetworkType networkType = NetworkType.Mainnet;
    public bool isTestnet = false;

    public enum NetworkType
    {
        Mainnet,
        Testnet,
        Custom
    }

    /// <summary>
    /// Convierte la configuración a formato JSON para Privy
    /// </summary>
    public string ToJson()
    {
        var networkData = new
        {
            chainId = chainIdHex,
            chainName = networkName,
            nativeCurrency = new
            {
                name = currencyName,
                symbol = currencySymbol,
                decimals = currencyDecimals
            },
            rpcUrls = new[] { rpcUrl },
            blockExplorerUrls = new[] { blockExplorerUrl }
        };

        return JsonUtility.ToJson(networkData, true);
    }

    /// <summary>
    /// Valida que la configuración esté completa
    /// </summary>
    public bool IsValid()
    {
        return !string.IsNullOrEmpty(networkName) &&
               !string.IsNullOrEmpty(chainId) &&
               !string.IsNullOrEmpty(chainIdHex) &&
               !string.IsNullOrEmpty(rpcUrl);
    }
}