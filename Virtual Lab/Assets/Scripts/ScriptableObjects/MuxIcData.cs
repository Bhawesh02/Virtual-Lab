using UnityEngine;

[CreateAssetMenu(fileName = "New_MUX_IC_Data", menuName = "Ic/MuxIcData")]
public class MuxIcData : IcData
{
    public MuxTypes MuxType;
    public int[] InputPins;
    public int[] OutpuPins;
    public int[] SelectInputPins;
    public int StrobePin;
}