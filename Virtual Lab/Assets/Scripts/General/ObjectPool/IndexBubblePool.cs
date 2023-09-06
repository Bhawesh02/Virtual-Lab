

using UnityEngine;

public class IndexBubblePool : PoolService<IndexBubbleController>
{
    private IndexBubbleController indexBubblePrefab;

    private IndexBubbleController indexBubbleController;

    public IndexBubblePool(IndexBubbleController indexBubblePrefab)
    {
        this.indexBubblePrefab = indexBubblePrefab;
    }
    public override IndexBubbleController GetItem()
    {
        indexBubbleController = base.GetItem();
        indexBubbleController.gameObject.SetActive(true);
        return indexBubbleController;
    }
    protected override IndexBubbleController CreateItem()
    {
        indexBubbleController = GameObject.Instantiate(indexBubblePrefab);
        return indexBubbleController;
    }
    public override void ReturnItem(IndexBubbleController item)
    {
        item.gameObject.SetActive(false);
        base.ReturnItem(item);
    }
}
