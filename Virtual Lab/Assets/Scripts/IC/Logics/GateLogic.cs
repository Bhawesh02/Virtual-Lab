
using System.Collections.Generic;
using Unity.VisualScripting;

public static class GateLogic
{
    public static void NandGateLogic(PinController outputPin, List<PinController> inputPins)
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
    public static void NorGateLogic(PinController outputPin, List<PinController> inputPins)
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

    public static void AndGateLogic(PinController outputPin, List<PinController> inputPins)
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

    public static void OrGateLogic(PinController outputPin, List<PinController> inputPins)
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

    public static void NotGateLogic(PinController outputPin, List<PinController> inputPins)
    {

        if (inputPins[0].value == PinValue.Negative)
            outputPin.value = PinValue.Positive;
        else
            outputPin.value = PinValue.Negative;
    }
    public static void XorGateLogic(PinController outputPin, List<PinController> inputPins)
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

    public static void ThreeInputNandLogic(PinController outputPin, List<PinController> inputPins)
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
