using UnityEngine;

public class Explotion : MonoBehaviour
{
    System.Action<Explotion> OnExplotionDone;
    public void Init(Vector2 pos, System.Action<Explotion> OnExplotionDone)
    {
        this.OnExplotionDone = OnExplotionDone;
        transform.position = pos;
        Invoke("Reset", 1);
    }
    private void Reset()
    {
        OnExplotionDone(this);
    }
}
