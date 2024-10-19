using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageTimer : MonoBehaviour
{
    public float maxTime = 5f;
    public bool tick = false;

    private Image image;
    private float currentTime;

    void Start()
    {
        image = GetComponent<Image>();
        currentTime = maxTime;
    }

    void Update()
    {   
        tick = false;
        currentTime -= Time.deltaTime;

        if (currentTime <= 0) {
            currentTime = maxTime;
            tick = true;
        }

        image.fillAmount = currentTime / maxTime;
    }

    public void ResetTimer() {
        tick = false;
        currentTime = maxTime;
    }
}
