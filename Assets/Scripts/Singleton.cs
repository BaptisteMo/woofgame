using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>  
{
    public static T instance;
    public static T Instance { get { return instance; } }

    protected virtual void Awake() {
        if (instance != null && this.gameObject != null) {
            Destroy(this.gameObject);
            return;
        } else {
            instance = (T)this;
        }

        if (!gameObject.transform.parent) {
            DontDestroyOnLoad(gameObject);
        }
        
    }
}