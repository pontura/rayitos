using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class InputManager : MonoBehaviour
{
    float timer;
    float delayToShoot;
    float initialDelayToShoot = 0.25f;
    float minDelayToShoot = 0.05f;
    float shootDecrease = 0.075f;

    states state1;
    states state2;

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
    int touchID = 0;
    void Update()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (Input.touchCount >0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                timer = 0;
                delayToShoot = initialDelayToShoot;
                Shoot(0);
                state1 = states.pressed;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended)
            {
                state1 = states.idle;
                gameManager.EndShot(0);
            }
            if (Input.touchCount > 1)
            {
                if (Input.touches[1].phase == TouchPhase.Began)
                {
                    timer = 0;
                    delayToShoot = initialDelayToShoot;
                    Shoot(1);
                    state2 = states.pressed;
                }
                else if (Input.touches[1].phase == TouchPhase.Ended)
                {
                    state2 = states.idle;
                    gameManager.EndShot(1);
                }
            }
        }
        timer += Time.deltaTime;
        if (state1 == states.pressed)
        {
            if (timer > delayToShoot)
            {
                delayToShoot -= shootDecrease;
                if (delayToShoot < minDelayToShoot)
                    delayToShoot = minDelayToShoot;
                gameManager.EndShot(0);
                Shoot(0);
            }
        }
        else if (state1 != states.idle && timer > delayToShoot)
        {
            state1 = states.idle;
            gameManager.EndShot(0);
        }



        if (state2 == states.pressed)
        {
            if (timer > delayToShoot)
            {
                delayToShoot -= shootDecrease;
                if (delayToShoot < minDelayToShoot)
                    delayToShoot = minDelayToShoot;
                gameManager.EndShot(1);
                Shoot(1);
            }
        }
        else if (state2 != states.idle && timer > delayToShoot)
        {
            state2 = states.idle;
            gameManager.EndShot(1);
        }





#else

        if (Input.GetMouseButtonDown(0))
        {
            timer = 0;
            delayToShoot = initialDelayToShoot;
            Shoot(touchID);
            state = states.pressed;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            state = states.released;
            state = states.idle;
            gameManager.EndShot(touchID);
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
                gameManager.EndShot(touchID);
                Shoot(touchID);
            }
        }
        else if (state != states.idle && timer > delayToShoot)
        {
            state = states.idle;
            gameManager.EndShot(touchID);
        }
#endif

    }
    void Shoot(int touchID)
    {
        print("Shoot");
        timer = 0;
        gameManager.Shoot(touchID);
    }
}
