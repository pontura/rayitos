using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;
using YaguarLib.Pool;

public class GameManager : MonoBehaviour
{
    float timerToAdd;
    float maxTimerToAdd = 1.9f;
    int level = 1;
    float minTimerToAdd = 0.018f;
    float timerDesc = 0.05f;
    float timer;
    float timeTeSafeArea = 12;

    float initialEnemySpeed = 1.5f;
    [SerializeField] float enemySpeed;
    float maxEnemySpeed = 4;
    float enemySpeedAcceleration = 3;

    public  PoolObjects pool;
    [SerializeField] Explotion explotion;
    EnemiesManager enemiesManager;

    [SerializeField] Transform container;
    [SerializeField] UIManager ui;
    states state;

    int life;
    enum states
    {
        playing,
        done
    }
    void Start()
    {
        enemiesManager = GetComponent<EnemiesManager>();
        enemiesManager.Init();
        Restart();
    }
    void Restart()
    {
        enemySpeed = initialEnemySpeed;
        level = 1;
        timer = 0;
        life = 3;
        ui.Restart();
        enemiesManager.Restart();

        CancelInvoke();

        state = states.playing;
        timerToAdd = maxTimerToAdd;
        Invoke("Loop", timerToAdd);
    }
    void Loop()
    {
        if (timer > (timeTeSafeArea+ level )* level)
            InitSafeArea();
        else
        {
            if (state == states.playing)
            {
                timerToAdd -= timerDesc;
                if (timerToAdd < minTimerToAdd)
                    timerToAdd = minTimerToAdd;
                AddEnemy();
            }
            Invoke("Loop", UnityEngine.Random.Range(minTimerToAdd, timerToAdd) + UnityEngine.Random.Range(0, 0.2f));
        }
    }
    void InitSafeArea()
    {
        print("InitSafeArea level" + level);
        level++;
        timerToAdd += timerDesc * UnityEngine.Random.Range(0.5f, 0.5f + level);
        Invoke("Loop", UnityEngine.Random.Range(1f, 4.1f));
    }
    void AddEnemy()
    {
        ui.Added();
        enemiesManager.AddEnemy(); 
    }
    private void Update()
    {
        timer += Time.deltaTime;

        if(enemySpeed>maxEnemySpeed) enemySpeed = maxEnemySpeed;
        else enemySpeed += (enemySpeedAcceleration/100) * Time.deltaTime;

        enemiesManager.OnUpdate(enemySpeed);
    }
    public void Kill(Enemy e, bool hasWon = true)
    {
        ui.SetScore(1);
        if (hasWon)
            GameOver();
        enemiesManager.Kill(e);
        if (enemiesManager.Count() <= 0)
            ui.Empty();
    }
    void OnDie(Enemy e)
    {
        Pool(e);
    }
    public void Pool(Enemy e)
    {
        print("Pool " + e);
        pool.Pool(e.gameObject);
    }
    void GameOver()
    {
        CancelInvoke();
        state = states.done;
        Invoke("GameOverScreen", 0.5f);
    }
    void GameOverScreen()
    {
        ui.Gameover();
        Invoke("SetHiscores", 1);
    }
    void SetHiscores()
    {
        ui.SetScoreScreen();
        Invoke("Restart", 4);
    }
   
    Enemy enemyShooted;
    public void Shoot(int touchID)
    {
        ui.Shoot();
        int enemiesCount = enemiesManager.Count();
        if (enemiesCount <= 0)
            GameOver();
        else
        {
            if (enemiesCount > 0)
            {
                enemyShooted = enemiesManager.GetEnemy();
                enemyShooted.Shooted();
                Vector2 pos = enemyShooted.transform.position;
                AddExplotion(pos);
            } else
                GameOver();
        }
    }
    public void EndShot(int touchID)
    {
        if (enemyShooted == null) return;
        Kill(enemyShooted, false);
        enemyShooted = null;
    }
    void AddExplotion(Vector2 pos)
    {
        GameObject go = pool.Get("explotion");
        go.GetComponent<Explotion>().Init(pos, ExplotionDone);
    }
    void ExplotionDone(Explotion explotion)
    {
        pool.Pool(explotion.gameObject);
    }
}
