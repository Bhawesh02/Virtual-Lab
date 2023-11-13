
using System;
using UnityEngine;

public class ClockController : MonoBehaviour
{
    [field: SerializeField]
    public PinController ClockPinController { get;private set; }

    [SerializeField]
    private float pluseDelay = 1f;
    private float changePluseTime;
    private void Start()
    {
        ValuePropagateService.Instance.ClockPins.Add(ClockPinController);
        EventService.Instance.SimulationStarted += () => { changePluseTime = Time.time + pluseDelay;  };
    }

    private void Update()
    {
        if (!SimulatorManager.Instance.SimulationRunning)
            return;
        if(Time.time >= changePluseTime)
        {
            ChangeClockPinValue();
        }
    }

    private void ChangeClockPinValue()
    {
        if (ClockPinController.value == PinValue.Negative)
            ClockPinController.value = PinValue.Positive;
        else
            ClockPinController.value = PinValue.Negative;
    }
}
