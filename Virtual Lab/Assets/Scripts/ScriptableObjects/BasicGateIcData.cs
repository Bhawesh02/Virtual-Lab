using UnityEngine;

[CreateAssetMenu(fileName = "New_Basic_Gate_IC_Data", menuName = "Ic/BasicGateIcData")]
public class BasicGateIcData : IcData
{
    public BasicGateTypes BasicGateType;
    public BasicGatePinMapping[] PinMapping;
}