using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New_Asynchronous", menuName = "Ic/AsynchronousData")]
public class AsynchronousCounterData : IcData
{
    public AsynchronousCountersTypes AsynchronousCountersType;
    public List<AsynchronousCounterPinMapping> asynchronousCounterPinMappings;
}