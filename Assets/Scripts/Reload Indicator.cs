using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadIndicator : MonoBehaviour
{
    float maxValue;
    [SerializeField] PlayerController player;
    Image image;
    void Start()
    {
        image = this.GetComponent<Image>();
        maxValue = player.getRangedAttackCooldown();
    }

    void Update()
    {
        image.fillAmount = Mathf.Clamp(player.getRangedAttackTimer() / maxValue, 0, 1);
    }
}
