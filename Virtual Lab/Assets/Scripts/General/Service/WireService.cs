
using UnityEngine;

public class WireService : MonoGenericSingelton<WireService>
{
    public bool doingConnection = false;

    public GameObject Wire;


    private void Update()
    {
        if (doingConnection)
        {
            SetWireEndToMousePointer();
            if (Input.GetMouseButtonDown(1))
            {
                Destroy(Wire);
                Wire = null;
                doingConnection = false;
            }
        }
    }

    private void SetWireEndToMousePointer()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 endPosition = new(mousePosition.x, mousePosition.y, mousePosition.z);
        Wire.GetComponent<WireController>().SetWireEnd(endPosition);
    }
}
