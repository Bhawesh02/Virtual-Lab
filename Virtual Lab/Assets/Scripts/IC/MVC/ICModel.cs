

public class ICModel
{
    public ICBase thisIC { get; }

    public PinController VccPin;
    public PinController GndPin;

    public ICModel(ICBase thisIC)
    {
        this.thisIC = thisIC;
    }

}
