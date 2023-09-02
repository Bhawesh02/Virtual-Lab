using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IndexBubbleController : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer backgroundSpriteRenderer;
    [SerializeField]
    private TextMeshPro textMesh;

    [SerializeField]
    private Vector2 padding =new(2f,2f);

    
    public void SetIndex(int index)
    {
        textMesh.text = "Index: " + index;
        textMesh.ForceMeshUpdate();
        Vector2 textSize = textMesh.GetRenderedValues(false);
        backgroundSpriteRenderer.size = textSize + padding;
        backgroundSpriteRenderer.transform.localPosition = textMesh.transform.localPosition;
    }
}
