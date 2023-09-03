
using UnityEngine;

public class IndexBubblePoolService : MonoGenericSingelton<IndexBubblePoolService>
{
    private IndexBubblePool indexBubblePool = null;
    [SerializeField]
    private IndexBubbleController indexBubblePrefab;

    protected override void Awake()
    {
        base.Awake();
        indexBubblePool = new(indexBubblePrefab);
    }
    public IndexBubbleController GetBubble()
    {
        
        return indexBubblePool.GetItem();
    }
    public void ReturnBubble(IndexBubbleController bubbleToReturn)
    {
        bubbleToReturn.transform.SetParent(transform);
        indexBubblePool.ReturnItem(bubbleToReturn);
    }
}
