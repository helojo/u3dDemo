using System;
using UnityEngine;

[RequireComponent(typeof(UILabel))]
public class LabelLeftAncher : MonoBehaviour
{
    private UILabel label;
    public Transform LeftAncher;

    private void Start()
    {
        this.label = base.GetComponent<UILabel>();
    }

    private void Update()
    {
        if (this.label != null)
        {
            float width = this.label.width;
            base.transform.localPosition = new Vector3(this.LeftAncher.localPosition.x + (width / 2f), base.transform.localPosition.y, base.transform.localPosition.z);
        }
    }
}

