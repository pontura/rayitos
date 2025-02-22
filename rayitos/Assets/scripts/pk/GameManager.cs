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

    [SerializeField] PoolObjects pool;
    [SerializeField] Explotion explotion;
    Vector2 initialLimits = new Vector2(-10, 10);
    Vector2 limits;
    float wallIncrease = 1;
    List<Enemy> enemies;
    [SerializeField] Enemy heart_to_add;
    [SerializeField] List<GameObject> walls;
    [SerializeField] Transform container;
    [SerializeField] Transform[] hearts_container;
    [SerializeField] UIManager ui;
    RaysManager raysManager;
    states state;

    int life;
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
        level = 1;
        timer = 0;
        life = 3;
        ui.Restart();
        if (enemies.Count>0)
        foreach (Enemy enemy in enemies)
            Pool(enemy);
        enemies.Clear();
        AddHearts();

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
        timer += Time.deltaTime;
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
        Scape();
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
    void ShootWalls()
    {
        return;
        limits.x += wallIncrease;
        limits.y -= wallIncrease;
        UpdateWalls();
    }
    Enemy enemyShooted;
    public void Shoot(int touchID)
    {
        ui.Shoot();
        if (enemies.Count <=0)
            ShootWalls();
        else
        {
            enemyShooted = GetEnemy();
            enemyShooted.Shooted();
            Vector2 pos = enemyShooted.transform.position;
            raysManager.Init(pos, touchID);
            AddExplotion(pos);
        }
    }
    void Scape()
    {
        enemyShooted = LoseLife();
        Kill(enemyShooted, false);
    }
    Enemy GetEnemy()
    {
        print(enemies.Count);
        if (enemies.Count > life)
            return enemies[life];
        else
            return LoseLife();
    }
    Enemy LoseLife()
    {

        life--;
        if (life <= 0)
            GameOver();
        return enemies[0];
    }
    public void EndShot(int touchID)
    {
        raysManager.SetOff(touchID);
        if (enemyShooted == null) return;
        Kill(enemyShooted, false);
        enemyShooted = null;
    }
    public void AddHearts()
    {
        for (int a = 0; a < life; a++) 
        {
            AddHeart(a);    
        }
    }
    public void AddHeart(int id)
    {
        Enemy e = Instantiate(heart_to_add, hearts_container[id]);
        enemies.Add(e);
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
