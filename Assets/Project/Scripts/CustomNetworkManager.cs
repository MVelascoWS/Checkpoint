using Newtonsoft.Json;
using Privy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Gestor de Redes EVM personalizadas para Privy Unity SDK
/// Permite cambiar entre diferentes redes blockchain (Polygon, Base, Arbitrum, etc.)
/// </summary>
public class CustomNetworkManager : MonoBehaviour
{
    [Header("Configuraciones de Red")]
    [SerializeField] private List<CustomNetworkConfig> availableNetworks = new List<CustomNetworkConfig>();
    [SerializeField] private CustomNetworkConfig currentNetwork;

    [Header("Privy Configuration")]
    [SerializeField] private string privyAppId = "your-app-id";

  
    private string userWalletAddress;

    // =====================================================
    // REDES PRECONFIGURADAS
    // =====================================================

    /// <summary>
    /// Crea configuraciones para redes populares
    /// </summary>
    public static class PresetNetworks
    {
        // POLYGON MAINNET
        public static CustomNetworkConfig PolygonMainnet()
        {
            var config = ScriptableObject.CreateInstance<CustomNetworkConfig>();
            config.networkName = "Polygon Mainnet";
            config.chainId = "137";
            config.chainIdHex = "0x89";
            config.rpcUrl = "https://polygon-rpc.com";
            config.blockExplorerUrl = "https://polygonscan.com";
            config.currencyName = "MATIC";
            config.currencySymbol = "MATIC";
            config.currencyDecimals = 18;
            config.networkType = CustomNetworkConfig.NetworkType.Mainnet;
            config.isTestnet = false;
            return config;
        }

        // POLYGON AMOY TESTNET
        public static CustomNetworkConfig PolygonAmoy()
        {
            var config = ScriptableObject.CreateInstance<CustomNetworkConfig>();
            config.networkName = "Polygon Amoy Testnet";
            config.chainId = "80002";
            config.chainIdHex = "0x13882";
            config.rpcUrl = "https://rpc-amoy.polygon.technology";
            config.blockExplorerUrl = "https://amoy.polygonscan.com";
            config.currencyName = "MATIC";
            config.currencySymbol = "MATIC";
            config.currencyDecimals = 18;
            config.networkType = CustomNetworkConfig.NetworkType.Testnet;
            config.isTestnet = true;
            return config;
        }

        // BASE MAINNET
        public static CustomNetworkConfig BaseMainnet()
        {
            var config = ScriptableObject.CreateInstance<CustomNetworkConfig>();
            config.networkName = "Base";
            config.chainId = "8453";
            config.chainIdHex = "0x2105";
            config.rpcUrl = "https://mainnet.base.org";
            config.blockExplorerUrl = "https://basescan.org";
            config.currencyName = "Ether";
            config.currencySymbol = "ETH";
            config.currencyDecimals = 18;
            config.networkType = CustomNetworkConfig.NetworkType.Mainnet;
            config.isTestnet = false;
            return config;
        }

        // BASE SEPOLIA TESTNET
        public static CustomNetworkConfig BaseSepolia()
        {
            var config = ScriptableObject.CreateInstance<CustomNetworkConfig>();
            config.networkName = "Base Sepolia";
            config.chainId = "84532";
            config.chainIdHex = "0x14a34";
            config.rpcUrl = "https://sepolia.base.org";
            config.blockExplorerUrl = "https://sepolia.basescan.org";
            config.currencyName = "Ether";
            config.currencySymbol = "ETH";
            config.currencyDecimals = 18;
            config.networkType = CustomNetworkConfig.NetworkType.Testnet;
            config.isTestnet = true;
            return config;
        }

        // ARBITRUM ONE
        public static CustomNetworkConfig ArbitrumOne()
        {
            var config = ScriptableObject.CreateInstance<CustomNetworkConfig>();
            config.networkName = "Arbitrum One";
            config.chainId = "42161";
            config.chainIdHex = "0xa4b1";
            config.rpcUrl = "https://arb1.arbitrum.io/rpc";
            config.blockExplorerUrl = "https://arbiscan.io";
            config.currencyName = "Ether";
            config.currencySymbol = "ETH";
            config.currencyDecimals = 18;
            config.networkType = CustomNetworkConfig.NetworkType.Mainnet;
            config.isTestnet = false;
            return config;
        }

        // ARBITRUM SEPOLIA
        public static CustomNetworkConfig ArbitrumSepolia()
        {
            var config = ScriptableObject.CreateInstance<CustomNetworkConfig>();
            config.networkName = "Arbitrum Sepolia";
            config.chainId = "421614";
            config.chainIdHex = "0x66eee";
            config.rpcUrl = "https://sepolia-rollup.arbitrum.io/rpc";
            config.blockExplorerUrl = "https://sepolia.arbiscan.io";
            config.currencyName = "Ether";
            config.currencySymbol = "ETH";
            config.currencyDecimals = 18;
            config.networkType = CustomNetworkConfig.NetworkType.Testnet;
            config.isTestnet = true;
            return config;
        }

        // OPTIMISM MAINNET
        public static CustomNetworkConfig OptimismMainnet()
        {
            var config = ScriptableObject.CreateInstance<CustomNetworkConfig>();
            config.networkName = "Optimism";
            config.chainId = "10";
            config.chainIdHex = "0xa";
            config.rpcUrl = "https://mainnet.optimism.io";
            config.blockExplorerUrl = "https://optimistic.etherscan.io";
            config.currencyName = "Ether";
            config.currencySymbol = "ETH";
            config.currencyDecimals = 18;
            config.networkType = CustomNetworkConfig.NetworkType.Mainnet;
            config.isTestnet = false;
            return config;
        }

        // OPTIMISM SEPOLIA
        public static CustomNetworkConfig OptimismSepolia()
        {
            var config = ScriptableObject.CreateInstance<CustomNetworkConfig>();
            config.networkName = "Optimism Sepolia";
            config.chainId = "11155420";
            config.chainIdHex = "0xaa37dc";
            config.rpcUrl = "https://sepolia.optimism.io";
            config.blockExplorerUrl = "https://sepolia-optimism.etherscan.io";
            config.currencyName = "Ether";
            config.currencySymbol = "ETH";
            config.currencyDecimals = 18;
            config.networkType = CustomNetworkConfig.NetworkType.Testnet;
            config.isTestnet = true;
            return config;
        }

        // AVALANCHE C-CHAIN
        public static CustomNetworkConfig AvalancheMainnet()
        {
            var config = ScriptableObject.CreateInstance<CustomNetworkConfig>();
            config.networkName = "Avalanche C-Chain";
            config.chainId = "43114";
            config.chainIdHex = "0xa86a";
            config.rpcUrl = "https://api.avax.network/ext/bc/C/rpc";
            config.blockExplorerUrl = "https://snowtrace.io";
            config.currencyName = "Avalanche";
            config.currencySymbol = "AVAX";
            config.currencyDecimals = 18;
            config.networkType = CustomNetworkConfig.NetworkType.Mainnet;
            config.isTestnet = false;
            return config;
        }

        // AVALANCHE FUJI TESTNET
        public static CustomNetworkConfig AvalancheFuji()
        {
            var config = ScriptableObject.CreateInstance<CustomNetworkConfig>();
            config.networkName = "Avalanche Fuji Testnet";
            config.chainId = "43113";
            config.chainIdHex = "0xa869";
            config.rpcUrl = "https://api.avax-test.network/ext/bc/C/rpc";
            config.blockExplorerUrl = "https://testnet.snowtrace.io";
            config.currencyName = "Avalanche";
            config.currencySymbol = "AVAX";
            config.currencyDecimals = 18;
            config.networkType = CustomNetworkConfig.NetworkType.Testnet;
            config.isTestnet = true;
            return config;
        }

        // BNB SMART CHAIN
        public static CustomNetworkConfig BSCMainnet()
        {
            var config = ScriptableObject.CreateInstance<CustomNetworkConfig>();
            config.networkName = "BNB Smart Chain";
            config.chainId = "56";
            config.chainIdHex = "0x38";
            config.rpcUrl = "https://bsc-dataseed.binance.org";
            config.blockExplorerUrl = "https://bscscan.com";
            config.currencyName = "BNB";
            config.currencySymbol = "BNB";
            config.currencyDecimals = 18;
            config.networkType = CustomNetworkConfig.NetworkType.Mainnet;
            config.isTestnet = false;
            return config;
        }

        // BNB TESTNET
        public static CustomNetworkConfig BSCTestnet()
        {
            var config = ScriptableObject.CreateInstance<CustomNetworkConfig>();
            config.networkName = "BNB Smart Chain Testnet";
            config.chainId = "97";
            config.chainIdHex = "0x61";
            config.rpcUrl = "https://data-seed-prebsc-1-s1.binance.org:8545";
            config.blockExplorerUrl = "https://testnet.bscscan.com";
            config.currencyName = "BNB";
            config.currencySymbol = "BNB";
            config.currencyDecimals = 18;
            config.networkType = CustomNetworkConfig.NetworkType.Testnet;
            config.isTestnet = true;
            return config;
        }

        // FANTOM OPERA
        public static CustomNetworkConfig FantomMainnet()
        {
            var config = ScriptableObject.CreateInstance<CustomNetworkConfig>();
            config.networkName = "Fantom Opera";
            config.chainId = "250";
            config.chainIdHex = "0xfa";
            config.rpcUrl = "https://rpc.ftm.tools";
            config.blockExplorerUrl = "https://ftmscan.com";
            config.currencyName = "Fantom";
            config.currencySymbol = "FTM";
            config.currencyDecimals = 18;
            config.networkType = CustomNetworkConfig.NetworkType.Mainnet;
            config.isTestnet = false;
            return config;
        }

        // CELO MAINNET
        public static CustomNetworkConfig CeloMainnet()
        {
            var config = ScriptableObject.CreateInstance<CustomNetworkConfig>();
            config.networkName = "Celo Mainnet";
            config.chainId = "42220";
            config.chainIdHex = "0xa4ec";
            config.rpcUrl = "https://forno.celo.org";
            config.blockExplorerUrl = "https://explorer.celo.org";
            config.currencyName = "CELO";
            config.currencySymbol = "CELO";
            config.currencyDecimals = 18;
            config.networkType = CustomNetworkConfig.NetworkType.Mainnet;
            config.isTestnet = false;
            return config;
        }

        // RONIN MAINNET (para referencia)
        public static CustomNetworkConfig RoninMainnet()
        {
            var config = ScriptableObject.CreateInstance<CustomNetworkConfig>();
            config.networkName = "Ronin Mainnet";
            config.chainId = "2020";
            config.chainIdHex = "0x7e4";
            config.rpcUrl = "https://api.roninchain.com/rpc";
            config.blockExplorerUrl = "https://explorer.roninchain.com";
            config.currencyName = "RON";
            config.currencySymbol = "RON";
            config.currencyDecimals = 18;
            config.networkType = CustomNetworkConfig.NetworkType.Mainnet;
            config.isTestnet = false;
            return config;
        }

        // RONIN SAIGON TESTNET
        public static CustomNetworkConfig RoninSaigon()
        {
            var config = ScriptableObject.CreateInstance<CustomNetworkConfig>();
            config.networkName = "Ronin Saigon Testnet";
            config.chainId = "2021";
            config.chainIdHex = "0x7e5";
            config.rpcUrl = "https://saigon-testnet.roninchain.com/rpc";
            config.blockExplorerUrl = "https://saigon-explorer.roninchain.com";
            config.currencyName = "RON";
            config.currencySymbol = "RON";
            config.currencyDecimals = 18;
            config.networkType = CustomNetworkConfig.NetworkType.Testnet;
            config.isTestnet = true;
            return config;
        }
    }

    // =====================================================
    // INICIALIZACIÓN
    // =====================================================

    void Start()
    {
        // Cargar redes preconfiguradas
        LoadPresetNetworks();

        // Seleccionar red por defecto
        if (currentNetwork == null && availableNetworks.Count > 0)
        {
            currentNetwork = availableNetworks[0];
        }

        Debug.Log($"Red actual: {currentNetwork?.networkName ?? "None"}");
    }

    /// <summary>
    /// Carga las redes preconfiguradas
    /// </summary>
    private void LoadPresetNetworks()
    {
        if (availableNetworks.Count == 0)
        {
            // Agregar algunas redes populares por defecto
            availableNetworks.Add(PresetNetworks.PolygonAmoy());
            availableNetworks.Add(PresetNetworks.BaseSepolia());
            availableNetworks.Add(PresetNetworks.ArbitrumSepolia());
            availableNetworks.Add(PresetNetworks.RoninSaigon());
        }
    }

    // =====================================================
    // CAMBIO DE RED
    // =====================================================

    /// <summary>
    /// Cambia a una red específica
    /// </summary>
    public async Task<bool> SwitchNetwork(CustomNetworkConfig network)
    {
        try
        {
            if (!network.IsValid())
            {
                Debug.LogError($"Configuración de red inválida: {network.networkName}");
                return false;
            }

            Debug.Log($"Cambiando a {network.networkName} (Chain ID: {network.chainIdHex})");

            // Agregar la red a Privy si no existe
            await AddNetworkToPrivy(network);

            // Cambiar a la red
            await SwitchToChain(network.chainIdHex);

            currentNetwork = network;

            Debug.Log($"Cambiado exitosamente a {network.networkName}");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error cambiando de red: {e.Message}");
            return false;
        }
    }

    /// <summary>
    /// Agrega una red personalizada a Privy
    /// Equivalente a wallet_addEthereumChain
    /// </summary>
    private async Task AddNetworkToPrivy(CustomNetworkConfig network)
    {
        try
        {
            var provider = await GetPrivyProvider();

            // Crear el request para agregar la red
            var addChainRequest = new
            {
                method = "wallet_addEthereumChain",
                @params = new[]
                {
                    new
                    {
                        chainId = network.chainIdHex,
                        chainName = network.networkName,
                        nativeCurrency = new
                        {
                            name = network.currencyName,
                            symbol = network.currencySymbol,
                            decimals = network.currencyDecimals
                        },
                        rpcUrls = new[] { network.rpcUrl },
                        blockExplorerUrls = new[] { network.blockExplorerUrl }
                    }
                }
            };

            string requestJson = JsonConvert.SerializeObject(addChainRequest);

            // Enviar request a Privy
            // await provider.Request(requestJson);

            Debug.Log($"Red agregada a Privy: {network.networkName}");
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Error agregando red (puede que ya exista): {e.Message}");
            // No es crítico si la red ya existe
        }
    }

    /// <summary>
    /// Cambia a una chain específica
    /// Equivalente a wallet_switchEthereumChain
    /// </summary>
    private async Task SwitchToChain(string chainIdHex)
    {
        try
        {
            var provider = await GetPrivyProvider();

            var switchChainRequest = new
            {
                method = "wallet_switchEthereumChain",
                @params = new[]
                {
                    new { chainId = chainIdHex }
                }
            };

            string requestJson = JsonConvert.SerializeObject(switchChainRequest);

            // Enviar request a Privy
            // await provider.Request(requestJson);

            Debug.Log($"Cambiado a chain: {chainIdHex}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error en switch chain: {e.Message}");
            throw;
        }
    }

    // =====================================================
    // HELPERS
    // =====================================================

    /// <summary>
    /// Obtiene el provider de Privy
    /// </summary>
    private async Task<object> GetPrivyProvider()
    {
        // Implementación depende del SDK de Privy
        // Pseudo-código:
        // return await privyClient.GetEthereumProvider(userWalletAddress);

        await Task.Delay(100);
        return new object();
    }

    /// <summary>
    /// Obtiene la red actual
    /// </summary>
    public CustomNetworkConfig GetCurrentNetwork()
    {
        return currentNetwork;
    }

    /// <summary>
    /// Obtiene todas las redes disponibles
    /// </summary>
    public List<CustomNetworkConfig> GetAvailableNetworks()
    {
        return availableNetworks;
    }

    /// <summary>
    /// Agrega una red personalizada
    /// </summary>
    public void AddCustomNetwork(CustomNetworkConfig network)
    {
        if (!availableNetworks.Contains(network))
        {
            availableNetworks.Add(network);
            Debug.Log($"Red agregada: {network.networkName}");
        }
    }

    /// <summary>
    /// Busca una red por chain ID
    /// </summary>
    public CustomNetworkConfig FindNetworkByChainId(string chainId)
    {
        return availableNetworks.Find(n => n.chainId == chainId || n.chainIdHex == chainId);
    }

    /// <summary>
    /// Busca una red por nombre
    /// </summary>
    public CustomNetworkConfig FindNetworkByName(string name)
    {
        return availableNetworks.Find(n =>
            n.networkName.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0
        );
    }

    // =====================================================
    // MÉTODOS DE UI
    // =====================================================

    public async void OnSwitchToPolygon()
    {
        var polygon = PresetNetworks.PolygonAmoy();
        await SwitchNetwork(polygon);
    }

    public async void OnSwitchToBase()
    {
        var base_ = PresetNetworks.BaseSepolia();
        await SwitchNetwork(base_);
    }

    public async void OnSwitchToArbitrum()
    {
        var arbitrum = PresetNetworks.ArbitrumSepolia();
        await SwitchNetwork(arbitrum);
    }

    public async void OnSwitchToOptimism()
    {
        var optimism = PresetNetworks.OptimismSepolia();
        await SwitchNetwork(optimism);
    }
}
