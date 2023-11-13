
using System.Collections.Generic;
using UnityEngine;

public class WireController : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private EdgeCollider2D edgeCollider;
    public PinController initialPin;
    public PinController finalPin;

    public bool valuePropagated;
   
    public ConnectionDirection connectionDirection;


    private void OnEnable()
    {
        SimulatorManager.Instance.WiresInSystem.Add(this);
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
        if (pin.CurrentPinInfo.Type == PinType.MonoShot||pin.CurrentPinInfo.Type == PinType.Input || pin.CurrentPinInfo.Type == PinType.IcOutput || pin.CurrentPinInfo.Type == PinType.Vcc || pin.CurrentPinInfo.Type == PinType.Gnd)
            return true;
        

        return false;
    }

    private bool IsAOutputPin(PinController pin)
    {
        return !IsAInputPin(pin);
    }

    private void SetIsInputPinConnectedToTrue(PinController pin)
    {
        pin.gameObject.GetComponent<OutputPinConnectionCheck>().IsInputPinConnected = true;
        
        foreach(WireController wire in pin.Wires)
        {
            if(wire.initialPin == pin)
            {
                if (wire.finalPin.GetComponent<OutputPinConnectionCheck>().IsInputPinConnected == true)
                    continue;
                SetIsInputPinConnectedToTrue(wire.finalPin);
                continue;
            }
            if (wire.initialPin.GetComponent<OutputPinConnectionCheck>().IsInputPinConnected == true)
                continue;
            SetIsInputPinConnectedToTrue(wire.initialPin);

        }
    }
    public void ConfirmConnection()
    {
        /*SimulatorManager.Instance.WiresConnectionChecked.Clear();*/
        if ((initialPin == finalPin) || (finalPin.CurrentPinInfo.Type == PinType.Null) || (IsAInputPin(initialPin) && IsAInputPin(finalPin)))
        {
            WireService.Instance.ReturnWire(this);
            return;
        }
        //Initial Pin - Input , FinalPin - Output
        if(IsAInputPin(initialPin) && IsAOutputPin(finalPin))
        {
            if (finalPin.gameObject.GetComponent<OutputPinConnectionCheck>().IsInputPinConnected)
            {
                WireService.Instance.ReturnWire(this);
                return;
            }
            
            SetIsInputPinConnectedToTrue(finalPin);
            connectionDirection = ConnectionDirection.InititalToFinal;
            ChangeDirectionForConnectedWires(finalPin);

        }

        //Initial Pin - Output , FinalPin - Input

        if (IsAInputPin(finalPin) && IsAOutputPin(initialPin))
        {
            if (initialPin.gameObject.GetComponent<OutputPinConnectionCheck>().IsInputPinConnected)
            {
                WireService.Instance.ReturnWire(this);
                return;
            }
            SetIsInputPinConnectedToTrue(initialPin);
            connectionDirection = ConnectionDirection.FinalToInitial;
            ChangeDirectionForConnectedWires(initialPin);


        }

        //Initial Pin - Output , FinalPin - Output


        if (IsAOutputPin(initialPin) && IsAOutputPin(finalPin))
        {
            if (initialPin.gameObject.GetComponent<OutputPinConnectionCheck>().IsInputPinConnected && finalPin.gameObject.GetComponent<OutputPinConnectionCheck>().IsInputPinConnected)
            {
                WireService.Instance.ReturnWire(this);
                return;
            }
            if (initialPin.gameObject.GetComponent<OutputPinConnectionCheck>().IsInputPinConnected)
            {
                SetIsInputPinConnectedToTrue(finalPin);
                connectionDirection = ConnectionDirection.InititalToFinal;
                ChangeDirectionForConnectedWires(finalPin);

            }
            else if (finalPin.gameObject.GetComponent<OutputPinConnectionCheck>().IsInputPinConnected)
            {
                SetIsInputPinConnectedToTrue(initialPin);
                connectionDirection = ConnectionDirection.FinalToInitial;
                ChangeDirectionForConnectedWires(initialPin);
            }
        }
        
        initialPin.Wires.Add(this);
        finalPin.Wires.Add(this);
        /*SimulatorManager.Instance.WiresConnectionChecked.Clear();*/
        SetEdgeCollider();
    }

    private void ChangeDirectionForConnectedWires(PinController pin)
    {
        foreach (WireController wire in pin.Wires)
        {

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


    private void SetEdgeCollider()
    {
        List<Vector2> edges = new();
        for(int point = 0; point< lineRenderer.positionCount; point++)
        {
            edges.Add(lineRenderer.GetPosition(point));
        }
        edgeCollider.SetPoints(edges);
    }

    private void OnDisable()
    {
        SimulatorManager.Instance.WiresInSystem.Remove(this);
    }
}
