public abstract class IcState
{
    protected ICController _icController;

    public ICTypes IcType => _icController.Model.IcData.ICType;
    protected IcState(ICController icController)
    {
        _icController = icController;
    }
    public abstract void SetData();
    public abstract void RunLogic();
    public abstract void SetPins();
    public abstract void PropagateOutputPinValues();
}