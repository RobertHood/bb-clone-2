using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

// Sinh (spawn) các block lựa chọn cho người chơi
public class BlockSpawner : MonoBehaviour
{
    
    public List<GameObject> blockArrays = new List<GameObject>();

    private Vector3 firstSlot = new Vector3(-2, (float)-3.5, -1);
    private Vector3 secondSlot = new Vector3(0, (float)-3.5, -1);
    private Vector3 thirdSlot = new Vector3(2, (float)-3.5, -1);
    public List<GameObject> options;
    public List<GameObject> currentBlocks = new List<GameObject>();

    private GameObject GenerateSmartBlock(GridManager gm)
    {
        // 1️⃣ Weighted random
        // Giả sử mỗi block prefab có trọng số difficulty
        // có thể lưu trong ScriptableObject hoặc component nhỏ

        List<GameObject> candidates = new List<GameObject>(blockArrays);
        // Tạo phân bố xác suất đơn giản (ví dụ dựa theo số ô)
        float totalWeight = 0f;
        Dictionary<GameObject, float> weights = new Dictionary<GameObject, float>();
        foreach (var prefab in candidates)
        {
            // Count direct children as cells; BlockData.cells may be empty on prefab assets
            int cellCount = 0;
            foreach (Transform child in prefab.transform)
                cellCount++;
            float w = 1f / Mathf.Max(cellCount, 1); // block càng to → xác suất càng thấp
            weights[prefab] = w;
            totalWeight += w;
        }

        // 2️⃣ Random theo trọng số
        float rand = Random.value * totalWeight;
        foreach (var kv in weights)
        {
            rand -= kv.Value;
            if (rand <= 0)
                return kv.Key;
        }
        return candidates[0];
    }

    private bool IsBlockPlaceable(GridManager gm, GameObject prefab)
    {
        // Duyệt tất cả các cell trống
        for (int x = gm.minX; x <= gm.maxX; x++)
        for (int y = gm.minY; y <= gm.maxY; y++)
        {
            Vector3Int cell = new Vector3Int(x, y, 0);
            if (!gm.IsCellFree(cell)) continue;

            // kiểm tra nếu khối này đặt được vào vị trí đó
            Vector3Int[] testCells = gm.GetPreviewCellsAtGrid(prefab, cell);
            bool valid = true;
            foreach (var c in testCells)
            {
                if (!gm.IsInsideGrid(c) || !gm.IsCellFree(c))
                {
                    valid = false;
                    break;
                }
            }
            if (valid) return true;
        }
        return false;
}

    public void SpawnBlock()

    {
        currentBlocks.Clear();
        GridManager gm = FindAnyObjectByType<GridManager>();

        // Kiểm tra đủ số lượng prefab khác nhau
        HashSet<GameObject> uniqueCheck = new HashSet<GameObject>(blockArrays);
        if (uniqueCheck.Count < 3)
        {
            Debug.LogWarning("BlockSpawner < 3");
        }

        Vector3[] slots = { firstSlot, secondSlot, thirdSlot };
        HashSet<GameObject> usedPrefabs = new HashSet<GameObject>();

        for (int i = 0; i < 3; i++)
        {
            GameObject prefab = null;
            int attempts = 0;

            // Lặp tối đa 10 lần để tìm block phù hợp
            while (attempts < 20)
            {
                attempts++;
                var candidate = GenerateSmartBlock(gm);
                // bỏ qua candidate nếu đã dùng
			    if (usedPrefabs.Contains(candidate)) continue;
                if (IsBlockPlaceable(gm, candidate))
                {
                    prefab = candidate;
                    break;
                }
            }

            // Nếu không tìm được block hợp lệ sau 10 lần → random fallback
            if (prefab == null)
            {
                // Build danh sách còn lại chưa dùng
                List<GameObject> remaining = new List<GameObject>();
                foreach (var p in blockArrays)
                {
                    if (!usedPrefabs.Contains(p))
                        remaining.Add(p);
                }

                if (remaining.Count > 0)
                {
                    prefab = remaining[Random.Range(0, remaining.Count)];
                }
                else
                {
                    // Không còn prefab khác để chọn (trường hợp unique < 3)
                    // Ở đây quyết định: hoặc báo cảnh báo và bỏ qua, hoặc buộc chọn lại kể cả trùng.
                    // Theo yêu cầu "không được trùng", ta sẽ chỉ cảnh báo và RETURN để tránh vi phạm.
                    Debug.LogWarning("BlockSpawner: Không còn prefab khác để đảm bảo 3 khối khác nhau. Dừng spawn lô này.");
                    return;
                }
            }
            GameObject block = Instantiate(prefab, transform);
            block.transform.position = slots[i];
            currentBlocks.Add(block);
            usedPrefabs.Add(prefab);
        }
    }
        // currentBlocks.Clear();
        // options = new List<GameObject>(blockArrays);
        // for (int i = 0; i < options.Count; i++)
        // {
        //     int rand = Random.Range(i, options.Count);
        //     var temp = options[i];
        //     options[i] = options[rand];
        //     options[rand] = temp;
        // }

        // var block1 = Instantiate(options[Random.Range(0, options.Count - 1)], transform);
        // var block2 = Instantiate(options[Random.Range(0, options.Count - 1)], transform);
        // var block3 = Instantiate(options[Random.Range(0, options.Count - 1)], transform);

        // block1.GetComponent<Transform>().position = firstSlot;
        // block2.GetComponent<Transform>().position = secondSlot;
        // block3.GetComponent<Transform>().position = thirdSlot;

        // currentBlocks.Add(block1);
        // currentBlocks.Add(block2);
        // currentBlocks.Add(block3);
    void Start()
    {
        // Spawn bộ block ban đầu
        SpawnBlock();
    }

    // Kiểm tra trạng thái các block hiển thị; nếu tất cả đã lock (đặt) thì spawn lô mới
    // ... (Giữ nguyên hàm Update và GetCurrentBlocks)
    void Update()
    {
        if (currentBlocks.Count == 0) return;

        bool allLocked = true;

        for (int i = currentBlocks.Count - 1; i >= 0; i--)
        {
            GameObject block = currentBlocks[i];

            if (block == null)
            {
                currentBlocks.RemoveAt(i);
                continue;
            }

            BlockData data = block.GetComponent<BlockData>();
            if (data != null && data.isLocked)
            {
                currentBlocks.RemoveAt(i);
            }
            else {
                allLocked = false;
            }
        }

        if (currentBlocks.Count == 0)
        {
            SpawnBlock();
        }
    }

    public List<GameObject> GetCurrentBlocks()
    {
        // Sửa lại một chút để nó chỉ trả về những block chưa bị khóa
        List<GameObject> unplacedBlocks = new List<GameObject>();
        foreach (var block in currentBlocks)
        {
            if (block != null && !block.GetComponent<BlockData>().isLocked)
            {
                unplacedBlocks.Add(block);
            }
        }
        return unplacedBlocks;
    }
}
