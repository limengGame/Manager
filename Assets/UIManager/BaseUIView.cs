using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUIView {
    private int renderQueue = 3000;
    private int layerDepth = 50;
    private int sortOrder = 50;
    public bool IsShow = false;
    public GameObject GameObject
    {
        get;
        private set;
    }
    public Transform Transform
    {
        get;
        private set;
    }
    public BaseUIController controller;

    public void Init(GameObject go, BaseUIController controller)
    {
        if (go != null)
        {
            this.GameObject = go;
            this.Transform = this.GameObject.transform;
        }
        InitPanelDepth();
    }
    public void InitPanelDepth()
    {
        UIPanel[] panels = this.Transform.GetComponentsInChildren<UIPanel>();
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].depth = (int)this.controller.layer * layerDepth + i;
            panels[i].sortingOrder = sortOrder;
        }
        UIPanel panel = this.Transform.GetComponent<UIPanel>();
        if (panel != null)
        {
            panel.renderQueue = UIPanel.RenderQueue.StartAt;
            panel.startingRenderQueue = renderQueue;
            panel.sortingOrder = sortOrder;
        }
    }
    public void Show()
    {
        if (this.GameObject == null || this.IsShow)
            return;
        this.GameObject.SetActive(true);
        this.IsShow = true;
        this.OnShow();
    }

    public void SetUIEffectRenderQueue(GameObject effect)
    {
        Renderer[] render = effect.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < render.Length; i++)
        {
            Material[] material = render[i].sharedMaterials;
            for (int j = 0; j < material.Length; j++)
            {
                material[j].renderQueue = this.renderQueue + (layerDepth - 1);
            }
        }
    }

    public void Update()
    {
        this.OnUpdate();
    }

    public void Hide()
    {
        this.OnHide();
    }
    public void Destroy()
    {
        this.OnDestroy();
    }

    public virtual void OnShow()
    {
    }
    public virtual void OnUpdate()
    {
    }
    public virtual void OnHide()
    {
    }
    public virtual void OnDestroy()
    {
    }

}
