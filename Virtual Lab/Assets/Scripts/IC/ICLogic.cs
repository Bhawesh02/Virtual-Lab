
using System.Collections.Generic;
using UnityEngine;

public class ICLogic : MonoBehaviour
{
    public IC IcData;
    [SerializeField]
    private ICController iCController;
    private void Awake()
    {
        this.enabled = false;
        
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
            GameObject OutputPin = GetComponent<ICController>().Pins[OutputPinIndex];
            List<GameObject> InputPins = new();
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

    private bool CheckEachInputOfGate(PinMapping gate, List<GameObject> InputPins, bool anyInputNull)
    {
        foreach (int inputPinNumber in gate.InputPin)
        {
            int InputPinIndex = inputPinNumber - 1;
            GameObject InputPin = GetComponent<ICController>().Pins[InputPinIndex];
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
        List<GameObject> pins = iCController.thisIC.Pins;
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

    private void GenerateOutputValue(GameObject outputPin, List<GameObject> inputPins)
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
        if (outputPin.GetComponent<PinController>().value != PinValue.Null)
            ValuePropagateService.Instance.TransferData(outputPin.GetComponent<PinController>());

    }

    #endregion

    #region Gate Logic
    private void NandGateLogic(GameObject outputPin, List<GameObject> inputPins)
    {
        PinValue input1Value=inputPins[0].GetComponent<PinController>().value;
        PinValue input2Value=inputPins[1].GetComponent<PinController>().value;
        if (input1Value == PinValue.Positive && input2Value == PinValue.Positive)
        {
            outputPin.GetComponent<PinController>().value = PinValue.Negative;
            return;
        }
        outputPin.GetComponent<PinController>().value = PinValue.Positive;
    }
    private void NorGateLogic(GameObject outputPin, List<GameObject> inputPins)
    {
        PinValue input1Value = inputPins[0].GetComponent<PinController>().value;
        PinValue input2Value = inputPins[1].GetComponent<PinController>().value;
        if (input1Value == PinValue.Negative && input2Value == PinValue.Negative)
        {
            outputPin.GetComponent<PinController>().value = PinValue.Positive ;
            return;
        }
        outputPin.GetComponent<PinController>().value = PinValue.Negative ;
    }

    private void AndGateLogic(GameObject outputPin, List<GameObject> inputPins)
    {
        PinValue input1Value = inputPins[0].GetComponent<PinController>().value;
        PinValue input2Value = inputPins[1].GetComponent<PinController>().value;
        if (input1Value == PinValue.Negative || input2Value == PinValue.Negative)
        {
            outputPin.GetComponent<PinController>().value = PinValue.Negative;
            return;
        }
        outputPin.GetComponent<PinController>().value = PinValue.Positive;
    }

    private void OrGateLogic(GameObject outputPin, List<GameObject> inputPins)
    {
        PinValue input1Value = inputPins[0].GetComponent<PinController>().value;
        PinValue input2Value = inputPins[1].GetComponent<PinController>().value;
        if (input1Value == PinValue.Positive || input2Value == PinValue.Positive)
        {
            outputPin.GetComponent<PinController>().value = PinValue.Positive;
            return;
        }
        outputPin.GetComponent<PinController>().value = PinValue.Negative;

    }

    private void NotGateLogic(GameObject outputPin, List<GameObject> inputPins)
    {
        if (inputPins[0].GetComponent<PinController>().value == PinValue.Negative)
            outputPin.GetComponent<PinController>().value = PinValue.Positive;
        else
            outputPin.GetComponent<PinController>().value = PinValue.Negative;
    }
    private void XorGateLogic(GameObject outputPin, List<GameObject> inputPins)
    {
        PinValue inputValue1 = inputPins[0].GetComponent<PinController>().value;
        PinValue inputValue2 = inputPins[1].GetComponent<PinController>().value;
        if (inputValue1 != inputValue2)
        {
            outputPin.GetComponent<PinController>().value = PinValue.Positive;
        }
        else
        {
            outputPin.GetComponent<PinController>().value = PinValue.Negative;
        }
    }

    #endregion
}
