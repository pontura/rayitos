using UnityEngine;

public class InputManager : MonoBehaviour
{
    float timer;
    float delayToShoot;
    float initialDelayToShoot = 0.25f;
    float minDelayToShoot = 0.05f;
    float shootDecrease = 0.075f;

    states state;
    enum states
    {
        idle,
        pressed,
        released
    }
    GameManager gameManager;
    private void Start()
    {
        gameManager = GetComponent<GameManager>();  
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            timer = 0;
            delayToShoot = initialDelayToShoot;
            Shoot(); 
            state = states.pressed;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            state = states.released;
            state = states.idle;
            gameManager.EndShot();
        }

        if (state != states.idle)
            timer += Time.deltaTime;
        if (state == states.pressed)
        {
            if (timer > delayToShoot)
            {
                delayToShoot -= shootDecrease;
                if (delayToShoot < minDelayToShoot)
                    delayToShoot = minDelayToShoot;
                gameManager.EndShot();
                Shoot();
            }
        }
        else if (state != states.idle && timer > delayToShoot)
        {
            state = states.idle;
            gameManager.EndShot();
        }
    }
    void Shoot()
    {
        print("Shoot");
        timer = 0;
        gameManager.Shoot();
    }
}
