
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIService : MonoGenericSingelton<UIService>
{
    [SerializeField]
    private TextMeshProUGUI SimulationStatus;


    [SerializeField]
    private Button StartButton;

    [SerializeField]
    private Button StopButton;

    [SerializeField]
    private Button ResetButton;
    [SerializeField]
    private Button BackButton;

    [SerializeField]
    private CurrentStatusDisplayer currentStatusDisplayer;


    [SerializeField]
    private Image TruthTable;


    protected override void Awake()
    {
        base.Awake();
        SimulationStatus.text = "Simulation not running";
    }

    private void Start()
    {
        StartButton.onClick.AddListener(StartSimulation);
        StopButton.onClick.AddListener(StopSimulation);
        ResetButton.onClick.AddListener(ResetConnection);
        BackButton.onClick.AddListener(() =>
          {
              BackButton.transform.parent.gameObject.SetActive(false);
          });

        EventService.Instance.RightClickOnIC += ShowTruthTable;
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


    public void ShowTruthTable(IC icData)
    {
        TruthTable.sprite = icData.TruthTable;
        TruthTable.SetNativeSize();
        TruthTable.transform.parent.gameObject.SetActive(true);
    }
}
