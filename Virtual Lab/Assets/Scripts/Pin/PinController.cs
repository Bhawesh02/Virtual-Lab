
using System.Collections.Generic;
using UnityEngine;

public class PinController : MonoBehaviour
{
    public PinValue value;
    public PinInfo CurrentPinInfo;
    public List<WireController> Wires = new();


    private SpriteRenderer ShowColor;

    private SimulatorManager simulationManager;

    private WireService wireService;

    private int pinIndex;
    private MessageBubbleController messageBubble;
    private Vector2 messageBubbleOffset = new(0f, 0.5f);

    private void Awake()
    {
        CurrentPinInfo.pinConnection = this;
        ShowColor = null;
    }
    private void Start()
    {
        simulationManager = SimulatorManager.Instance;
        wireService = WireService.Instance;
        if (CurrentPinInfo.Type == PinType.Vcc)
            value = PinValue.Vcc;
        if (CurrentPinInfo.Type == PinType.Gnd)
            value = PinValue.Gnd;

        SendReferenceToValuePropagator();

        if (CurrentPinInfo.Type == PinType.Input || CurrentPinInfo.Type == PinType.Output)
        {
            ShowColor = transform.GetChild(0).GetComponent<SpriteRenderer>();
            ShowColor.sortingOrder = 1;
        }
    }

    private void SendReferenceToValuePropagator()
    {
        ValuePropagateService valuePropagate = ValuePropagateService.Instance;
        switch (CurrentPinInfo.Type)
        {
            case PinType.Null:
                break;
            case PinType.Gnd:
                valuePropagate.GndPins.Add(this);
                break;
            case PinType.Input:
                valuePropagate.InputPins.Add(this);
                break;
            case PinType.Output:
                valuePropagate.OutputPins.Add(this);
                break;
            case PinType.Vcc:
                valuePropagate.VccPins.Add(this);
                break;
        }
    }
    private void Update()
    {
        ChangeSpriteBasedOnValue();
    }

    private void ChangeSpriteBasedOnValue()
    {
        if (ShowColor == null)
        {
            return;
        }
        switch (value)
        {
            case PinValue.Null:
                ShowColor.sprite = null;
                break;
            case PinValue.Positive:
                ShowColor.sprite = simulationManager.PinPostive;
                break;
            case PinValue.Negative:
                ShowColor.sprite = simulationManager.PinNegative;
                break;

        }
    }

    private void OnMouseDown()
    {
        if (simulationManager.SimulationRunning || CurrentPinInfo.Type == PinType.Null)
            return;
        if (!wireService.doingConnection)
        {
            wireService.CreateNewWire(this);
            return;
        }
        wireService.CompleteExsistingWire(this);
    }

    public void SetPinIndex(int index) => pinIndex = index;
    private void OnMouseEnter()
    {
        if (!simulationManager.SimulationRunning || (CurrentPinInfo.Type != PinType.Input && CurrentPinInfo.Type != PinType.Output) || Wires.Count == 0)
            return;
        messageBubble = MessageBubblePoolService.Instance.GetBubble();
        messageBubble.transform.SetParent(transform);
        messageBubble.SetIndex(pinIndex);
        if (CurrentPinInfo.Type == PinType.Output)
            messageBubbleOffset.x = 0.5f;
        messageBubble.transform.position = (Vector2)transform.position + messageBubbleOffset;
    }
    private void OnMouseExit()
    {
        if (messageBubble != null)
            MessageBubblePoolService.Instance.ReturnBubble(messageBubble);

    }
}
