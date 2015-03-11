using System;
using UnityEngine;

public class CheckImageShow : MonoBehaviour
{
    private void Start()
    {
        base.gameObject.SetActive(Screen.width > 960);
    }

    private void Update()
    {
    }
}

