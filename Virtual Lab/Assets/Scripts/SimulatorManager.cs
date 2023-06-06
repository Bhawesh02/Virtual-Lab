
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SimulatorManager : MonoBehaviour
{
    private static SimulatorManager instance;
    public static SimulatorManager Instance { get { return instance; } }

    public bool doingConnection = false;

    public GameObject Wire;

    public GameObject WiresGameObject;

    public ICBase SelectedIcBase;

    public List<ICBase> ICBases;

    public List<WireController> Wires;


    public bool SimulationRunning;

    public TextMeshProUGUI SimulationStatus;

    public Sprite PinPostive;

    public Sprite PinNegative;
    
    public Sprite PinNull;

    public GameObject ICSelection;

    [SerializeField]
    private Button StartButton;

    [SerializeField]
    private Button StopButton;

    [SerializeField]
    private Button ResetButton;

    /*
        public List<WireController> WiresConnectionChecked;*/

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

    private void Start()
    {
        ICSelection.SetActive(false);
        StartButton.onClick.AddListener(StartSimulation);
        StopButton.onClick.AddListener(StopSimulation);
        ResetButton.onClick.AddListener(ResetConnection);
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
        Vector3 endPosition = new(mousePosition.x, mousePosition.y, mousePosition.z);
        Wire.GetComponent<WireController>().SetWireEnd(endPosition);
    }
    public void StartSimulation()
    {
        SimulationRunning = true;
        SimulationStatus.text = "Simulation Running";
        ValuePropagate.Instance.StartTransfer();
    }
    public void StopSimulation()
    {
        SimulationStatus.text = "Simulation not running";
        SimulationRunning = false;
    }

    private void ResetConnection()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
