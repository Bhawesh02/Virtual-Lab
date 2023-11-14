
using System.Collections;
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
        highPin.value = PinValue.Negative;
        lowPin.value = PinValue.Positive;
    }

    private void TransferMonoData()
    {
        if (!SimulatorManager.Instance.SimulationRunning)
            return;
        highPin.value = PinValue.Positive;
        lowPin.value = PinValue.Negative;
        EventService.Instance.InvokeInputValueChanged();
        StartCoroutine(ChangeToNull());
    }

    private IEnumerator ChangeToNull()
    {
        yield return new WaitForSeconds(changeToNullDelay);
        highPin.value = PinValue.Negative;
        lowPin.value = PinValue.Positive;
    }
}
