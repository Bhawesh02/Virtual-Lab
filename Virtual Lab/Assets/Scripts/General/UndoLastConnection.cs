
using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UndoLastConnection : MonoBehaviour
{
    private Button undoButton;

    public WireController lastWire;
    private void Awake()
    {
        undoButton = GetComponent<Button>();
    }
    private void Start()
    {
        undoButton.onClick.AddListener(undoLastConnection);
    }

    private void undoLastConnection()
    {
        if (SimulatorManager.Instance.SimulationRunning)
            return;
        if (SimulatorManager.Instance.Wires.Count == 0)
            return;
        lastWire = SimulatorManager.Instance.Wires[^1];
        PinController inititalPin = lastWire.initialPin;
        PinController finalPin = lastWire.finalPin;
        ChangePinValue(inititalPin);
        ChangePinValue(finalPin);
        RemoveInputPinConnected();
        SimulatorManager.Instance.Wires.Remove(lastWire);
        inititalPin.Wires.Remove(lastWire);
        finalPin.Wires.Remove(lastWire);
        WireService.Instance.ReturnWire(lastWire);
    }

    private void RemoveInputPinConnected()
    {
        if (lastWire.connectionDirection == ConnectionDirection.Null)
            return;
        if (lastWire.connectionDirection == ConnectionDirection.InititalToFinal)
        {
            lastWire.connectionDirection = ConnectionDirection.Null;
            MakeIsInputConnectedFalse(lastWire.finalPin);
            return;
        }
        lastWire.connectionDirection = ConnectionDirection.Null;
        MakeIsInputConnectedFalse(lastWire.initialPin);
    }

    private void MakeIsInputConnectedFalse(PinController pin)
    {
        if (pin.GetComponent<OutputPinConnectionCheck>() == null)
            return;
        pin.GetComponent<OutputPinConnectionCheck>().IsInputPinConnected = false;
        foreach(WireController wire in pin.Wires)
        {
            if (wire.connectionDirection == ConnectionDirection.Null)
                continue;
            wire.connectionDirection = ConnectionDirection.Null;
            if(pin == wire.initialPin)
            {
                MakeIsInputConnectedFalse(wire.finalPin);
            }
            else
            {
                MakeIsInputConnectedFalse(wire.initialPin);

            }
        }
    }

    private static void ChangePinValue(PinController pin)
    {
        switch (pin.CurrentPinInfo.Type)
        {
            case PinType.Input:
                pin.value = PinValue.Negative;
                break;
            case PinType.Vcc:
                pin.value = PinValue.Vcc;
                break;
            case PinType.Gnd:
                pin.value = PinValue.Gnd;
                break;
            default:
                pin.value = PinValue.Null;
                break;
        }
    }
}
