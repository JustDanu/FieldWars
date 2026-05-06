using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    [SyncVar] public int health = 100;

    [Server]
    public void TakeDamage(int dmg)
    {
        health -= dmg;

        if (health <= 0)
        {
            Debug.Log("Dead");
        }
    }
}
