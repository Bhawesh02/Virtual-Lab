
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrentStatusDisplayer : MonoBehaviour
{
    private ValuePropagateService valuePropogate;
    private List<PinController> inputPinsWithWire;
    private List<PinController> outputPinsWithWire;

    [SerializeField]
    private GameObject pinStatusTemplate;
    [SerializeField]
    private ContentSizeFitter inputPinsContent;
    [SerializeField]
    private ContentSizeFitter outputPinsContent;

    private Stack<GameObject> statuShowing;


    private void Awake()
    {
        EventService.Instance.AllValuePropagated += ShowStatus;
        inputPinsWithWire = new();
        outputPinsWithWire = new();
        statuShowing = new();
    }
    private void Start()
    {
        valuePropogate = ValuePropagateService.Instance;

    }
    private void ShowStatus()
    {
        valuePropogate ??= ValuePropagateService.Instance;
        DestroyExsistingStatus();
        GetInputAndOuptPinsWithWires();
        ShowPinsStatusOfInputAndOutput();
    }

    private void DestroyExsistingStatus()
    {
        GameObject pinStatus;
        while(statuShowing.Count > 0)
        {
            pinStatus = statuShowing.Pop();
            Destroy(pinStatus);
        }
    }

    private void GetInputAndOuptPinsWithWires()
    {
        inputPinsWithWire.Clear();
        outputPinsWithWire.Clear();
        for (int i = 0; i < valuePropogate.InputPins.Count; i++)
        {
            if (valuePropogate.InputPins[i].Wires.Count > 0)
            {
                inputPinsWithWire.Add(valuePropogate.InputPins[i]);
            }
        }
        for (int i = 0; i < valuePropogate.OutputPins.Count; i++)
        {
            if (valuePropogate.OutputPins[i].Wires.Count > 0)
            {
                outputPinsWithWire.Add(valuePropogate.OutputPins[i]);
            }
        }
    }
    private void ShowPinsStatusOfInputAndOutput()
    {
        ShowPinStatus(inputPinsWithWire,inputPinsContent);
        ShowPinStatus(outputPinsWithWire,outputPinsContent);
    }

    private void ShowPinStatus(List<PinController> pins,ContentSizeFitter content)
    {
        GameObject pinStatus;
        TextMeshProUGUI index;
        TextMeshProUGUI status;
        for (int i = 0; i < pins.Count; i++)
        {
            pinStatus = Instantiate(pinStatusTemplate, content.transform);
            index = pinStatus.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            status = pinStatus.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            index.text = i.ToString();
            if (pins[i].value == PinValue.Positive || pins[i].value == PinValue.Vcc)
                status.text = "1";
            else if (pins[i].value == PinValue.Negative || pins[i].value == PinValue.Gnd)
                status.text = "0";
            statuShowing.Push(pinStatus);
        }
    }
}
