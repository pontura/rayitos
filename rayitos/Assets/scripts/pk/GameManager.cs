using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using YaguarLib.Pool;

public class GameManager : MonoBehaviour
{
    float timerToAdd;
    float maxTimerToAdd = 2;
    float minTimerToAdd = 0.025f;
    float timerDesc = 0.04f;

    [SerializeField] PoolObjects pool;
    Vector2 initialLimits = new Vector2(-10, 10);
    Vector2 limits;
    float wallIncrease = 1;
    List<Enemy> enemies;
    [SerializeField] List<GameObject> walls;
    [SerializeField] Transform container;
    [SerializeField] UIManager ui;
    RaysManager raysManager;
    states state;

    enum states
    {
        playing,
        done
    }
    void Start()
    {
        raysManager = GetComponent<RaysManager>();
        enemies = new List<Enemy>();
        Restart();
    }
    void Restart()
    {
        ui.Restart();
        if (enemies.Count>0)
        foreach (Enemy enemy in enemies)
            Pool(enemy);
        enemies.Clear();

        CancelInvoke();
        limits.x = initialLimits.x;
        limits.y = initialLimits.y;

        state = states.playing;
        timerToAdd = maxTimerToAdd;
        Invoke("Loop", timerToAdd);
        UpdateWalls();
    }
    void Loop()
    {
        if (state == states.playing)
        {
            timerToAdd -= timerDesc;
            if (timerToAdd < minTimerToAdd)
                timerToAdd = minTimerToAdd;
            AddEnemy();
        }
        print("timerToAdd " + timerToAdd);
        Invoke("Loop", UnityEngine.Random.Range(minTimerToAdd, timerToAdd) + UnityEngine.Random.Range(0, 0.2f));
    }
    void AddEnemy()
    {
        ui.Added();
        print("AddEnemy ");
        GameObject obj = pool.Get("Enemy");
        int dir = 1;
        float pos;
        if (Random.Range(0, 10) < 5)
        {
            dir = -1;
            pos = limits.y;
        }
        else
        {
            dir = 1;
            pos = limits.x;
        }
        Enemy enemy = obj.GetComponent<Enemy>();
        enemy.Init(pos, dir);
        enemies.Add(enemy); 
    }
    private void Update()
    {
        Enemy enemyWon = null;
        foreach (Enemy enemy in enemies)
        {
            if(enemy != null && enemy.IsActived())
            {
                if (enemy.direction == -1 && enemy.transform.position.x < limits.x)
                    enemyWon = enemy;
                else if (enemy.direction == 1 && enemy.transform.position.x > limits.y)
                    enemyWon = enemy;
                else
                    enemy.Move();
            }
        }
        if (enemyWon != null)
            Kill(enemyWon);
    }
    void Kill(Enemy e, bool hasWon = true)
    {
        ui.SetScore(1);
        if(hasWon)
            Win(e.direction == -1);

        e.Die();
        Pool(e);
        print("Kill");
    }
    void OnDie(Enemy e)
    {
        Pool(e);
    }
    void Pool(Enemy e)
    {
        print("Pool " + e);
        enemies.Remove(e);
        pool.Pool(e.gameObject);
    }
    void Win(bool left)
    {
        return;
        if (left)
            limits.x += wallIncrease;
        else
            limits.y -= wallIncrease;
        UpdateWalls();
    }
    void UpdateWalls()
    {
        walls[0].transform.position = new Vector2(limits.x,0);
        walls[1].transform.position = new Vector2(limits.y, 0);
        if (walls[0].transform.position.x >= walls[1].transform.position.x)
            GameOver();
    }
    void GameOver()
    {
        CancelInvoke();
        state = states.done;
        ui.Gameover();
        Invoke("SetHiscores", 1);
    }
    void SetHiscores()
    {
        ui.SetScoreScreen();
        Invoke("Restart", 4);
    }
    void ShootWalls()
    {
        GameOver();
        return;
        limits.x += wallIncrease;
        limits.y -= wallIncrease;
        UpdateWalls();
    }
    Enemy enemyShooted;
    public void Shoot()
    {
        ui.Shoot();
        if (enemies.Count <=0)
            ShootWalls();
        else
        {
            enemyShooted = enemies[0];
            enemyShooted.Shooted();
            raysManager.Init(enemyShooted.transform.position, 0);
        }
    }
   
    public void EndShot()
    {
        raysManager.SetOff(0);
        if (enemyShooted == null) return;
        Kill(enemyShooted, false);
        enemyShooted = null;
    }
}
