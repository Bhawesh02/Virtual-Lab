using System.Collections.Generic;
using UnityEngine;
using static GateLogic;

public class BasicGateIcState : IcState
{
    private BasicGateIcData basicGateIcData;

    public BasicGateIcState(ICController icController) : base(icController)
    {
    }
    public override void SetData()
    {
        basicGateIcData = (BasicGateIcData)_icController.Model.IcData;
    }

    public override void RunLogic()
    {
        foreach (BasicGatePinMapping gate in basicGateIcData.PinMapping)
        {
            int OutputPinIndex = gate.OutputPin - 1;
            PinController OutputPin = _icController.Model.Pins[OutputPinIndex];
            List<PinController> InputPins = new();
            if (CheckAnyInputNotHaveWire(gate, InputPins))
            {
                OutputPin.value = PinValue.Null;
                continue;
            }

            if (CheckAnyInputHasValue(gate))
                GenerateOutputValue(OutputPin, InputPins);
        }
    }

    public override void SetPins()
    {
        foreach (BasicGatePinMapping basicGatePinMapping in basicGateIcData.PinMapping)
        {
            int pinNumber;
            for (int pinIndex = 0; pinIndex < basicGatePinMapping.InputPin.Length; pinIndex++)
            {
                pinNumber = basicGatePinMapping.InputPin[pinIndex] - 1;
                pinNumber =_icController.Skip8and9ifApplicable(pinNumber);
                _icController.ChangePinType(pinNumber, PinType.IcInput);
                ValuePropagateService.Instance.IcInputPins.Add(_icController.Model.Pins[pinNumber].GetComponent<PinController>());
                _icController.Model.Pins[pinNumber].gameObject.AddComponent<OutputPinConnectionCheck>();
            }
            pinNumber = basicGatePinMapping.OutputPin - 1;
            pinNumber = _icController.Skip8and9ifApplicable(pinNumber);
            _icController.ChangePinType(pinNumber, PinType.IcOutput);
            ValuePropagateService.Instance.IcOutputPins.Add(_icController.Model.Pins[pinNumber].GetComponent<PinController>());
        }
    }

    private bool CheckAnyInputNotHaveWire(BasicGatePinMapping gate, List<PinController> InputPins)
    {
        for (int i = 0; i < gate.InputPin.Length; i++)
        {
            int InputPinIndex = gate.InputPin[i] - 1;
            PinController InputPin = _icController.Model.Pins[InputPinIndex];
            if (InputPin.Wires.Count == 0)
            {
                return true;
            }

            InputPins.Add(InputPin);
        }

        return false;
    }

    private bool CheckAnyInputHasValue(BasicGatePinMapping gate)
    {
        for (int i = 0; i < gate.InputPin.Length; i++)
        {
            int InputPinIndex = gate.InputPin[i] - 1;
            PinController InputPin = _icController.Model.Pins[InputPinIndex];
            if (InputPin.value != PinValue.Null)
            {
                return true;
            }
        }

        return false;
    }

    private void GenerateOutputValue(PinController outputPin, List<PinController> inputPins)
    {
        PinValue oldOutputPinValue = outputPin.value;
        switch (basicGateIcData.BasicGateType)
        {
            case BasicGateTypes.Not:
                NotGateLogic(outputPin, inputPins);
                break;
            case BasicGateTypes.Or:
                OrGateLogic(outputPin, inputPins);
                break;
            case BasicGateTypes.And:
                AndGateLogic(outputPin, inputPins);
                break;
            case BasicGateTypes.Xor:
                XorGateLogic(outputPin, inputPins);
                break;
            case BasicGateTypes.Nor:
                NorGateLogic(outputPin, inputPins);
                break;
            case BasicGateTypes.Nand:
                NandGateLogic(outputPin, inputPins);
                break;
            case BasicGateTypes.ThreeInputNand:
                ThreeInputNandLogic(outputPin, inputPins);
                break;
            default:
                Debug.Log("IC Logic Not given");
                break;
        }

        if (outputPin.value == PinValue.Null)
            return;
        if (oldOutputPinValue != PinValue.Null && oldOutputPinValue != outputPin.value)
        {
            EventService.Instance.InvokeOutputPinValueChange(outputPin);
            return;
        }

        ValuePropagateService.Instance.TransferData(outputPin);
    }
}