

using System;

public class EventService : GenericSingelton<EventService>
{
    public event Action SimulationStarted;
    public event Action SimulationStopped;
    public event Action AllValuePropagated;
    public event Action InputValueChanged;
    public void InvokeSimulationStarted()
    {
        SimulationStarted?.Invoke();
    }
    public void InvokeSimulationStopped()
    {
        SimulationStopped?.Invoke();
    }
    public void InvokeAllValuePropagated()
    {
        AllValuePropagated?.Invoke();
    }
    public void InvokeInputValueChanged()
    {
        InputValueChanged?.Invoke();
    }
}
