
using System.Collections.Generic;
using Unity.VisualScripting;

public class GateLogic
{
    protected void NandGateLogic(PinController outputPin, List<PinController> inputPins)
    {
        PinValue input1Value = inputPins[0].value;
        PinValue input2Value = inputPins[1].value;
        
        if (input1Value == PinValue.Negative || input2Value == PinValue.Negative)
        {
            outputPin.value = PinValue.Positive;
            return;
        }
        outputPin.value = PinValue.Negative;
    }
    protected void NorGateLogic(PinController outputPin, List<PinController> inputPins)
    {
        PinValue input1Value = inputPins[0].value;
        PinValue input2Value = inputPins[1].value;
        
        if (input1Value == PinValue.Negative && input2Value == PinValue.Negative)
        {
            outputPin.value = PinValue.Positive;
            return;
        }
        outputPin.value = PinValue.Negative;
    }

    protected void AndGateLogic(PinController outputPin, List<PinController> inputPins)
    {
        PinValue input1Value = inputPins[0].value;
        PinValue input2Value = inputPins[1].value;
        
        if (input1Value == PinValue.Negative || input2Value == PinValue.Negative)
        {
            outputPin.value = PinValue.Negative;
            return;
        }
        outputPin.value = PinValue.Positive;
    }

    protected void OrGateLogic(PinController outputPin, List<PinController> inputPins)
    {
        PinValue input1Value = inputPins[0].value;
        PinValue input2Value = inputPins[1].value;
        
        if (input1Value == PinValue.Positive || input2Value == PinValue.Positive)
        {
            outputPin.value = PinValue.Positive;
            return;
        }
        outputPin.value = PinValue.Negative;

    }

    protected void NotGateLogic(PinController outputPin, List<PinController> inputPins)
    {

        if (inputPins[0].value == PinValue.Negative)
            outputPin.value = PinValue.Positive;
        else
            outputPin.value = PinValue.Negative;
    }
    protected void XorGateLogic(PinController outputPin, List<PinController> inputPins)
    {
        PinValue inputValue1 = inputPins[0].value;
        PinValue inputValue2 = inputPins[1].value;
        
        if (inputValue1 != inputValue2)
        {
            outputPin.value = PinValue.Positive;
        }
        else
        {
            outputPin.value = PinValue.Negative;
        }
    }

    protected void ThreeInputNandLogic(PinController outputPin, List<PinController> inputPins)
    {
        PinValue inputValue1 = inputPins[0].value;
        PinValue inputValue2 = inputPins[1].value;
        PinValue inputValue3 = inputPins[2].value;
        
        if(inputValue1 == PinValue.Negative||inputValue2==PinValue.Negative||inputValue3 == PinValue.Negative)
            outputPin.value=PinValue.Positive;
        else
        outputPin.value = PinValue.Negative;
    }
}
