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
    private int projectileAmount = 2;

    //Completion related objects and references
    [SerializeField] TextMeshProUGUI progressCounter;

    [Header("Shooting/Position related variables")]

    [SerializeField] RectTransform shootingPosition1;
    [SerializeField] RectTransform shootingPosition2;

    private int rotationValue = 60;

    //set a variable that handles the max amount of bullets before we're done with the task


    [Header("Completion related variables")]

    //Tesla detection related variables
    [SerializeField] GameObject teslaButton;
    [SerializeField] float teslaRadius;


    //Score related variables
    private const int completionScore = 5;
    [SerializeField] int currentProgress = 0;



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

        //Set the beginning score
        progressCounter.text = $"0 / {completionScore}";
    }

    private void Update()
    {
        //Check if the bullet is inside the area of the tesla
        Collider2D teslaSuccessArea = Physics2D.OverlapCircle(teslaButton.transform.position, teslaRadius);
        for (int i = 0; i < projectileAmount; i++)
        {
            //check if bullet tag is in the overlap circle
            //also check if we're tapping the button 
        }



        //update progress





        ////if the task isn't completed yet
        //if ()
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
                projectilesList[i].GetComponent<Rigidbody2D>().AddForce(new Vector2(-2, 3));
            }
            else    //bullets at position 2
            {
                projectilesList[i].GetComponent<Rigidbody2D>().AddForce(new Vector2(2, 3));
            }
        }
    }

    #region Shooting related functions
    private void InstantiateAndSetBullets(List<GameObject> bulletsCollection, GameObject bulletPreFab, RectTransform shootPos1, RectTransform shootPos2, Transform parent)
    {
        Vector2 pos1Position = new Vector2(shootPos1.position.x, shootPos1.position.y);
        Vector2 pos2Position = new Vector2(shootPos2.position.x, shootPos2.position.y);

        ////Instantiate the "starting" bullets
        //for (int i = 0; i < projectileAmount; i++)
        //{
        //Bullet 1 instantiated and added to the list
        GameObject bullet1 = Instantiate(bulletPreFab, pos1Position, Quaternion.Euler(0, 0, rotationValue), parent);
        bulletsCollection.Add(bullet1);

        //Bullet 2 instantiated and added to the list
        GameObject bullet2 = Instantiate(bulletPreFab, pos2Position, Quaternion.Euler(0, 0, -rotationValue), parent);
        bulletsCollection.Add(bullet2);
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

    #region Unity based functions
    private void OnDrawGizmos()
    {
        //tesla overlap circle
        Gizmos.DrawSphere(teslaButton.transform.position, teslaRadius);
    }

    #endregion
}