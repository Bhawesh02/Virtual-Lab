

using System;

public class EventService : MonoGenericSingelton<EventService>
{
    public event Action SimulationStarted;
    public event Action SimulationStopped;
    public event Action AllValuePropagated;
    public event Action InputValueChanged;
    public event Action<IC> ShowICTT;
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
    public void InvokeShowICTT(IC ic)
    {
        ShowICTT?.Invoke(ic);
    }
}
