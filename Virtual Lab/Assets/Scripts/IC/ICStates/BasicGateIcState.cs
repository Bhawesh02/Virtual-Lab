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
        List<PinController> inputPins = new();
        PinController outputPin;
        foreach (BasicGatePinMapping gate in basicGateIcData.PinMapping)
        {
            SetInputAndOutputPins(inputPins,out outputPin, gate);
            if (CheckAnyInputNotHaveWire(inputPins))
            {
                outputPin.value = PinValue.Null;
                continue;
            }

            if (CheckAnyInputHasValue(inputPins))
                GenerateOutputValue(outputPin, inputPins);
        }
    }

    private void SetInputAndOutputPins(List<PinController> inputPins,out PinController outputPin, BasicGatePinMapping gate)
    {
        inputPins.Clear();
        int outputPinIndex = gate.OutputPin - 1;
        outputPinIndex = _icController.Skip8and9ifApplicable(outputPinIndex);
        outputPin = _icController.Model.Pins[outputPinIndex];
        int inputPinIndex;
        foreach (int inputPinNumber in gate.InputPin)
        {
            inputPinIndex = inputPinNumber - 1;
            inputPinIndex = _icController.Skip8and9ifApplicable(inputPinIndex);
            inputPins.Add(_icController.Model.Pins[inputPinIndex]);
        }
    }

    public override void SetPins()
    {
        foreach (BasicGatePinMapping basicGatePinMapping in basicGateIcData.PinMapping)
        {
            int pinNumber;
            for (int pinIndex = 0; pinIndex < basicGatePinMapping.InputPin.Length; pinIndex++)
            {
                pinNumber = basicGatePinMapping.InputPin[pinIndex];
               _icController.SetAsInputPin(pinNumber);
            }
            _icController.SetAsOutputPin(basicGatePinMapping.OutputPin);
        }
    }

    private bool CheckAnyInputNotHaveWire(List<PinController> inputPins)
    {
        foreach (PinController inputPin in inputPins)
        {
            if (inputPin.Wires.Count == 0)
            {
                return true;
            }
        }
        return false;
    }

    private bool CheckAnyInputHasValue(List<PinController> inputPins)
    {
        foreach (PinController inputPin in inputPins)
        {
            if (inputPin.value != PinValue.Null)
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