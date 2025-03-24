using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Enemy : MonoBehaviour
{
    float speed;
    public states state;
    public types type;
    Vector3 pos;

    public enum states
    {
        playing,
        shooted,
        dead
    }
    public enum types
    {
        simple,
        bomb
    }
    public void Init(float pos_x, float pos_z, float speed)
    {
        this.speed = speed;
        this.speed += Random.Range(0f, 1.5f);
        pos = new Vector3(pos_x, 0, pos_z);
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
        Vector3 pos = transform.position;
        pos.z -= Time.deltaTime * speed;
        transform.position = pos;
    }
    public virtual void Shooted()
    {
        state = states.shooted;
    }
    public void Die()
    {
        state = states.dead;
    }
}
