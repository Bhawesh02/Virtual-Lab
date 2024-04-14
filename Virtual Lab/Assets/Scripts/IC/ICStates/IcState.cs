public abstract class IcState
{
    protected ICController _icController;
    protected IcState(ICController icController)
    {
        _icController = icController;
    }
    public abstract void SetData();
    public abstract void RunLogic();
    public abstract void SetPins();
}