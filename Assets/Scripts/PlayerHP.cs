using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    public MovePlayer movePlayerScript;

    [SerializeField] Rigidbody2D rb;

    public int maxHealth = 100;
    public int currentHealth;
    private int missingHealth;

    private int healPower = 50;  //Effectivity of heal
    private int drinkingDuration = 2;
    public HealthBar healthBar;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        movePlayerScript = rb.GetComponent<MovePlayer>();
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        //Healing with wine flask
        if (Input.GetKeyDown(KeyCode.R) && (currentHealth < maxHealth) && movePlayerScript.IsGrounded())
        {
            DrinkFlask();
        }

        //Getting damage
        if (Input.GetKeyDown(KeyCode.X))
        {
            TakeDamage(20);
        }
    }

    private void DrinkFlask()
    {
        healthBar.ShowDrinkingText(true);
        FreezePlayer(true);
        Invoke("GetHeal", drinkingDuration);
    }
    private void GetHeal()
    {
        missingHealth = maxHealth - currentHealth;
        currentHealth += Mathf.Clamp(healPower, 0, missingHealth);
        healthBar.SetHealth(currentHealth);
        FreezePlayer(false);
        healthBar.ShowDrinkingText(false);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    public void FreezePlayer(bool state)
    {
        if (state) {
            GetComponent<MovePlayer>().enabled = false;
            rb.velocity = new Vector2(0, 0);
        }
        else
        {
            GetComponent<MovePlayer>().enabled = true;
        }
        
    }
}
