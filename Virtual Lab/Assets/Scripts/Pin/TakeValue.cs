using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PinConnection))]
public class TakeValue : MonoBehaviour
{
    private PinConnection pinConnection;
    private void Awake()
    {
        pinConnection = GetComponent<PinConnection>();
    }
    // Update is called once per frame
    void Update()
    {
        PinInfo connectedPin = pinConnection.ConnectedPinInfo;
        if (connectedPin.Type != PinType.Null)
            ChangeValue(connectedPin);
    }

    private void ChangeValue(PinInfo connectedPin)
    {
        pinConnection.value = connectedPin.pinConnection.value; 
    }
}
