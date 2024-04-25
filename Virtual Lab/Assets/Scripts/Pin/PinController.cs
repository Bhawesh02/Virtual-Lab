using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PinController : MonoBehaviour
{
    public PinValue value;
    public PinInfo CurrentPinInfo;
    public List<WireController> Wires = new();

    [SerializeField] private Light2D colorLight;

    private SimulatorManager simulationManager;

    private WireService wireService;

    private int pinIndex;
    private MessageBubbleController messageBubble;
    private Vector2 messageBubbleOffset = new(0f, 0.5f);

    private void Awake()
    {
        CurrentPinInfo.pinConnection = this;
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

        if (colorLight != null)
        {
            colorLight.color = Color.black;
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
        if (!colorLight)
        {
            return;
        }

        switch (value)
        {
            case PinValue.Null:
                colorLight.color = Color.black;
                break;
            case PinValue.Positive:
                colorLight.color = simulationManager.PinPostive;
                break;
            case PinValue.Negative:
                colorLight.color = simulationManager.PinNegative;
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
        if (!simulationManager.SimulationRunning ||
            (CurrentPinInfo.Type != PinType.Input && CurrentPinInfo.Type != PinType.Output) || Wires.Count == 0)
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