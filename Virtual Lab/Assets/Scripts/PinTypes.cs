
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
[Serializable]
public class PinInfo
{
    public PinType Type;
    public int PinNumber;
}