
using System;
using System.Collections.Generic;
using UnityEngine;

public class SimulatorManager : MonoGenericSingelton<SimulatorManager>
{

    [SerializeField]
    private Camera mainCamera;

    public List<ICModel> ICModels = new();

    public List<WireController> WiresInSystem = new();


    public bool SimulationRunning;

    public Sprite PinPostive;

    public Sprite PinNegative;

    public Sprite PinNull;



    public List<Color> colorList = new() { Color.red, Color.black, Color.blue };

    private Vector2 mosuePos;

    private float rightClickDetectionRadius = 0.05f;

    [SerializeField]
    private LayerMask rightClickDetectionLayers;
    private float callTime;
    protected override void Awake()
    {
        base.Awake();
        SimulationRunning = false;
        callTime = Time.time;
    }
    private void Start()
    {
        EventService.Instance.SimulationStarted += () =>
        {
            SimulationRunning = true;
        };
        EventService.Instance.SimulationStopped += () =>
        {
            SimulationRunning = false;
        };
    }
    private void Update()
    {
        RightClickControl();
        RunValuePropagatinAfterDelay();
    }

    private void RunValuePropagatinAfterDelay()
    {
        if (!SimulationRunning)
            return;
        if (Time.time >= callTime)
        {
            ValuePropagateService.Instance.StartTransfer();
            callTime = Time.time + 0.05f;
        }
    }

    private void RightClickControl()
    {
        if (!Input.GetMouseButtonDown(1))
            return;
        mosuePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Collider2D collider = Physics2D.OverlapCircle(mosuePos, rightClickDetectionRadius, rightClickDetectionLayers);
        if (collider == null)
            return;
        if (collider.GetComponent<ICView>())
            RightClickIcBase(collider);
        else if (collider.GetComponent<WireController>())
            RightClickWire(collider);
    }

    private void RightClickIcBase(Collider2D collider)
    {
        ICView iCView = collider.GetComponent<ICView>();
        if (iCView != null && iCView.Controller.Model.IcData != null)
        {
            EventService.Instance.InvokeChangeIC(iCView.Controller.Model, null);
        }
    }
    private void RightClickWire(Collider2D collider)
    {
        WireController wire = collider.GetComponent<WireController>();
        WireService.Instance.RemoveWireConnection(wire);
    }

    private void OnDestroy()
    {
        EventService.Instance.SimulationStarted -= () =>
        {
            SimulationRunning = true;
        };
        EventService.Instance.SimulationStopped -= () =>
        {
            SimulationRunning = false;
        };
    }

}
