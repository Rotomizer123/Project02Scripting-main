using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{
    static Slider healthBar = null;

    public void Awake()
    {
        healthBar = this.GetComponent<Slider>();
    }

    public static void HealthBar(float currentHealth)
    {
        healthBar.value = currentHealth;
    }
}
