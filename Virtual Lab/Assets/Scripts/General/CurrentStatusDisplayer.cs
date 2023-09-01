
using UnityEngine;

public class CurrentStatusDisplayer : MonoBehaviour
{
    private SimulatorManager simulatorManager;    
    private void Awake()
    {
        EventService.Instance.AllValuePropagated += ShowStatus;

    }
    private void Start()
    {
        simulatorManager = SimulatorManager.Instance;
    }
    private void ShowStatus()
    {
        //Get info of all inout and output pins

    }
}
