using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICController : MonoBehaviour
{
    public List<GameObject> Pins;

    public ICBase thisIC = new();

    private void Start()
    {
        GetPins();
        SendToSimulationManager();
    }

    private void SendToSimulationManager()
    {
        thisIC.IcLogic = GetComponent<ICLogic>();
        thisIC.ICSprite = GetComponent<SpriteRenderer>();
        thisIC.Pins = Pins;
        SimulatorManager.Instance.ICBases.Add(thisIC);
    }

    private void GetPins()
    {
        Transform PinsGameObject = transform.GetChild(0);
        foreach (Transform child in PinsGameObject)
        {
            Pins.Add(child.gameObject);
        }
    }

    private void OnMouseDown()
    {
        if (SimulatorManager.Instance.SimulationRunning)
            return;
        SimulatorManager.Instance.SelectedIcBase = thisIC;
        SimulatorManager.Instance.ICSelection.SetActive(true);

    }
}
