
using UnityEngine;

public class MessageBubblePoolService : MonoGenericSingelton<MessageBubblePoolService>
{
    private MessageBubblePool messageBubblePool = null;
    [SerializeField]
    private MessageBubbleController messageBubblePrefab;

    protected override void Awake()
    {
        base.Awake();
        messageBubblePool = new(messageBubblePrefab);
    }
    public MessageBubbleController GetBubble()
    {
        
        return messageBubblePool.GetItem();
    }
    public void ReturnBubble(MessageBubbleController bubbleToReturn)
    {
        bubbleToReturn.transform.SetParent(transform);
        messageBubblePool.ReturnItem(bubbleToReturn);
    }
}
