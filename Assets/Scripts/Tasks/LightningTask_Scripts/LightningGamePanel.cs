using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LightningGamePanel : MonoBehaviour
{
    [Header("Create objects and reference")]

    //"General" variables
    GameObject lastFirstSelectedGameObject;
    [SerializeField] Task masterTask;
    [SerializeField] Transform canvasParent;

    //Shooting related objects and references
    [SerializeField] GameObject preFabProjectile;  //prefab bullet
    [SerializeField] List<GameObject> projectilesList = new List<GameObject>();
    private const int expectedProjectileAmount = 2;

    //Completion related objects and references
    [SerializeField] TextMeshProUGUI progressCounter;

    [Header("Shooting/Position related variables")]

    [SerializeField] RectTransform shootingPosition1;
    [SerializeField] RectTransform shootingPosition2;

    private int rotationValue = 60;

    [SerializeField] [Range(1, 1.5f)] float bulletSpawnCooldown;

    //set a variable that handles the max amount of bullets before we're done with the task


    [Header("Completion related variables")]

    //Tesla detection related variables
    [SerializeField] Button teslaButton;
    [SerializeField] float teslaRadius;
    private Collider2D[] teslaSuccessArea;


    //Score related variables
    [SerializeField] int currentProgress = 0;
    private const int completionScore = 5;


    [Header("Resolve related variables")]

    private bool gameInitiated = false;
    private bool gameOngoing = false;


    // Start is called before the first frame update
    void Start()
    {
        gameInitiated = true;
        gameOngoing = true;

        //debate addlistner here for the tesla button!

        if (gameInitiated)
        {
            InstantiateAndSetBullets(projectilesList, preFabProjectile, shootingPosition1, shootingPosition2, canvasParent);
        }
        teslaSuccessArea = Physics2D.OverlapCircleAll(teslaButton.transform.position, teslaRadius);

        //Set the beginning score
        progressCounter.text = $"{currentProgress} / {completionScore}";
    }

    private void Update()
    {
        //Everytime teslaButton is clicked run the designated function
        teslaButton.onClick.AddListener(CheckIfBulletIsInRange);


        ////Update progress

        // Change the progress counter
        progressCounter.text = $"{currentProgress} / {completionScore}";

        //Spawn in more bullets when there's less than the expected amount (2)
        if (projectilesList.Count < expectedProjectileAmount)
        {
            InstantiateAndSetBullets(projectilesList, preFabProjectile, shootingPosition1, shootingPosition2, canvasParent);
        }
    }

    private void FixedUpdate()
    {
        if (projectilesList != null)
        {
            //Give the bullets a rigidbody and add force
            for (int i = 0; i < expectedProjectileAmount; i++)
            {
                if (i % 2 == 0 && projectilesList.Count <= expectedProjectileAmount)     //bullet at position 2
                {
                    projectilesList[i].GetComponent<Rigidbody2D>().AddForce(new Vector2(-2, 3));
                }
                else if (i % 2 != 0 && projectilesList.Count <= expectedProjectileAmount)    //bullets at position 1
                {
                    projectilesList[i].GetComponent<Rigidbody2D>().AddForce(new Vector2(2, 3));
                }
                else
                {
                    return;
                }
            }
        }
    }

    #region Shooting related functions
    private void InstantiateAndSetBullets(List<GameObject> bulletsCollection, GameObject bulletPreFab, RectTransform shootPos1, RectTransform shootPos2, Transform parent)
    {
        Vector2 pos1Position = new Vector2(shootPos1.position.x, shootPos1.position.y);
        Vector2 pos2Position = new Vector2(shootPos2.position.x, shootPos2.position.y);
        float spawnTimer = Time.deltaTime;

        ////Instantiate the "starting" bullets
        for (int i = 0; i < expectedProjectileAmount; i++)
        {
            if (spawnTimer >= bulletSpawnCooldown)
            {
                if (i % 2 == 0 && projectilesList.Count <= expectedProjectileAmount)
                {
                    //Bullet 2 instantiated and added to the list
                    GameObject bullet = Instantiate(bulletPreFab, pos2Position, Quaternion.Euler(0, 0, -rotationValue), parent);
                    bulletsCollection.Add(bullet);
                }
                else if (i % 2 != 0 && projectilesList.Count <= expectedProjectileAmount)
                {
                    //Bullet 1 instantiated and added to the list
                    GameObject bullet = Instantiate(bulletPreFab, pos1Position, Quaternion.Euler(0, 0, rotationValue), parent);
                    bulletsCollection.Add(bullet);
                }
                else
                {
                    return;
                }
            }
        }
    }

    void CheckIfBulletIsInRange()
    {
        //Check if bullet tag is in the overlap circle
        //also check if we're tapping the button

        foreach (Collider2D collisionHit in teslaSuccessArea)
        {
            //If there's a bullet around the tesla
            if (collisionHit.CompareTag("Bullet"))
            {
                //Destroy the bullet that is in the tesla area on click
                Destroy(collisionHit.gameObject);
                Debug.Log("Destroyed bullet");

                //change relevant values
                currentProgress++;

                //Save the index of the destroyed gameobject within the list
                //then run that index against the for loops in fixed up. and 
                //the instantiateAndSet bullets function


            }
        }
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

    #region Unity based functions
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(teslaButton.transform.position, teslaRadius);
    }
    #endregion
}