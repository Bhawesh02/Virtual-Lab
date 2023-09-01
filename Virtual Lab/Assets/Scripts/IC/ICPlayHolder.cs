
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
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            ICSpawner.Instance.SpawnIC(IcData);
        }
        else if(eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("Show TT");
        }
    }



}
