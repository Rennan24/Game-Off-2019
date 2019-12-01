using UnityEngine;
using UnityEngine.Assertions;

public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Inst
    {
        get
        {
            Assert.IsNotNull(inst, $"Singleton {typeof(T)} is null!");
            return inst;
        }
    }

    private static T inst;

    public virtual void Awake()
    {
        Assert.IsNull(inst, $"There are more than one Singleton {typeof(T)}");
        inst = GetComponent<T>();
    }
}
