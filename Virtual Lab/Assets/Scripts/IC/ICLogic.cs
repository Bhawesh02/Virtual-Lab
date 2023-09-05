
using System.Collections.Generic;
using UnityEngine;

public class ICLogic : MonoBehaviour
{
    public IC IcData;
    [SerializeField]
    private ICView ICView;
    private void Awake()
    {
        enabled = false;
    }
    


    #region Basic Ic Logic

    public void RunIcLogic()
    {
        if (IcData == null)
            return;

        int VccPinNumber = IcData.VccPin - 1;
        int GndPinNumber = IcData.GndPin - 1;
        GetVccAndGndPinInIC(VccPinNumber, GndPinNumber, out PinController VccPinInIc, out PinController GndPinInIc);
        if (VccPinInIc.value != PinValue.Vcc || GndPinInIc.value != PinValue.Gnd)
        {
            Debug.Log("VCC or Gnd notConnected / WrongConnected");
            return;
        }
        if (IcData.ICType == ICTypes.Null)
            return;
        RunLogicForEachGate();
    }

    private void RunLogicForEachGate()
    {
        foreach (PinMapping gate in IcData.pinMapping)
        {
            int OutputPinIndex = gate.OutputPin - 1;
            PinController OutputPin = GetComponent<ICView>().Controller.Model.thisIC.Pins[OutputPinIndex];
            List<PinController> InputPins = new();
            bool anyInputNull = false;
            anyInputNull = CheckEachInputOfGate(gate, InputPins, anyInputNull);
            if (anyInputNull)
            {
                OutputPin.GetComponent<PinController>().value = PinValue.Null;
                continue;
            }
            GenerateOutputValue(OutputPin, InputPins);

        }
    }

    private bool CheckEachInputOfGate(PinMapping gate, List<PinController> InputPins, bool anyInputNull)
    {
        foreach (int inputPinNumber in gate.InputPin)
        {
            int InputPinIndex = inputPinNumber - 1;
            PinController InputPin = GetComponent<ICView>().Controller.Model.thisIC.Pins[InputPinIndex];
            if (InputPin.GetComponent<PinController>().value == PinValue.Null)
            {
                anyInputNull = true;
                break;
            }
            InputPins.Add(InputPin);

        }

        return anyInputNull;
    }

    private void GetVccAndGndPinInIC(int VccPinNumber, int GndPinNumber, out PinController VccPinInIc, out PinController GndPinInIc)
    {
        List<PinController> pins = ICView.Controller.Model.thisIC.Pins;
        VccPinInIc = null;
        GndPinInIc = null;
        PinController pinController;
        for (int i = 0;i< pins.Count; i++)
        {
            pinController = pins[i].GetComponent<PinController>();
            if (pinController.CurrentPinInfo.Type == PinType.IcVcc)
                VccPinInIc = pins[i].GetComponent<PinController>();

            else if (pinController.CurrentPinInfo.Type == PinType.IcGnd)
                GndPinInIc = pins[i].GetComponent<PinController>();
        }

    }

    private void GenerateOutputValue(PinController outputPin, List<PinController> inputPins)
    {
        switch (IcData.ICType)
        {
            case ICTypes.Not:
                NotGateLogic(outputPin, inputPins);
                break;
            case ICTypes.Or:
                OrGateLogic(outputPin, inputPins);
                break;
            case ICTypes.And:
                AndGateLogic(outputPin, inputPins);
                break;
            case ICTypes.Xor:
                XorGateLogic(outputPin, inputPins);
                break;
            case ICTypes.Nor:
                NorGateLogic(outputPin, inputPins);
                break;
            case ICTypes.Nand:
                NandGateLogic(outputPin,inputPins);
                break;
            default:
                Debug.Log("IC Logic Not given");
                break;
        }
        if (outputPin.value != PinValue.Null)
            ValuePropagateService.Instance.TransferData(outputPin.GetComponent<PinController>());

    }

    #endregion

    #region Gate Logic
    private void NandGateLogic(PinController outputPin, List<PinController> inputPins)
    {
        PinValue input1Value=inputPins[0].value;
        PinValue input2Value=inputPins[1].value;
        if (input1Value == PinValue.Positive && input2Value == PinValue.Positive)
        {
            outputPin.value = PinValue.Negative;
            return;
        }
        outputPin.value = PinValue.Positive;
    }
    private void NorGateLogic(PinController outputPin, List<PinController> inputPins)
    {
        PinValue input1Value = inputPins[0].value;
        PinValue input2Value = inputPins[1].value;
        if (input1Value == PinValue.Negative && input2Value == PinValue.Negative)
        {
            outputPin.value = PinValue.Positive ;
            return;
        }
        outputPin.value = PinValue.Negative ;
    }

    private void AndGateLogic(PinController outputPin, List<PinController> inputPins)  
    {
        PinValue input1Value = inputPins[0].value;
        PinValue input2Value = inputPins[1].value;
        if (input1Value == PinValue.Negative || input2Value == PinValue.Negative)
        {
            outputPin.value = PinValue.Negative;
            return;
        }
        outputPin.value = PinValue.Positive;
    }

    private void OrGateLogic(PinController outputPin, List<PinController> inputPins)
    {
        PinValue input1Value = inputPins[0].value;
        PinValue input2Value = inputPins[1].value;
        if (input1Value == PinValue.Positive || input2Value == PinValue.Positive)
        {
            outputPin.value = PinValue.Positive;
            return;
        }
        outputPin.value = PinValue.Negative;

    }

    private void NotGateLogic(PinController outputPin, List<PinController> inputPins)
    {
        if (inputPins[0].value == PinValue.Negative)
            outputPin.value = PinValue.Positive;
        else
            outputPin.value = PinValue.Negative;
    }
    private void XorGateLogic(PinController outputPin, List<PinController> inputPins)
    {
        PinValue inputValue1 = inputPins[0].value;
        PinValue inputValue2 = inputPins[1].value;
        if (inputValue1 != inputValue2)
        {
            outputPin.value = PinValue.Positive;
        }
        else
        {
            outputPin.value = PinValue.Negative;
        }
    }

    #endregion
}
