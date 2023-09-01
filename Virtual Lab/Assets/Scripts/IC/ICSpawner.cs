
using UnityEngine;

public class ICSpawner : MonoGenericSingelton<ICSpawner>
{
    [SerializeField]
    private ListOfIc Ics;
    [SerializeField]
    private ICDragAndDrop iCPrefab;
    [SerializeField]
    private ICPlaceHolder iCPlaceHolderPrefab;
    private void Start()
    {
        for(int i = 0; i < Ics.IcList.Count; i++)
        {
            iCPlaceHolderPrefab.IcData = Ics.IcList[i];
            Instantiate(iCPlaceHolderPrefab, transform);
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
