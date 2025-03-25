using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;


public class InputManager : MonoBehaviour
{
    [SerializeField] Joystick joystick;
    float timer;
    float delayToShoot;
    float initialDelayToShoot = 0.5f;
    float minDelayToShoot = 0.05f;
    float shootDecrease = 0.075f;

    states state1;
    states state2;

    [SerializeField] states state;
    enum states
    {
        off,
        idle,
        pressed,
        released
    }
    GameManager gameManager;

    float deadZone = 0.01f;

    float y_axis;
    private void Start()
    {
        gameManager = GetComponent<GameManager>();
        Invoke("Init", 1);

    }

    

    public void Init()
    {
        state = states.idle;
    }
    void Update()
    {
        if (state == states.off) return;
        float new_y_axis = joystick.Direction.y;
        float new_abs_y_axis = Mathf.Abs(new_y_axis);

        if (new_abs_y_axis > deadZone && y_axis == 0)
        {
            print("press " + new_y_axis);
            timer = 0;
            delayToShoot = initialDelayToShoot;
            Shoot(new_y_axis);
            state = states.pressed; 
        }
        else if (new_abs_y_axis <= deadZone && Mathf.Abs(y_axis) > deadZone)
        {
            print("release ");
            state = states.released;
            state = states.idle;
            gameManager.EndShot();
        }
        y_axis = new_y_axis;

        if (state != states.idle)
            timer += Time.deltaTime;
        if (state == states.pressed)
        {
            if (timer > delayToShoot)
            {
                delayToShoot -= new_abs_y_axis/100;
                if (delayToShoot < minDelayToShoot)
                    delayToShoot = minDelayToShoot;
                gameManager.ShotDone();
                Shoot(y_axis);
            }
        }
        else if (state != states.idle && timer > delayToShoot)
        {
            state = states.idle;
            gameManager.ShotDone();
        }

    }
    void Shoot(float y_axis)
    {
        print("Shoot" + y_axis);
        timer = 0;
        gameManager.Shoot(y_axis);
    }
}
