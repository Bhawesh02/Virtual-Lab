
using UnityEngine;

[CreateAssetMenu(fileName = "New IC")]
public class IC : ScriptableObject
{
    public Sprite IcSprite;
    public int[] inputPins;
    public int[] outputPins;
    public PinMapping[] pinMapping;
    public int VccPin;
    public int GndPin;
}