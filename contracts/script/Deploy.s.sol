// SPDX-License-Identifier: MIT
pragma solidity ^0.8.20;

import "forge-std/Script.sol";
import "../src/GameResourcesERC1155.sol";

contract DeployScript is Script {
    function run() external {
        uint256 deployerPrivateKey = vm.envUint("PRIVATE_KEY");
        
        vm.startBroadcast(deployerPrivateKey);
        
        GameResourcesERC1155 gameResources = new GameResourcesERC1155();
        
        console.log("GameResourcesERC1155 deployed at:", address(gameResources));
        
        vm.stopBroadcast();
    }
}