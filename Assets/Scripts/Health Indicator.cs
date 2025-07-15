using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthIndicator : MonoBehaviour
{
    [SerializeField] PlayerController player;
    Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        float fillAmount = (float)player.GetCurrentHealth() / player.GetMaxHealth();
        image.fillAmount = Mathf.Clamp01(fillAmount);
    }
}

