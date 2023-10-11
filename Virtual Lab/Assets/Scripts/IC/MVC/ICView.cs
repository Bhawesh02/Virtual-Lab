
using UnityEngine;

public class ICView : MonoBehaviour
{


    [SerializeField]
    private GameObject pinHolderGameobject;

    private float callTime;

    public ICController Controller { get; private set; }
    private void Start()
    {
        Controller = new(this);
        Controller.SetPins(pinHolderGameobject);
        callTime = Time.time;
    }
    private void Update()
    {
        if(Time.time >= callTime)
        {
            Controller.RunIcLogic();
            callTime = Time.time + 1f;
        }
    }

    private void OnMouseDown()
    {
        if (Controller.Model.IcData == null) return;

        EventService.Instance.InvokeShowICTT(Controller.Model.IcData);
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
