
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

    private ICDragAndDrop iCThatDrags;
    private void Start()
    {
        for(int i = 0; i < Ics.IcList.Count; i++)
        {
            iCPlaceHolderPrefab.IcData = Ics.IcList[i];
            Instantiate(iCPlaceHolderPrefab, icSpawner.transform);
        }
        iCThatDrags = Instantiate(iCPrefab, infintePos, iCPrefab.transform.rotation);
        iCThatDrags.transform.parent = transform;
        iCThatDrags.gameObject.SetActive(false);
    }
    public void SpawnIC(IC iCData)
    {
        iCThatDrags.IcData = iCData;
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        iCThatDrags.transform.position = pos;
        iCThatDrags.gameObject.SetActive(true);
        icSpawner.gameObject.SetActive(false);
    }
    public void TakeBackIc()
    {
        iCThatDrags.transform.position = infintePos;
        iCThatDrags.gameObject.SetActive(false);
        icSpawner.gameObject.SetActive(true);
    }
}
