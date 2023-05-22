using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinConnection : MonoBehaviour
{
    public PinInfo CurrentPinInfo;
    public PinInfo ConnectedPinInfo;
    [SerializeField]
    private GameObject wireGameObject;
    private void OnMouseDown()
    {
        if (!SimulatorManager.Instance.doingConnection)
        {
            createNewWire();
            return;
        }
        completeExsistingWire();
    }

    private void completeExsistingWire()
    {
        SimulatorManager.Instance.doingConnection = false;
        WireController wireController = SimulatorManager.Instance.Wire.GetComponent<WireController>();
        wireController.SetWireEnd(this.transform.position);
        wireController.finalPin = this;
        MakeFinalConnection(wireController);

    }

    private void createNewWire()
    {
        GameObject wire = Instantiate(wireGameObject);
        WireController wireController = wire.GetComponent<WireController>();
        SimulatorManager.Instance.Wire = wire;
        wireController.MakeWire(this.transform.position);
        wireController.initialPin = this;
        SimulatorManager.Instance.doingConnection = true;
    }

    public void MakeFinalConnection(WireController wireController)
    {

        wireController.initialPin.ConnectedPinInfo = wireController.finalPin.CurrentPinInfo;
        wireController.finalPin.ConnectedPinInfo = wireController.initialPin.CurrentPinInfo;
    }
}
