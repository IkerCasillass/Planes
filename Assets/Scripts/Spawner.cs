using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour

{
    public GameObject enemigoPrefab; //Que va spawnear
    public GameObject[] enemigos;
    public int spawnCount = 1; //cuantos
    //public float spawnOffset; //Limites establecer la region
    public float minXOffset = -8.5f; // Valor mínimo del offset
    public float maxXOffset = 8.5f;  // Valor máximo del offset
    public float minYOffset = -2f; // Valor mínimo del offset
    public float maxYOffset = 2f;  // Valor máximo del offset


    private int deadEnemies = 0;

    private bool canSpawn = true;
    private float spawnRate = 3f;

    // Start is called before the first frame update
    void Start()
    {

        if (enemigoPrefab != null)
        {
            //Spawn
            StartCoroutine(SpawnerControl());
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogError("No tiene enemigos asignados");
#endif
        }
    }
    void Update()
    {
        transform.Translate(Vector2.up * 2f * Time.deltaTime);
    }

   

    void SpawnEnemigo()
    {

        for (int i = 0; i < spawnCount; i++)
        {
            float randomXOffset = Random.Range(minXOffset, maxXOffset);
            float randomYOffset = Random.Range(minYOffset, maxYOffset);

            // Calcular las posiciones de spawn en función del transform del Spawner
            Vector3 spawnPosition = transform.position + new Vector3(randomXOffset, randomYOffset, 0);

            GameObject newEnemy = Instantiate(enemigos[GetRandomEnemy()], spawnPosition, Quaternion.identity);


            // Asignar un evento al script del globo para que notifique cuando se reviente
            EnemyBehavoiur enemyScript = newEnemy.GetComponent<EnemyBehavoiur>();
            if (enemyScript != null)
            {
                enemyScript.OnDeadEnemy += OnDeadEnemy;
            }
        }
    }

    int GetRandomEnemy()
    {

        if (enemigos.Length == 0)
        {
            Debug.LogError("El arreglo 'enemigos' está vacío.");
            return 0; // O devuelve un valor predeterminado válido
        }


        int randomEnemy = Random.Range(0, enemigos.Length);
        return randomEnemy;
    }

    void OnDeadEnemy()
    {
        deadEnemies++;

        if (deadEnemies >= spawnCount)
        {
            Debug.Log("All dead");
        }
    }

    private IEnumerator SpawnerControl()
    {
        WaitForSeconds wait = new WaitForSeconds(spawnRate);

        while (canSpawn)
        {
            yield return wait;

            SpawnEnemigo();
        }
    } 




}
