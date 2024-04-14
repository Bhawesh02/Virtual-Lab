using static MuxLogic;

public class MuxIcState : IcState
{
    private MuxIcData _muxIcData;
    private PinController _outputPinCotroller;
    private PinController _complimentOutputPinCotroller;

    public MuxIcState(ICController icController) : base(icController)
    {
    }

    public override void SetData()
    {
        _muxIcData = (MuxIcData)_icController.Model.IcData;
        _outputPinCotroller = _icController.Model.Pins[_muxIcData.OutpuPins[0] - 1];
        _complimentOutputPinCotroller = _icController.Model.Pins[_muxIcData.OutpuPins[^1] - 1];
    }

    public override void RunLogic()
    {
        PinValue outputPinValue = PinValue.Null;
        switch (_muxIcData.MuxType)
        {
            case MuxTypes.NULL:
                break;
            case MuxTypes.EIGHT_X_ONE:
                outputPinValue = EightXOneLogic(_icController.Model.Pins, _muxIcData);
                break;
        }

        SetOutPutValue(outputPinValue);
    }


    public override void SetPins()
    {
        foreach (int inputPin in _muxIcData.InputPins)
        {
            SetAsInputPin(inputPin);
        }

        foreach (int selectInputPin in _muxIcData.SelectInputPins)
        {
            SetAsInputPin(selectInputPin);
        }

        SetAsInputPin(_muxIcData.StrobePin);
        foreach (int outputPin in _muxIcData.OutpuPins)
        {
            SetAsOutputPin(outputPin);
        }
    }

    private void SetAsInputPin(int inputPin)
    {
        int inputPinNumber = inputPin - 1;
        inputPinNumber = _icController.Skip8and9ifApplicable(inputPinNumber);
        _icController.ChangePinType(inputPinNumber, PinType.IcInput);
        PinController inputPinController = _icController.Model.Pins[inputPinNumber].GetComponent<PinController>();
        inputPinController.value = PinValue.Negative;
        ValuePropagateService.Instance.IcInputPins.Add(inputPinController);
        _icController.Model.Pins[inputPinNumber].gameObject.AddComponent<OutputPinConnectionCheck>();
    }

    private void SetAsOutputPin(int outputPin)
    {
        int outputPinNumber = outputPin - 1;
        outputPinNumber = _icController.Skip8and9ifApplicable(outputPinNumber);
        _icController.ChangePinType(outputPinNumber, PinType.IcOutput);
        PinController outputPinController = _icController.Model.Pins[outputPinNumber].GetComponent<PinController>();
        outputPinController.value = PinValue.Negative;
        ValuePropagateService.Instance.IcOutputPins.Add(outputPinController);
    }

    private void SetOutPutValue(PinValue outputPinValue)
    {
        if (outputPinValue == PinValue.Null)
        {
            return;
        }
        
        _outputPinCotroller.value = outputPinValue;
        _complimentOutputPinCotroller.value =
            outputPinValue == PinValue.Positive ? PinValue.Negative : PinValue.Positive;
        EventService.Instance.InvokeOutputPinValueChange(_outputPinCotroller);
        EventService.Instance.InvokeOutputPinValueChange(_complimentOutputPinCotroller);
        ValuePropagateService.Instance.TransferData(_outputPinCotroller);
        ValuePropagateService.Instance.TransferData(_complimentOutputPinCotroller);
    }
}