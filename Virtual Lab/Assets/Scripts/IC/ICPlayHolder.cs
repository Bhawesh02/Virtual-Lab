
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ICPlayHolder : MonoBehaviour, IPointerDownHandler
{
    public IC IcData;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        image.sprite = IcData.IcSprite;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        ICSpawner.Instance.SpawnIC(IcData);
    }
    

   
}
