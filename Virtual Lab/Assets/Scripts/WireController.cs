
using System.Collections.Generic;
using UnityEngine;

public class WireController : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;
    public PinController initialPin;
    public PinController finalPin;

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
        SetWireColor();
        lineRenderer.SetPosition(0, startPos);
    }

    private void SetWireColor()
    {
        int colorNum = Random.Range(0, SimulatorManager.Instance.colorList.Count);
        Color color = SimulatorManager.Instance.colorList[colorNum];
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.SetColors(color, color);
    }

    public void SetWireEnd(Vector3 endPos)
    {
        lineRenderer.SetPosition(1, endPos);
    }

    private bool IsAInputPin(PinController pin)
    {
        if (pin.CurrentPinInfo.Type == PinType.Input || pin.CurrentPinInfo.Type == PinType.IcOutput || pin.CurrentPinInfo.Type == PinType.Vcc || pin.CurrentPinInfo.Type == PinType.Gnd)
            return true;
        

        return false;
    }

    private bool IsAOutputPin(PinController pin)
    {
        return !IsAInputPin(pin);
    }

    private void ChangeIsInputPinConnected(PinController pin)
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
        if ((initialPin == finalPin) || (finalPin.CurrentPinInfo.Type == PinType.Null) || (IsAInputPin(initialPin) && IsAInputPin(finalPin)))
        {
            Destroy(gameObject);
            return;
        }
        //Initial Pin - Input , FinalPin - Output
        if(IsAInputPin(initialPin) && IsAOutputPin(finalPin))
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

        if (IsAInputPin(finalPin) && IsAOutputPin(initialPin))
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


        if (IsAOutputPin(initialPin) && IsAOutputPin(finalPin))
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

    private void ChangeDirectionForConnectedWires(PinController pin)
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
