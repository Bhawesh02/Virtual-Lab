

using UnityEngine;

public class IndexBubblePool : PoolService<IndexBubbleController>
{
    private IndexBubbleController indexBubblePrefab;

    public IndexBubblePool(IndexBubbleController indexBubblePrefab)
    {
        this.indexBubblePrefab = indexBubblePrefab;
    }
    protected override IndexBubbleController CreateItem()
    {
        IndexBubbleController indexBubbleController = GameObject.Instantiate(indexBubblePrefab);
        return indexBubbleController;
    }
}
