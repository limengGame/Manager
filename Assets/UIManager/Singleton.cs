using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> where T : class, new ()
{
    private static T _instance;

    public static void CreateInstance()
    {
        if (_instance == null)
            _instance = new T();
    }

    public static void ReleaseInstance()
    {
        _instance = null;
    }

    public static T Instance
    {
        get
        {
            return _instance;
        }
    }

}
