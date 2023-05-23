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
