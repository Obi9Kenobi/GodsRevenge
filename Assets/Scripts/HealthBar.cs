using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private GameObject drinkingText;    //temporary
    private void Start()
    {        
        drinkingText = GameObject.Find("HealingText");   //temporary
        drinkingText.SetActive(false);                   //temporary
    }
    public Slider slider;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }

    public void ShowDrinkingText(bool state)
    {
        drinkingText.SetActive(state);      //temporary
    }
}
