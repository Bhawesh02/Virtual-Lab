

using System;

public class EventService : MonoGenericSingelton<EventService>
{
    public event Action SimulationStarted;
    public event Action SimulationStopped;
    public event Action AllValuePropagated;
    public event Action InputValueChanged;
    public event Action<IC> ShowICTT;
    public event Action<WireController> RemoveWireConnection;
    public event Action<ICModel, IC> ChangeIC;
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
    public void InvokeRemoveWireConnection(WireController wire)
    {
        RemoveWireConnection?.Invoke(wire);
    }

    public void InvokeChangeIC(ICModel model, IC ic)
    {
        ChangeIC?.Invoke(model, ic);
    }
}
