using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_U1_G4_Enemy : MonoBehaviour
{
    public S2_U1_Game4 game;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            game.Hit();
            if (GetComponent<PlanetMovement>() != null)
                GetComponent<PlanetMovement>().enabled = false;
        }
    }
}
