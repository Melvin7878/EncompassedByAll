using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private LightningGamePanel gamePanelScript;
    private void Awake()
    {
        gamePanelScript = FindObjectOfType<LightningGamePanel>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
        gamePanelScript.projectilesList.Remove(gameObject);
        gamePanelScript.horizontalBulletVelocity = gamePanelScript.horizontalBulletVelocity * -1;
    }
}