using UnityEngine;

public class Enemy : MonoBehaviour
{
    float speed = 10;
    public int direction;
    states state;
    enum states
    {
        playing,
        shooted,
        dead
    }

    public void Init(float pos, int direction)
    {
        state = states.playing;
        this.direction = direction;
        transform.position = new Vector2(pos, Random.Range(-4,4));
    }
    public bool IsActived()
    {
        return state != states.dead;  
    }
    public void Move()
    {
        if (state != states.playing) return;
        float s = Time.deltaTime * direction * speed;
        Vector2 pos = transform.position;
        pos.x += s;
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
