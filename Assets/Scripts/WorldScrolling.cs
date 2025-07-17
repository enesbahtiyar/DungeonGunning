using System;
using UnityEngine;

public class WorldScrolling : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    private Vector2Int currentTilePosition = new Vector2Int(0,0);
    public Vector2Int playerTilePosition;
    [SerializeField] private float tileSize = 20f;
    private Vector2Int onTilePlayerGridPosition;
    private GameObject[,] terrainTiles;
    
    [SerializeField] int terrainTileHorizontalCount;
    [SerializeField] int terrainTileVerticalCount;

    [SerializeField] private int fieldOfVisionHeight = 3;
    [SerializeField] private int fieldOfVisionWidth = 3;

    private void Start()
    {
        terrainTiles = new GameObject[terrainTileHorizontalCount, terrainTileVerticalCount];
    }

    private void Update()
    {
        playerTilePosition.x = (int)(playerTransform.position.x / tileSize);
        playerTilePosition.y = (int)(playerTransform.position.y / tileSize);

        playerTilePosition.x -= playerTransform.position.x < 0 ? 1 : 0;
        playerTilePosition.y -= playerTransform.position.y < 0 ? 1 : 0;
        
        if (currentTilePosition != playerTilePosition)
        {
            currentTilePosition = playerTilePosition;

            onTilePlayerGridPosition.x = CalculatePositionOnAxisWithWrap(onTilePlayerGridPosition.x, true);
            onTilePlayerGridPosition.y = CalculatePositionOnAxisWithWrap(onTilePlayerGridPosition.y, false);
            
            UpdateTileOnScreen();
        }
    }

    private void UpdateTileOnScreen()
    {
        for (int pov_x = -(fieldOfVisionWidth/2); pov_x < fieldOfVisionWidth; pov_x++)
        {
            for (int pov_y = -(fieldOfVisionHeight/2); pov_y < fieldOfVisionHeight; pov_y++)
            {
                int tileToUpdate_x = CalculatePositionOnAxisWithWrap(playerTilePosition.x + pov_x, true);
                int tileToUpdate_y = CalculatePositionOnAxisWithWrap(playerTilePosition.y + pov_y, false);
                
               // Debug.Log("TileToUpdate_x" + tileToUpdate_x + "_y" + tileToUpdate_y);
                GameObject tile = terrainTiles[tileToUpdate_x, tileToUpdate_y];
                
                tile.transform.position = CalculateTilePosition(playerTilePosition.x + pov_x, playerTilePosition.y + pov_y);
            }
        }
    }

    private Vector3 CalculateTilePosition(int x, int y)
    {
        return new Vector3(x * tileSize, y * tileSize, 0);
    }

    private int CalculatePositionOnAxisWithWrap(int currentValue, bool horizontal)
    {
        if (horizontal)
        {
            if (currentValue >= 0)
            {
                currentValue = currentValue % terrainTileHorizontalCount;
            }
            else
            {
                currentValue += 1;
                currentValue = terrainTileHorizontalCount - 1
                               + currentValue % terrainTileHorizontalCount;
            }
        }
        else
        {
            if (currentValue >= 0)
            {
                currentValue = currentValue % terrainTileVerticalCount;
            }
            else
            {
                currentValue += 1;
                currentValue = terrainTileVerticalCount - 1
                               + currentValue % terrainTileVerticalCount;
            }
        }

        return (int)currentValue;
    }

    internal void AddTile(GameObject tile, Vector2Int position)
    {
        terrainTiles[position.x, position.y] = tile;
    }
}
