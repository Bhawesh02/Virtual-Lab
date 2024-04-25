
using System;
using UnityEngine;

public class ICView : MonoBehaviour
{
    [SerializeField]
    private GameObject pinHolderGameobject;

    [SerializeField] private SpriteRenderer icSprite;
    public ICController Controller { get; private set; }

    public SpriteRenderer IcSprite => icSprite;
    private void Start()
    {
        Controller = new(this);
        Controller.SetPins(pinHolderGameobject);
    }
    
    private void OnMouseDown()
    {
        if (Controller.Model.IcData == null) return;
        EventService.Instance.InvokeShowICTruthTable(Controller.Model.IcData);
    }

    private void OnMouseEnter()
    {
        Controller.ShowMessage();
    }
    private void OnMouseExit()
    {
        Controller.RemoveMessage();
    }
}
