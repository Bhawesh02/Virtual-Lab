using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ICChange : MonoBehaviour
{
    [SerializeField]
    private IC ic;

    private Button changeButton;
    [SerializeField]
    private GameObject ICSelection;

    private ICBase IcBase;

    private void Awake()
    {
        changeButton = GetComponent<Button>();
    }
    private void Start()
    {
        changeButton.onClick.AddListener(ChangeIc);
    }

    private void ChangeIc()
    {
        IcBase = SimulatorManager.Instance.IcBase;
        IcBase.ICSprite.sprite = ic.IcSprite;
        IcBase.IcLogic.enabled = true;
        IcBase.IcLogic.Ic = ic;
        SetInputAndOutputPins();
        SetVccAndGndPin();
        ICSelection.SetActive(false);
    }

    private void SetVccAndGndPin()
    {
        // VCC pin
        int pinNumber = ic.VccPin - 1;
        ChangePinType(pinNumber, PinType.IcVcc);
        SimulatorManager.Instance.valuePropagate.IcVccPin = (IcBase.Pins[pinNumber].GetComponent<PinConnection>());


        //Gnd pin
        pinNumber = ic.GndPin - 1;
        ChangePinType(pinNumber, PinType.IcGnd);
        SimulatorManager.Instance.valuePropagate.IcGndPin = (IcBase.Pins[pinNumber].GetComponent<PinConnection>());

    }

    private void SetInputAndOutputPins()
    {
        for (int i = 0; i < ic.inputPins.Length; i++)
        {
            //input pin
            int pinNumber = ic.inputPins[i] - 1;
            ChangePinType(pinNumber, PinType.IcInput);
            SimulatorManager.Instance.valuePropagate.IcInputPins.Add(IcBase.Pins[pinNumber].GetComponent<PinConnection>());
            //Output pin
            pinNumber = ic.outputPins[i] - 1;
            ChangePinType(pinNumber, PinType.IcOutput);
            SimulatorManager.Instance.valuePropagate.IcOutputPins.Add(IcBase.Pins[pinNumber].GetComponent<PinConnection>());


        }
    }

    private void ChangePinType(int PinNumber, PinType type)
    {
        PinInfo currentPinInfo = IcBase.Pins[PinNumber].GetComponent<PinConnection>().CurrentPinInfo;
        currentPinInfo.PinNumber = PinNumber + 1;
        currentPinInfo.Type = type;

    }
}
