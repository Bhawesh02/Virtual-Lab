using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulatorManager : MonoBehaviour
{
    private static SimulatorManager instance;
    public static SimulatorManager Instance { get { return instance; } }

    public bool doingConnection = false;

    public GameObject Wire;

    public GameObject Wires;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        if (doingConnection)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 endPosition = new Vector3(mousePosition.x, mousePosition.y,mousePosition.z);
            Wire.GetComponent<WireController>().SetWireEnd(endPosition);
        }
    }

}
