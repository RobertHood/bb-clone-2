using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class BlockData : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Collider2D col;

    public List<Transform> cells = new List<Transform>();
    public Vector3 originPos;
    public bool isLocked = false;
    private void Awake()
    {
        col = GetComponent<Collider2D>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        foreach (Transform child in transform)
        {
            cells.Add(child);
        }
        if (sr != null)
        {
            sr.sortingLayerName = "Block";
            sr.sortingOrder = 6;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isLocked)
        {
            return;
        }
        canvasGroup.blocksRaycasts = false;
        originPos = transform.position;
        Debug.Log("OnBeginDrag");
        if (col != null) // tắt colider trong lúc kéo block
        {
            col.enabled = false;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (isLocked)
        {
            return;
        }
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(eventData.position);
        worldPoint.z = 0f;
        transform.position = worldPoint;
        Debug.Log("OnDrag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isLocked)
        {
            return;
        }
        if (col != null)
        {
            col.enabled = true;
        }
        Debug.Log("OnEndDrag");
        canvasGroup.blocksRaycasts = true;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        worldPos.z = 0;
        int tilemapLayerMask = LayerMask.GetMask("Tilemap");

        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero, Mathf.Infinity, tilemapLayerMask); // hit tra ve mot tia raycast

        if (hit.collider != null && hit.collider.GetComponent<Tilemap>() != null)
        //neu ma raycast duoc mot tia tu con tro chuot den tilemap
        {
            GridManager gridManager = hit.collider.GetComponent<GridManager>();
            if (gridManager != null)
            {
                gridManager.HandleDrop(this.gameObject, worldPos);
            }
        }
        else
        {
            Debug.Log("Dropped outside tilemap");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {  
        if (isLocked)
        {
            return;
        }
        Debug.Log("OnPointerDown");
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
