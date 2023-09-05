

using System.Collections.Generic;
using UnityEngine;

public class ICModel
{
    public SpriteRenderer ICSprite;
    public ICLogic IcLogic;
    public List<PinController> Pins;


    public PinController VccPin;
    public PinController GndPin;

    public ICModel(ICLogic icLogic, SpriteRenderer spriteRenderer)
    {

        IcLogic = icLogic;
        ICSprite = spriteRenderer;
        Pins = new();
        
    }

}
