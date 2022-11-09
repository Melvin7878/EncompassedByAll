using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningGamePanel : MonoBehaviour
{
    //Variables
    [Header("Create objects and reference")]
    GameObject lastFirstSelectedGameObject;

    [SerializeField] GameObject projectile;  //prefab bullet
    //GameObject[] projectiles = new GameObject[2];
    public List<GameObject> projectilesList;

    [SerializeField] Task masterTask;

    [Header("Shooting position related variables")]
    [SerializeField] RectTransform shootingPosition1;
    [SerializeField] RectTransform shootingPosition2;
    //private RectTransform[] shootingPositions = new RectTransform[2];

    private int rotationValue = 60;

    //set a variable that handles the max amount of bullets before we're done with the task

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("in here");
        //shootingPositions[0] = shootingPosition1;
        //shootingPositions[1] = shootingPosition2;

        InstantiateBullets(projectilesList, projectile, shootingPosition1, shootingPosition2);
        SetValues(projectilesList, shootingPosition1, shootingPosition2);
    }

    #region Shooting related functions
    void InstantiateBullets(List<GameObject> bulletsCollection, GameObject bullet, RectTransform position1, RectTransform position2)
    {
        for (int i = 0; i < bulletsCollection.Count; i++)
        {
            Instantiate(bullet);

            //bulletsCollection[i] = bullet;
            projectilesList.Add(bullet);
        }
    }

    void SetValues(List<GameObject> bulletsCollection, /*GameObject bullet,*/ RectTransform shootPos1, RectTransform shootPos2)
    {
        Vector2 pos1Position = new Vector2(shootPos1.position.x, shootPos1.position.y);
        Vector2 pos2Position = new Vector2(shootPos2.position.x, shootPos2.position.y);

        Debug.Log("in function");
        //Set position of bullets
        for (int i = 0; i < bulletsCollection.Count; i++)            //check conditionals
        {
            Debug.Log("in for loop");
            if (i > 0 && i < bulletsCollection.Count)
            {
                bulletsCollection[i].transform.position = pos1Position;
                Debug.Log(bulletsCollection[i].transform.position);
            }
            else
            {
                bulletsCollection[i].transform.position = pos2Position;
                Debug.Log(bulletsCollection[i].transform.position);
            }
        }

        //Set rotation of bullets
        for (int i = 0; i < bulletsCollection.Count; i++)
        {
            //Setting the rotation of the shooting positions
            bulletsCollection[i].transform.rotation = Quaternion.identity;

            if (i > 0 && i < bulletsCollection.Count)
            {
                bulletsCollection[i].transform.rotation = Quaternion.Euler(rotationValue, 0, 0);
                Debug.Log($"pos 1 rotation: {bulletsCollection[i].transform.rotation.x}");
            }
            else
            {
                bulletsCollection[i].transform.rotation = Quaternion.Euler(-rotationValue, 0, 0);
                Debug.Log($"pos 2 rotation: {bulletsCollection[i].transform.rotation.x}");
            }
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