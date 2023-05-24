using System;
using UnityEngine;

public enum ICTypes
{
    Or,
    Ans,
    Not
}

[Serializable]
public class ICBase
{
    public SpriteRenderer ICSprite;
    public GameObject[] Pins;
}
[Serializable]
public class PinMapping
{
    public int OutputPin;
    public int[] InputPin;
}