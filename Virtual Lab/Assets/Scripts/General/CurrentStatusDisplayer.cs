
using System;
using UnityEngine;

public class CurrentStatusDisplayer : MonoBehaviour
{
    
    private void Awake()
    {
        EventService.Instance.AllValuePropagated += ShowStatus;

    }

    private void ShowStatus()
    {
        //Get info of all inout and output pins
    }
}
