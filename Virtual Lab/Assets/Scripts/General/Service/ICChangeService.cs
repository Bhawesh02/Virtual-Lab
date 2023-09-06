

public class ICChangeService : MonoGenericSingelton<ICChangeService>
{

    private ICModel icModel;
    private IC icData;

    private void Start()
    {
        EventService.Instance.ChangeIC += ChangeIc;
    }
    private void OnDestroy()
    {
        EventService.Instance.ChangeIC -= ChangeIc;
    }
    public void ChangeIc(ICModel icModel, IC icData)
    {
        this.icModel = icModel;
        this.icData = icData;
        if (icModel.IcData != null)
        {
            RemoveWiresConnectedToIcBase();
        }
        int numOfPinsInSelectedIcBase = this.icModel.Pins.Count;
        int numOfPinsInSelecetedIC = this.icData.inputPins.Length + this.icData.outputPins.Length + 2;
        if (numOfPinsInSelectedIcBase < numOfPinsInSelecetedIC)
            return;
        ValuePropagateService.Instance.ICViews.Remove(this.icModel.Controller.View);
        bool smallIcInBigBase = Is14pinbeingputin16pin(numOfPinsInSelectedIcBase, numOfPinsInSelecetedIC);
        this.icModel.ICSprite.sprite = this.icData.IcSprite;
        this.icModel.Controller.SetIcData(icData);
        SetInputAndOutputPins(smallIcInBigBase);
        SetVccAndGndPin(smallIcInBigBase);
        ValuePropagateService.Instance.ICViews.Add(this.icModel.Controller.View);
    }

    private void RemoveWiresConnectedToIcBase()
    {
        for (int i = 0; i < icModel.Pins.Count; i++)
        {
            if (icModel.Pins[i].Wires.Count == 0)
                continue;
            for (int j = 0; j < icModel.Pins[i].Wires.Count; j++)
                EventService.Instance.InvokeRemoveWireConnection(icModel.Pins[i].Wires[j]);
        }
    }

    private bool Is14pinbeingputin16pin(int numOfPinsInSelectedIcBase, int numOfPinsInSelecetedIC)
    {
        if (numOfPinsInSelectedIcBase == 16 && numOfPinsInSelecetedIC == 14)
            return true;
        return false;
    }


    private void SetVccAndGndPin(bool smallIcInBigBase)
    {
        // VCC pin
        int pinNumber = icData.VccPin - 1;
        pinNumber = Skip8and9ifApplicable(smallIcInBigBase, pinNumber);
        ChangePinType(pinNumber, PinType.IcVcc);
        ValuePropagateService.Instance.IcVccPin.Add(icModel.Pins[pinNumber].GetComponent<PinController>());
        icModel.Pins[pinNumber].gameObject.AddComponent<OutputPinConnectionCheck>();
        icModel.Controller.SetVccPin(icModel.Pins[pinNumber].GetComponent<PinController>());
        //Gnd pin
        pinNumber = icData.GndPin - 1;
        pinNumber = Skip8and9ifApplicable(smallIcInBigBase, pinNumber);
        ChangePinType(pinNumber, PinType.IcGnd);
        ValuePropagateService.Instance.IcGndPin.Add(icModel.Pins[pinNumber].GetComponent<PinController>());
        icModel.Pins[pinNumber].gameObject.AddComponent<OutputPinConnectionCheck>();
        icModel.Controller.SetGndPin(icModel.Pins[pinNumber].GetComponent<PinController>());
    }



    private void SetInputAndOutputPins(bool smallIcInBigBase)
    {
        for (int i = 0; i < icData.inputPins.Length; i++)
        {
            //input pin
            int pinNumber = icData.inputPins[i] - 1;
            pinNumber = Skip8and9ifApplicable(smallIcInBigBase, pinNumber);

            ChangePinType(pinNumber, PinType.IcInput);
            ValuePropagateService.Instance.IcInputPins.Add(icModel.Pins[pinNumber].GetComponent<PinController>());
            icModel.Pins[pinNumber].gameObject.AddComponent<OutputPinConnectionCheck>();
        }
        for (int i = 0; i < icData.outputPins.Length; i++)
        {
            //Output pin
            int pinNumber = icData.outputPins[i] - 1;
            pinNumber = Skip8and9ifApplicable(smallIcInBigBase, pinNumber);

            ChangePinType(pinNumber, PinType.IcOutput);
            ValuePropagateService.Instance.IcOutputPins.Add(icModel.Pins[pinNumber].GetComponent<PinController>());
        }
    }
    private static int Skip8and9ifApplicable(bool smallIcInBigBase, int pinNumber)
    {
        if (smallIcInBigBase && pinNumber + 1 >= 8)
        {
            pinNumber += 2;
        }

        return pinNumber;
    }
    private void ChangePinType(int PinNumber, PinType type)
    {
        PinInfo currentPinInfo = icModel.Pins[PinNumber].GetComponent<PinController>().CurrentPinInfo;
        currentPinInfo.PinNumber = PinNumber + 1;
        currentPinInfo.Type = type;
    }
    


}
