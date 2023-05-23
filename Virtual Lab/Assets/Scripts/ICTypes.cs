using System;
using UnityEngine;

public enum ICTypes
{
    OR,
    AND
}

[Serializable]
public class ICBase
{
    public GameObject ICSoccet;
    public GameObject[] Pins;
}

public class IC
{
    public Sprite IcSprite;
    public int[] inputPins;
    public int[] outputPins;
    public int VccPin;
    public int GndPin;
}