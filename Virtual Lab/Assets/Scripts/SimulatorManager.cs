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

    public Sprite PinPostive;

    public Sprite PinNegative;
    
    public Sprite PinNull;

    public List<WireController> WiresConnectionChecked;

    /*public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;*/

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
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
            SetWireEndToMousePointer();
            if (Input.GetMouseButtonDown(1))
            { 
                Destroy(Wire);
                Wire = null;
                doingConnection = false;
            }
        }



    }

    private void SetWireEndToMousePointer()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 endPosition = new Vector3(mousePosition.x, mousePosition.y, mousePosition.z);
        Wire.GetComponent<WireController>().SetWireEnd(endPosition);
    }

    public void stopSimulation()
    {
        SimulationStatus.text = "Simulation not running";
        SimulationRunning = false;
    }

}
