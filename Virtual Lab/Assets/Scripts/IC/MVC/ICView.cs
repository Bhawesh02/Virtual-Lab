
using System.Collections.Generic;
using UnityEngine;

public class ICView : MonoBehaviour
{
    public List<GameObject> Pins;

    public ICBase thisIC = new();

    public PinController VccPin;
    public PinController GndPin;
    private void Start()
    {
        GetPins();
        SetThisIC();
    }

    private void SetThisIC()
    {
        thisIC.IcLogic = GetComponent<ICLogic>();
        thisIC.ICSprite = GetComponent<SpriteRenderer>();
        thisIC.Pins = Pins;
        SimulatorManager.Instance.ICBases.Add(thisIC);
    }

    private void GetPins()
    {
        Transform PinsGameObject = transform.GetChild(1);
        foreach (Transform child in PinsGameObject)
        {
            Pins.Add(child.gameObject);
        }
    }

}
