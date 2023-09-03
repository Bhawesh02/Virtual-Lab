


using System.Collections.Generic;

public abstract class PoolService<T>
{
    private class PooledItem<t>
    {
        public t Item;
        public bool IsUsed;
    }

    private List<PooledItem<T>> pooledItem = new();

    protected T GetItem()
    {
        if(pooledItem.Count > 0)
        {
            PooledItem<T> item = pooledItem.Find(x=>!x.IsUsed);
            if (item != null)
            {
                item.IsUsed = true;
                return item.Item;
            }
        }
        return CreateNewItem();
    }

    private T CreateNewItem()
    {
        PooledItem<T> item = new()
        {
            Item = CreateItem(),
            IsUsed = true
        };
        pooledItem.Add(item);
        return item.Item;
    }

    protected abstract T CreateItem();

    protected virtual void ReturnItem(T item)
    {
        PooledItem<T> usedItem = pooledItem.Find(x=>x.Item.Equals(item));
        usedItem.IsUsed = false;
    }
}
