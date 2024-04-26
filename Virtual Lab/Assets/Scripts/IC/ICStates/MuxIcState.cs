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
            _icController.SetAsInputPin(inputPin);
        }

        foreach (int selectInputPin in _muxIcData.SelectInputPins)
        {
            _icController.SetAsInputPin(selectInputPin);
        }

        _icController.SetAsInputPin(_muxIcData.StrobePin);
        foreach (int outputPin in _muxIcData.OutpuPins)
        {
            _icController.SetAsOutputPin(outputPin);
        }
    }

    public override void PropagateOutputPinValues()
    {
        EventService.Instance.InvokeOutputPinValueChange(_outputPinCotroller);
        EventService.Instance.InvokeOutputPinValueChange(_complimentOutputPinCotroller);
        ValuePropagateService.Instance.TransferData(_outputPinCotroller);
        ValuePropagateService.Instance.TransferData(_complimentOutputPinCotroller);
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
    }
}