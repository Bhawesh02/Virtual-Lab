using System;
using UnityEngine;

public enum ICTypes
{
    Null,
    Or,
    And,
    Not
}

[Serializable]
public class ICBase
{
    public SpriteRenderer ICSprite;
    public ICLogic IcLogic;
    public GameObject[] Pins;
}
[Serializable]
public class PinMapping
{
    public int OutputPin;
    public int[] InputPin;
}