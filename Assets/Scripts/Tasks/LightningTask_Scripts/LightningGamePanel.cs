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

    //Completion related objects and references
    [SerializeField] TextMeshProUGUI progressCounter;

    [Header("Shooting/Position related variables")]

    [SerializeField] RectTransform shootingPosition1;
    [SerializeField] RectTransform shootingPosition2;

    private int rotationValue = 60;
    private float spawnTimer = 0;  //timer set in update
    private const int expectedProjectileAmount = 2;
    private bool firstBullet;

    private const float BULLETSPAWNCOOLDOWN = 5;


    [Header("Completion related variables")]

    //Tesla detection related variables
    [SerializeField] Button teslaButton;
    [SerializeField] float teslaRadius;
    private Collider2D[] teslaSuccessArea;


    //Score related variables
    [SerializeField] int currentProgress = 0;
    private const int COMPLETIONSCORE = 5;


    private bool gameInitiated = false;


    // Start is called before the first frame update
    void Start()
    {
        gameInitiated = true;
        firstBullet = true;

        if (gameInitiated)
        {
            InstantiateAndSetBullets(projectilesList, preFabProjectile, shootingPosition1, shootingPosition2, canvasParent, spawnTimer);
        }

        //Set the beginning score
        progressCounter.text = $"0 / {COMPLETIONSCORE}";
    }

    private void Update()
    {
        //Everytime teslaButton is clicked run the designated function
        teslaButton.onClick.AddListener(CheckIfBulletIsInRange);

        //Start bullet spawn timer
        if (spawnTimer <= BULLETSPAWNCOOLDOWN)
        {
            spawnTimer += Time.deltaTime;
        }
        ////Update progress
        // Change the progress counter accordingly
        progressCounter.text = $"{currentProgress} / {COMPLETIONSCORE}";

        //Spawn in more bullets when there's less than the expected amount (2)
        if (projectilesList.Count < expectedProjectileAmount)
        {
            InstantiateAndSetBullets(projectilesList, preFabProjectile, shootingPosition1, shootingPosition2, canvasParent, spawnTimer);
        }
    }

    private void FixedUpdate()
    {
        if (projectilesList != null || projectilesList.Count < expectedProjectileAmount && 
            spawnTimer >= BULLETSPAWNCOOLDOWN)
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
        else
        {
            return;
        }
    }

    #region Shooting related functions
    private void InstantiateAndSetBullets(List<GameObject> bulletsCollection, GameObject bulletPreFab,
        RectTransform shootPos1, RectTransform shootPos2, Transform parent, float cooldown)
    {
        Vector2 pos1Position = new Vector2(shootPos1.position.x, shootPos1.position.y);
        Vector2 pos2Position = new Vector2(shootPos2.position.x, shootPos2.position.y);

        ////Instantiate the "starting" bullets
        if (cooldown >= BULLETSPAWNCOOLDOWN)
        {
            for (int i = 0; i < expectedProjectileAmount; i++)
            {
                if (i % 2 == 0 && projectilesList.Count <= expectedProjectileAmount)
                {
                    //Bullet 2 instantiated and added to the list
                    GameObject bullet = Instantiate(bulletPreFab, pos2Position, Quaternion.Euler(0, 0, -rotationValue), parent);
                    bulletsCollection.Add(bullet);
                }
                else if (i % 2 != 0 && projectilesList.Count <= expectedProjectileAmount || firstBullet)
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
            firstBullet = false;
            cooldown = 0;
        }
    }

    void CheckIfBulletIsInRange()
    {
        //Check if bullet tag is in the overlap circle

        teslaSuccessArea = Physics2D.OverlapCircleAll(teslaButton.transform.position, teslaRadius);
        foreach (Collider2D collisionHit in teslaSuccessArea)
        {
            //If there's a bullet around the tesla and it has the tag "Bullet"
            if (collisionHit.CompareTag("Bullet"))
            {
                //Destroy the bullet that is in the tesla area on click
                Destroy(collisionHit.gameObject);

                //Make relevant changes to destroying bullet
                currentProgress++;
                projectilesList.Remove(collisionHit.gameObject);
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