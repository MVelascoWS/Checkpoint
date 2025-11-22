// SPDX-License-Identifier: MIT
pragma solidity ^0.8.20;

import "forge-std/Script.sol";
import "../src/GameResourcesERC1155.sol";

contract TestContractScript is Script {
    function run() external {
        uint256 deployerPrivateKey = vm.envUint("PRIVATE_KEY");
        address contractAddress = vm.envAddress("CONTRACT_ADDRESS");
        
        vm.startBroadcast(deployerPrivateKey);
        
        GameResourcesERC1155 game = GameResourcesERC1155(contractAddress);
        
        // Mint starter pack al deployer
        address deployer = vm.addr(deployerPrivateKey);
        game.mintStarterPack(deployer);
        
        console.log("Starter pack minted to:", deployer);
        
        // Verificar balances
        uint256[4] memory balances = game.getPlayerBalances(deployer);
        console.log("Food:", balances[0]);
        console.log("Wood:", balances[1]);
        console.log("Gold:", balances[2]);
        console.log("Stone:", balances[3]);
        
        vm.stopBroadcast();
    }
}