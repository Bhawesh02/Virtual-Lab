
using System.Collections.Generic;
using UnityEngine;

public class PinController : MonoBehaviour
{
    public PinValue value;
    public PinInfo CurrentPinInfo;
    public List<WireController> Wires = new();
    [SerializeField]
    private GameObject wireGameObject;

    private SpriteRenderer ShowColor;

    private SimulatorManager simulationManager;

    private WireService wireService;

    private void Awake()
    {
        value = PinValue.Null;
        CurrentPinInfo.pinConnection = this;
        ShowColor = null; 
    }
    
    

    private void Start()
    {
        simulationManager = SimulatorManager.Instance;
        wireService = WireService.Instance;
        
        if (CurrentPinInfo.Type == PinType.Input)
            value = PinValue.Negative;

        if (CurrentPinInfo.Type == PinType.Vcc)
            value = PinValue.Vcc;
        if (CurrentPinInfo.Type == PinType.Gnd)
            value = PinValue.Gnd;
        
        SendReferenceToValuePropagator();

        if(CurrentPinInfo.Type == PinType.Input || CurrentPinInfo.Type == PinType.Output)
        {
            ShowColor = transform.GetChild(0).GetComponent<SpriteRenderer>();
            ShowColor.sortingOrder = 1;
        }
    }

    private void SendReferenceToValuePropagator()
    {
        ValuePropagateService valuePropagate = ValuePropagateService.Instance;
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
        if (ShowColor == null)
        {
            return;
        }
        switch (value)
        {
            case PinValue.Null:
                ShowColor.sprite = null;
                break;
            case PinValue.Positive:
                ShowColor.sprite = simulationManager.PinPostive;
                break;
            case PinValue.Negative:
                ShowColor.sprite = simulationManager.PinNegative;
                break;

        }
    }

    private void OnMouseDown()
    {
        if (simulationManager.SimulationRunning)
            return;
        if (CurrentPinInfo.Type == PinType.Null)
            return;
        if (!wireService.doingConnection)
        {
            CreateNewWire();
            return;
        }
        CompleteExsistingWire();
    }

    private void CreateNewWire()
    {
        GameObject wire = Instantiate(wireGameObject, simulationManager.WiresGameObject.transform);
        WireController wireController = wire.GetComponent<WireController>();
        wireService.Wire = wire;
        wireController.MakeWire(this.transform.position);
        wireController.initialPin = this;
        wireService.doingConnection = true;
    }

    private void CompleteExsistingWire()
    {
        wireService.doingConnection = false;
        WireController wireController = wireService.Wire.GetComponent<WireController>();
        wireController.SetWireEnd(this.transform.position);
        wireController.finalPin = this;
        wireController.ConfirmConnection();


    }

}
