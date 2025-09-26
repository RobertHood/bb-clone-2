using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.Rendering;

public class BlockSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<GameObject> blockArrays = new List<GameObject>();
    
    private Vector3 firstSlot = new Vector3(-2, (float)-3.5, 0);
    private Vector3 secondSlot = new Vector3(0, (float)-3.5, 0);
    private Vector3 thirdSlot = new Vector3(2, (float)-3.5, 0);
    public List<GameObject> options;
    private List<GameObject> currentBlocks = new List<GameObject>();

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
        SpawnBlock(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (currentBlocks.Count == 0) return;
        bool allLocked = true;
        foreach (GameObject block in currentBlocks)
        {
            if (!block.GetComponent<BlockData>().isLocked)
            {
                allLocked = false;
                break;
            }
        }
        if (allLocked)
        {
            SpawnBlock();
        }
    }
}
