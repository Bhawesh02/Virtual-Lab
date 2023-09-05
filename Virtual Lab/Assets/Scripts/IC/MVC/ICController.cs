
using UnityEngine;
public class ICController 
{
    private ICView view;
    public ICModel Model { get; private set; }
    public ICController(ICView view,ICLogic iCLogic, GameObject PinsGameObject)
    {
        this.view = view;
        SetPins(PinsGameObject);
        ICBase thisIC = new()
        {
            IcLogic = iCLogic,
            ICSprite = view.GetComponent<SpriteRenderer>(),
            Pins = view.Pins
        };
        Model = new(thisIC);
        SimulatorManager.Instance.ICBases.Add(Model.thisIC);


    }
    private void SetPins(GameObject PinsGameObject)
    {
        for (int i = 0; i < PinsGameObject.transform.childCount; i++)
        {
            view.Pins.Add(PinsGameObject.transform.GetChild(i).gameObject.GetComponent<PinController>());
        }
    }
    public void SetVccPin(PinController vcc)
    {
        Model.VccPin = vcc;
    }
    public void SetGndPin(PinController gnd)
    {
        Model.GndPin = gnd;
    }
    
}
