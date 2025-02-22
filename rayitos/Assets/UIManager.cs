using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject flash;

    [SerializeField] GameObject hiscoreInGame;
    [SerializeField] Image hiscoreBar;
    [SerializeField] TMPro.TMP_Text hiscoreFieldInGame;

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

        if (hiscore > 0) SetHiscoreInGame();
        else  hiscoreInGame.SetActive(false);
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
        hiscoreBar.fillAmount = 0;
        score = 0;
        field.text = "wait...";
        field.gameObject.SetActive(true);
        gameOver.SetActive(false);
        field.gameObject.SetActive(true);
        gameOver.SetActive(false);
        hiscoreGO.SetActive(false);
        SetHiscoreInGame();
    }
    public void SetScore(int add)
    {
        score += add; 
        SetScore();
        UpdateHiscoreInGame();
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
    void SetHiscoreInGame()
    {
        if (hiscore == 0) return;
        hiscoreInGame.SetActive(true);
        hiscoreFieldInGame.text = hiscore.ToString();
    }
    void UpdateHiscoreInGame()
    {
        if (hiscore == 0) return;
        if(score > hiscore)
            hiscoreInGame.SetActive(false);
        else
            hiscoreBar.fillAmount = (float)score/ (float)hiscore;
    }
}
