using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningGamePanel : MonoBehaviour
{
    //Variables
    [Header("Create objects and reference")]

    GameObject lastFirstSelectedGameObject;
    [SerializeField] Task masterTask;

    [SerializeField] GameObject projectile;  //prefab bullet
    [SerializeField] List<GameObject> projectilesList = new List<GameObject>();
    private int projectileAmount = 2;


    [Header("Shooting/Position related variables")]

    [SerializeField] RectTransform shootingPosition1;
    [SerializeField] RectTransform shootingPosition2;

    private int rotationValue = 57;

    //set a variable that handles the max amount of bullets before we're done with the task

    [Header("Resolve related variables")]

    [SerializeField] bool gameInitiated = false;


    // Start is called before the first frame update
    void Start()
    {
        gameInitiated = true;

        Debug.Log($"When game start - shoot position 1: {shootingPosition1.position} \n shoot position 2: {shootingPosition2.position}");

        if (gameInitiated)
        {
            InstantiateAndSetBullets(projectilesList, projectile, shootingPosition1, shootingPosition2);
        }

        //Debug.Log($"shoot position 1: {shootingPosition1.position} \n shoot position 2: {shootingPosition2.position}");
        //SetValues(projectilesList, projectile, shootingPosition1, shootingPosition2);
    }

    private void Update()
    {
        ////if the task isn't completed yet
        //if (projectilesList.Count)
        //{
        //    projectileCounter = bulletsCollection.Count;
        //}
        //Debug.Log(projectileCounter);
    }

    private void FixedUpdate()
    {
        //Give the bullets a rigidbody and add force
        for (int i = 0; i < projectilesList.Count; i++)
        {
            if (i % 2 != 0)
            {
                projectilesList[i].GetComponent<Rigidbody2D>().AddForce(new Vector2(100, 100));
                //Debug.Log($"pos 1 bullet's position: {projectilesList[i].transform.position}");
                //Debug.Log($"pos 1 bullet rotation: {projectilesList[i].transform.rotation}");
            }
            else
            {
                projectilesList[i].GetComponent<Rigidbody2D>().AddForce(new Vector2(-100, 100));
                //Debug.Log($"pos 2 bullets position: {projectilesList[i].transform.position}");
                //Debug.Log($"pos 2 bullet rotation: {projectilesList[i].transform.rotation}");
            }
        }
    }

    #region Shooting related functions
    void InstantiateAndSetBullets(List<GameObject> bulletsCollection, GameObject bullet, RectTransform shootPos1, RectTransform shootPos2)
    {
        Vector2 pos1Position = new Vector2(shootPos1.position.x, shootPos1.position.y);
        Vector2 pos2Position = new Vector2(shootPos2.position.x, shootPos2.position.y);

        //Instantiate the "starting" bullets
        for (int i = 0; i < projectileAmount; i++)
        {
            Instantiate(bullet);

            bulletsCollection.Add(bullet);
        }

        //Set position of bullets
        for (int i = 0; i < bulletsCollection.Count; i++)
        {
            if (i % 2 != 0)          //bullets at position 1
            {
                bulletsCollection[i].transform.position = pos1Position;
                Debug.Log($"bullet pos 1 start position: {bulletsCollection[i].transform.position}");
            }
            else     //bullets at position 2
            {
                bulletsCollection[i].transform.position = pos2Position;
                Debug.Log($"bullet pos 2 start position: {bulletsCollection[i].transform.position}");
            }
        }

        //Set rotation of bullets
        for (int i = 0; i < bulletsCollection.Count; i++)
        {
            bulletsCollection[i].transform.rotation = Quaternion.identity;

            if (i % 2 != 0)     //bullets at position 1
            {
                bulletsCollection[i].transform.rotation = Quaternion.Euler(0, 0, rotationValue);
                Debug.Log($"bullets pos 1 rotation: {bulletsCollection[i].transform.rotation.z}");
            }
            else    //bullets at position 2 
            {
                bulletsCollection[i].transform.rotation = Quaternion.Euler(0, 0, -rotationValue);
                Debug.Log($"bullets pos 2 rotation: {bulletsCollection[i].transform.rotation.z}");
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