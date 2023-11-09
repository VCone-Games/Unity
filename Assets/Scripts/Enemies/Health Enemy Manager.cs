using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEnemyManager : MonoBehaviour
{
    [Header("Health enemy params")]
    [SerializeField] private int max_health;
    [SerializeField] private int current_health;

    [Header("Enemy components")]
    [SerializeField] private Enemy enemyComponent;

    // Start is called before the first frame update
    void Start()
    {
        enemyComponent = GetComponent<Enemy>();
        current_health = max_health;
    }

	public void TakeDamage(int damage)
	{
		// Implementa cómo el enemigo maneja el daño
		current_health -= damage;
		if (current_health <= 0)
		{
            enemyComponent.EventDie?.Invoke(this, null);
		}
	}

}
