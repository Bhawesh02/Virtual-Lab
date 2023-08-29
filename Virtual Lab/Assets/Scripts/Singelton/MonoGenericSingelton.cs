
using UnityEngine;

public class MonoGenericSingelton<T> : MonoBehaviour where T : MonoGenericSingelton<T>
{
    public static T Instance { get; private set; }
    protected virtual void Awake()
    {
        if(Instance == null)
        {
            Instance = (T)this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
