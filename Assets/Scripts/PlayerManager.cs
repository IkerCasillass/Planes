using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    [Header("Speed")]
    public float speed = 10f;
    public float speedY = 2f;
    [Header("Limites")]
    public float limiteX = 7.5f;

    [Header("Disparo")] 
    public GameObject prefabDisparo;
    public float disparoSpeed =8f;
    public float timeDisparoDestroy = 2f;
    private float cooldown = 0.08f;
    private bool canShoot = true;

    public Transform weapon1;
    public Transform weapon2;
    private bool isFire = false;
    private Rigidbody2D rb;
    

    [Header("Vida")]
    public int vida = 20;


    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();

        if (Input.GetAxis("Fire1") == 1f && canShoot)
        {
            StartCoroutine(disparar());
        }


        //StartFire();
    }

    public void MovePlayer()
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, speedY);
        if (transform.position.x > limiteX)
        {
            transform.position = new Vector2(limiteX, transform.position.y);
        }
        else if(transform.position.x < -limiteX)
        {
            transform.position = new Vector2(-limiteX, transform.position.y);
        }
    }


    IEnumerator disparar()
    {

        //Instantiate your projectile

        isFire = true;
        GameObject disparoInstance = Instantiate(prefabDisparo);
        disparoInstance.transform.SetParent(transform.parent);

        disparoInstance.transform.position = weapon1.position;
        disparoInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, disparoSpeed);
        Destroy(disparoInstance, timeDisparoDestroy);

        GameObject disparoInstance2 = Instantiate(prefabDisparo);
        disparoInstance2.transform.SetParent(transform.parent);
        disparoInstance2.transform.position = weapon2.position;
        //Move
        disparoInstance2.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, disparoSpeed);
        Destroy(disparoInstance2, timeDisparoDestroy);

        canShoot = false;

        //wait for some time
        yield return new WaitForSeconds(cooldown);
        canShoot = true;
    }


    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "disparoEnemigo")
        {
            gameObject.SetActive(false);
            Destroy(collision.gameObject);
            GameOver();
        }
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
        {
            if (otherCollider.tag == "disparoEnemigo")
            {
                Debug.Log("Hit by enemy shot");
                vida--;
                Destroy(otherCollider.gameObject);

                if (vida <= 0)
                {
                    // El jugador ha perdido todas sus vidas
                    gameObject.SetActive(false);
                    GameOver();
                }
            }
        }

    public void GameOver()
    {
        SceneManager.LoadScene(1);

    }



}

