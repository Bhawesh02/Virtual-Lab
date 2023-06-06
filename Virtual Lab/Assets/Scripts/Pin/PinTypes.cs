
using System;

public enum PinType
{
    Null, // NO PIN-TYPE (FOR ERROR HANDELLING)
    Input,
    Output,
    Vcc,
    Gnd,
    IcInput,
    IcOutput,
    IcVcc,
    IcGnd
}

public enum ConnectionDirection
{
    Null,
    InititalToFinal,
    FinalToInitial
}

public enum PinValue
{
    Null,
    Positive,
    Negative,
    Vcc,
    Gnd
}
[Serializable]
public class PinInfo
{
    public PinType Type;
    public int PinNumber;
    public PinController pinConnection;
}