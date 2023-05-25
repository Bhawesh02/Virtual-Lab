using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICLogic : MonoBehaviour
{
    public IC Ic;
    private bool ranOnce;
    private void Awake()
    {
        this.enabled = false;
        ranOnce = false;
    }

    private void Update()
    {
        //if(SimulationRunning)
        if(ranOnce)
        RunIcLogic();
    }
    public void RunIcLogic()
    {
        if (Ic == null)
            return;
        if (!SimulatorManager.Instance.SimulationRunning)
            return;
        if (!ranOnce)
            ranOnce = true;
        int VccPinNumber = Ic.VccPin - 1;
        int GndPinNumber = Ic.GndPin - 1;
        PinConnection VccPin = SimulatorManager.Instance.IcBase.Pins[VccPinNumber].GetComponent<PinConnection>();
        PinConnection GndPin = SimulatorManager.Instance.IcBase.Pins[GndPinNumber].GetComponent<PinConnection>();

        if (VccPin.value != PinValue.Vcc || GndPin.value != PinValue.Gnd)
            return;
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
        if (outputPin.GetComponent<PinConnection>().value != PinValue.Null)
            ValuePropagate.Instance.TransferData(outputPin.GetComponent<PinConnection>());

    }

    private static void NotGateLogic(GameObject outputPin, List<GameObject> inputPins)
    {
        if (inputPins[0].GetComponent<PinConnection>().value == PinValue.Negative)
            outputPin.GetComponent<PinConnection>().value = PinValue.Positive;
        else
            outputPin.GetComponent<PinConnection>().value = PinValue.Negative;
    }
}
