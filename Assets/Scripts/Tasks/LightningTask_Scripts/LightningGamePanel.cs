using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningGamePanel : MonoBehaviour
{
    [Header("Create objects and reference")]

    GameObject lastFirstSelectedGameObject;
    [SerializeField] Task masterTask;
    [SerializeField] Transform canvasParent;

    [SerializeField] GameObject preFabProjectile;  //prefab bullet
    private GameObject bullet1;
    private GameObject bullet2;

    [SerializeField] List<GameObject> projectilesList = new List<GameObject>();
    private int projectileAmount = 2;


    [Header("Shooting/Position related variables")]

    [SerializeField] RectTransform shootingPosition1;
    [SerializeField] RectTransform shootingPosition2;

    private int rotationValue = 60;

    //set a variable that handles the max amount of bullets before we're done with the task

    [Header("Resolve related variables")]

    private bool gameInitiated = false;
    private bool gameOngoing = false;


    // Start is called before the first frame update
    void Start()
    {
        gameInitiated = true;
        gameOngoing = true;

        if (gameInitiated)
        {
            InstantiateAndSetBullets(projectilesList, preFabProjectile, bullet1, bullet2, shootingPosition1, shootingPosition2, canvasParent);
        }
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
            if (i % 2 != 0)     //bullet at position 1
            {
                projectilesList[i].GetComponent<Rigidbody2D>().AddForce(new Vector2(2, 2));
            }
            else    //bullets at position 2
            {
                projectilesList[i].GetComponent<Rigidbody2D>().AddForce(new Vector2(-2, 2));
            }
        }
    }

    #region Shooting related functions
    void InstantiateAndSetBullets(List<GameObject> bulletsCollection, GameObject bulletPreFab, GameObject bullet1, GameObject bullet2, 
        RectTransform shootPos1, RectTransform shootPos2, Transform parent)
    {
        Vector2 pos1Position = new Vector2(shootPos1.position.x, shootPos1.position.y);
        Vector2 pos2Position = new Vector2(shootPos2.position.x, shootPos2.position.y);

        ////Instantiate the "starting" bullets
        //for (int i = 0; i < projectileAmount; i++)
        //{
        //Bullet 1 instantiated and added to the list
        bullet1 = Instantiate(bulletPreFab, pos1Position, Quaternion.Euler(0, 0, rotationValue), parent);
        bulletsCollection.Add(bullet1);

        //Bullet 1 instantiated and added to the list
        bullet2 = Instantiate(bulletPreFab, pos2Position, Quaternion.Euler(0, 0, -rotationValue), parent);
        bulletsCollection.Add(bullet2);
        //}


        ////Set position of bullets
        ////bullets positioned at pos 2 (even numbers)
        //for (int i = 0; i < bulletsCollection.Count; i += 2)
        //{
        //    bulletsCollection[i].transform.position = pos2Position;
        //    Debug.Log($"bullet pos 2 start position: {bulletsCollection[i].transform.position}");
        //}
        ////bullets positioned at pos 1 (uneven numbers)
        //for (int i = 1; i < bulletsCollection.Count; i += 2)
        //{
        //    bulletsCollection[i].transform.position = pos1Position;
        //    Debug.Log($"bullet pos 1 start position: {bulletsCollection[i].transform.position}");
        //}


        //////Set the rotation of the bullets
        ////bullets rotated at pos 2 (even numbers)
        //for (int i = 0; i < bulletsCollection.Count; i += 2)
        //{
        //    bulletsCollection[i].transform.rotation = Quaternion.Euler(0, 0, -rotationValue);
        //    Debug.Log($"bullet pos 2 start position: {bulletsCollection[i].transform.rotation.z}");
        //}
        ////bullets rotated at pos 1 (uneven numbers)
        //for (int i = 1; i < bulletsCollection.Count; i += 2)
        //{
        //    bulletsCollection[i].transform.rotation = Quaternion.Euler(0, 0, rotationValue);
        //    Debug.Log($"bullet pos 1 start position: {bulletsCollection[i].transform.rotation.z}");
        //}
    }
    #endregion

    #region Game mechanic-smooth functions
    public void ExitOnClick()   //used by a button in the game panel
    {
        //remove all the relevant stuff upon closing the game panel


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