using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class ICBase
{
    public SpriteRenderer ICSprite;
    public ICLogic IcLogic;
    public List<PinController> Pins;
}
