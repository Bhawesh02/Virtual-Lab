
using System.Collections.Generic;
using UnityEngine;

public class PinController : MonoBehaviour
{
    public PinValue value;
    public PinInfo CurrentPinInfo;
    public List<WireController> Wires = new();
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private GameObject wireGameObject;


    private void Awake()
    {
        value = PinValue.Null;
        spriteRenderer = GetComponent<SpriteRenderer>();
        CurrentPinInfo.pinConnection = this;
    }
    
    

    private void Start()
    {

        if (CurrentPinInfo.Type == PinType.Input)
            value = PinValue.Negative;
        if (CurrentPinInfo.Type == PinType.Vcc)
            value = PinValue.Vcc;
        if (CurrentPinInfo.Type == PinType.Gnd)
            value = PinValue.Gnd;
        
        SendReferenceToValuePropagator();
    }

    private void SendReferenceToValuePropagator()
    {
        ValuePropagate valuePropagate = ValuePropagate.Instance;
        switch (CurrentPinInfo.Type)
        {
            case PinType.Null:
                break;
            case PinType.Gnd:
                valuePropagate.GndPins.Add(this);
                break;
            case PinType.Input:
                valuePropagate.InputPins.Add(this);
                break;
            case PinType.Output:
                valuePropagate.OutputPins.Add(this);
                break;
            case PinType.Vcc:
                valuePropagate.VccPins.Add(this);
                break;
        }
    }
    private void Update()
    {
        ChangeSpriteBasedOnValue();
    }

    private void ChangeSpriteBasedOnValue()
    {
        /*if (spriteRenderer != null)
        {
            switch (value)
            {
                case PinValue.Null:
                    spriteRenderer.sprite = SimulatorManager.Instance.PinNull;
                    break;
                case PinValue.Positive:
                    spriteRenderer.sprite = SimulatorManager.Instance.PinPostive;
                    break;
                case PinValue.Negative:
                    spriteRenderer.sprite = SimulatorManager.Instance.PinNegative;
                    break;

            }
        }*/
    }

    private void OnMouseDown()
    {
        if (SimulatorManager.Instance.SimulationRunning)
            return;
        if (CurrentPinInfo.Type == PinType.Null)
            return;
        if (!SimulatorManager.Instance.doingConnection)
        {
            CreateNewWire();
            return;
        }
        CompleteExsistingWire();
    }

    /*private void OnMouseEnter()
    {
        Cursor.SetCursor(SimulatorManager.Instance.cursorTexture, SimulatorManager.Instance.hotSpot,SimulatorManager.Instance.cursorMode);
    }
    private void OnMouseExit()
    {
        Cursor.SetCursor(null, SimulatorManager.Instance.hotSpot,SimulatorManager.Instance.cursorMode);

    }*/
    private void CreateNewWire()
    {
        GameObject wire = Instantiate(wireGameObject, SimulatorManager.Instance.WiresGameObject.transform);
        WireController wireController = wire.GetComponent<WireController>();
        SimulatorManager.Instance.Wire = wire;
        wireController.MakeWire(this.transform.position);
        wireController.initialPin = this;
        SimulatorManager.Instance.doingConnection = true;
    }

    private void CompleteExsistingWire()
    {
        SimulatorManager.Instance.doingConnection = false;
        WireController wireController = SimulatorManager.Instance.Wire.GetComponent<WireController>();
        wireController.SetWireEnd(this.transform.position);
        wireController.finalPin = this;
        wireController.ConfirmConnection();


    }

}
