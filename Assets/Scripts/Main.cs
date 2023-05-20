using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using JetBrains.Annotations;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Main : MonoBehaviour
{
    public static Main Instance;
    public GameObject[] blockPrefabs;
    public Transform[] stackParents;
    public BlockRoot blockDatas;
    public GameObject infoPanel;
    public TMP_Text infoTitle, infoDescription;

    [SerializeField] Camera cam;

    List<GameObject> glassBlocks = new List<GameObject>();
    Vector3 blockSize = new(3.5f, 0.65f, 1f);
    Block lastHighlightedBlock;
    bool isSelectionOn;


    private void Awake()
    {
        Instance = this;
        Physics.gravity = Vector3.zero;
    }

    void Start()
    {
        //Get data from Api, serialize from json and build blocks
        Api.Instance.GetBlocksData((o) =>
        {
            blockDatas = JsonUtility.FromJson<BlockRoot>("{\"items\":" + o + "}");
            blockDatas.items = blockDatas.items
            .OrderBy(o => o.grade)
            .ThenBy(o => o.domain)
            .ThenBy(o => o.cluster)
            .ThenBy(o => o.standardid)
            .ToList();
            BuildBlocks();
        });
    }

    void Update()
    {
        //Right click on blocks to get info panel
        if (Input.GetMouseButtonDown(1))
        {
            if (MouseArea.isInside)
            {
                RaycastForInfo();
            }
        }
        //Deselect highlighted block on mouse up
        if (Input.GetMouseButtonUp(1))
        {
           infoPanel.SetActive(false);
            isSelectionOn = false;
        }
        RaycastForHighlight();
    }

    //Build blocks. Starting from bottom back.
    void BuildBlocks()
    {
        int[] blockCount = new int[3];

        for (int i = 0; i < blockDatas.items.Count; i++)
        {
            int stackIndex = GetStackIndex(blockDatas.items[i].grade);
            int index = blockCount[stackIndex];
            Vector3 pos = new Vector3(0, 0, 0);
            GameObject go = Instantiate(blockPrefabs[blockDatas.items[i].mastery], stackParents[stackIndex]);

            if(blockDatas.items[i].mastery == 0)
                glassBlocks.Add(go);

            go.GetComponent<Block>().Set(blockDatas.items[i]);

            pos.y = blockSize.y / 2f + (index / 3) * blockSize.y;
            if ((index / 3) % 2 == 1)
            {
                pos.x = ((blockSize.x / 2f) - (blockSize.z / 2f)) * (index % 3 - 1f);
                go.transform.localRotation = Quaternion.Euler(-90f, 0f, 90f);
            }
            else
            {
                pos.z = ((blockSize.x / 2f) - (blockSize.z / 2f)) * (-index % 3 + 1f);
            }
            go.transform.localPosition = pos;

            blockCount[stackIndex]++;
        }

        //Convert string to index
        int GetStackIndex(string grade)
        {
            switch (grade)
            {
                case "7th Grade":
                    return 1;
                case "8th Grade":
                    return 2;
                default:
                    return 0;
            }
        }
    }

    //Raycast on right click. If raycast to mouse position hits a block, it is highlighted with specific color
    //and show info panel filled with BlockData properties
    public void RaycastForInfo()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        int layerMask = 1 << 6;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            Block block = hit.transform.gameObject.GetComponent<Block>();
            block.Highlight(true);
            infoPanel.SetActive(true);
            infoTitle.text = $"<#4fdee3><b>{block.blockData.grade}:</b></color> {block.blockData.domain}";
            infoDescription.text = $"{block.blockData.cluster}\n\n <#f26d95><b>{block.blockData.standardid}:</b></color> {block.blockData.standarddescription}";
            Debug.Log(block.blockData.standarddescription);
            isSelectionOn = true;
        }
    }

    //Raycast to select block under mouse cursor.
    public void RaycastForHighlight()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        int layerMask = 1 << 6;

        if (lastHighlightedBlock != null && !isSelectionOn)
            lastHighlightedBlock.UnHighlight();

        if (!isSelectionOn && Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            Block block = hit.transform.gameObject.GetComponent<Block>();
            lastHighlightedBlock = block;
            block.Highlight(false);
        }
    }

    //Remove glass blocks and activate gravity.
    public void ChallengeStacks()
    {
        int blockCount = glassBlocks.Count;
        for (int i = 0; i < blockCount; i++)
        {
            Destroy(glassBlocks[i]);
        }
        glassBlocks.Clear();
        Physics.gravity = new Vector3(0f, -9.8f, 0f);
    }

    [System.Serializable]
    public class BlockRoot
    {
        public List<BlockData> items;
    }
}

[System.Serializable]
public class BlockData
{
    public int id;
    public string subject;
    public string grade;
    public int mastery;
    public string domainid;
    public string domain;
    public string cluster;
    public string standardid;
    public string standarddescription;
}