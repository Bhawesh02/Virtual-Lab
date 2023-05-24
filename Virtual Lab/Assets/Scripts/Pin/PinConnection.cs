using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinConnection : MonoBehaviour
{
    public PinValue value ;
    public PinInfo CurrentPinInfo;
    public PinInfo ConnectedPinInfo;
    public Sprite PinPostive;
    public Sprite PinNegative;
    public Sprite PinNull;
    public SpriteRenderer spriteRenderer;

    [SerializeField]
    private GameObject wireGameObject;
    private void Awake()
    {
        value = PinValue.Null;
        spriteRenderer = GetComponent<SpriteRenderer>();
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
