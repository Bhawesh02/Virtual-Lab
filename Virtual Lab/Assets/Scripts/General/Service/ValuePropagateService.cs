
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValuePropagateService : MonoGenericSingelton<ValuePropagateService> 
{

    public List<PinController> OutputPins;
    public List<PinController> InputPins;
    public List<PinController> VccPins;
    public List<PinController> GndPins;
    public List<PinController> IcInputPins;
    public List<PinController> IcOutputPins;
    public List<PinController> IcVccPin;
    public List<PinController> IcGndPin;
    public List<ICView> ICViews;

    private SimulatorManager simulatorManager;
    private void Start()
    {
        simulatorManager =SimulatorManager.Instance;
        EventService.Instance.SimulationStarted += StartTransfer;
        EventService.Instance.InputValueChanged += StartTransfer;
        EventService.Instance.OutputPinValueChange += TransferNewOutputValue;
    }
    private void SetWiresValuePropagetedToFalse()
    {

        for (int i = 0; i < simulatorManager.WiresInSystem.Count; i++)
        {
            simulatorManager.WiresInSystem[i].valuePropagated = false;
        }


    }

    public void StartTransfer()
    {
        if (SimulatorManager.Instance.WiresInSystem.Count == 0)
        {
            EventService.Instance.InvokeShowError("NO wires connected");
            return;
        }
        SetWiresValuePropagetedToFalse();
        for (int i = 0; i < VccPins.Count; i++)
        {
            TransferData(VccPins[i]);
        }

        for (int i = 0; i < GndPins.Count; i++)
        {
            TransferData(GndPins[i]);
        }

        for (int i = 0; i < InputPins.Count; i++)
        {
            TransferData(InputPins[i]);
        }
        for (int i = 0; i < ICViews.Count; i++)
        {
            ICViews[i].Controller.RunIcLogic();
        }
        EventService.Instance.InvokeAllValuePropagated();
    }


    private void TransferNewOutputValue(PinController pin)
    {
        Debug.Log(pin.transform.name + " Value Changed");
        ResetValueProgatedForWiresInPin(pin);
        TransferData(pin);
    }

    private static void ResetValueProgatedForWiresInPin(PinController pin)
    {
        PinController transferedToPin = null;
        foreach (WireController wire in pin.Wires)
        {
            if (!wire.valuePropagated)
            { continue; }
            if (wire.connectionDirection == ConnectionDirection.InititalToFinal)
                transferedToPin = wire.finalPin;
            else if (wire.connectionDirection == ConnectionDirection.FinalToInitial)
                transferedToPin = wire.initialPin;
            wire.valuePropagated = false;
            ResetValueProgatedForWiresInPin(transferedToPin);
        }
    }

    public void TransferData(PinController pin)
    {
        if (pin.value == PinValue.Null)
            return;
        if (pin.Wires.Count == 0)
            return;
        PinController transferFromPin = null,transferToPin = null;
        foreach (WireController wire in pin.Wires)
        {

            if (wire.valuePropagated)
                continue;
            if (wire.connectionDirection == ConnectionDirection.InititalToFinal)
            {
                transferFromPin = wire.initialPin;
                transferToPin = wire.finalPin;
            }
            else if (wire.connectionDirection == ConnectionDirection.FinalToInitial)
            {
                transferFromPin = wire.finalPin;
                transferToPin = wire.initialPin;
            }
            TransferValue(transferFromPin, transferToPin, wire);
        }
    }

    private void TransferValue(PinController transferFromPin, PinController transferToPin, WireController wire)
    {
        transferToPin.value = transferFromPin.value;
        wire.valuePropagated = true;
        TransferData(transferToPin);
    }

    private void OnDestroy()
    {
        EventService.Instance.SimulationStarted -= StartTransfer;
        EventService.Instance.InputValueChanged -= StartTransfer;
    }
}
