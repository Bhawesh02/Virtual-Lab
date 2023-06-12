
using System.Collections.Generic;
using UnityEngine;

public class ValuePropagate : MonoBehaviour
{
    private static ValuePropagate instance;
    public static ValuePropagate Instance { get { return instance; } }

    public List<PinController> OutputPins;

    public List<PinController> InputPins;

    public List<PinController> VccPins;
    public List<PinController> GndPins;
    public List<PinController> IcInputPins;
    public List<PinController> IcOutputPins;
    public List<PinController> IcVccPin;
    public List<PinController> IcGndPin;

    public List<ICLogic> ICLogics;
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

   
    public void StartTransfer()
    {
        if (SimulatorManager.Instance.Wires.Count == 0)
            return;
        SetWiresValuePropagetedToFalse();
        foreach (PinController pin in VccPins)
            TransferData(pin);
        foreach (PinController pin in GndPins)
            TransferData(pin);
        foreach (PinController pin in InputPins)
        {
            TransferData(pin);
        }
        foreach (ICLogic IcLogic in ICLogics)
            IcLogic.RunIcLogic();

    }
    private void SetWiresValuePropagetedToFalse()
    {

        foreach (WireController wire in SimulatorManager.Instance.Wires)
        {
            wire.valuePropagated = false;
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
