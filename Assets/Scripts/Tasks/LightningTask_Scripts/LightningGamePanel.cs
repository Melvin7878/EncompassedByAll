using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LightningGamePanel : MonoBehaviour
{
    [Header("Create objects and reference")]

    //"General" variables
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
    private const int EXPECTEDPROJECTILEAMOUNT = 2;
    private const float BULLETSPAWNCOOLDOWN = 5;

    [SerializeField] int BULLETHORIZONTALVELOCITY = 2;

    private bool firstBullet = true;
    private bool latestPos1 = false;
    private bool latestPos2 = false;


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
        else if (spawnTimer > BULLETSPAWNCOOLDOWN && projectilesList.Count < EXPECTEDPROJECTILEAMOUNT)
        {
            spawnTimer = 0;
        }

        ////Update progress
        // Change the progress counter accordingly
        progressCounter.text = $"{currentProgress} / {COMPLETIONSCORE}";

        //Spawn in more bullets when there's less than the expected amount (2) on screen
        if (projectilesList.Count < EXPECTEDPROJECTILEAMOUNT)
        {
            InstantiateAndSetBullets(projectilesList, preFabProjectile, shootingPosition1, shootingPosition2, canvasParent, spawnTimer);
        }

        //Set win condition
        if (currentProgress == COMPLETIONSCORE)
        {
            ////We've completed the task
            //remove all the relevant stuff upon hiding the game panel
            for (int i = 0; i < projectilesList.Count; i++)
            {
                Destroy(projectilesList[i]);
                projectilesList.Remove(projectilesList[i]);
            }

            masterTask.SetAsResolved();
            Invoke("Hide", 0.7f);

            spawnTimer = 0;
            currentProgress = 0;
        }
    }

    private void FixedUpdate()
    {
        //Give the bullets a rigidbody and add force
        for (int i = 0; i < projectilesList.Count; i++)
        {
            if (i % 2 == 0 && projectilesList.Count <= EXPECTEDPROJECTILEAMOUNT)     //bullet at position 2
            {
                projectilesList[i].GetComponent<Rigidbody2D>().AddForce(new Vector2(BULLETHORIZONTALVELOCITY, 3));
            }
            else if (i % 2 != 0 && projectilesList.Count <= EXPECTEDPROJECTILEAMOUNT)    //bullets at position 1
            {
                projectilesList[i].GetComponent<Rigidbody2D>().AddForce(new Vector2(-BULLETHORIZONTALVELOCITY, 3));
            }
        }
    }

    #region Shooting related functions
    private void InstantiateAndSetBullets(List<GameObject> bulletsCollection, GameObject bulletPreFab,
        RectTransform shootPos1, RectTransform shootPos2, Transform parent, float cooldown)
    {
        Vector2 pos1Position = new Vector2(shootPos1.position.x, shootPos1.position.y);
        Vector2 pos2Position = new Vector2(shootPos2.position.x, shootPos2.position.y);

        ////Instantiate the "starting" bullets
        if (cooldown >= BULLETSPAWNCOOLDOWN && projectilesList.Count <= EXPECTEDPROJECTILEAMOUNT || firstBullet)
        {
            for (int i = 0; i < EXPECTEDPROJECTILEAMOUNT; i++)
            {
                if (i % 2 == 0 && latestPos1 && !firstBullet)
                {
                    //Bullet at pos2 instantiated and added to the list
                    GameObject bullet = Instantiate(bulletPreFab, pos2Position, Quaternion.Euler(0, 0, -rotationValue), parent);
                    bulletsCollection.Add(bullet);
                    latestPos2 = true;
                    latestPos1 = false;
                    return;
                }
                else if (i % 2 != 0 && latestPos2 || firstBullet)
                {
                    //Bullet at pos1 instantiated and added to the list
                    GameObject bullet = Instantiate(bulletPreFab, pos1Position, Quaternion.Euler(0, 0, rotationValue), parent);
                    bulletsCollection.Add(bullet);

                    latestPos1 = true;
                    latestPos2 = false;
                    firstBullet = false;
                    return;
                }
            }
        }
    }

    void CheckIfBulletIsInRange()
    {
        teslaSuccessArea = Physics2D.OverlapCircleAll(teslaButton.transform.position, teslaRadius);

        //Check if bullet tag is in the overlap circle
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

                //Change direction of the velocity on the bullets on screen thanks
                //to changes in the list
                BULLETHORIZONTALVELOCITY = BULLETHORIZONTALVELOCITY * -1;
            }
        }
    }
    #endregion

    #region Game mechanic-smooth functions
    public void ExitOnClick()   //used by the "hide" button in the game panel
    {
        //remove all the relevant stuff upon hiding the game panel
        for (int i = 0; i < projectilesList.Count; i++)
        {
            Destroy(projectilesList[i]);
            projectilesList.Remove(projectilesList[i]);
        }

        //put away the game panel with the "Hide" function
        Hide();

        spawnTimer = 0;
        currentProgress = 0;
    }

    public void Show()
    {
        gameObject.SetActive(true);
        //lastFirstSelectedGameObject = GameManager.Instance.EventSystem.firstSelectedGameObject;
        GameManager.Instance.EventSystem.firstSelectedGameObject = gameObject;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        GameManager.Instance.EventSystem.firstSelectedGameObject = gameObject;
    }
    #endregion
}