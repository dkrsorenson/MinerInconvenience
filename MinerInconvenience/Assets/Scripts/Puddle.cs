using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Puddle : MonoBehaviour
{
    public GameObject chocolateDroplet;
    public GameObject chocolateSplashPrefab;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void PlaySplashParticles(Transform spawnPoint)
    {
        var splash = Instantiate(chocolateSplashPrefab);

        // move the particles to the correct position
        splash.GetComponent<ParticleSystem>().transform.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y, 0f);

        // play the splash particles
        splash.GetComponent<ParticleSystem>().Play();
    }

    public void Splash(Transform spawnPoint)
    {
        var random = Random.Range(5, 10);
        for (int i = 0; i < random; i++)
        {
            GameObject droplet = Instantiate(chocolateDroplet);
            droplet.transform.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y, 0f);
            var randomX = Random.Range(-1.0f, 1.0f);
            var randomY = Random.Range(-1.0f, 1.0f);
            var direction = new Vector2(randomX, randomY).normalized;
            var xForce = (Vector2.right * direction.x).x * 5.0f;
            var yForce = (Vector2.up * direction.y).y * 2.0f;
            droplet.GetComponent<Rigidbody2D>().AddForce(new Vector2(xForce, yForce), ForceMode2D.Impulse);


            //droplet.GetComponent<PuddleDroplet>().Move();
            //droplet.transform.rotation = Quaternion.Euler(droplet.gameObjectSetAct * new Vector3(1, 1, 1));
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "ChocolateDroplet")
        {
            Destroy(collision.gameObject);
        }
    }
}
