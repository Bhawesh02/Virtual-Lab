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
    private void Awake()
    {
        SimulatorManager.Instance.Wires.Add(this);
        valuePropagated = false;
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
    public void ConfirmConnection()
    {
        if ((initialPin == finalPin) || (finalPin.CurrentPinInfo.Type == PinType.Null))
        {
            Destroy(gameObject);
            return;
        }
        initialPin.Wires.Add(this);
        finalPin.Wires.Add(this);
    }
    private void OnDestroy()
    {
        SimulatorManager.Instance.Wires.Remove(this);
    }
}
