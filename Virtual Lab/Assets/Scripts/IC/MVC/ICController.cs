using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICController
{
    private MessageBubbleController messageBubble;
    private Vector2 messageBubbleOffset = new(0f, 0.5f);
    private IcState currentIcState;
    private IcState basicGateIcState;
    private IcState muxIcState;
    private IcState flipFlopIcState;
    private IcState asynConterIcState;
    private bool isSmallIcInBigbase;
    private bool haslogicRanTwice = false;
    private Coroutine runLogicAgainCoroutine;

    public ICView View { get; }
    public ICModel Model { get; }

    #region Constuctor and setter

    public ICController(ICView view)
    {
        View = view;
        Model = new(view.IcSprite, this);
        SimulatorManager.Instance.ICModels.Add(Model);
        basicGateIcState = new BasicGateIcState(this);
        muxIcState = new MuxIcState(this);
        flipFlopIcState = new FlipFlopIcState(this);
        asynConterIcState = new AsynchronousCounterIcState(this);
        EventService.Instance.ChangeIC += ChangeIc;
    }

    ~ICController()
    {
        EventService.Instance.ChangeIC -= ChangeIc;
    }

    public void SetPins(GameObject pinsGameObject)
    {
        for (int i = 0; i < pinsGameObject.transform.childCount; i++)
        {
            Model.Pins.Add(pinsGameObject.transform.GetChild(i).gameObject.GetComponent<PinController>());
        }
    }

    private void SetVccPin(PinController vcc)
    {
        Model.VccPin = vcc;
    }

    private void SetGndPin(PinController gnd)
    {
        Model.GndPin = gnd;
    }

    private void SetIcData(IcData icData)
    {
        Model.IcData = icData;
        if (!icData)
        {
            return;
        }
        switch (icData.ICType)
        {
            case ICTypes.NULL:
                break;
            case ICTypes.BASIC_GATES:
                currentIcState = basicGateIcState;
                break;
            case ICTypes.MUX:
                currentIcState = muxIcState;
                break;
            case ICTypes.FLIP_FLOP:
                currentIcState = flipFlopIcState;
                break;
            case ICTypes.ASYNC_COUNTERS:
                currentIcState = asynConterIcState;
                break;
        }

        currentIcState.SetData();
    }

    #endregion

    #region change IC

    private void ChangeIc(ICView icView, IcData data)
    {
        if (icView != View)
        {
            return;
        }

        if (Model.IcData != null)
        {
            RemoveWiresConnectedToIcBase();
        }
        SetIcData(data);
        if (data != null)
        {
            int numOfPinsInSelectedIcBase = Model.Pins.Count;
            int numOfPinsInSelecetedIC = data.NoOfPins;
            if (numOfPinsInSelectedIcBase < numOfPinsInSelecetedIC)
                return;
            isSmallIcInBigbase = Is14pinbeingputin16pin(numOfPinsInSelectedIcBase, numOfPinsInSelecetedIC);
            Model.ICSprite.sprite = data.IcSprite;
            SetVccAndGndPin(data);
            currentIcState.SetPins();
        }
        else
        {
            Model.ICSprite.sprite = null;
        }

        ValuePropagateService.Instance.ICViews.Remove(View);
        if (data != null)
            ValuePropagateService.Instance.ICViews.Add(View);
    }

    private void RemoveWiresConnectedToIcBase()
    {
        for (int i = 0; i < Model.Pins.Count; i++)
        {
            if (Model.Pins[i].Wires.Count == 0)
                continue;
            for (int j = 0; j < Model.Pins[i].Wires.Count; j++)
                EventService.Instance.InvokeRemoveWireConnection(Model.Pins[i].Wires[j]);
        }
    }

    private bool Is14pinbeingputin16pin(int numOfPinsInSelectedIcBase, int numOfPinsInSelecetedIC)
    {
        return numOfPinsInSelectedIcBase == 16 && numOfPinsInSelecetedIC == 14;
    }


    private void SetVccAndGndPin(IcData data)
    {
        // VCC pin
        int pinNumber = data.VccPin - 1;
        pinNumber = Skip8and9ifApplicable(pinNumber);
        ChangePinType(pinNumber, PinType.IcVcc);
        ValuePropagateService.Instance.IcVccPin.Add(Model.Pins[pinNumber].GetComponent<PinController>());
        Model.Pins[pinNumber].gameObject.AddComponent<OutputPinConnectionCheck>();
        Model.Controller.SetVccPin(Model.Pins[pinNumber].GetComponent<PinController>());
        //Gnd pin
        pinNumber = data.GndPin - 1;
        pinNumber = Skip8and9ifApplicable(pinNumber);
        ChangePinType(pinNumber, PinType.IcGnd);
        ValuePropagateService.Instance.IcGndPin.Add(Model.Pins[pinNumber].GetComponent<PinController>());
        Model.Pins[pinNumber].gameObject.AddComponent<OutputPinConnectionCheck>();
        Model.Controller.SetGndPin(Model.Pins[pinNumber].GetComponent<PinController>());
    }

    public int Skip8and9ifApplicable(int pinNumber)
    {
        if (isSmallIcInBigbase && pinNumber + 1 >= 8)
        {
            pinNumber += 2;
        }

        return pinNumber;
    }

    public void ChangePinType(int pinNumber, PinType type)
    {
        PinInfo currentPinInfo = Model.Pins[pinNumber].CurrentPinInfo;
        currentPinInfo.PinNumber = pinNumber + 1;
        currentPinInfo.Type = type;
    }

    #endregion

    #region Ic Logic

    public void RunIcLogic()
    {
        if (!Model.IcData)
        {
            return;
        }
        int vccPinNumber = Model.IcData.VccPin - 1;
        int gndPinNumber = Model.IcData.GndPin - 1;
        GetVccAndGndPinInIC(vccPinNumber, gndPinNumber, out PinController VccPinInIc, out PinController GndPinInIc);
        if (VccPinInIc.value != PinValue.Vcc || GndPinInIc.value != PinValue.Gnd)
        {
            EventService.Instance.InvokeShowError("VCC or Gnd notConnected / WrongConnected for " + View.name);
            return;
        }

        if (Model.IcData.ICType == ICTypes.NULL)
            return;
        if (runLogicAgainCoroutine != null)
        {
            View.StopCoroutine(runLogicAgainCoroutine);
        }
        haslogicRanTwice = false;
        currentIcState.RunLogic();
        runLogicAgainCoroutine = View.StartCoroutine(RungLogicAgain());
    }

    private IEnumerator RungLogicAgain()
    {
        if (haslogicRanTwice)
        {
            yield break;
        }
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        yield return waitForEndOfFrame;
        currentIcState.RunLogic();
        TransferOutputPinValue();
        haslogicRanTwice = true;
    }
    public void TransferOutputPinValue()
    {
        if (Model.IcData == null)
            return;
        currentIcState.PropagateOutputPinValues();
    }
    private void GetVccAndGndPinInIC(int VccPinNumber, int GndPinNumber, out PinController VccPinInIc,
        out PinController GndPinInIc)
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

    #endregion

    public void SetAsInputPin(int inputPin)
    {
        int inputPinNumber = inputPin - 1;
        inputPinNumber = Skip8and9ifApplicable(inputPinNumber);
        ChangePinType(inputPinNumber, PinType.IcInput);
        PinController inputPinController = Model.Pins[inputPinNumber];
        inputPinController.value = PinValue.Negative;
        ValuePropagateService.Instance.IcInputPins.Add(inputPinController);
        Model.Pins[inputPinNumber].gameObject.AddComponent<OutputPinConnectionCheck>();
    }

    public void SetAsOutputPin(int outputPin)
    {
        int outputPinNumber = outputPin - 1;
        outputPinNumber = Skip8and9ifApplicable(outputPinNumber);
        ChangePinType(outputPinNumber, PinType.IcOutput);
        PinController outputPinController = Model.Pins[outputPinNumber];
        outputPinController.value = PinValue.Negative;
        ValuePropagateService.Instance.IcOutputPins.Add(outputPinController);
        EventService.Instance.InvokeOutputPinValueChange(outputPinController);
        ValuePropagateService.Instance.TransferData(outputPinController);
    }

    #region Message Bubble

    public void ShowMessage()
    {
        if (WireService.Instance.doingConnection || Model.IcData == null)
            return;
        messageBubble = MessageBubblePoolService.Instance.GetBubble();
        messageBubble.transform.SetParent(View.transform);
        messageBubble.SetMessage("LeftClick: ShowTT , RightClick: Remove");
        messageBubble.transform.position = (Vector2)View.transform.position + messageBubbleOffset;
    }

    public void RemoveMessage()
    {
        if (messageBubble != null)
            MessageBubblePoolService.Instance.ReturnBubble(messageBubble);
    }

    #endregion
}