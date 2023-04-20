using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient grad;
    public Image fill;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = grad.Evaluate(1f);
    }

    public void setHealth(int health)
    {
        slider.value = health;
        fill.color = grad.Evaluate(slider.normalizedValue);
    }
}
