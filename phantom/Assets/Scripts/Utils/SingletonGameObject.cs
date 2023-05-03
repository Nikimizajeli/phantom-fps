using UnityEngine;

public abstract class SingletonGameObject<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public virtual void OnDestroy()
    {
        _instance = null;
    }

    public static bool Exists
    {
        get
        {
            if (_instance == null)
            {
                _instance = (T)FindObjectOfType(typeof(T));
            }

            return _instance != null;
        }
    }

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (T)FindObjectOfType(typeof(T));

                if (FindObjectsOfType(typeof(T)).Length > 1)
                {
                    Debug.LogError("More than one instance of " + typeof(T));
                }

                if (_instance == null)
                {
                    Debug.LogError("Could not find instance of " + typeof(T));
                }
            }

            return _instance;
        }
    }
}