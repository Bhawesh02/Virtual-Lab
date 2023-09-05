
using System.Collections.Generic;
using TMPro;
using UnityEditor.MPE;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SimulatorManager : MonoGenericSingelton<SimulatorManager>
{





    public List<ICModel> ICModels = new();

    public List<WireController> Wires;


    public bool SimulationRunning;

    public Sprite PinPostive;

    public Sprite PinNegative;
    
    public Sprite PinNull;


    private EventService eventService;

    public List<Color> colorList = new() { Color.red, Color.black, Color.blue };


    protected override void Awake()
    {
        base.Awake();
        SimulationRunning = false;
    }
    private void Start()
    {
        eventService = EventService.Instance;
        eventService.SimulationStarted += () => {
            SimulationRunning = true;
        };
        eventService.SimulationStopped += () =>
        {
            SimulationRunning = false;
        };
    }

    
    
}
