
public class ICChange {

    private ICBase IcBase;
    private IC ic;
    private bool Is14pinbeingputin16pin(int numOfPinsInSelectedIcBase,int numOfPinsInSelecetedIC)
    {
        if(numOfPinsInSelectedIcBase == 16 && numOfPinsInSelecetedIC == 14)
            return true;
        return false;
    }
    public void ChangeIc(ICBase _IcBase, IC _IcData)
    {
        IcBase = _IcBase;
        ic = _IcData;
        int numOfPinsInSelectedIcBase = IcBase.Pins.Count;
        int numOfPinsInSelecetedIC = ic.inputPins.Length+ic.outputPins.Length+2;
        if (numOfPinsInSelectedIcBase < numOfPinsInSelecetedIC )
            return;

        if (IcBase.IcLogic != null)
        {
            ValuePropagateService.Instance.ICLogics.Remove(IcBase.IcLogic);
        }
        bool smallIcInBigBase = Is14pinbeingputin16pin(numOfPinsInSelectedIcBase, numOfPinsInSelecetedIC);
        IcBase.ICSprite.sprite = ic.IcSprite;
        IcBase.IcLogic.enabled = true;
        IcBase.IcLogic.IcData = ic;
        SetInputAndOutputPins(smallIcInBigBase);
        SetVccAndGndPin(smallIcInBigBase);
        ValuePropagateService.Instance.ICLogics.Add(IcBase.IcLogic);
    }

    private void SetVccAndGndPin(bool smallIcInBigBase)
    {
        // VCC pin
        int pinNumber = ic.VccPin - 1;
        pinNumber = Skip8and9ifApplicable(smallIcInBigBase, pinNumber);
        ChangePinType(pinNumber, PinType.IcVcc);
        ValuePropagateService.Instance.IcVccPin.Add(IcBase.Pins[pinNumber].GetComponent<PinController>());
        IcBase.Pins[pinNumber].AddComponent<OutputPinConnectionCheck>();
        IcBase.IcLogic.gameObject.GetComponent<ICController>().VccPin = IcBase.Pins[pinNumber].GetComponent<PinController>();

        //Gnd pin
        pinNumber = ic.GndPin - 1;
        pinNumber = Skip8and9ifApplicable(smallIcInBigBase, pinNumber);

        ChangePinType(pinNumber, PinType.IcGnd);
        ValuePropagateService.Instance.IcGndPin.Add(IcBase.Pins[pinNumber].GetComponent<PinController>());
        IcBase.Pins[pinNumber].AddComponent<OutputPinConnectionCheck>();
        IcBase.IcLogic.gameObject.GetComponent<ICController>().GndPin = IcBase.Pins[pinNumber].GetComponent<PinController>();

    }



    private void SetInputAndOutputPins(bool smallIcInBigBase)
    {
        for (int i = 0; i < ic.inputPins.Length; i++)
        {
            //input pin
            int pinNumber = ic.inputPins[i] - 1;
            pinNumber = Skip8and9ifApplicable(smallIcInBigBase, pinNumber);

            ChangePinType(pinNumber, PinType.IcInput);
            ValuePropagateService.Instance.IcInputPins.Add(IcBase.Pins[pinNumber].GetComponent<PinController>());
            IcBase.Pins[pinNumber].AddComponent<OutputPinConnectionCheck>();
        }
        for(int i = 0; i < ic.outputPins.Length; i++)
        {
            //Output pin
            int pinNumber = ic.outputPins[i] - 1;
            pinNumber = Skip8and9ifApplicable(smallIcInBigBase, pinNumber);

            ChangePinType(pinNumber, PinType.IcOutput);
            ValuePropagateService.Instance.IcOutputPins.Add(IcBase.Pins[pinNumber].GetComponent<PinController>());
        }
    }
    private static int Skip8and9ifApplicable(bool smallIcInBigBase, int pinNumber)
    {
        if (smallIcInBigBase && pinNumber+1 >= 8)
        {
            pinNumber += 2;
        }

        return pinNumber;
    }
    private void ChangePinType(int PinNumber, PinType type)
    {
        PinInfo currentPinInfo = IcBase.Pins[PinNumber].GetComponent<PinController>().CurrentPinInfo;
        currentPinInfo.PinNumber = PinNumber + 1;
        currentPinInfo.Type = type;

    }
}
