using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningTask : Task
{
    [SerializeField]
    LightningGamePanel gamePanel;

    protected override void OnStart()
    {

    }

    public override void OnInteract()
    {
        gamePanel.Show();
    }
}