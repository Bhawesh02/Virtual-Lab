using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinConnection : MonoBehaviour
{
    public PinValue value;
    public PinInfo CurrentPinInfo;
    public List<WireController> Wires = new();
    [SerializeField]
    private Sprite PinPostive;
    [SerializeField]
    private Sprite PinNegative;
    [SerializeField]
    private Sprite PinNull;
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

        AddToValuePropagator();

    }
    private void AddToValuePropagator()
    {
        ValuePropagate valuePropagate = SimulatorManager.Instance.valuePropagate;
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
        if (spriteRenderer != null)
        {
            switch (value)
            {
                case PinValue.Null:
                    spriteRenderer.sprite = PinNull;
                    break;
                case PinValue.Positive:
                    spriteRenderer.sprite = PinPostive;
                    break;
                case PinValue.Negative:
                    spriteRenderer.sprite = PinNegative;
                    break;

            }
        }
    }

    private void OnMouseDown()
    {
        if (CurrentPinInfo.Type == PinType.Null)
            return;
        if (!SimulatorManager.Instance.doingConnection)
        {
            CreateNewWire();
            return;
        }
        CompleteExsistingWire();
    }
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
