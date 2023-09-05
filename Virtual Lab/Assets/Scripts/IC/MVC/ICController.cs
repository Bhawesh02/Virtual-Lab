
using UnityEngine;
public class ICController 
{
    private ICView view;
    public ICBase thisIC{ get;private set; }

    public PinController VccPin { get; private set; }
    public PinController GndPin { get; private set; }
    public ICController(ICView view,ICLogic iCLogic, GameObject PinsGameObject)
    {
        this.view = view;
        SetPins(PinsGameObject);
        thisIC = new()
        {
            IcLogic = iCLogic,
            ICSprite = view.GetComponent<SpriteRenderer>(),
            Pins = view.Pins
        };
        SimulatorManager.Instance.ICBases.Add(thisIC);


    }
    private void SetPins(GameObject PinsGameObject)
    {
        for (int i = 0; i < PinsGameObject.transform.childCount; i++)
        {
            view.Pins.Add(PinsGameObject.transform.GetChild(i).gameObject);
        }
    }
    public void SetVccPin(PinController vcc)
    {
        VccPin = vcc;
    }
    public void SetGndPin(PinController gnd)
    {
        GndPin = gnd;

    }
    
}
