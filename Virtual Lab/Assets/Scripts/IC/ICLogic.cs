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
        RunIcLogic();
    }
    public void RunIcLogic()
    {
        if (Ic.ICType == ICTypes.Null)
            return;
        foreach (PinMapping gate in Ic.pinMapping)
        {
            int OutputPinNumber = gate.OutputPin - 1;
            GameObject OutputPin = SimulatorManager.Instance.IcBase.Pins[OutputPinNumber];
            List<GameObject> InputPins = new();
            bool anyInputNull = false;
            foreach (int inputPin in gate.InputPin)
            {
                int InputPinNumber = inputPin - 1;
                GameObject InputPin = SimulatorManager.Instance.IcBase.Pins[InputPinNumber];
                InputPins.Add(InputPin);
                if (InputPin.GetComponent<PinConnection>().value == PinValue.Null)
                {
                    anyInputNull = true;
                    break;
                }
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
                if (inputPins[0].GetComponent<PinConnection>().value == PinValue.Negative)
                    outputPin.GetComponent<PinConnection>().value = PinValue.Positive;
                else
                    outputPin.GetComponent<PinConnection>().value = PinValue.Negative;
                break;
            default:
                Debug.Log("Wrong IC type");
                break;
        }
    }
}
