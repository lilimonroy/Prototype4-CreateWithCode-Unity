using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float powerupStreght = 15.0f;
    public bool haspowerup = false;

    private Rigidbody playerRB;
    private GameObject focalPoint;
    public GameObject powerupIndicator;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        //Because our GameObject is in the scene we can use FIND
        focalPoint = GameObject.Find("Focal Point");
        //The line down is a WRONG instruction because the function FIND just works with ACTIVE GameObjects.
        //powerupIndicator = GameObject.Find("Powerup Indicator");
        powerupIndicator.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        float fowardInput = Input.GetAxis("Vertical");

        //playerRB.AddForce(Vector3.forward * speed * fowardInput);
        playerRB.AddForce(focalPoint.transform.forward * speed * fowardInput);
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);
            haspowerup = true;
            StartCoroutine(powerupCountdownRoutine());
            powerupIndicator.gameObject.SetActive(true);
        }
    }

    IEnumerator powerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        haspowerup = false;
        powerupIndicator.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && haspowerup)
        {
            Rigidbody enemyRB = collision.gameObject.GetComponent<Rigidbody>();
            //Vector Direction -> EnemyPos - PlayerPos
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);

            Debug.Log("Collide with " + collision.gameObject.name + " with powerup set to" + haspowerup);

            //Add an impulse to away the enemy with the powerup
            enemyRB.AddForce(awayFromPlayer * powerupStreght, ForceMode.Impulse);
        }
    }
}
