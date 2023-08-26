using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    GameObject healingText;    //temporary

    [SerializeField] Rigidbody2D rb;

    public int maxHealth = 100;
    public int currentHealth;
    int missingHealth;

    int wineStrength = 50;  //Effectivity of heal
    int drinkSpeed = 2;

    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        healingText = GameObject.Find("HealingText");   //temporary
        healingText.SetActive(false);                   //temporary

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        InputHandler();
    }

    void InputHandler()
    {
        //Healing with wine flask
        if (Input.GetKeyDown(KeyCode.R) && currentHealth < maxHealth)    //<-- в цій іфці ще треба перевіряти чи гравець торкається землі але 
                                                          // я не знаю чи краще заново це все перевіряти, чи зробити в MovePlayer цю перевірку публічною (якщо так можна)
        {
            healingText.SetActive(true);    //temporary
            GetComponent<MovePlayer>().enabled = false;
            rb.velocity = new Vector2(0, 0);
            //в цьому рядку ще має бути вмикання анімації пиття (коли зробиш її)

            Invoke("GetHeal", drinkSpeed);
        }

        //Getting damage
        if (Input.GetKeyDown(KeyCode.X))
        {
            TakeDamage(20);
        }
    }

    void GetHeal()
    {
        healingText.SetActive(false);    //temporary
        missingHealth = maxHealth - currentHealth;

        currentHealth += Mathf.Clamp(wineStrength, 0, missingHealth);
        healthBar.SetHealth(currentHealth);
        
        GetComponent<MovePlayer>().enabled = true;
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }
}
