
using System;
using System.Collections.Generic;
using UnityEngine;

public class CurrentStatusDisplayer : MonoBehaviour
{
    private ValuePropagate valuPropogate;
    private List<PinController> inputPins;
    private List<PinController> outputPins;
    
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
            
        }
        for (int i = 0; i < valuPropogate.OutputPins.Count; i++)
        {

        }
    }
}
