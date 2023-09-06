
using System.Collections.Generic;
using UnityEngine;
public class ICController : GateLogic
{
    public ICView View { get; }
    public ICModel Model { get; private set; }


    #region Constuctor and setter
    public ICController(ICView view)
    {
        this.View = view;
        Model = new(view.GetComponent<SpriteRenderer>(),this);
        
        SimulatorManager.Instance.ICModels.Add(Model);


    }
    public void SetPins(GameObject PinsGameObject)
    {
        for (int i = 0; i < PinsGameObject.transform.childCount; i++)
        {
            Model.Pins.Add(PinsGameObject.transform.GetChild(i).gameObject.GetComponent<PinController>());
        }
        //Model.Pins = Model.Pins;
    }
    public void SetVccPin(PinController vcc)
    {
        Model.VccPin = vcc;
    }
    public void SetGndPin(PinController gnd)
    {
        Model.GndPin = gnd;
    }

    public void SetIcData(IC idData)
    {
        Model.IcData = idData;
    }
    #endregion
    
    #region Basic Ic Logic

    public void RunIcLogic()
    {
        if (Model.IcData == null)
            return;

        int VccPinNumber = Model.IcData.VccPin - 1;
        int GndPinNumber = Model.IcData.GndPin - 1;
        GetVccAndGndPinInIC(VccPinNumber, GndPinNumber, out PinController VccPinInIc, out PinController GndPinInIc);
        if (VccPinInIc.value != PinValue.Vcc || GndPinInIc.value != PinValue.Gnd)
        {
            EventService.Instance.InvokeShowError("VCC or Gnd notConnected / WrongConnected for " +View.name);
            return;
        }
        if (Model.IcData.ICType == ICTypes.Null)
            return;
        RunLogicForEachGate();
    }
    private void GetVccAndGndPinInIC(int VccPinNumber, int GndPinNumber, out PinController VccPinInIc, out PinController GndPinInIc)
    {
        List<PinController> pins = Model.Pins;
        VccPinInIc = null;
        GndPinInIc = null;
        PinController pinController;
        for (int i = 0; i < pins.Count; i++)
        {
            pinController = pins[i];
            if (pinController.CurrentPinInfo.Type == PinType.IcVcc)
                VccPinInIc = pins[i];

            else if (pinController.CurrentPinInfo.Type == PinType.IcGnd)
                GndPinInIc = pins[i];
        }

    }

    private void RunLogicForEachGate()
    {
        foreach (PinMapping gate in Model.IcData.pinMapping)
        {
            int OutputPinIndex = gate.OutputPin - 1;
            PinController OutputPin = Model.Pins[OutputPinIndex];
            List<PinController> InputPins = new();
            bool anyInputNull = CheckEachInputOfGate(gate, InputPins);
            if (anyInputNull)
            {
                OutputPin.value = PinValue.Null;
                continue;
            }
            GenerateOutputValue(OutputPin, InputPins);

        }
    }

    private bool CheckEachInputOfGate(PinMapping gate, List<PinController> InputPins)
    {
        for(int i=0;i< gate.InputPin.Length; i++)
        {
            int InputPinIndex = gate.InputPin[i] - 1;
            PinController InputPin = Model.Pins[InputPinIndex];
            if (InputPin.value == PinValue.Null)
            {
                return true;
            }
            InputPins.Add(InputPin);
        }

        return false;
    }

   
    private void GenerateOutputValue(PinController outputPin, List<PinController> inputPins)
    {
        switch (Model.IcData.ICType)
        {
            case ICTypes.Not:
                NotGateLogic(outputPin, inputPins);
                break;
            case ICTypes.Or:
                OrGateLogic(outputPin, inputPins);
                break;
            case ICTypes.And:
                AndGateLogic(outputPin, inputPins);
                break;
            case ICTypes.Xor:
                XorGateLogic(outputPin, inputPins);
                break;
            case ICTypes.Nor:
                NorGateLogic(outputPin, inputPins);
                break;
            case ICTypes.Nand:
                NandGateLogic(outputPin, inputPins);
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
