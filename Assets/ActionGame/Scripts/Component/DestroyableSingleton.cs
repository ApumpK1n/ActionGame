using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ISingleton
{
    void OnSingletonInit();
}

/// <summary>
/// A mono singleton class that can be destroyed
/// </summary>
public class DestroyableSingleton<T> : MonoBehaviour, ISingleton where T : DestroyableSingleton<T>
{
    private static T _instance;
    private static object _lock = new object();

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    //Not created in playing state
                    if (!Application.isPlaying)
                        return null;
                    //Find if the scene already exists
                    _instance = GameObject.FindObjectOfType<T>();

                    //If not, create it
                    if (_instance == null)
                    {
                        GameObject objT = new GameObject(typeof(T).Name);
                        _instance = objT.AddComponent<T>();
                    }

                    _instance.OnSingletonInit();
                }
            }

            return _instance;
        }
    }

    public virtual void OnSingletonInit()
    {

    }
}

