using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    List<Enemy> enemies;
    GameManager gameManager;
    Vector2 initialLimits = new Vector2(-2, 2);
    Vector2 limits;
    float init_z = 6f;
    float enemySpeed;
    [SerializeField] bool bombOnScreen;

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
        bombOnScreen = false;
        foreach (Enemy enemy in enemies)
            Pool(enemy);
        enemies.Clear();
    }
    public void AddEnemy()
    {
        GameObject obj;

        if (bombOnScreen || Random.Range(0, 10)<7)     
            obj = gameManager.pool.Get("Enemy_Simple"); 
        else
        {
            bombOnScreen = true;
            obj = gameManager.pool.Get("Enemy_Bomb");
        }

        Enemy enemy = obj.GetComponent<Enemy>();
        enemy.Init(Random.Range(limits.x, limits.y), init_z, enemySpeed);
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
                if (enemy.transform.position.z <= -init_z)
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
    public void BombActivated()
    {
        int i = enemies.Count;
        while(i>0)
        {
            Enemy enemy = enemies[i-1];
            i--;
            gameManager.Kill(enemy, false);
        }
    }
    public void Kill(Enemy enemy)
    {
        if(enemy.type == Enemy.types.bomb)
            bombOnScreen = false;
        Pool(enemy);
    }
    void Pool(Enemy enemy)
    {
        enemies.Remove(enemy);
        gameManager.Pool(enemy);
    }
}
