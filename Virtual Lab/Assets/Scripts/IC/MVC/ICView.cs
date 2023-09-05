
using System.Collections.Generic;
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
    }

   

}
