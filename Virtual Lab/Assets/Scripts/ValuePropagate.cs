using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValuePropagate : MonoBehaviour
{
    private static ValuePropagate instance;
    public static ValuePropagate Instance { get { return instance; } }
    
    public List<PinConnection> OutputPins;

    public List<PinConnection> InputPins;

    public List<PinConnection> VccPins;
    public List<PinConnection> GndPins;
    public List<PinConnection> IcInputPins;
    public List<PinConnection> IcOutputPins;
    public PinConnection IcVccPin;
    public PinConnection IcGndPin;

    [SerializeField]
    private ICLogic IcLogic;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {

        
    }

    public void StartButton()
    {
        SimulatorManager.Instance.SimulationRunning = true;
        SimulatorManager.Instance.SimulationStatus.text = "Simulation Running";
        StartTransfer();
    }
    public void StartTransfer()
    {
        if (SimulatorManager.Instance.Wires.Count == 0)
            return;
        SetWiresValuePropagetedToFalse();
        foreach (PinConnection pin in VccPins)
            TransferData(pin);
        foreach (PinConnection pin in GndPins)
            TransferData(pin);
        foreach (PinConnection pin in InputPins)
        {
            TransferData(pin);
        }

        IcLogic.RunIcLogic();

    }
    private void SetWiresValuePropagetedToFalse()
    {

        foreach (WireController wire in SimulatorManager.Instance.Wires)
        {
            wire.valuePropagated = false;
        }
    }

    bool DoesThisPinTakeValue(PinConnection pin)
    {
        switch (pin.CurrentPinInfo.Type)
        {
            case PinType.Output:
            case PinType.IcInput:
            case PinType.IcVcc:
            case PinType.IcGnd:
                return true;
            default:
                return false;

        }
    }

    public void TransferData(PinConnection pin)
    {
        if (pin.value == PinValue.Null)
                return;
        if (pin.Wires.Count == 0)
                return;/*
        Debug.Log("Pin: "+pin.CurrentPinInfo.Type+" "+pin.CurrentPinInfo.PinNumber);*/
        foreach (WireController wire in pin.Wires)
        {
            if (wire.valuePropagated)
                continue;
            if (wire.initialPin == pin && DoesThisPinTakeValue(wire.finalPin))
            {
                wire.finalPin.value = wire.initialPin.value;
                wire.valuePropagated = true;
                TransferData(wire.finalPin);
            }
            else if (wire.finalPin == pin && DoesThisPinTakeValue(wire.initialPin))
            {
                wire.initialPin.value = wire.finalPin.value;
                wire.valuePropagated = true;
                TransferData(wire.initialPin);

            }
        }
    }
}
