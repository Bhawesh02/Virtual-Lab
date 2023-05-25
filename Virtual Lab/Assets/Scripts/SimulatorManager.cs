using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SimulatorManager : MonoBehaviour
{
    private static SimulatorManager instance;
    public static SimulatorManager Instance { get { return instance; } }

    public bool doingConnection = false;

    public GameObject Wire;

    public GameObject WiresGameObject;

    public ICBase IcBase;

    public List<WireController> Wires;

    public ValuePropagate valuePropagate;

    public bool SimulationRunning;

    public TextMeshProUGUI SimulationStatus;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
        SimulationRunning = false;
        SimulationStatus.text = "Simulation not running";
    }
    private void Update()
    {
        if (doingConnection)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 endPosition = new Vector3(mousePosition.x, mousePosition.y,mousePosition.z);
            Wire.GetComponent<WireController>().SetWireEnd(endPosition);
        }



    }
    public void stopSimulation()
    {
        SimulationStatus.text = "Simulation not running";
        SimulationRunning = false;
    }

}
