using System;
using UnityEngine;

public class TerrainTile : MonoBehaviour
{
    [SerializeField] private Vector2Int tilePosition;
    [SerializeField] WorldScrolling worldScrolling;
    private void Start()
    {
        worldScrolling.AddTile(gameObject, tilePosition);
    }
}
