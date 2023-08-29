
using UnityEngine;

public class ICSpawner : MonoGenericSingelton<ICSpawner>
{
    [SerializeField]
    private ListOfIc Ics;
    [SerializeField]
    private ICDragAndDrop iCPrefab;
    [SerializeField]
    private ICPlayHolder iCPlayHolderPrefab;
    private void Start()
    {
        for(int i = 0; i < Ics.IcList.Count; i++)
        {
            iCPlayHolderPrefab.IcData = Ics.IcList[i];
            Instantiate(iCPlayHolderPrefab, transform);
        }
        
    }
    public void SpawnIC(IC iCData)
    {
        iCPrefab.IcData = iCData;
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Instantiate(iCPrefab, pos, iCPrefab.transform.rotation);
        gameObject.SetActive(false);
    }
    
}
