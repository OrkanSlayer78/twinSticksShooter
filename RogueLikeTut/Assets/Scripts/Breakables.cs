using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakables : MonoBehaviour
{
    public GameObject[] brokenPieces;
    public int maxPieces = 10;

    public bool shouldDropItem;
    public GameObject[] itemsToDrop;
    public float itemDropPercent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
     if(other.tag == "Player")
        {
            if (PlayerController.instance.dashCounter > 0)
            {
                Destroy(gameObject);
                AudioManager.instance.PlaySFX(0);
                
                //show debris
                int piecesToDrop = Random.Range(1, maxPieces);

                for(int i = 0; i<piecesToDrop; i++)
                {
                    int randomPiece = Random.Range(0, brokenPieces.Length);
                    Instantiate(brokenPieces[randomPiece], transform.position, transform.rotation);
                }

                //random item drop
                if (shouldDropItem)
                {
                    float dropChance = Random.Range(0f, 100f);
                    if(dropChance < itemDropPercent)
                    {
                        int randomDrop = Random.Range(0, itemsToDrop.Length);

                        Instantiate(itemsToDrop[randomDrop], transform.position, transform.rotation);

                    }
                }
                
            }
        }   
    }
}
