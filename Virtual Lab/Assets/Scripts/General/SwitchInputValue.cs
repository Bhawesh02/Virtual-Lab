
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SwitchInputValue : MonoBehaviour
{
    [SerializeField]
    private PinController inputPin;

    private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
    }
    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            ChangeInputValue();
            EventService.Instance.InvokeInputValueChanged();
        });
    }

    private void ChangeInputValue()
    {
        if (inputPin.value == PinValue.Positive)
            inputPin.value = PinValue.Negative;
        else
            inputPin.value = PinValue.Positive;
    }
}
