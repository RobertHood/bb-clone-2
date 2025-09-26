using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;


public class GridManager: MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase highlightTile;
    public TileBase originalTile;
    public int x;
    public int y;
    private Vector3Int previousGridPos = new Vector3Int(999, 999);
    UnityEngine.Vector3 currentMousePos;
    Vector3Int currentGridPos;

    private Vector3Int gridMin = new Vector3Int(-2, -6, 0);
    private Vector3Int gridMax = new Vector3Int(5, 1, 0);

    private Dictionary<Vector3Int, int> gridMap = new Dictionary<Vector3Int, int>();

    //exp block
    //Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (tilemap == null)
        {
            tilemap = GetComponent<Tilemap>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentMousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
        currentMousePos.z = 0;
        currentGridPos = tilemap.WorldToCell(currentMousePos);

        ClearHighlight();
        if (currentGridPos.x >= -2 && currentGridPos.x <= 5 && currentGridPos.y >= -6 && currentGridPos.y <= 1) // gioi han trong grid //cai nay chac viet lai bang cellbounds dc
        {
            SetHighlight();
        }
        //debug
        x = currentGridPos.x;
        y = currentGridPos.y;
    }

    void ClearHighlight()
    {
        if (currentGridPos != previousGridPos)
        {
            if (tilemap.GetTile(previousGridPos) == highlightTile)
            {
                tilemap.SetTile(previousGridPos, originalTile);
            }
        }
    }

    void SetHighlight()
    {
        if (tilemap.GetTile(currentGridPos) == originalTile)
        {
            tilemap.SetTile(currentGridPos, highlightTile);
            previousGridPos = currentGridPos;
        }
    }

    public void HandleDrop(GameObject droppedObject, Vector3 worldPos)
    {
        Vector3Int[] targetPos = GetTargetGridPositions(droppedObject);

        foreach (Vector3Int gridPos in targetPos)
        {
            if (!IsCellFree(gridPos) || !IsCellWithinBound(gridPos))
            {
                droppedObject.transform.position = droppedObject.GetComponent<BlockData>().originPos;
                return;
            }
        }

        SnapToGrid(droppedObject, targetPos);
    }

    private void SnapToGrid(GameObject droppedObject, Vector3Int[] targetPos)
    {
        Vector3 firstCellCenter = tilemap.GetCellCenterWorld(targetPos[0]);
        Vector3 offset = firstCellCenter - droppedObject.GetComponent<BlockData>().cells[0].position;
        droppedObject.GetComponent<BlockData>().isLocked = true;
        if (droppedObject.GetComponent<Collider2D>() != null)
        {
            droppedObject.GetComponent<Collider2D>().enabled = false;
        }

        droppedObject.transform.position += offset;
        foreach (Vector3Int gridPos in targetPos)
        {
            SetGridPosValue(gridPos, 1);
        }
    }

    private void SetGridPosValue(Vector3Int gridPos, int v)
    {
        gridMap[gridPos] = v;
    }

    private bool IsCellFree(Vector3Int gridPos)
    {
        return GetGridPosValue(gridPos) == 0;
    }

    private int GetGridPosValue(Vector3Int gridPos)
    {
        return gridMap.TryGetValue(gridPos, out int value) ? value : 0;
    }

    private Vector3Int[] GetTargetGridPositions(GameObject droppedObject)
    {
        BlockData blockdata = droppedObject.GetComponent<BlockData>();
        Vector3Int[] positions = new Vector3Int[blockdata.cells.Count];
        for (int i = 0; i < blockdata.cells.Count; i++)
        {
            Vector3 worldCellPos = blockdata.cells[i].position;
            positions[i] = tilemap.WorldToCell(worldCellPos);
        }
        return positions;
    }
    private bool IsCellWithinBound(Vector3Int gridPos)
    {
            return gridPos.x >= gridMin.x && gridPos.x <= gridMax.x &&
            gridPos.y >= gridMin.y && gridPos.y <= gridMax.y;
    }
}
