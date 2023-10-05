using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavoiur : MonoBehaviour
{
    [Header("Speed")]
    public float speed = 2f;

    
    [Header("Disparo")] 
    public GameObject prefabDisparo;
    public float disparoSpeed =2f;
    public float shootingInterval = 10f;
    public float timeDisparoDestroy = 2f;
    
    private float _shootingTimer;
    
    public Transform weapon1;
    public Transform weapon2;
    private bool isShootingPaused = false;
    private float shootingPauseTime = 4.0f;

    public float horizontalSpeed = 1.0f; // Velocidad de movimiento lateral
    public float horizontalRange = 15.0f; // Rango de movimiento horizontal
    private bool isMovingRight = true; // Variable para controlar la dirección

    public delegate void DeadEnemyDelegate();
    public event DeadEnemyDelegate OnDeadEnemy;

    [Header("Vida")]
    public int vida = 3;


    // Start is called before the first frame update
    void Start()
    {
        _shootingTimer = Random.Range (0f, shootingInterval);
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, speed);
        GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0);
    }

    // Update is called once per frame
    void Update()
    {
        StartFire();

        // Cambiar la dirección cuando el enemigo alcanza los límites
        if (transform.position.x >= horizontalRange && isMovingRight)
        {
            isMovingRight = false;
            GetComponent<Rigidbody2D>().velocity = new Vector2(-horizontalSpeed, 0);
        }
        else if (transform.position.x <= -horizontalRange && !isMovingRight)
        {
            isMovingRight = true;
            GetComponent<Rigidbody2D>().velocity = new Vector2(horizontalSpeed, 0);
        }
    }
    
    public void StartFire()
    {
        _shootingTimer -= Time.deltaTime;
        if (!isShootingPaused && _shootingTimer <= 0f)
        {
            _shootingTimer = shootingInterval;
           GameObject disparoInstance = Instantiate(prefabDisparo);
           disparoInstance.transform.SetParent(transform.parent);
           disparoInstance.transform.position = weapon1.position;
           
           disparoInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, disparoSpeed);
           Destroy(disparoInstance,timeDisparoDestroy);
                    
           GameObject disparoInstance2 = Instantiate(prefabDisparo);
           disparoInstance2.transform.SetParent(transform.parent);
           disparoInstance2.transform.position = weapon2.position;
           
           disparoInstance2.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, disparoSpeed);
           Destroy(disparoInstance2,timeDisparoDestroy);
            
        }
    }
    
    void OnTriggerEnter2D (Collider2D otherCollider) {
        //otherCollider.gameObject.GetComponent<DisparoBehaviour>()
        if (otherCollider.tag == "disparoPlayer" || otherCollider.tag == "Player") {
            Debug.Log("hit");
            
            if (vida == 0) {
                gameObject.SetActive(false);
                Destroy(otherCollider.gameObject);

                // Notificar al Spawner que el globo se ha reventado
                OnDeadEnemy?.Invoke();
            }
            else
            {
                vida--;
                StartCoroutine(PauseShooting());
            }
            
        }
    }

    // Corrutina para pausar el disparo
    private IEnumerator PauseShooting()
    {
        isShootingPaused = true;
        yield return new WaitForSeconds(shootingPauseTime);
        isShootingPaused = false;
    }


    public void OnBecameInvisible()
    {
        // Cuando el objeto se vuelve invisible, destrúyelo
        Destroy(gameObject);
    }

}
