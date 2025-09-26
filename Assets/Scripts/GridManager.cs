using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    [Header("Tilemap & Tile Assets")]
    public Tilemap tilemap;
    public TileBase highlightTile;
    public TileBase originalTile;

    [Header("Board Settings (8x8)")]
    public int minX = -2, maxX = 5;   // theo trục X
    public int minY = -6, maxY = 1;   // theo trục Y

    // Lưu preview highlight
    private List<Vector3Int> previousPreview = new List<Vector3Int>();
    private Dictionary<Vector3Int, int> gridMap = new Dictionary<Vector3Int, int>();

    // Đối tượng đang drag
    private GameObject objectBeingDragged;

    void Start()
    {
        if (tilemap == null)
            tilemap = GetComponent<Tilemap>();
    }

    void Update()
    {
        if (objectBeingDragged == null) return;

        // Lấy vị trí chuột trong world & cell
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
        mouseWorld.z = 0;

        // Lấy preview cell positions cho toàn block
        Vector3Int[] previewCells = GetPreviewCells(objectBeingDragged, mouseWorld);

        // Clear highlight cũ
        ClearHighlight();

        // Set highlight mới (nếu hợp lệ trong board)
        List<Vector3Int> validCells = new List<Vector3Int>();
        foreach (var pos in previewCells)
        {
            if (IsInsideGrid(pos))
                validCells.Add(pos);
        }
        SetHighlight(validCells);
    }

    // --- Khi bắt đầu drag block ---
    public void StartDrag(GameObject block)
    {
        objectBeingDragged = block;
    }

    // --- Khi thả block ---
    public void EndDrag(GameObject block)
    {
        if (objectBeingDragged != block) return;

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
        mouseWorld.z = 0;
        Vector3Int[] targetCells = GetPreviewCells(block, mouseWorld);

        // Kiểm tra hợp lệ
        foreach (var cell in targetCells)
        {
            if (!IsInsideGrid(cell) || !IsCellFree(cell))
            {
                // Trả block về vị trí cũ
                block.transform.position = block.GetComponent<BlockData>().originPos;
                objectBeingDragged = null;
                ClearHighlight();
                return;
            }
        }

        // Snap vào grid
        SnapToGrid(block, targetCells);
        objectBeingDragged = null;
        ClearHighlight();
    }

    // --- Xử lý Snap ---
    private void SnapToGrid(GameObject block, Vector3Int[] targetCells)
    {
        Vector3 firstCellCenter = tilemap.GetCellCenterWorld(targetCells[0]);
        Vector3 offset = firstCellCenter - block.GetComponent<BlockData>().cells[0].position;

        block.GetComponent<BlockData>().isLocked = true;
        if (block.GetComponent<Collider2D>() != null)
            block.GetComponent<Collider2D>().enabled = false;

        block.transform.position += offset;

        foreach (var cell in targetCells)
            SetGridPosValue(cell, 1);
    }

    // --- Clear highlight ---
    private void ClearHighlight()
    {
        foreach (var pos in previousPreview)
        {
            if (tilemap.GetTile(pos) == highlightTile)
                tilemap.SetTile(pos, originalTile);
        }
        previousPreview.Clear();
    }

    // --- Set highlight ---
    private void SetHighlight(List<Vector3Int> cells)
    {
        foreach (var pos in cells)
        {
            if (tilemap.GetTile(pos) == originalTile)
            {
                tilemap.SetTile(pos, highlightTile);
                previousPreview.Add(pos);
            }
        }
    }

    // --- Lấy preview cell positions của block dựa theo chuột ---
    private Vector3Int[] GetPreviewCells(GameObject block, Vector3 mouseWorld)
    {
        BlockData data = block.GetComponent<BlockData>();
        Vector3Int[] positions = new Vector3Int[data.cells.Count];

        // Tính offset: từ cell đầu tiên của block đến vị trí chuột
        Vector3Int mouseCell = tilemap.WorldToCell(mouseWorld);
        Vector3 cellCenter = tilemap.GetCellCenterWorld(mouseCell);
        Vector3 offset = cellCenter - data.cells[0].position;

        for (int i = 0; i < data.cells.Count; i++)
        {
            Vector3 worldPos = data.cells[i].position + offset;
            positions[i] = tilemap.WorldToCell(worldPos);
        }
        return positions;
    }

    // --- Helpers ---
    private void SetGridPosValue(Vector3Int gridPos, int v) => gridMap[gridPos] = v; // dánh dấu cell có block 1 hoặc 0
    private bool IsCellFree(Vector3Int gridPos) => GetGridPosValue(gridPos) == 0; // kiểm tra ô còn trống không
    private int GetGridPosValue(Vector3Int gridPos) => gridMap.TryGetValue(gridPos, out int value) ? value : 0; 

    private bool IsInsideGrid(Vector3Int pos) // kiểm tra có nằm trong khung 8x8 không
    {
        return pos.x >= minX && pos.x <= maxX && pos.y >= minY && pos.y <= maxY;
    }
}
