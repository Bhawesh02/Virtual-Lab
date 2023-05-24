using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICLogic : MonoBehaviour
{
    public IC Ic;
    private void Awake()
    {
        this.enabled = false;
    }

    private void Update()
    {
        //if(SimulationRunning)
        RunIcLogic();
    }
    public void RunIcLogic()
    {
        if (Ic.ICType == ICTypes.Null)
            return;
        foreach (PinMapping gate in Ic.pinMapping)
        {
            int OutputPinIndex = gate.OutputPin - 1;
            GameObject OutputPin = SimulatorManager.Instance.IcBase.Pins[OutputPinIndex];
            List<GameObject> InputPins = new();
            bool anyInputNull = false;
            foreach (int inputPinNumber in gate.InputPin)
            {
                int InputPinIndex = inputPinNumber - 1;
                GameObject InputPin = SimulatorManager.Instance.IcBase.Pins[InputPinIndex];
                if (InputPin.GetComponent<PinConnection>().value == PinValue.Null)
                {
                    anyInputNull = true;
                    break;
                }
                InputPins.Add(InputPin);

            }
            if (anyInputNull)
            {
                OutputPin.GetComponent<PinConnection>().value = PinValue.Null;
                continue;
            }
            GenerateOutputValue(OutputPin, InputPins);

        }
    }

    private void GenerateOutputValue(GameObject outputPin, List<GameObject> inputPins)
    {
        switch (Ic.ICType)
        {
            case ICTypes.Not:
                NotGateLogic(outputPin, inputPins);
                break;
            default:
                Debug.Log("Wrong IC type");
                break;
        }
    }

    private static void NotGateLogic(GameObject outputPin, List<GameObject> inputPins)
    {
        if (inputPins[0].GetComponent<PinConnection>().value == PinValue.Negative)
            outputPin.GetComponent<PinConnection>().value = PinValue.Positive;
        else
            outputPin.GetComponent<PinConnection>().value = PinValue.Negative;
    }
}
