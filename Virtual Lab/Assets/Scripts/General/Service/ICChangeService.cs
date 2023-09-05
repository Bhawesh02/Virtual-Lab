
public class ICChangeService:GenericSingelton<ICChangeService> {

    private ICBase icBase;
    private IC icData;

    public void ChangeIc(ICBase icBase, IC icData)
    {
        this.icBase = icBase;
        this.icData = icData;
        int numOfPinsInSelectedIcBase = this.icBase.Pins.Count;
        int numOfPinsInSelecetedIC = this.icData.inputPins.Length + this.icData.outputPins.Length + 2;
        if (numOfPinsInSelectedIcBase < numOfPinsInSelecetedIC)
            return;

        if (this.icBase.IcLogic != null)
        {
            ValuePropagateService.Instance.ICLogics.Remove(this.icBase.IcLogic);
        }
        bool smallIcInBigBase = Is14pinbeingputin16pin(numOfPinsInSelectedIcBase, numOfPinsInSelecetedIC);
        this.icBase.ICSprite.sprite = this.icData.IcSprite;
        this.icBase.IcLogic.enabled = true;
        this.icBase.IcLogic.IcData = this.icData;
        SetInputAndOutputPins(smallIcInBigBase);
        SetVccAndGndPin(smallIcInBigBase);
        ValuePropagateService.Instance.ICLogics.Add(this.icBase.IcLogic);
    }
    private bool Is14pinbeingputin16pin(int numOfPinsInSelectedIcBase,int numOfPinsInSelecetedIC)
    {
        if(numOfPinsInSelectedIcBase == 16 && numOfPinsInSelecetedIC == 14)
            return true;
        return false;
    }
    

    private void SetVccAndGndPin(bool smallIcInBigBase)
    {
        // VCC pin
        int pinNumber = icData.VccPin - 1;
        pinNumber = Skip8and9ifApplicable(smallIcInBigBase, pinNumber);
        ChangePinType(pinNumber, PinType.IcVcc);
        ValuePropagateService.Instance.IcVccPin.Add(icBase.Pins[pinNumber].GetComponent<PinController>());
        icBase.Pins[pinNumber].gameObject.AddComponent<OutputPinConnectionCheck>();
        icBase.IcLogic.gameObject.GetComponent<ICView>().Controller.SetVccPin(icBase.Pins[pinNumber].GetComponent<PinController>()) ;

        //Gnd pin
        pinNumber = icData.GndPin - 1;
        pinNumber = Skip8and9ifApplicable(smallIcInBigBase, pinNumber);

        ChangePinType(pinNumber, PinType.IcGnd);
        ValuePropagateService.Instance.IcGndPin.Add(icBase.Pins[pinNumber].GetComponent<PinController>());
        icBase.Pins[pinNumber].gameObject.AddComponent<OutputPinConnectionCheck>();
        icBase.IcLogic.gameObject.GetComponent<ICView>().Controller.SetGndPin(icBase.Pins[pinNumber].GetComponent<PinController>());

    }



    private void SetInputAndOutputPins(bool smallIcInBigBase)
    {
        for (int i = 0; i < icData.inputPins.Length; i++)
        {
            //input pin
            int pinNumber = icData.inputPins[i] - 1;
            pinNumber = Skip8and9ifApplicable(smallIcInBigBase, pinNumber);

            ChangePinType(pinNumber, PinType.IcInput);
            ValuePropagateService.Instance.IcInputPins.Add(icBase.Pins[pinNumber].GetComponent<PinController>());
            icBase.Pins[pinNumber].gameObject.AddComponent<OutputPinConnectionCheck>();
        }
        for(int i = 0; i < icData.outputPins.Length; i++)
        {
            //Output pin
            int pinNumber = icData.outputPins[i] - 1;
            pinNumber = Skip8and9ifApplicable(smallIcInBigBase, pinNumber);

            ChangePinType(pinNumber, PinType.IcOutput);
            ValuePropagateService.Instance.IcOutputPins.Add(icBase.Pins[pinNumber].GetComponent<PinController>());
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
        PinInfo currentPinInfo = icBase.Pins[PinNumber].GetComponent<PinController>().CurrentPinInfo;
        currentPinInfo.PinNumber = PinNumber + 1;
        currentPinInfo.Type = type;

    }
}
