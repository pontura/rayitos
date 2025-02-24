using UnityEngine;

public class ShotButton : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text buttonField;
    Animation shotButtonAnim;
    private void Awake()
    {
        shotButtonAnim = GetComponent<Animation>();
    }
    public void Empty()
    {
        buttonField.text = "DON´t";
        shotButtonAnim.Play("empty");
    }

    public void Active()
    {
        buttonField.text = "SHOT!";
        shotButtonAnim.Play("active");
    }
}
