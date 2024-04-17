using UnityEngine;

[CreateAssetMenu(fileName = "DeMuxIcData", menuName = "Ic/DeMuxIcData")]
public class DeMuxIcData : IcData
{
    public DeMuxTypes DeMuxType;
    public int InputPin;
    public int[] OutputPins;
    public int[] SelectPins;
}