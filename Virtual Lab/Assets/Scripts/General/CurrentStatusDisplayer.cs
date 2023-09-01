
using System;
using System.Collections.Generic;
using UnityEngine;

public class CurrentStatusDisplayer : MonoBehaviour
{
    private ValuePropagate valuPropogate;
    private List<PinController> inputPinswithwire = new();
    private List<PinController> outputPinswithwire = new();
    private void Awake()
    {
        EventService.Instance.AllValuePropagated += ShowStatus;

    }
    private void Start()
    {
        valuPropogate = ValuePropagate.Instance;
    }
    private void ShowStatus()
    {
        //Get info of all inout and output pins
        GetInputAndOuptPins();
    }

    private void GetInputAndOuptPins()
    {
        for(int i = 0; i < valuPropogate.InputPins.Count; i++) 
        { 
            if(valuPropogate.InputPins[i].Wires.Count>0){
                inputPinswithwire.Add(valuPropogate.InputPins[i]);
            }   
        }
        for (int i = 0; i < valuPropogate.OutputPins.Count; i++)
        {
            if(valuPropogate.OutputPins[i].Wires.Count>0){
                outputPinswithwire.Add(valuPropogate.OutputPins[i]);
            }   
        }
    }
}
