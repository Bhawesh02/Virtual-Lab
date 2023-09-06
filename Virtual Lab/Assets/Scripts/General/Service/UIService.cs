
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIService : MonoGenericSingelton<UIService>
{
    [SerializeField]
    private TextMeshProUGUI SimulationStatus;


    [SerializeField]
    private Button startButton;

    [SerializeField]
    private Button stopButton;

    [SerializeField]
    private Button resetButton;
    [SerializeField]
    private Button backButton;
    [SerializeField]
    private Button undoButton;
    [SerializeField]
    private CurrentStatusDisplayer currentStatusDisplayer;


    [SerializeField]
    private Image TruthTable;
    [Header("Error Popup")]
    [SerializeField]
    private GameObject errorPopupGameObject;
    [SerializeField]
    private TextMeshProUGUI errorPopupMesssage;
    [SerializeField]
    private Button errorPopupOkButton;

    protected override void Awake()
    {
        base.Awake();
        SimulationStatus.text = "Simulation not running";
        errorPopupGameObject.SetActive(false);
    }

    private void Start()
    {
        startButton.onClick.AddListener(StartSimulation);
        stopButton.onClick.AddListener(StopSimulation);
        resetButton.onClick.AddListener(ResetConnection);
        undoButton.onClick.AddListener(UndoLastConnection);
        backButton.onClick.AddListener(() =>
          {
              backButton.transform.parent.gameObject.SetActive(false);
          });
        errorPopupOkButton.onClick.AddListener(() =>
        {
            EventService.Instance.InvokeSimulationStopped();
            errorPopupGameObject.SetActive(false);
        });
        EventService.Instance.ShowICTT += ShowTruthTable;

    }

    public void StartSimulation()
    {
        SimulationStatus.text = "Simulation Running";
        ICSpawnerService.Instance.gameObject.SetActive(false);
        currentStatusDisplayer.gameObject.SetActive(true);
        EventService.Instance.InvokeSimulationStarted();

    }
    public void StopSimulation()
    {
        SimulationStatus.text = "Simulation not running";
        ICSpawnerService.Instance.gameObject.SetActive(true);
        currentStatusDisplayer.gameObject.SetActive(false);
        EventService.Instance.InvokeSimulationStopped();
    }

    private void ResetConnection()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    private void UndoLastConnection()
    {
        if (SimulatorManager.Instance.SimulationRunning || SimulatorManager.Instance.WiresInSystem.Count == 0)
            return;
        WireController lastWire = SimulatorManager.Instance.WiresInSystem[^1];
        EventService.Instance.InvokeRemoveWireConnection(lastWire);
    }

    

    public void ShowTruthTable(IC icData)
    {
        if (SimulatorManager.Instance.SimulationRunning || WireService.Instance.doingConnection)
            return;
        TruthTable.sprite = icData.TruthTable;
        TruthTable.SetNativeSize();
        TruthTable.transform.parent.gameObject.SetActive(true);
    }
    private void OnDestroy()
    {
        EventService.Instance.ShowICTT -= ShowTruthTable;
    }
}
