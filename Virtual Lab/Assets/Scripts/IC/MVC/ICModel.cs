

using System.Collections.Generic;
using UnityEngine;

public class ICModel
{
    public SpriteRenderer ICSprite;
    public List<PinController> Pins;


    public PinController VccPin;
    public PinController GndPin;

    public IcData IcData;

    public ICController Controller { get; }

    public ICModel(SpriteRenderer spriteRenderer, ICController controller)
    {

        ICSprite = spriteRenderer;
        Pins = new();
        Controller = controller;
    }

}
