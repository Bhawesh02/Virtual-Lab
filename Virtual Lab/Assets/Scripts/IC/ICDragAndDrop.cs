
using UnityEngine;

public class ICDragAndDrop : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public IC IcData;
    private ICChange IcChange = new();

    [SerializeField]
    private float detectionRadius = 1f;

    private float detectionDelay = 0.1f;

    private float nextDetectionTime;

    private Collider2D collided;
    [SerializeField]
    private LayerMask IcBaseLayer;
    private Camera mainCamera;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
    }
    private void Start()
    {
        spriteRenderer.sprite = IcData.IcSprite;
        nextDetectionTime = Time.time;
    }
    private void LateUpdate()
    {
        if(Input.GetMouseButtonUp(0))
        {
            MouseReleased();
        }
        Vector2 newPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
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

    private void MouseReleased()
    {
        if(collided!=null)
        {
            ICBase IcBase = collided.transform.parent.GetComponent<ICController>().thisIC;
            IcChange.ChangeIc(IcBase, IcData);
        }
        ICSpawner.Instance.gameObject.SetActive(true);
        Destroy(gameObject);
    }

    private void FixedUpdate()
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
