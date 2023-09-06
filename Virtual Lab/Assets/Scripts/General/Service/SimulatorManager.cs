
using System.Collections.Generic;
using UnityEngine;

public class SimulatorManager : MonoGenericSingelton<SimulatorManager>
{





    public List<ICModel> ICModels = new();

    public List<WireController> WiresInSystem = new();


    public bool SimulationRunning;

    public Sprite PinPostive;

    public Sprite PinNegative;
    
    public Sprite PinNull;



    public List<Color> colorList = new() { Color.red, Color.black, Color.blue };

    protected override void Awake()
    {
        base.Awake();
        SimulationRunning = false;
    }
    private void Start()
    {
        EventService.Instance.SimulationStarted += () => {
            SimulationRunning = true;
        };
        EventService.Instance.SimulationStopped += () =>
        {
            SimulationRunning = false;
        };
    }

    private void OnDestroy()
    {
        EventService.Instance.SimulationStarted -= () => {
            SimulationRunning = true;
        };
        EventService.Instance.SimulationStopped -= () =>
        {
            SimulationRunning = false;
        };
    }


}
