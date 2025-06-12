using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaIndicator : MonoBehaviour
{
    float maxValue;
    [SerializeField] PlayerController player;
    Image image;
    void Start()
    {
        image = this.GetComponent<Image>();
        maxValue = player.GetStaminaMax();
    }

    void Update()
    {
        image.fillAmount = Mathf.Clamp(player.GetStaminaCurrent() / maxValue, 0, 1);
    }
}
