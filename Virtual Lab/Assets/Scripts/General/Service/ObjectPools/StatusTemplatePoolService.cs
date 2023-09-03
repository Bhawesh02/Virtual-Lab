
using UnityEngine;

public class StatusTemplatePoolService : MonoGenericSingelton<StatusTemplatePoolService>
{
    private StatusTemplatePool templatePool = null;

    [SerializeField]
    private RectTransform statusTemplatePrefab;

    protected override void Awake()
    {
        base.Awake();
        templatePool = new(statusTemplatePrefab);
    }

    public RectTransform GetTemplate()
    {
        return templatePool.GetItem();
    }

    public void ReturnTemplate(RectTransform templateToReturn)
    {
        templateToReturn.transform.SetParent(transform);
        templatePool.ReturnItem(templateToReturn);
    }
}
