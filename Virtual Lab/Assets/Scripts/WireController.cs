using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireController : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;
    public PinConnection initialPin;
    public PinConnection finalPin;

    public bool valuePropagated;
   
    public ConnectionDirection connectionDirection;
    private void Awake()
    {
        SimulatorManager.Instance.Wires.Add(this);
        valuePropagated = false;
        connectionDirection = ConnectionDirection.Null;
    }
    public void MakeWire(Vector3 startPos)
    {
        gameObject.SetActive(true);
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPos);
    }

    public void SetWireEnd(Vector3 endPos)
    {
        lineRenderer.SetPosition(1, endPos);
    }

    private bool DoseThisPinGiveValue(PinConnection pin)
    {
        if (pin.CurrentPinInfo.Type == PinType.Input || pin.CurrentPinInfo.Type == PinType.IcOutput || pin.CurrentPinInfo.Type == PinType.Vcc || pin.CurrentPinInfo.Type == PinType.Gnd)
            return true;
        

        return false;
    }

    private bool DoseThisPinTakeValue(PinConnection pin)
    {
        return !DoseThisPinGiveValue(pin);
    }

    private void ChangeIsInputPinConnected(PinConnection pin)
    {
        pin.gameObject.GetComponent<OutputPinConnectionCheck>().IsInputPinConnected = true;
        /*foreach(WireController wire in pin.Wires)
        {
            if (SimulatorManager.Instance.WiresConnectionChecked.Contains(wire))
                continue;
            SimulatorManager.Instance.WiresConnectionChecked.Add(wire);
            if (wire.initialPin == pin)
                ChangeIsInputPinConnected(wire.finalPin);
            else
                ChangeIsInputPinConnected(wire.initialPin);
        }*/
        foreach(WireController wire in pin.Wires)
        {
            if(wire.initialPin == pin)
            {
                if (wire.finalPin.GetComponent<OutputPinConnectionCheck>().IsInputPinConnected == true)
                    continue;
                ChangeIsInputPinConnected(wire.finalPin);
                continue;
            }
            if (wire.initialPin.GetComponent<OutputPinConnectionCheck>().IsInputPinConnected == true)
                continue;
            ChangeIsInputPinConnected(wire.initialPin);

        }
    }
    public void ConfirmConnection()
    {
        /*SimulatorManager.Instance.WiresConnectionChecked.Clear();*/
        if ((initialPin == finalPin) || (finalPin.CurrentPinInfo.Type == PinType.Null) || (DoseThisPinGiveValue(initialPin) && DoseThisPinGiveValue(finalPin)))
        {
            Destroy(gameObject);
            return;
        }
        //Initial Pin - Input , FinalPin - Output
        if(DoseThisPinGiveValue(initialPin) && DoseThisPinTakeValue(finalPin))
        {
            if (finalPin.gameObject.GetComponent<OutputPinConnectionCheck>().IsInputPinConnected)
            {
                Destroy(gameObject);
                return;
            }
            
            ChangeIsInputPinConnected(finalPin);
            connectionDirection = ConnectionDirection.InititalToFinal;
            ChangeDirectionForConnectedWires(finalPin);

        }

        //Initial Pin - Output , FinalPin - Input

        if (DoseThisPinGiveValue(finalPin) && DoseThisPinTakeValue(initialPin))
        {
            if (initialPin.gameObject.GetComponent<OutputPinConnectionCheck>().IsInputPinConnected)
            {
                Destroy(gameObject);
                return;
            }
            ChangeIsInputPinConnected(initialPin);
            connectionDirection = ConnectionDirection.FinalToInitial;
            ChangeDirectionForConnectedWires(initialPin);


        }

        //Initial Pin - Output , FinalPin - Output


        if (DoseThisPinTakeValue(initialPin) && DoseThisPinTakeValue(finalPin))
        {
            if (initialPin.gameObject.GetComponent<OutputPinConnectionCheck>().IsInputPinConnected && finalPin.gameObject.GetComponent<OutputPinConnectionCheck>().IsInputPinConnected)
            {
                Destroy(gameObject);
                return;
            }
            if (initialPin.gameObject.GetComponent<OutputPinConnectionCheck>().IsInputPinConnected)
            {
                ChangeIsInputPinConnected(finalPin);
                connectionDirection = ConnectionDirection.InititalToFinal;
                ChangeDirectionForConnectedWires(finalPin);

            }
            else if (finalPin.gameObject.GetComponent<OutputPinConnectionCheck>().IsInputPinConnected)
            {
                ChangeIsInputPinConnected(initialPin);
                connectionDirection = ConnectionDirection.FinalToInitial;
                ChangeDirectionForConnectedWires(initialPin);
            }
        }
        
        initialPin.Wires.Add(this);
        finalPin.Wires.Add(this);
        /*SimulatorManager.Instance.WiresConnectionChecked.Clear();*/

    }

    private void ChangeDirectionForConnectedWires(PinConnection pin)
    {
        foreach (WireController wire in pin.Wires)
        {
            Debug.Log(wire);

            if (wire.connectionDirection != ConnectionDirection.Null)
                continue;
            if (pin == wire.initialPin)
            {
                wire.connectionDirection = ConnectionDirection.InititalToFinal;
                ChangeDirectionForConnectedWires(wire.finalPin);
            }
            else
            {
                wire.connectionDirection = ConnectionDirection.FinalToInitial;
                ChangeDirectionForConnectedWires(wire.initialPin);

            }
        }
    }

    private void OnDestroy()
    {
        SimulatorManager.Instance.Wires.Remove(this);
    }
}
