using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AsynchronousCounterLogic
{
    private static void BasicModConfig(AsynchronousCounterPinMapping asynchronousCounterPinMapping, ICController icController, out  PinController clockPin, 
        out PinController setPinController, out PinController resetPinController)
    {
        clockPin = icController.Model.Pins[asynchronousCounterPinMapping.ClockPin - 1];
        setPinController = icController.Model.Pins[asynchronousCounterPinMapping.SetPin - 1];
        resetPinController = icController.Model.Pins[asynchronousCounterPinMapping.ResetPin - 1];
        if (setPinController.value != PinValue.Negative)
        {
            EventService.Instance.InvokeShowError($"Set needs to br negative at {asynchronousCounterPinMapping.SetPin}");
        }
        if (clockPin.value == PinValue.Positive)
        {
            asynchronousCounterPinMapping.HasEncounteredHighState = true;
        }

    }
    public static void Mod2Logic(AsynchronousCounterPinMapping asynchronousCounterPinMapping, ICController icController)
    {
        PinController outputPin = icController.Model.Pins[asynchronousCounterPinMapping.OutputPins[0] - 1];
        BasicModConfig(asynchronousCounterPinMapping, icController, out PinController clockPin, out PinController setPin, out PinController resetPin);
        if (resetPin.value == PinValue.Positive)
        {
            asynchronousCounterPinMapping.HasEncounteredHighState = false;
            asynchronousCounterPinMapping.TimesClocksChanged = 0;
            outputPin.value = PinValue.Negative;
            EventService.Instance.InvokeOutputPinValueChange(outputPin);
            ValuePropagateService.Instance.TransferData(outputPin);
            return;
        }
        if (!(clockPin.value == PinValue.Negative && asynchronousCounterPinMapping.HasEncounteredHighState == true))
        {
            return;
        }
        asynchronousCounterPinMapping.HasEncounteredHighState = false;
        asynchronousCounterPinMapping.TimesClocksChanged++;
        if (asynchronousCounterPinMapping.TimesClocksChanged % 2 == 0)
        {
            asynchronousCounterPinMapping.TimesClocksChanged = 0;
        }
        if (asynchronousCounterPinMapping.TimesClocksChanged == 0)
        {
            outputPin.value = PinValue.Negative;
        }
        else
        {
            outputPin.value = PinValue.Positive;
        }
        EventService.Instance.InvokeOutputPinValueChange(outputPin);
        ValuePropagateService.Instance.TransferData(outputPin);
    }
    public static void Mod5Logic(AsynchronousCounterPinMapping asynchronousCounterPinMapping, ICController icController)
    {
        BasicModConfig(asynchronousCounterPinMapping, icController, out PinController clockPin, out PinController setPin, out PinController resetPin);
        if (resetPin.value == PinValue.Positive)
        {
            asynchronousCounterPinMapping.TimesClocksChanged = 0;
            asynchronousCounterPinMapping.HasEncounteredHighState = false;
            foreach (int outputPinIndex in asynchronousCounterPinMapping.OutputPins)
            {
                PinController outputPin = icController.Model.Pins[outputPinIndex - 1];
                outputPin.value = PinValue.Negative;
                EventService.Instance.InvokeOutputPinValueChange(outputPin);
                ValuePropagateService.Instance.TransferData(outputPin);
            }
            return;
        }
        if (!(clockPin.value == PinValue.Negative && asynchronousCounterPinMapping.HasEncounteredHighState == true))
        {
            return;
        }
        asynchronousCounterPinMapping.HasEncounteredHighState = false;
        asynchronousCounterPinMapping.TimesClocksChanged++;
        if (asynchronousCounterPinMapping.TimesClocksChanged % 5 == 0)
        {
            asynchronousCounterPinMapping.TimesClocksChanged = 0;
        }
        if (resetPin.value == PinValue.Positive)
        {
            asynchronousCounterPinMapping.TimesClocksChanged = 0;
        }
        List<PinController> outputPinBits = new()
        {
            icController.Model.Pins[asynchronousCounterPinMapping.OutputPins[0] - 1],
            icController.Model.Pins[asynchronousCounterPinMapping.OutputPins[1] - 1],
            icController.Model.Pins[asynchronousCounterPinMapping.OutputPins[2] - 1]
        };
        if (asynchronousCounterPinMapping.TimesClocksChanged == 0)
        {
            foreach (PinController outputPinBit in outputPinBits)
            {
                outputPinBit.value = PinValue.Negative;
            }
        }
        else
        {
            BitArray bitArray = new BitArray(new int[] { asynchronousCounterPinMapping.TimesClocksChanged });
            bool[] bits = new bool[bitArray.Count];
            bitArray.CopyTo(bits, 0);
            for (int outputPinIndex = 0; outputPinIndex < outputPinBits.Count;outputPinIndex++)
            {
                outputPinBits[outputPinIndex].value = bits[outputPinIndex] ?
                    PinValue.Positive : PinValue.Negative;
            }
        }

        foreach (PinController outputPinBit in outputPinBits)
        {
            EventService.Instance.InvokeOutputPinValueChange(outputPinBit);
            ValuePropagateService.Instance.TransferData(outputPinBit);
        }
    }
    public static void Mod10Logic(AsynchronousCounterData asynchronousCounterData,ICController icController)
    {
        AsynchronousCounterPinMapping mod2PinMapping =
            asynchronousCounterData.asynchronousCounterPinMappings.Find(mapping =>
                mapping.AsynchronousCountersType == AsynchronousCountersTypes.MOD_2);
        
        AsynchronousCounterPinMapping mod5PinMapping =
            asynchronousCounterData.asynchronousCounterPinMappings.Find(mapping =>
                mapping.AsynchronousCountersType == AsynchronousCountersTypes.MOD_5);
        Mod2Logic(mod2PinMapping,icController);
        Mod5Logic(mod5PinMapping,icController);

    }

}