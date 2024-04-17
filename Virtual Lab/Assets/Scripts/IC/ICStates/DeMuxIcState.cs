using UnityEngine;
using static DeMuxLogic;

public class DeMuxIcState : IcState
{
    private DeMuxIcData deMuxIcData;

    public DeMuxIcState(ICController icController) : base(icController)
    {
    }

    public override void SetData()
    {
        deMuxIcData = (DeMuxIcData)_icController.Model.IcData;
    }

    public override void RunLogic()
    {
        int outputPinNumber = -1;
        switch (deMuxIcData.DeMuxType)
        {
            case DeMuxTypes.NULL:
                break;
            case DeMuxTypes.ONE_X_EIGHT:
                outputPinNumber = OneXEightLogic(_icController.Model.Pins, deMuxIcData);
                break;
        }
        SetOutPutValues(outputPinNumber);
    }

    private void SetOutPutValues(int outputPinNumber)
    {
        Debug.Assert(outputPinNumber != -1,$"Got output -1");
        PinController outputPin;
        PinController inputPin = _icController.Model.Pins[deMuxIcData.InputPin - 1];
        foreach (int outputPinIndex in deMuxIcData.OutputPins)
        {
            outputPin = _icController.Model.Pins[outputPinIndex - 1];
            if (outputPinIndex == outputPinNumber)
            {
                outputPin.value = inputPin.value;
            }
            else
            {
                outputPin.value = PinValue.Negative;
            }
            EventService.Instance.InvokeOutputPinValueChange(outputPin);
            ValuePropagateService.Instance.TransferData(outputPin);
        }
    }

    public override void SetPins()
    {
        foreach (int outputPin in deMuxIcData.OutputPins)
        {
            _icController.SetAsOutputPin(outputPin);
        }

        foreach (int selectPin in deMuxIcData.SelectPins)
        {
            _icController.SetAsInputPin(selectPin);
        }

        _icController.SetAsInputPin(deMuxIcData.InputPin);
    }
}