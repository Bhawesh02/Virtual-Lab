

using System;

public class EventService : MonoGenericSingelton<EventService>
{
    public event Action SimulationStarted;
    public event Action SimulationStopped;
    public event Action AllValuePropagated;
    public event Action InputValueChanged;
    public event Action<IcData> ShowICTT;
    public event Action<WireController> RemoveWireConnection;
    public event Action<ICView, IcData> ChangeIC;
    public event Action<PinController> OutputPinValueChange;
    public event Action<String> ShowError;
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
    public void InvokeShowICTT(IcData icData)
    {
        ShowICTT?.Invoke(icData);
    }
    public void InvokeRemoveWireConnection(WireController wire)
    {
        RemoveWireConnection?.Invoke(wire);
    }

    public void InvokeChangeIC(ICView view, IcData icData)
    {
        ChangeIC?.Invoke(view, icData);
    }

    public void InvokeShowError(String message)
    {
        ShowError?.Invoke(message);
    }
    public void InvokeOutputPinValueChange(PinController pin)
    {
        OutputPinValueChange?.Invoke(pin);
    }
}
