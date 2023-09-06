
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

    protected override void Awake()
    {
        base.Awake();
        SimulationRunning = false;
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
        mosuePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(1))
        {
            Collider2D collider = Physics2D.OverlapCircle(mosuePos, rightClickDetectionRadius);
            Debug.Log(collider);
        }
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
