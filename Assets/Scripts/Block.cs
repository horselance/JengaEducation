using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Block : MonoBehaviour
{
    public BlockData blockData;
    public MeshRenderer meshRenderer;
    [SerializeField] TextMeshPro txt_Type1, txt_Type2;
    public Color defaultColor, selectedColor, highlightColor;
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    public void Set(BlockData data)
    {
        blockData = data;
        txt_Type1.text = txt_Type2.text = MasteryType(blockData.mastery);

    }
    public void Highlight(bool selected)
    {
        meshRenderer.material.color = selected ? selectedColor : highlightColor;
    }

    public void UnHighlight()
    {
        meshRenderer.material.color = defaultColor;
    }

    string MasteryType(int masteryNo)
    {
        switch (masteryNo)
        { 
            default:
                return "";
            case 1:
                return "Learned";
            case 2:
                return "Mastered";
        }
    }
}
