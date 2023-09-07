
using TMPro;
using UnityEngine;

public class MessageBubbleController : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer backgroundSpriteRenderer;
    [SerializeField]
    private TextMeshPro textMesh;

    [SerializeField]
    private Vector2 padding =new(0.3f,0.2f);

    
    public void SetIndex(int index)
    {
        textMesh.text = "Index: " + index;
        padding.x = 0.2f;
        SetSizeAndPos();
    }

    private void SetSizeAndPos()
    {
        textMesh.ForceMeshUpdate();
        Vector2 textSize = textMesh.GetRenderedValues(false);
        backgroundSpriteRenderer.size = textSize + padding;
        backgroundSpriteRenderer.transform.localPosition = textMesh.transform.localPosition;
    }

    public void SetMessage(string message)
    {
        textMesh.text = message;
        padding.x = 1f;
        SetSizeAndPos();
    }
}
