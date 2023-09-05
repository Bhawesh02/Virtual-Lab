
using System.Collections.Generic;
using UnityEngine;

public class ICLogic : MonoBehaviour
{
    public IC IcData;
    [SerializeField]
    private ICView ICView;

    private GateLogic gateLogic = GateLogic.Instance;
    private void Awake()
    {
        enabled = false;
    }
    


    #region Basic Ic Logic

    public void RunIcLogic()
    {
        if (IcData == null)
            return;

        int VccPinNumber = IcData.VccPin - 1;
        int GndPinNumber = IcData.GndPin - 1;
        GetVccAndGndPinInIC(VccPinNumber, GndPinNumber, out PinController VccPinInIc, out PinController GndPinInIc);
        if (VccPinInIc.value != PinValue.Vcc || GndPinInIc.value != PinValue.Gnd)
        {
            Debug.Log("VCC or Gnd notConnected / WrongConnected");
            return;
        }
        if (IcData.ICType == ICTypes.Null)
            return;
        RunLogicForEachGate();
    }

    private void RunLogicForEachGate()
    {
        foreach (PinMapping gate in IcData.pinMapping)
        {
            int OutputPinIndex = gate.OutputPin - 1;
            PinController OutputPin = GetComponent<ICView>().Controller.Model.Pins[OutputPinIndex];
            List<PinController> InputPins = new();
            bool anyInputNull = false;
            anyInputNull = CheckEachInputOfGate(gate, InputPins, anyInputNull);
            if (anyInputNull)
            {
                OutputPin.value = PinValue.Null;
                continue;
            }
            GenerateOutputValue(OutputPin, InputPins);

        }
    }

    private bool CheckEachInputOfGate(PinMapping gate, List<PinController> InputPins, bool anyInputNull)
    {
        foreach (int inputPinNumber in gate.InputPin)
        {
            int InputPinIndex = inputPinNumber - 1;
            PinController InputPin = GetComponent<ICView>().Controller.Model.Pins[InputPinIndex];
            if (InputPin.value == PinValue.Null)
            {
                anyInputNull = true;
                break;
            }
            InputPins.Add(InputPin);

        }

        return anyInputNull;
    }

    private void GetVccAndGndPinInIC(int VccPinNumber, int GndPinNumber, out PinController VccPinInIc, out PinController GndPinInIc)
    {
        List<PinController> pins = ICView.Controller.Model.Pins;
        VccPinInIc = null;
        GndPinInIc = null;
        PinController pinController;
        for (int i = 0;i< pins.Count; i++)
        {
            pinController = pins[i];
            if (pinController.CurrentPinInfo.Type == PinType.IcVcc)
                VccPinInIc = pins[i];

            else if (pinController.CurrentPinInfo.Type == PinType.IcGnd)
                GndPinInIc = pins[i];
        }

    }

    private void GenerateOutputValue(PinController outputPin, List<PinController> inputPins)
    {
        switch (IcData.ICType)
        {
            case ICTypes.Not:
                gateLogic.NotGateLogic(outputPin, inputPins);
                break;
            case ICTypes.Or:
                gateLogic.OrGateLogic(outputPin, inputPins);
                break;
            case ICTypes.And:
                gateLogic.AndGateLogic(outputPin, inputPins);
                break;
            case ICTypes.Xor:
                gateLogic.XorGateLogic(outputPin, inputPins);
                break;
            case ICTypes.Nor:
                gateLogic.NorGateLogic(outputPin, inputPins);
                break;
            case ICTypes.Nand:
                gateLogic.NandGateLogic(outputPin,inputPins);
                break;
            default:
                Debug.Log("IC Logic Not given");
                break;
        }
        if (outputPin.value != PinValue.Null)
            ValuePropagateService.Instance.TransferData(outputPin);

    }

    #endregion

    
}
