using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEditor.XR;
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

    public void SpawnBlock()
    {
        currentBlocks.Clear();
        options = new List<GameObject>(blockArrays);
        for (int i = 0; i < options.Count; i++)
        {
            int rand = Random.Range(i, options.Count);
            var temp = options[i];
            options[i] = options[rand];
            options[rand] = temp;
        }

        var block1 = Instantiate(options[Random.Range(0, options.Count - 1)], transform);
        var block2 = Instantiate(options[Random.Range(0, options.Count - 1)], transform);
        var block3 = Instantiate(options[Random.Range(0, options.Count - 1)], transform);

        block1.GetComponent<Transform>().position = firstSlot;
        block2.GetComponent<Transform>().position = secondSlot;
        block3.GetComponent<Transform>().position = thirdSlot;

        currentBlocks.Add(block1);
        currentBlocks.Add(block2);
        currentBlocks.Add(block3);

        
    }
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
