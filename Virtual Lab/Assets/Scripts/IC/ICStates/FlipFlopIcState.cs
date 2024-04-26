using static FlipFlopLogic;

public class FlipFlopIcState : IcState
{
    private FlipFlopData flipFlopData;

    public FlipFlopIcState(ICController icController) : base(icController)
    {
    }

    public override void SetData()
    {
        flipFlopData = (FlipFlopData)_icController.Model.IcData;
    }

    public override void SetPins()
    {
        foreach (FlipFlopPinMapping flipFlopPinMapping in flipFlopData.FlipFlopPinMappings)
        {
            foreach (int inputPin in flipFlopPinMapping.InputPins)
            {
                _icController.SetAsInputPin(inputPin);
            }

            _icController.SetAsInputPin(flipFlopPinMapping.ClockPin);
            _icController.SetAsInputPin(flipFlopPinMapping.PresetPin);
            _icController.SetAsInputPin(flipFlopPinMapping.ClearPin);
            foreach (int outputPin in flipFlopPinMapping.OutputPins)
            {
                _icController.SetAsOutputPin(outputPin);
            }
        }
    }
    public override void RunLogic()
    {
        switch (flipFlopData.FlipFlopType)
        {
            case TypeOfFlipFlops.J_K:
                JKLogic(flipFlopData, _icController);
                break;
            case TypeOfFlipFlops.D:
                DLogic(flipFlopData, _icController);
                break;
        }
    }
    public override void PropagateOutputPinValues()
    {
        PinController outputPinController;
        foreach (FlipFlopPinMapping flipFlopPinMapping in flipFlopData.FlipFlopPinMappings)
        {
            foreach (int outputPin in flipFlopPinMapping.OutputPins)
            {
                outputPinController = _icController.Model.Pins[outputPin - 1];
                EventService.Instance.InvokeOutputPinValueChange(outputPinController);
            }
        }
    }

}