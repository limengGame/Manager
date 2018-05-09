using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    public static Main Instance
    {
        get;
        private set;
    }
    private void Awake()
    {
        Instance = this;
    }

    void Start () {
        UIManager.CreateInstance();
        UIControllerManager.CreateInstance();
        UIManager.Instance.Init();
        UIControllerManager.Instance.Init();
	}
	
	void Update () {
        UIManager.Instance.Update();
	}

    public void DisposeManagers()
    {
        UIControllerManager.Instance.Dispose();
        UIManager.Instance.Dispose();
    }

}
