using System;

[Serializable]
public struct FlipFlopPinMapping
{
    public int[] InputPins;
    public int[] OutputPins;
    public int ClockPin;
    public int PresetPin;
    public int ClearPin;
}