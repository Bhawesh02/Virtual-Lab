
using System.Collections.Generic;
using UnityEngine;

public class ICLogic : MonoBehaviour
{
    public IC IcData;
    private bool ranOnce;
    private void Awake()
    {
        this.enabled = false;
        ranOnce = false;
    }

    private void Update()
    {
        //if(SimulationRunning)
        if (ranOnce)
            RunIcLogic();
    }
    public void RunIcLogic()
    {
        if (IcData == null)
            return;
        if (!SimulatorManager.Instance.SimulationRunning)
            return;
        if (!ranOnce)
            ranOnce = true;
        int VccPinNumber = IcData.VccPin - 1;
        int GndPinNumber = IcData.GndPin - 1;
        PinController VccPin = SimulatorManager.Instance.SelectedIcBase.Pins[VccPinNumber].GetComponent<PinController>();
        PinController GndPin = SimulatorManager.Instance.SelectedIcBase.Pins[GndPinNumber].GetComponent<PinController>();

        if (VccPin.value != PinValue.Vcc || GndPin.value != PinValue.Gnd)
        {
            Debug.Log("VCC or Gnd notConnected / WrongConnected");
            return;
        }
        if (IcData.ICType == ICTypes.Null)
            return;
        foreach (PinMapping gate in IcData.pinMapping)
        {
            int OutputPinIndex = gate.OutputPin - 1;
            GameObject OutputPin = GetComponent<ICController>().Pins[OutputPinIndex];
            List<GameObject> InputPins = new();
            bool anyInputNull = false;
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
            if (anyInputNull)
            {
                OutputPin.GetComponent<PinController>().value = PinValue.Null;
                continue;
            }
            GenerateOutputValue(OutputPin, InputPins);

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
            default:
                Debug.Log("IC Logic Not given");
                break;
        }
        if (outputPin.GetComponent<PinController>().value != PinValue.Null)
            ValuePropagate.Instance.TransferData(outputPin.GetComponent<PinController>());

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
}
