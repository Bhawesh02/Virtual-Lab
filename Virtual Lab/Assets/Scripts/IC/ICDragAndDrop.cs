
using System;
using Unity.VisualScripting;
using UnityEngine;

public class ICDragAndDrop : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Vector3 initalPos;
    public IC IcData;
    private bool inContactWithIcBase;
    private ICBase IcBase = new();
    private ICChange IcChange = new();
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 1;
        initalPos = transform.position;
    }
    private void OnMouseDrag()
    {
        Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPos.z = 0;
        transform.position = newPos; 
        
    }
    private void OnMouseUp()
    {
        transform.position = initalPos;
        if (!inContactWithIcBase)
            return;
        Debug.Log("ChangeIC");
        IcChange.ChangeIc(IcBase,IcData);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        ICController iCController = collision.transform.parent.GetComponent<ICController>();
        if (iCController == null)
            return;
        Debug.Log("Collided");
        inContactWithIcBase = true;
        IcBase = iCController.thisIC;

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.transform.parent.GetComponent<ICController>() == null)
            return;
        inContactWithIcBase = false;
    }
}
