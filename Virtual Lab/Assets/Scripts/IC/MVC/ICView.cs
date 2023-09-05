
using UnityEngine;

public class ICView : MonoBehaviour
{
    

    [SerializeField]
    private GameObject pinHolderGameobject;


    public ICController Controller { get;private set; }
    private void Start()
    {
        Controller = new(this);
        Controller.SetPins(pinHolderGameobject);
        hasRightClick = false;
    }


    private void OnMouseDown()
    {
        if (Controller.Model.IcData == null) return;
        
        EventService.Instance.InvokeShowICTT(Controller.Model.IcData);
    }


}
