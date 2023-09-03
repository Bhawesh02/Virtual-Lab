
using UnityEngine;

public class StatusTemplatePool : PoolService<RectTransform>
{
    private RectTransform statusTemplatePrefab;

    private RectTransform statusTemplate;

    public StatusTemplatePool(RectTransform statusTemplatePrefab)
    {
        this.statusTemplatePrefab = statusTemplatePrefab;
    }
    public override RectTransform GetItem()
    {
        statusTemplate = base.GetItem();
        statusTemplate.gameObject.SetActive(true);
        return statusTemplate;
    }
    protected override RectTransform CreateItem()
    {
        statusTemplate = GameObject.Instantiate(statusTemplatePrefab);
        return statusTemplate;
    }
    public override void ReturnItem(RectTransform item)
    {
        item.gameObject.SetActive(false);
        base.ReturnItem(item);
    }

    
}
