
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ICChange : MonoBehaviour
{
    [SerializeField]
    private IC ic;

    private Button changeButton;
    [SerializeField]
    private GameObject ICSelection;

    private ICBase IcBase;

    private void Awake()
    {
        changeButton = GetComponent<Button>();
    }
    private void Start()
    {
        changeButton.onClick.AddListener(ChangeIc);
    }

    private void ChangeIc()
    {
        IcBase = SimulatorManager.Instance.SelectedIcBase;
        int numOfPinsInSelectedIcBase = IcBase.Pins.Count;
        int numOfPinsInSelecetedIC = ic.inputPins.Length+ic.outputPins.Length+2;
        if (numOfPinsInSelectedIcBase < numOfPinsInSelecetedIC )
            return;

        if (IcBase.IcLogic != null)
        {
            ValuePropagate.Instance.ICLogics.Remove(IcBase.IcLogic);
        }
        IcBase.ICSprite.sprite = ic.IcSprite;
        IcBase.IcLogic.enabled = true;
        IcBase.IcLogic.IcData = ic;
        SetInputAndOutputPins();
        SetVccAndGndPin();
        ICSelection.SetActive(false);
        ValuePropagate.Instance.ICLogics.Add(IcBase.IcLogic);
    }

    private void SetVccAndGndPin()
    {
        // VCC pin
        int pinNumber = ic.VccPin - 1;
        ChangePinType(pinNumber, PinType.IcVcc);
        ValuePropagate.Instance.IcVccPin.Add(IcBase.Pins[pinNumber].GetComponent<PinController>());
        IcBase.Pins[pinNumber].AddComponent<OutputPinConnectionCheck>();

        //Gnd pin
        pinNumber = ic.GndPin - 1;
        ChangePinType(pinNumber, PinType.IcGnd);
        ValuePropagate.Instance.IcGndPin.Add(IcBase.Pins[pinNumber].GetComponent<PinController>());
        IcBase.Pins[pinNumber].AddComponent<OutputPinConnectionCheck>();
    }

    private void SetInputAndOutputPins()
    {
        for (int i = 0; i < ic.inputPins.Length; i++)
        {
            //input pin
            int pinNumber = ic.inputPins[i] - 1;
            ChangePinType(pinNumber, PinType.IcInput);
            ValuePropagate.Instance.IcInputPins.Add(IcBase.Pins[pinNumber].GetComponent<PinController>());
            IcBase.Pins[pinNumber].AddComponent<OutputPinConnectionCheck>();
        }
        for(int i = 0; i < ic.outputPins.Length; i++)
        {
            //Output pin
            int pinNumber = ic.outputPins[i] - 1;
            ChangePinType(pinNumber, PinType.IcOutput);
            ValuePropagate.Instance.IcOutputPins.Add(IcBase.Pins[pinNumber].GetComponent<PinController>());
        }
    }

    private void ChangePinType(int PinNumber, PinType type)
    {
        PinInfo currentPinInfo = IcBase.Pins[PinNumber].GetComponent<PinController>().CurrentPinInfo;
        currentPinInfo.PinNumber = PinNumber + 1;
        currentPinInfo.Type = type;

    }
}
