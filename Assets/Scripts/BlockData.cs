using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

// Quản lý dữ liệu và tương tác của 1 block (kéo thả)
public class BlockData : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private CanvasGroup canvasGroup;
    public GridManager gm ;
    private Collider2D col;

    [Header("Block Cells")]
    public List<Transform> cells = new List<Transform>();

    [Header("State")]
    public Vector3 originPos;
    public bool isLocked = false;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        canvasGroup = GetComponent<CanvasGroup>();
        gm = GridManager.FindAnyObjectByType<GridManager>();
        // Collect only direct child transforms as cells (exclude the root transform)
        cells.Clear();
        foreach (Transform child in transform)
        {
            cells.Add(child);
        }
        transform.localScale = Vector3.one * 0.6f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isLocked) return;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isLocked) return;

        canvasGroup.blocksRaycasts = false;
        originPos = transform.position;

        if (col != null) col.enabled = false;

        transform.localScale = Vector3.one;
        transform.position = new Vector3(transform.position.x, transform.position.y, -2);
        if (gm == null) gm = GridManager.FindAnyObjectByType<GridManager>();
        gm?.StartDrag(this.gameObject);

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isLocked) return;

        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(eventData.position);
        worldPoint.z = 0f;
        transform.position = new Vector3(worldPoint.x, worldPoint.y, transform.position.z);

        // Preview ô hợp lệ sẽ được GridManager cập nhật trong Update()
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isLocked) return;

        if (col != null) col.enabled = true;
        canvasGroup.blocksRaycasts = true;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        worldPos.z = 0;
        if (gm != null)
        {
            gm.EndDrag(this.gameObject);
        }
        // Nếu block chưa được lock (drop outside) → reset về vị trí gốc
        if (!isLocked)
        {
            transform.position = originPos;
            transform.localScale = Vector3.one * 0.6f;
        }
    }

    void Update()
    {
        if (transform.childCount == 0)
        {
            Destroy(gameObject);
        }
    }
}
