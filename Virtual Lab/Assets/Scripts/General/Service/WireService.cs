
using Unity.VisualScripting;
using UnityEngine;

public class WireService : MonoGenericSingelton<WireService>
{
    public bool doingConnection = false;

    

    [SerializeField]
    private WireController wirePrefab;


    private WireController NewWireMakingConnection;

    private WirePool wirePool;
    private void Start()
    {
        wirePool = new(wirePrefab);
    }
    private void Update()
    {
        if (doingConnection)
        {
            SetWireEndToMousePointer();
            if (Input.GetMouseButtonDown(1))
            {
                ReturnWire(NewWireMakingConnection);
                NewWireMakingConnection = null;
                doingConnection = false;
            }
        }
    }

    private void SetWireEndToMousePointer()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 endPosition = new(mousePosition.x, mousePosition.y, mousePosition.z);
        NewWireMakingConnection.SetWireEnd(endPosition);
    }

    public void CreateNewWire(PinController initialPinController)
    {
        NewWireMakingConnection = wirePool.GetItem();
        NewWireMakingConnection.transform.SetParent(transform);
        NewWireMakingConnection.MakeWire(initialPinController.transform.position);
        NewWireMakingConnection.initialPin = initialPinController;
        doingConnection = true;
    }

    public void CompleteExsistingWire(PinController finalPinController)
    {
        doingConnection = false;
        NewWireMakingConnection.SetWireEnd(finalPinController.transform.position);
        NewWireMakingConnection.finalPin = finalPinController;
        NewWireMakingConnection.ConfirmConnection();
    }
    public void ReturnWire(WireController wireToReturn)
    {
        wirePool.ReturnItem(wireToReturn);
    }
}
