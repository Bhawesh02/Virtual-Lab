

using System.Collections.Generic;
using UnityEngine;

public class ICModel
{
    public ICBase thisIC { get; }

    //public List<PinController> Pins =new();

    public PinController VccPin;
    public PinController GndPin;

    public ICModel(ICLogic icLogic, SpriteRenderer spriteRenderer)
    {
        thisIC = new()
        {
            IcLogic = icLogic,
            ICSprite = spriteRenderer,
            Pins = new()
        };
    }

}
