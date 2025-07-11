using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Coin : MonoBehaviour
{
    void Start()
    {
        
    }

    
    void Update()
    {
        transform.Rotate(20 * Time.deltaTime, 0, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            FindObjectOfType<AudioManager>().PlaySound("PickUpCoin");
            PlayerManager.numberOfCoins += 1;  
            Debug.Log("Coins:" + PlayerManager.numberOfCoins);
            Destroy(gameObject);
        }
    }
}
