using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;


public class GridManager : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase highlightTile;
    public TileBase originalTile;
    public int x;
    public int y;
    private Vector3Int previousGridPos = new Vector3Int(999, 999);
    UnityEngine.Vector3 currentMousePos;
    Vector3Int currentGridPos;

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
    Vector3Int gridPos = tilemap.WorldToCell(worldPos);
    Vector3 snappedPos = tilemap.GetCellCenterWorld(gridPos);
    droppedObject.transform.position = snappedPos;
    }
}
