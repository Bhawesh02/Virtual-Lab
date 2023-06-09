
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

    private bool Is14pinbeingputin16pin(int numOfPinsInSelectedIcBase,int numOfPinsInSelecetedIC)
    {
        if(numOfPinsInSelectedIcBase == 16 && numOfPinsInSelecetedIC == 14)
            return true;
        return false;
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
        bool smallIcInBigBase = Is14pinbeingputin16pin(numOfPinsInSelectedIcBase, numOfPinsInSelecetedIC);
        IcBase.ICSprite.sprite = ic.IcSprite;
        IcBase.IcLogic.enabled = true;
        IcBase.IcLogic.IcData = ic;
        SetInputAndOutputPins(smallIcInBigBase);
        SetVccAndGndPin(smallIcInBigBase);
        ICSelection.SetActive(false);
        ValuePropagate.Instance.ICLogics.Add(IcBase.IcLogic);
    }

    private void SetVccAndGndPin(bool smallIcInBigBase)
    {
        // VCC pin
        int pinNumber = ic.VccPin - 1;
        pinNumber = Skip8and9ifApplicable(smallIcInBigBase, pinNumber);
        ChangePinType(pinNumber, PinType.IcVcc);
        ValuePropagate.Instance.IcVccPin.Add(IcBase.Pins[pinNumber].GetComponent<PinController>());
        IcBase.Pins[pinNumber].AddComponent<OutputPinConnectionCheck>();

        //Gnd pin
        pinNumber = ic.GndPin - 1;
        pinNumber = Skip8and9ifApplicable(smallIcInBigBase, pinNumber);

        ChangePinType(pinNumber, PinType.IcGnd);
        ValuePropagate.Instance.IcGndPin.Add(IcBase.Pins[pinNumber].GetComponent<PinController>());
        IcBase.Pins[pinNumber].AddComponent<OutputPinConnectionCheck>();
    }

    

    private void SetInputAndOutputPins(bool smallIcInBigBase)
    {
        for (int i = 0; i < ic.inputPins.Length; i++)
        {
            //input pin
            int pinNumber = ic.inputPins[i] - 1;
            pinNumber = Skip8and9ifApplicable(smallIcInBigBase, pinNumber);

            ChangePinType(pinNumber, PinType.IcInput);
            ValuePropagate.Instance.IcInputPins.Add(IcBase.Pins[pinNumber].GetComponent<PinController>());
            IcBase.Pins[pinNumber].AddComponent<OutputPinConnectionCheck>();
        }
        for(int i = 0; i < ic.outputPins.Length; i++)
        {
            //Output pin
            int pinNumber = ic.outputPins[i] - 1;
            pinNumber = Skip8and9ifApplicable(smallIcInBigBase, pinNumber);

            ChangePinType(pinNumber, PinType.IcOutput);
            ValuePropagate.Instance.IcOutputPins.Add(IcBase.Pins[pinNumber].GetComponent<PinController>());
        }
    }
    private static int Skip8and9ifApplicable(bool smallIcInBigBase, int pinNumber)
    {
        if (smallIcInBigBase && pinNumber+1 >= 8)
        {
            pinNumber += 2;
        }

        return pinNumber;
    }
    private void ChangePinType(int PinNumber, PinType type)
    {
        PinInfo currentPinInfo = IcBase.Pins[PinNumber].GetComponent<PinController>().CurrentPinInfo;
        currentPinInfo.PinNumber = PinNumber + 1;
        currentPinInfo.Type = type;

    }
}
