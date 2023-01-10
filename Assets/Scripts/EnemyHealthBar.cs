using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider HealthSlider;
    public Color Low;
    public Color High;
    private Vector3 Offset = new Vector3(0,3,0);
    
    
    public void SetHealth(int health, int maxHealth)
    {
        HealthSlider.gameObject.SetActive(health < maxHealth);
        HealthSlider.value = health;
        HealthSlider.maxValue = maxHealth;

        HealthSlider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(Low, High, HealthSlider.normalizedValue);
    }

    // Update is called once per frame
    void Update()
    {
        HealthSlider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + Offset);
    }
}
