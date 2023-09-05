
using System.Collections.Generic;
using UnityEngine;

public class ICView : MonoBehaviour
{
    public List<PinController> Pins;
    [SerializeField]
    private ICLogic icLogic;

    [SerializeField]
    private GameObject pinHolderGameobject;

    public ICController Controller { get;private set; }
    private void Start()
    {
        Controller = new(this,icLogic, pinHolderGameobject);
    }

   

}
