using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;


public class InputManager : MonoBehaviour
{
    [SerializeField] Joystick joystick;
    float timer;
    float delayToShoot;
    float delayToEndShoot;

    float accelerationShot = 10;
    float minDelayToShoot = 0.5f;

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

    float deadZone = 0.1f;

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
    void SetOff()
    {
        if (state == states.pressed) EndShoot();
        state = states.off;
    }
    void Update()
    {

        if (state == states.off) return;
        if (!gameManager.IsPlaying()) { SetOff(); return; }

        float new_y_axis = joystick.Direction.y;
        float new_abs_y_axis = Mathf.Abs(new_y_axis);

        if (state == states.idle && new_abs_y_axis > deadZone && Mathf.Abs(y_axis) < deadZone)
            StartShooting(new_y_axis);
        else if (state == states.pressed && new_abs_y_axis <= deadZone && Mathf.Abs(y_axis) > deadZone)
            EndShoot();

        if (state == states.pressed)
        {
            delayToShoot = new_abs_y_axis;
            timer += Time.deltaTime;
            if (timer > delayToShoot)
                ShotDone(new_y_axis);
        }
        y_axis = new_y_axis;
    }
    void StartShooting(float new_y_axis)
    {
        print("StartShooting " + new_y_axis);
        timer = 0;
        state = states.pressed; 
        gameManager.InitShoot(new_y_axis);
    }
    void EndShoot()
    {
        print("EndShoot" + y_axis);
        gameManager.EndShot();
        state = states.idle;
    }
    void ShotDone(float new_y_axis)
    {
        print("ShotDone" + new_y_axis);
        gameManager.ShotDone(); 
        StartShooting(new_y_axis);
    }
}
