
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ClockController : MonoBehaviour
{
    [SerializeField]
    private PinController highPin;
    [SerializeField]
    private PinController lowPin;
    [SerializeField]
    private Button monoShotButton;

    private float changeToNullDelay = 0.05f;
    private void Start()
    {
        monoShotButton.onClick.AddListener(TransferMonoData);
    }

    private void TransferMonoData()
    {
        if (!SimulatorManager.Instance.SimulationRunning)
            return;
        highPin.value = PinValue.Positive;
        lowPin.value = PinValue.Negative;
        EventService.Instance.InvokeInputValueChanged();
        EventService.Instance.InvokeClocHasBeenSet(true);
        StartCoroutine(ChangeToNull());
    }

    private IEnumerator ChangeToNull()
    {
        yield return new WaitForEndOfFrame();
        highPin.value = PinValue.Negative;
        lowPin.value = PinValue.Positive;
        EventService.Instance.InvokeInputValueChanged();
        EventService.Instance.InvokeClocHasBeenSet(false);

    }
}
