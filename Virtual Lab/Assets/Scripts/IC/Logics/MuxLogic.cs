using System.Collections.Generic;

public static class MuxLogic
{
    public static PinValue EightXOneLogic(List<PinController> Pins, MuxIcData muxIcData)
    {
        PinController strobePin = Pins[muxIcData.StrobePin - 1];
        if (strobePin.value == PinValue.Positive)
        {
            return PinValue.Negative;
        }

        int[] selectLineValue = new int[muxIcData.SelectInputPins.Length];
        for (int selectIndex = 0; selectIndex < muxIcData.SelectInputPins.Length; selectIndex++)
        {
            PinController selectPin = Pins[muxIcData.SelectInputPins[selectIndex] - 1];
            if (selectPin.value == PinValue.Negative)
            {
                selectLineValue[selectIndex] = 0;
            }
            else
            {
                selectLineValue[selectIndex] = 1;
            }
        }

        int inputPinIndex = selectLineValue[0] * 4 + selectLineValue[1] * 2 + selectLineValue[2] * 1;
        PinController InputPin = Pins[muxIcData.InputPins[inputPinIndex] - 1];
        if (InputPin == null)
        {
            return PinValue.Null;
        }

        return InputPin.value;
    }
}