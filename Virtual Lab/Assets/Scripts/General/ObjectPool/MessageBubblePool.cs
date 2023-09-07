

using UnityEngine;

public class MessageBubblePool : PoolService<MessageBubbleController>
{
    private MessageBubbleController messageBubblePrefab;

    private MessageBubbleController messageBubbleController;

    public MessageBubblePool(MessageBubbleController indexBubblePrefab)
    {
        this.messageBubblePrefab = indexBubblePrefab;
    }
    public override MessageBubbleController GetItem()
    {
        messageBubbleController = base.GetItem();
        messageBubbleController.gameObject.SetActive(true);
        return messageBubbleController;
    }
    protected override MessageBubbleController CreateItem()
    {
        messageBubbleController = GameObject.Instantiate(messageBubblePrefab);
        return messageBubbleController;
    }
    public override void ReturnItem(MessageBubbleController item)
    {
        item.gameObject.SetActive(false);
        base.ReturnItem(item);
    }
}
