
using System;
using Unity.VisualScripting;
using UnityEngine;

public class ICDragAndDrop : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Vector3 initalPos;
    public IC IcData;
    private ICChange IcChange = new();

    [SerializeField]
    private float detectionRadius = 1f;

    private float detectionDelay = 0.1f;

    private float nextDetectionTime;

    private Collider2D collided;
    [SerializeField]
    private LayerMask IcBaseLayer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = IcData.IcSprite;
        initalPos = transform.position;
    }
    private void Start()
    {
        nextDetectionTime = Time.time;
    }
    private void OnMouseDrag()
    {
        
        Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPos.z = 0;
        transform.position = newPos; 
        if(collided == null)
        {
            spriteRenderer.color = Color.red;
        }
        else
        {
            spriteRenderer.color = Color.green;
        }
    }

    private void OnMouseUp()
    {
        transform.position = initalPos;
        spriteRenderer.color = Color.white;
        if (collided == null)
            return;
        ICBase IcBase = collided.transform.parent.GetComponent<ICController>().thisIC;
        IcChange.ChangeIc(IcBase,IcData);
    }

    private void LateUpdate()
    {
        if (Time.time >= nextDetectionTime)
        {
            collided = Physics2D.OverlapCircle(transform.position, detectionRadius, IcBaseLayer);
            nextDetectionTime = Time.time + detectionDelay;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        if (Application.isPlaying && collided != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(collided.transform.position, 0.1f);
        }
    }
}
