using UnityEngine;

public class CombosManager : MonoBehaviour
{
    [SerializeField] GameObject panelBig;
    [SerializeField] TMPro.TMP_Text buttonFieldBig;
    [SerializeField] GameObject panelSmall;
    [SerializeField] TMPro.TMP_Text buttonFieldSmall;
    [SerializeField] int num;
    [SerializeField] int max;

    public void Init()
    {
        num = 0;
        max = 0;    
        Reset();
    }
    void Reset()
    {
        panelBig.SetActive(false);
        panelSmall.SetActive(false);
    }
    public void EndShotSequence()
    {
        panelSmall.SetActive(false);
        if (num>max)
        {
            max = num;
            SetBigOn();
        }
        num = 0;
    }
    public void Add()
    {
        num++;
        if (num > 1)
        {
            panelSmall.SetActive(true);
            buttonFieldSmall.text = num.ToString();
        }
    }
    public void SetBigOn()
    {
        panelSmall.SetActive(false);
        panelBig.SetActive(true);
        buttonFieldBig.text = max.ToString();
        Invoke("Reset", 0.5f);
    }
}
