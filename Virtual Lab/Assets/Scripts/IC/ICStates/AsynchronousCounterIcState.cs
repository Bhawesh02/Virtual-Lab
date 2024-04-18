using static AsynchronousCounterLogic;

public class AsynchronousCounterIcState : IcState
{
    private AsynchronousCounterData asynchronousCounterData;
    public AsynchronousCounterIcState(ICController icController) : base(icController)
    {
    }

    public override void SetData()
    {
        asynchronousCounterData = (AsynchronousCounterData)_icController.Model.IcData;
    }
    public override void SetPins()
    {
        foreach (AsynchronousCounterPinMapping asynchronousCounterPinMapping in asynchronousCounterData.asynchronousCounterPinMappings)
        {
            _icController.SetAsInputPin(asynchronousCounterPinMapping.ClockPin);
            foreach (int outputPin in asynchronousCounterPinMapping.OutputPins)
            {
                _icController.SetAsOutputPin(outputPin);
            }
            _icController.SetAsInputPin(asynchronousCounterPinMapping.SetPin);
            _icController.SetAsInputPin(asynchronousCounterPinMapping.ResetPin);
            asynchronousCounterPinMapping.TimesClocksChanged = 0;
            asynchronousCounterPinMapping.HasEncounteredHighState = false;
        }
        
    }
    public override void RunLogic()
    {
        switch (asynchronousCounterData.AsynchronousCountersType)
        {
            case AsynchronousCountersTypes.MOD_10:
                Mod10Logic(asynchronousCounterData,_icController);
                break;
        }
    }

    
}