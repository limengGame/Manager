using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {
    
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
