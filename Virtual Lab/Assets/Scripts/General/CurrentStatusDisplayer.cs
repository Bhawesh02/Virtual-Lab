
using System.Collections;
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
    private ContentSizeFitter inputPinsContent;
    [SerializeField]
    private ContentSizeFitter outputPinsContent;

    private Stack<RectTransform> statuShowing;


    private void Awake()
    {
        inputPinsWithWire = new();
        outputPinsWithWire = new();
        statuShowing = new();
    }

    private void Start()
    {
        valuePropogate = ValuePropagateService.Instance;
        EventService.Instance.SimulationStopped += RemoveExsistingStatus;
        EventService.Instance.AllValuePropagated += ShowStatus;
        gameObject.SetActive(false);

    }
   
    private void ShowStatus()
    {
        valuePropogate ??= ValuePropagateService.Instance;
        RemoveExsistingStatus();
        GetInputAndOuptPinsWithWires();
        ShowPinsStatusOfInputAndOutput();
    }

    private void RemoveExsistingStatus()
    {
        RectTransform pinStatus;
        while (statuShowing.Count > 0)
        {
            pinStatus = statuShowing.Pop();
            StatusTemplatePoolService.Instance.ReturnTemplate(pinStatus);
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
                valuePropogate.InputPins[i].SetPinIndex(inputPinsWithWire.Count - 1);

            }
        }
        for (int i = 0; i < valuePropogate.OutputPins.Count; i++)
        {
            if (valuePropogate.OutputPins[i].Wires.Count > 0)
            {
                outputPinsWithWire.Add(valuePropogate.OutputPins[i]);
                valuePropogate.OutputPins[i].SetPinIndex(outputPinsWithWire.Count - 1);

            }
        }
    }
    private void ShowPinsStatusOfInputAndOutput()
    {
        ShowPinStatus(inputPinsWithWire, inputPinsContent);
        ShowPinStatus(outputPinsWithWire, outputPinsContent);
    }

    private void ShowPinStatus(List<PinController> pins, ContentSizeFitter content)
    {
        RectTransform pinStatus;
        TextMeshProUGUI index;
        TextMeshProUGUI status;
        for (int i = 0; i < pins.Count; i++)
        {
            pinStatus = StatusTemplatePoolService.Instance.GetTemplate();
            pinStatus.transform.SetParent(content.transform);
            index = pinStatus.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            status = pinStatus.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            index.text = i.ToString();
            if (pins[i].value == PinValue.Positive || pins[i].value == PinValue.Vcc)
                status.text = "1";
            else if (pins[i].value == PinValue.Negative || pins[i].value == PinValue.Gnd)
                status.text = "0";
            else
                status.text = "Null";
            statuShowing.Push(pinStatus);
        }
    }

    private void OnDestroy()
    {
        EventService.Instance.AllValuePropagated -= ShowStatus;
        EventService.Instance.SimulationStopped -= RemoveExsistingStatus;
    }
}
