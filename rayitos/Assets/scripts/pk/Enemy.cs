using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Enemy : MonoBehaviour
{
    float speed;
    states state;
    Vector2 pos;
    enum states
    {
        playing,
        shooted,
        dead
    }
    public void Init(float pos_x, float pos_y, float speed)
    {
        this.speed = speed;
        this.speed += Random.Range(0f, 1.5f);
        pos = new Vector2(pos_x, pos_y);
        state = states.playing;
        transform.position = pos;
    }
    public bool IsActived()
    {
        return state != states.dead;  
    }
    public void Move()
    {
        if (state != states.playing) return;
        Vector2 pos = transform.position;
        pos.y -= Time.deltaTime * speed;
        transform.position = pos;
    }
    public void Shooted()
    {
        state = states.shooted;
    }
    public void Die()
    {
        state = states.dead;
    }
}
