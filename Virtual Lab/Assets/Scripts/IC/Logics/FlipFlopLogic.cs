using UnityEngine;

public static class FlipFlopLogic
{
    private static PinValue GetComplimentValue(PinValue pinValue)
    {
        return pinValue switch
        {
            PinValue.Negative => PinValue.Positive,
            PinValue.Positive => PinValue.Negative,
            _ => PinValue.Null
        };
    }

    public static void JKLogic(FlipFlopData icData, ICController icController)
    {
        PinController outputPin;
        PinController outputComplimentPin;
        PinController jPin;
        PinController kPin;
        PinController presetPin;
        PinController clearPin;
        PinController clockPin;
        foreach (FlipFlopPinMapping flipFlopPinMapping in icData.FlipFlopPinMappings)
        {
            outputPin = icController.Model.Pins[flipFlopPinMapping.OutputPins[0] - 1];
            outputComplimentPin = icController.Model.Pins[flipFlopPinMapping.OutputPins[1]- 1];
            jPin = icController.Model.Pins[flipFlopPinMapping.InputPins[0]- 1];
            kPin = icController.Model.Pins[flipFlopPinMapping.InputPins[1]- 1];
            presetPin = icController.Model.Pins[flipFlopPinMapping.PresetPin- 1];
            clearPin = icController.Model.Pins[flipFlopPinMapping.ClearPin- 1];
            clockPin = icController.Model.Pins[flipFlopPinMapping.ClockPin- 1];
            if (outputPin.value == PinValue.Null)
            {
                outputPin.value = PinValue.Negative;
            }
            outputComplimentPin.value = GetComplimentValue(outputPin.value);
            if (presetPin.value != clearPin.value)
            {
                outputPin.value = clearPin.value;
                outputComplimentPin.value = GetComplimentValue(outputPin.value);
                continue;
            }
            if (presetPin.value == PinValue.Negative)
            {
                outputPin.value = PinValue.Positive;
                outputComplimentPin.value = PinValue.Positive;
                continue;
            }
            if (clockPin.value != PinValue.Positive)
            {
                continue;
            }
            if (jPin.value != kPin.value)
            {
                outputPin.value = jPin.value;
                outputComplimentPin.value = GetComplimentValue(outputPin.value);
                continue;
            }
            if (jPin.value == PinValue.Negative)
            {
                continue;
            }
            outputPin.value = outputComplimentPin.value;
            outputComplimentPin.value = GetComplimentValue(outputPin.value);
        }
    }

    public static void DLogic(FlipFlopData icData, ICController icController)
    {
        PinController outputPin;
        PinController outputComplimentPin;
        PinController dPin;
        PinController presetPin;
        PinController clearPin;
        PinController clockPin;
        foreach (FlipFlopPinMapping flipFlopPinMapping in icData.FlipFlopPinMappings)
        {
            outputPin = icController.Model.Pins[flipFlopPinMapping.OutputPins[0] - 1];
            outputComplimentPin = icController.Model.Pins[flipFlopPinMapping.OutputPins[1]- 1];
            dPin = icController.Model.Pins[flipFlopPinMapping.InputPins[0]- 1];
            presetPin = icController.Model.Pins[flipFlopPinMapping.PresetPin- 1];
            clearPin = icController.Model.Pins[flipFlopPinMapping.ClearPin- 1];
            clockPin = icController.Model.Pins[flipFlopPinMapping.ClockPin- 1];
            if (outputPin.value == PinValue.Null)
            {
                outputPin.value = PinValue.Negative;
            }
            outputComplimentPin.value = GetComplimentValue(outputPin.value);
            if (presetPin.value != clearPin.value)
            {
                outputPin.value = clearPin.value;
                outputComplimentPin.value = GetComplimentValue(outputPin.value);
                continue;
            }
            if (presetPin.value == PinValue.Negative)
            {
                outputPin.value = PinValue.Positive;
                outputComplimentPin.value = PinValue.Positive;
                continue;
            }
            if (clockPin.value != PinValue.Positive)
            {
                continue;
            }
            outputPin.value = dPin.value;
            outputComplimentPin.value = GetComplimentValue(outputPin.value);
        }
    }
}