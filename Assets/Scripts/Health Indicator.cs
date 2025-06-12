using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthIndicator : MonoBehaviour
{
    float maxValue;
    [SerializeField] PlayerController player;
    Image image;
    void Start()
    {
        image = this.GetComponent<Image>();
        maxValue = player.GetRangedAttackCooldown();
    }

    void Update()
    {
        image.fillAmount = Mathf.Clamp(player.GetRangedAttackTimer() / maxValue, 0, 1);
    }
}
