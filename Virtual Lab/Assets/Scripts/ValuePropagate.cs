
using System.Collections.Generic;
using UnityEngine;

public class ValuePropagate : MonoGenericSingelton<ValuePropagate> 
{

    public List<PinController> OutputPins;

    public List<PinController> InputPins;

    public List<PinController> VccPins;
    public List<PinController> GndPins;
    public List<PinController> IcInputPins;
    public List<PinController> IcOutputPins;
    public List<PinController> IcVccPin;
    public List<PinController> IcGndPin;

    public List<ICLogic> ICLogics;

    private SimulatorManager simulatorManager;
    private void Start()
    {
        simulatorManager =SimulatorManager.Instance;
    }

    public void StartTransfer()
    {
        if (SimulatorManager.Instance.Wires.Count == 0)
            return;
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
        for (int i = 0; i < ICLogics.Count; i++)
        {
            ICLogics[i].RunIcLogic();
        }
    }
    private void SetWiresValuePropagetedToFalse()
    {

        for(int i = 0;i< simulatorManager.Wires.Count; i++)
        {
            simulatorManager.Wires[i].valuePropagated = false;
        }


    }

    bool DoesThisPinTakeValue(PinController pin)
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

    public void TransferData(PinController pin)
    {
        if (pin.value == PinValue.Null)
            return;
        if (pin.Wires.Count == 0)
            return;
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
