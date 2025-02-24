using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    List<Enemy> enemies;
    GameManager gameManager;
    Vector2 initialLimits = new Vector2(-2, 2);
    Vector2 limits;
    float init_y = 4.5f;
    float enemySpeed;

    public void Init()
    {
        limits.x = initialLimits.x;
        limits.y = initialLimits.y;
        gameManager = GetComponent<GameManager>();
        enemies = new List<Enemy>();
    }
    public int Count() { return enemies.Count; }
    public void Restart()
    {
        foreach (Enemy enemy in enemies)
            Pool(enemy);
        enemies.Clear();
    }
    public void AddEnemy()
    {
        GameObject obj = gameManager.pool.Get("Enemy");
        Enemy enemy = obj.GetComponent<Enemy>();
        enemy.Init(Random.Range(limits.x, limits.y), init_y, enemySpeed);
        enemies.Add(enemy);
    }
    public void OnUpdate(float enemySpeed)
    {
        this.enemySpeed = enemySpeed;
        Enemy enemyWon = null;
        foreach (Enemy enemy in enemies)
        {
            if (enemy != null && enemy.IsActived())
            {
                if (enemy.transform.position.y <= -init_y)
                    enemyWon = enemy;
                else
                    enemy.Move();
            }
        }
        if (enemyWon != null)
            gameManager.Kill(enemyWon);
    }
    public Enemy GetEnemy()
    {
        return enemies[0];
    }
    public void Kill(Enemy enemy)
    {
        Pool(enemy);
    }
    void Pool(Enemy enemy)
    {
        enemies.Remove(enemy);
        gameManager.Pool(enemy);
    }
}
