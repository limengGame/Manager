using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager> {
    public static Camera uiCamera;
    public static Transform uiRoot;
    public Dictionary<Type, BaseUIController> controllerMaps;
    public Dictionary<LayerType, Transform> layerMaps;

    public static Camera UICamera
    {
        get
        {
            return uiCamera ?? (uiCamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>());
        }
    }

    public static Transform UIRoot
    {
        get
        {
            return uiRoot ?? (uiRoot = GameObject.FindGameObjectWithTag("UIRoot").transform);
        }
    }

    public bool Init()
    {
        controllerMaps = new Dictionary<Type, BaseUIController>();
        layerMaps = new Dictionary<LayerType, Transform>();
        InitLayer();
        GameObject.DontDestroyOnLoad(UIRoot);
        return true;
    }

    public void InitLayer()
    {
        TryCleanPanel();
        string[] values = System.Enum.GetNames(typeof(LayerType));
        for (int i = 0; i < values.Length; i++)
        {
            GameObject panelObj = new GameObject();
            panelObj.transform.parent = UICamera.transform;
            panelObj.name = values[i];
            LayerType layer = (LayerType)System.Enum.Parse(typeof(LayerType), values[i]);
            panelObj.layer = LayerMask.NameToLayer("UI");
            this.layerMaps.Add(layer, panelObj.transform);
        }
    }

    public void TryCleanPanel()
    {
        UIPanel[] panels = UIRoot.GetComponentsInChildren<UIPanel>();
        for (int i = 0; i < panels.Length; i++)
        {
            if (panels[i].transform == UIRoot.transform)
                continue;
            GameObject.Destroy(panels[i].gameObject);
        }
    }

    public T Show<T>() where T : BaseUIController
    {
        BaseUIController controller = FindUIControllerByType(typeof(T));
        HideLayerObjects(controller);

        controller.Show();
        return controller as T;
    }

    public void Update()
    {
        Dictionary<Type, BaseUIController>.Enumerator iter = controllerMaps.GetEnumerator();
        while (iter.MoveNext())
        {
            BaseUIController controller = iter.Current.Value;
            controller.Update();
        }
    }

    public BaseUIController FindUIControllerByType(Type type)
    {
        BaseUIController controller = null;
        if (controllerMaps.TryGetValue(type, out controller))
            Debug.LogWarning("cannot find controller : " + type.ToString());
        return controller;
    }

    public void HideLayerObjects(BaseUIController controller)
    {
        Dictionary<Type, BaseUIController>.Enumerator iter = controllerMaps.GetEnumerator();
        while (iter.MoveNext())
        {
            BaseUIController temp = iter.Current.Value;
            if (temp == controller || temp.layer != controller.layer)
                continue;
            temp.Hide();
        }
    }

    public T GetUIController<T>() where T : BaseUIController
    {
        BaseUIController controller = null;
        if (controllerMaps.TryGetValue(typeof(T), out controller))
            Debug.LogWarning("get controller error : " + typeof(T).ToString());
        return controller as T;
    }
    public void RegisteController(BaseUIController controller)
    {
        if (!controllerMaps.ContainsKey(controller.GetType()))
            this.controllerMaps[controller.GetType()] = controller;
        
    }
    public void RemoveController<T>() where T : BaseUIController
    {
        BaseUIController controller = null;
        if (controllerMaps.TryGetValue(typeof(T), out controller))
            Debug.LogWarning("remove controller not exist " + typeof(T));
        else
        {
            controllerMaps.Remove(typeof(T));
            controller.Destroy();
        }
    }

    public void ReleaseAll()
    {
        Dictionary<Type, BaseUIController>.Enumerator iter = controllerMaps.GetEnumerator();
        while (iter.MoveNext())
        {
            BaseUIController controller = iter.Current.Value;
            controller.Hide();
            controller.Destroy();
        }
    }

    public bool Contains<T>() where T : BaseUIController
    {
        return controllerMaps.ContainsKey(typeof(T));
    }

    public void DisposeLayer()
    {
        if (this.layerMaps == null)
            return;
        Dictionary<LayerType, Transform>.Enumerator iter = layerMaps.GetEnumerator();
        while (iter.MoveNext())
        {
            Transform trans = iter.Current.Value;
            GameObject.Destroy(trans);
        }
        this.layerMaps.Clear();
        this.layerMaps = null;
    }

    public void Dispose()
    {
        DisposeLayer();
        if (this.controllerMaps != null)
        {
            this.controllerMaps.Clear();
            this.controllerMaps = null;
        }
    }

}
