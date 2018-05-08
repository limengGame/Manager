using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUIController : Singleton<BaseUIController> {
    public LayerType layer
    {
        get;
        protected set;
    }
    public BaseUIView _view;
    public string prefabName;
    public bool IsShow
    {
        get
        {
            if (_view != null)
                return _view.IsShow;
            return false;
        }
    }
    public void Show()
    {
        OnShow();
    }
    public void Update()
    {
        if (_view != null)
            _view.Update();
    }
    public void Hide()
    {
        OnHide();
    }
    public void Destroy()
    {
        OnDestroy();
    }
    public void Dispose()
    {
        OnDispose();
    }
    public GameObject InstancePrefab()
    {
        GameObject prefab;
        string path = "prefab/ui/" + prefabName;
        prefab = GameObject.Instantiate(Resources.Load<GameObject>(path));
        prefab.transform.parent = UIManager.Instance.layerMaps.ContainsKey(layer) ? 
            UIManager.Instance.layerMaps[layer] : UIManager.UIRoot.transform;
        return prefab;
    }
    public virtual void OnShow()
    {
        if (_view == null)
            return;
        if (_view.GameObject == null)
        {
            GameObject prefab = InstancePrefab();
            this._view.Init(prefab, this);
        }
        this._view.Show();
    }
    public virtual bool Init()
    {
        return false;
    }
    public virtual void OnHide()
    {
    }
    public virtual void OnDestroy()
    {
    }
    public virtual void OnDispose()
    {
    }
}
