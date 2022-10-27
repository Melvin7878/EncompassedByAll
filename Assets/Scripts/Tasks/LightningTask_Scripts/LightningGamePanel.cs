using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningGamePanel : MonoBehaviour
{
    //Variables
    [Header("Create objects and reference")]
    GameObject lastFirstSelectedGameObject;
    [SerializeField] GameObject projectile;  //prefab
    private GameObject[] projectiles;

    [SerializeField] Task masterTask;

    [Header("Shooting position related variables")]
    [SerializeField] RectTransform shootingPosition1;
    [SerializeField] RectTransform shootingPosition2;
    private RectTransform[] shootingPositions = new RectTransform[2];

    private int rotationValue = 60;


    // Start is called before the first frame update
    void Start()
    {
        shootingPositions[0] = shootingPosition1;
        shootingPositions[1] = shootingPosition2;

        for (int i = 0; i < shootingPositions.Length; i++)
        {
            //Setting the rotation of the shooting positions
            shootingPositions[i].rotation = Quaternion.identity;
            if (i > 0 && i < shootingPositions.Length)
            {
                shootingPositions[i].rotation = Quaternion.Euler(rotationValue, 0, 0);
                Debug.Log($"pos 1 rotation: {shootingPositions[i].rotation.x}");
            }
            else
            {
                shootingPositions[i].rotation = Quaternion.Euler(-rotationValue, 0, 0);
                Debug.Log($"pos 2 rotation: {shootingPositions[i].rotation.x}");
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        InstantiateShoots(projectiles,, projectile, shootingPosition1, shootingPosition2);
    }

    #region Shooting related functions

    void InstantiateShoots(GameObject[] bulletsCollection, GameObject bullet, RectTransform position1, RectTransform position2)
    {
        Vector2 pos1Position = new Vector2(position1.position.x, position1.position.y);
        Vector2 pos2Position = new Vector2(position2.position.x, position2.position.y);
        for (int i = 0; i < shootingPositions.Length; i++)
        {
            Instantiate(bulletsCollection[i]);

            //if (i > 0 && i < shootingPositions.Length)
            //{
            //    bullet.transform.position = pos1Position;
            //    Debug.Log(bullet.transform.position);
            //}
            //else
            //{
            //    bullet.transform.position = pos2Position;
            //    Debug.Log(bullet.transform.position);
            //}
        }


    }

    #endregion

    #region Game mechanic-smooth functions
    public void ExitOnClick()   //used by a button in the game panel
    {
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        lastFirstSelectedGameObject = GameManager.Instance.EventSystem.firstSelectedGameObject;
        GameManager.Instance.EventSystem.firstSelectedGameObject = gameObject;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        GameManager.Instance.EventSystem.firstSelectedGameObject = gameObject;
    }
    #endregion
}