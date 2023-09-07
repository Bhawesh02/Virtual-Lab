
using UnityEngine;

public class IndexController : MonoBehaviour
{
    [SerializeField]
    private int pinIndex;

    private PinController pinController;


    private MessageBubbleController indexBubble;

    [SerializeField]
    private Vector2 offset = new(0f, 0.5f);
    private void Awake()
    {
        pinController = GetComponent<PinController>();
    }
    public void SetPinIndex(int index) => pinIndex = index;

    private void OnMouseEnter()
    {
        if (!SimulatorManager.Instance.SimulationRunning || pinController.Wires.Count == 0)
            return;
        
        indexBubble = MessageBubblePoolService.Instance.GetBubble();
        indexBubble.transform.SetParent(transform);
        indexBubble.SetIndex(pinIndex);
        indexBubble.transform.position = (Vector2)transform.position + offset;
    }
    private void OnMouseExit()
    {
        if (indexBubble != null)
            MessageBubblePoolService.Instance.ReturnBubble(indexBubble);

    }
}
