using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    static T _instance;

    public static T Instance
    {
        get { return _instance; } 
    }

    protected virtual void Awake()
    {
        if (_instance != null && this.gameObject != null)
            Destroy(this
                .gameObject); // So if this instance already exists and there is already another gameObject of that class in the scene it'll destroy any that would otherwise spawn.
        else
            _instance = (T)this;

        if (!gameObject.transform.parent) // IF this gameObject does not have a parent transform then don't destroy on load.
            DontDestroyOnLoad(this.gameObject);
    }
}