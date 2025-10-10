using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

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
        gm = FindObjectOfType<GridManager>();
        // Lưu các ô con của block
        foreach (Transform child in transform)
        {
            cells.Add(child);
        }

        // Scale mặc định khi spawn block
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

        // Scale về 1 khi bắt đầu kéo
        transform.localScale = Vector3.one;
        transform.position = new Vector3(transform.position.x, transform.position.y, -2);

        // Thông báo GridManager biết block này đang drag
        FindObjectOfType<GridManager>().StartDrag(this.gameObject);

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isLocked) return;

        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(eventData.position);
        worldPoint.z = 0f;
        transform.position = new Vector3(worldPoint.x, worldPoint.y, transform.position.z);

        // Preview sẽ tự động cập nhật trong GridManager.Update()
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
            Debug.Log("Dropped outside grid → Reset");
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
