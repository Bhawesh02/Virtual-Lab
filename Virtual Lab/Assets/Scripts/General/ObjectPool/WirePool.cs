
using UnityEngine;

public class WirePool : PoolService<WireController>
{
    private WireController wirePrefab;

    private WireController wire;

    public WirePool(WireController wirePrefab)
    {
        this.wirePrefab = wirePrefab;
    }
    public override WireController GetItem()
    {
        wire = base.GetItem();
        wire.gameObject.SetActive(true);
        return wire;
    }
    protected override WireController CreateItem()
    {
        wire = GameObject.Instantiate(wirePrefab);
        return wire;
    }
    public override void ReturnItem(WireController item)
    {
        item.gameObject.SetActive(false);
        base.ReturnItem(item);
    }

    
}
