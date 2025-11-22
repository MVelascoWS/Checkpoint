// SPDX-License-Identifier: MIT
pragma solidity ^0.8.24;

import "@openzeppelin/contracts/token/ERC1155/ERC1155.sol";
import "@openzeppelin/contracts/access/Ownable.sol";

/**
 * @title GameResourcesERC1155
 * @notice ERC-1155 contract para los 4 recursos del juego
 * @dev Tokens: 1=Food, 2=Wood, 3=Gold, 4=Stone
 */
contract GameResourcesERC1155 is ERC1155, Ownable {
    // Token IDs para recursos
    uint256 public constant FOOD = 1;   // 🌾
    uint256 public constant WOOD = 2;   // 🪵
    uint256 public constant GOLD = 3;   // 💰
    uint256 public constant STONE = 4;  // 🪨
    
    // Eventos personalizados
    event ResourcesProduced(address indexed player, uint256[] tokenIds, uint256[] amounts);
    event StarterPackMinted(address indexed player);
    
    constructor() ERC1155("https://game.example.com/api/resource/{id}.json") Ownable(msg.sender) {}
    
    /**
     * @notice Mint recursos iniciales al unirse (100 de cada uno)
     * @param player Address del jugador
     */
    function mintStarterPack(address player) external onlyOwner {
        require(player != address(0), "Invalid player address");
        
        uint256[] memory ids = new uint256[](4);
        uint256[] memory amounts = new uint256[](4);
        
        ids[0] = FOOD;
        ids[1] = WOOD;
        ids[2] = GOLD;
        ids[3] = STONE;
        
        amounts[0] = 100;
        amounts[1] = 100;
        amounts[2] = 100;
        amounts[3] = 100;
        
        _mintBatch(player, ids, amounts, "");
        
        emit StarterPackMinted(player);
    }
    
    /**
     * @notice Función para enviar recursos entre jugadores
     * @dev Esta función será llamada por las delegaciones
     */
    function sendResources(
        address from,
        address to,
        uint256 tokenId,
        uint256 amount
    ) external {
        require(tokenId >= FOOD && tokenId <= STONE, "Invalid token ID");
        require(to != address(0), "Invalid recipient");
        require(amount > 0, "Amount must be greater than 0");
        
        safeTransferFrom(from, to, tokenId, amount, "");
    }
    
    /**
     * @notice Función para enviar múltiples recursos
     * @dev Optimizado para batch transfers
     */
    function sendResourcesBatch(
        address from,
        address to,
        uint256[] memory tokenIds,
        uint256[] memory amounts
    ) external {
        require(to != address(0), "Invalid recipient");
        require(tokenIds.length == amounts.length, "Arrays length mismatch");
        
        for (uint i = 0; i < tokenIds.length; i++) {
            require(tokenIds[i] >= FOOD && tokenIds[i] <= STONE, "Invalid token ID");
            require(amounts[i] > 0, "Amount must be greater than 0");
        }
        
        safeBatchTransferFrom(from, to, tokenIds, amounts, "");
    }
    
    /**
     * @notice Producción periódica de recursos (llamada por backend)
     * @param player Address del jugador
     * @param amount Cantidad de cada recurso a producir
     */
    function produceResources(address player, uint256 amount) external onlyOwner {
        require(player != address(0), "Invalid player address");
        require(amount > 0, "Amount must be greater than 0");
        
        uint256[] memory ids = new uint256[](4);
        uint256[] memory amounts = new uint256[](4);
        
        for(uint i = 0; i < 4; i++) {
            ids[i] = i + 1;
            amounts[i] = amount;
        }
        
        _mintBatch(player, ids, amounts, "");
        
        emit ResourcesProduced(player, ids, amounts);
    }
    
    /**
     * @notice Actualizar URI base para metadata
     */
    function setURI(string memory newuri) external onlyOwner {
        _setURI(newuri);
    }
    
    /**
     * @notice Obtener balance de todos los recursos de un jugador
     */
    function getPlayerBalances(address player) external view returns (uint256[4] memory) {
        return [
            balanceOf(player, FOOD),
            balanceOf(player, WOOD),
            balanceOf(player, GOLD),
            balanceOf(player, STONE)
        ];
    }
}