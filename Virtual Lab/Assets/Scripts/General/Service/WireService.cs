
using UnityEngine;

public class WireService : MonoGenericSingelton<WireService>
{
    public bool doingConnection = false;

    private WireController LatestWire;

    [SerializeField]
    private WireController wirePrefab;

    private void Update()
    {
        if (doingConnection)
        {
            SetWireEndToMousePointer();
            if (Input.GetMouseButtonDown(1))
            {
                Destroy(LatestWire.gameObject);
                LatestWire = null;
                doingConnection = false;
            }
        }
    }

    private void SetWireEndToMousePointer()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 endPosition = new(mousePosition.x, mousePosition.y, mousePosition.z);
        LatestWire.SetWireEnd(endPosition);
    }

    public void CreateNewWire(PinController initialPinController)
    {
        LatestWire = Instantiate(wirePrefab);
        LatestWire.transform.SetParent(transform);
        LatestWire.MakeWire(initialPinController.transform.position);
        LatestWire.initialPin = initialPinController;
        doingConnection = true;
    }

    public void CompleteExsistingWire(PinController finalPinController)
    {
        doingConnection = false;
        LatestWire.SetWireEnd(finalPinController.transform.position);
        LatestWire.finalPin = finalPinController;
        LatestWire.ConfirmConnection();


    }
}
