using System;
using UnityEngine;
[Serializable]
public class AsynchronousCounterPinMapping
{
     public AsynchronousCountersTypes AsynchronousCountersType;
     public int ClockPin;
     public int[] OutputPins;
     public int SetPin;
     public int ResetPin;
     [HideInInspector] 
     public int TimesClocksChanged;
     [HideInInspector] 
     public bool HasEncounteredHighState;
}