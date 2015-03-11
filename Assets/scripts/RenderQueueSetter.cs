using System;
using UnityEngine;

public class RenderQueueSetter : MonoBehaviour
{
    public UIPanel basePanel;
    public bool isInhert;
    public int lastQueue;
    public int queue;
    public bool reUpdateIt;

    public void ChangeQueue(int _queue)
    {
        this.queue = _queue;
        foreach (Renderer renderer in base.GetComponents<Renderer>())
        {
            this.ChangeRender(renderer);
        }
        if (this.isInhert)
        {
            foreach (Renderer renderer2 in base.GetComponentsInChildren<Renderer>())
            {
                this.ChangeRender(renderer2);
            }
        }
    }

    private void ChangeRender(Renderer render)
    {
        foreach (Material material in render.sharedMaterials)
        {
            if ((material != null) && (material.shader != null))
            {
                int renderQueue = material.shader.renderQueue;
                if (this.basePanel != null)
                {
                    renderQueue = this.basePanel.startingRenderQueue;
                }
                material.renderQueue = renderQueue + this.queue;
                this.lastQueue = material.renderQueue;
            }
        }
    }

    private void Start()
    {
        this.ChangeQueue(this.queue);
    }

    private void Update()
    {
        if (this.reUpdateIt)
        {
            this.reUpdateIt = false;
            this.ChangeQueue(this.queue);
        }
    }
}

