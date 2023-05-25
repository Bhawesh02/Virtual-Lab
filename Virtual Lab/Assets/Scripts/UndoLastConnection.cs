
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
        PinConnection inititalPin = lastWire.initialPin;
        PinConnection finalPin = lastWire.finalPin;
        switch (inititalPin.CurrentPinInfo.Type)
        {
            case PinType.Input:
                inititalPin.value = PinValue.Negative;
                break;
            case PinType.Vcc:
                inititalPin.value = PinValue.Vcc;
                break;
            case PinType.Gnd:
                inititalPin.value = PinValue.Gnd;
                break;
            default:
                inititalPin.value = PinValue.Null;
                break;
        }
        switch (finalPin.CurrentPinInfo.Type)
        {
            case PinType.Input:
                finalPin.value = PinValue.Negative;
                break;
            case PinType.Vcc:
                finalPin.value = PinValue.Vcc;
                break;
            case PinType.Gnd:
                finalPin.value = PinValue.Gnd;
                break;
            default:
                finalPin.value = PinValue.Null;
                break;
        }
        SimulatorManager.Instance.Wires.Remove(lastWire);
        inititalPin.Wires.Remove(lastWire);
        finalPin.Wires.Remove(lastWire);
        Destroy(lastWire.gameObject);
        

    }
}
