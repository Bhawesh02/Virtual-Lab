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
            GameObject wire = Instantiate(wireGameObject);
            WireController wireController = wire.GetComponent<WireController>();
            SimulatorManager.Instance.Wire = wire;
            wireController.MakeWire(this.transform.position);
            SimulatorManager.Instance.doingConnection = true;
            return;
        }
        SimulatorManager.Instance.doingConnection = false;
        SimulatorManager.Instance.Wire.GetComponent<WireController>().SetWireEnd(this.transform.position);
    }
}
