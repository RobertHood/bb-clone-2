using System.Data.Common;
using UnityEngine;
using UnityEngine.Tilemaps;
public enum BlockShape
{
    I, J, L, O, S, T, Z
}
[System.Serializable]
public struct BlockShapeData
{
    public Tile tile;
    public BlockShape bs;
    public Vector2Int[] cells { get; private set; }
    public void Initialize()
    {
        cells = Data.Cells[bs];
    }
}
