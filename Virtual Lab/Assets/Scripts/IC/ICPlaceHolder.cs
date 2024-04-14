
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ICPlaceHolder : MonoBehaviour, IPointerDownHandler
{
    public IcData IcData;

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
            ICSpawnerService.Instance.SpawnIC(IcData);
        }
        else if(eventData.button == PointerEventData.InputButton.Right)
        {
            EventService.Instance.InvokeShowICTT(IcData);
        }
    }



}
