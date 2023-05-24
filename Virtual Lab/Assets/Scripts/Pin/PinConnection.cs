using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinConnection : MonoBehaviour
{
    public PinValue value ;
    public PinInfo CurrentPinInfo;
    public PinInfo ConnectedPinInfo;
    [SerializeField]
    private GameObject wireGameObject;
    private void Awake()
    {
        value = PinValue.Null;
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
        GameObject wire = Instantiate(wireGameObject, SimulatorManager.Instance.Wires.transform);
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
        MakeFinalConnection(wireController);

    }

    public void MakeFinalConnection(WireController wireController)
    {

        wireController.initialPin.ConnectedPinInfo = wireController.finalPin.CurrentPinInfo;
        wireController.finalPin.ConnectedPinInfo = wireController.initialPin.CurrentPinInfo;
        

        wireController.ConfirmConnection();
    }
}
