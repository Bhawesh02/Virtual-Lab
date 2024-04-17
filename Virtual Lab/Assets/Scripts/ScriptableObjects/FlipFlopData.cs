using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Flip_Flop",menuName = "Ic/Flip_Flop_IcData")]
public class FlipFlopData : IcData
{
    public TypeOfFlipFlops FlipFlopType;
    public FlipFlopPinMapping[] FlipFlopPinMappings;
}