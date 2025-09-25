using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class BlockData : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Collider2D col;
    private void Awake()
    {
        col = GetComponent<Collider2D>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        foreach (Transform child in transform) // duyêt các block add vào cell
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
        if (isLocked) // nếu bị khóa thì ko cho người chơi kéo block nữa
        {
            return;
        }
        canvasGroup.blocksRaycasts = false; // tắt raycasts để grid nhận chuột
        originPos = transform.position; // lưu lại vị trí ban đầu để xảy ra lỗi thả block về vị trí đầu tiên
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
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(eventData.position); // dịch tọa độ màn hình sang tọa độ thế giới game
        worldPoint.z = 0f; // đặt z = 0 để đảm bảo luôn nằm trong mặt phẳng 2d
        transform.position = worldPoint;
        Debug.Log("OnDrag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
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
        Debug.Log("OnPointerDown");
    }

    public void OnDrop(PointerEventData eventData)
    {
        
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
