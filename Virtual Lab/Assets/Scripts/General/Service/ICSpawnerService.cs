
using UnityEngine;
using UnityEngine.UI;

public class ICSpawnerService : MonoGenericSingelton<ICSpawnerService>
{
    [SerializeField]
    private ListOfIc Ics;
    [SerializeField]
    private ICDragAndDrop iCPrefab;
    [SerializeField]
    private ICPlaceHolder iCPlaceHolderPrefab;
    [SerializeField]
    private GridLayoutGroup icSpawner;

    private Vector2 infintePos = new(999f, 999f);

    private ICDragAndDrop iCBeingDraged;

    private void Start()
    {
        
        EventService.Instance.SimulationStarted += () =>
        {
            SetIcSpawnerActive(false);
        };
        EventService.Instance.SimulationStopped += () =>
        {
            SetIcSpawnerActive(true);
        };
        for (int i = 0; i < Ics.IcList.Count; i++)
        {
            iCPlaceHolderPrefab.IcData = Ics.IcList[i];
            Instantiate(iCPlaceHolderPrefab, icSpawner.transform);
        }
        iCBeingDraged = Instantiate(iCPrefab, infintePos, iCPrefab.transform.rotation);
        iCBeingDraged.transform.parent = transform;
        iCBeingDraged.gameObject.SetActive(false);
        
    }

    private void SetIcSpawnerActive(bool value) {
        icSpawner.gameObject.SetActive(value);
    }
    public void SpawnIC(IcData iCData)
    {
        iCBeingDraged.IcData = iCData;
        Vector2 newIcPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        iCBeingDraged.transform.position = newIcPosition;
        iCBeingDraged.gameObject.SetActive(true);
        SetIcSpawnerActive(false);
    }
    public void TakeBackIc()
    {
        iCBeingDraged.transform.position = infintePos;
        iCBeingDraged.gameObject.SetActive(false);
        SetIcSpawnerActive(true);
    }
    private void OnDestroy()
    {
        EventService.Instance.SimulationStarted -= () =>
        {
            SetIcSpawnerActive(false);
        };
        EventService.Instance.SimulationStopped -= () =>
        {
            SetIcSpawnerActive(true);
        };
    }
}
