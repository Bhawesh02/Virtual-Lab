
using UnityEngine;
public class ICController 
{
    private ICView view;
    public ICModel Model { get; private set; }

    #region Constuctor and setter
    public ICController(ICView view,ICLogic iCLogic)
    {
        this.view = view;
        Model = new(iCLogic,view.GetComponent<SpriteRenderer>());
        
        SimulatorManager.Instance.ICModels.Add(Model);


    }
    public void SetPins(GameObject PinsGameObject)
    {
        for (int i = 0; i < PinsGameObject.transform.childCount; i++)
        {
            Model.Pins.Add(PinsGameObject.transform.GetChild(i).gameObject.GetComponent<PinController>());
        }
        //Model.Pins = Model.Pins;
    }
    public void SetVccPin(PinController vcc)
    {
        Model.VccPin = vcc;
    }
    public void SetGndPin(PinController gnd)
    {
        Model.GndPin = gnd;
    }
    #endregion
}
