using System.Collections;
using System.Collections.Generic;
using GNW2.Player;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Image healthBar;
    public Slider slider;
    public int actualHealth;
    public int maxActualHealth;
    public Healthbar bar;
    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = bar.maxHealth;

    }

    // Update is called once per frame
    void Update()
    {

        slider.value = bar._currentHealth;
    }
}
