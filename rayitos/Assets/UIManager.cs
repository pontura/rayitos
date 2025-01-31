using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.Impl;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject flash;
    [SerializeField] GameObject gameOver;
    [SerializeField] GameObject hiscoreGO;
    [SerializeField] TMPro.TMP_Text field;
    [SerializeField] TMPro.TMP_Text hiscoreTitle;
    [SerializeField] TMPro.TMP_Text hiscoreField;
    int hiscore;
    int score;

    private void Start()
    {
        Reset();
        Restart();
        hiscore = PlayerPrefs.GetInt("hiscore");
    }
    public void Shoot()
    {
        CancelInvoke();
        flash.SetActive(true);
        Invoke("Reset", 0.15f);
    }
    private void Reset()
    {        
        flash.SetActive(false);
        SetScore();
    }
    public void Gameover()
    {
        field.gameObject.SetActive(false);
        gameOver.SetActive(true);
    }
    public void Added()
    {
        if (score == 0)
            field.text = "now!";
    }
    public void Restart()
    {
        score = 0;
        field.text = "wait...";
        field.gameObject.SetActive(true);
        gameOver.SetActive(false);
        field.gameObject.SetActive(true);
        gameOver.SetActive(false);
        hiscoreGO.SetActive(false);
    }
    public void SetScore(int add)
    {
        score += add; 
        SetScore();
    }
    void SetScore()
    {
        field.text = score.ToString();
    }
    public void SetScoreScreen()
    {
        hiscoreGO.SetActive(true);
        if (score > hiscore)
        {
            hiscore = score;
            PlayerPrefs.SetInt("hiscore", score);
            SetHiscore();
        }
        else
        {
            hiscoreTitle.text = "SCORE";
            hiscoreField.text = score.ToString();
            Invoke("SetHiscore", 1);
        }
    }
    void SetHiscore()
    {
        hiscoreGO.SetActive(true);
        if(score == hiscore)
            hiscoreTitle.text = "NEW HI-SCORE";
        else
             hiscoreTitle.text = "HI-SCORE";

        hiscoreField.text = hiscore.ToString();
    }
}
