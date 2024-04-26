using System;
using Unity.VisualScripting;
using UnityEngine;

public class WireService : MonoGenericSingelton<WireService>
{
    public bool doingConnection = false;


    [SerializeField] private WireController wirePrefab;


    private WireController NewWireMakingConnection;

    private WirePool wirePool;

    private void Start()
    {
        wirePool = new(wirePrefab);
        EventService.Instance.RemoveWireConnection += RemoveWireConnection;
    }

    private void Update()
    {
        if (doingConnection)
        {
            SetWireEndToMousePointer();
            if (Input.GetMouseButtonDown(1))
            {
                ReturnWire(NewWireMakingConnection);
                NewWireMakingConnection = null;
                doingConnection = false;
            }
        }
    }

    private void SetWireEndToMousePointer()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 endPosition = new(mousePosition.x, mousePosition.y, mousePosition.z);
        NewWireMakingConnection.SetWireEnd(endPosition);
    }

    public void CreateNewWire(PinController initialPinController)
    {
        NewWireMakingConnection = wirePool.GetItem();
        NewWireMakingConnection.transform.SetParent(transform);
        NewWireMakingConnection.MakeWire(initialPinController.transform.position);
        NewWireMakingConnection.initialPin = initialPinController;
        doingConnection = true;
    }

    public void CompleteExsistingWire(PinController finalPinController)
    {
        doingConnection = false;
        NewWireMakingConnection.SetWireEnd(finalPinController.transform.position);
        NewWireMakingConnection.finalPin = finalPinController;
        NewWireMakingConnection.ConfirmConnection();
    }

    public void ReturnWire(WireController wireToReturn)
    {
        wirePool.ReturnItem(wireToReturn);
    }


    #region Remove Connected Wire

    public void RemoveWireConnection(WireController wire)
    {
        if (SimulatorManager.Instance.SimulationRunning)
        {
            return;
        }

        PinController inititalPin = wire.initialPin;
        PinController finalPin = wire.finalPin;
        ResetPinValue(inititalPin);
        ResetPinValue(finalPin);
        RemoveInputPinConnected(wire);
        SimulatorManager.Instance.WiresInSystem.Remove(wire);
        inititalPin.Wires.Remove(wire);
        finalPin.Wires.Remove(wire);
        ReturnWire(wire);
    }

    private static void ResetPinValue(PinController pin)
    {
        
        switch (pin.CurrentPinInfo.Type)
        {
            case PinType.Output:
                pin.value = PinValue.Negative;
                break;
            case PinType.IcInput:
            case PinType.IcVcc:
            case PinType.IcGnd:
                pin.value = PinValue.Negative;
                break;
        }
    }

    private void RemoveInputPinConnected(WireController wire)
    {
        switch (wire.connectionDirection)
        {
            case ConnectionDirection.Null:
                return;
            case ConnectionDirection.InititalToFinal:
                MakeIsInputConnectedFalse(wire.finalPin);
                break;
            case ConnectionDirection.FinalToInitial:
                MakeIsInputConnectedFalse(wire.initialPin);
                break;
        }

        wire.connectionDirection = ConnectionDirection.Null;
    }

    private void MakeIsInputConnectedFalse(PinController pin)
    {
        OutputPinConnectionCheck outputPin = pin.GetComponent<OutputPinConnectionCheck>();
        if (outputPin == null)
            return;
        outputPin.IsInputPinConnected = false;
        foreach (WireController wire in pin.Wires)
        {
            if (wire.connectionDirection == ConnectionDirection.Null)
                continue;
            wire.connectionDirection = ConnectionDirection.Null;
            if (pin == wire.initialPin)
            {
                MakeIsInputConnectedFalse(wire.finalPin);
            }
            else
            {
                MakeIsInputConnectedFalse(wire.initialPin);
            }
        }
    }

    #endregion

    private void OnDestroy()
    {
        EventService.Instance.RemoveWireConnection -= RemoveWireConnection;
    }
}