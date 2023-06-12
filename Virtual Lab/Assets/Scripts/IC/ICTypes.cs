using System;
using System.Collections.Generic;
using UnityEngine;

public enum ICTypes
{
    Null,
    Or,
    And,
    Nor,
    Not,
    Xor,
    Nand
}

[Serializable]
public class ICBase
{
    public SpriteRenderer ICSprite;
    public ICLogic IcLogic;
    public List<GameObject> Pins;
}
[Serializable]
public class PinMapping
{
    public int OutputPin;
    public int[] InputPin;
}