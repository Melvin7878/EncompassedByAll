using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Task : MonoBehaviour
{
    //Variables
    [SerializeField]
    private string taskName;

    [SerializeField]
    private Sprite interactSprite;

    private Sprite originalSprite;
    private SpriteRenderer spriteRenderer;

    public string TaskName { get => taskName; }

    bool resolved = true;

    public bool IsResolved { get => resolved; }

    protected abstract void OnStart();

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;
        OnStart();
    }

    #region Highlight related
    public void HighlightTask()
    {
        if (interactSprite != null)
            spriteRenderer.sprite = interactSprite;
    }

    public void UnhighlightTask()
    {
        spriteRenderer.sprite = originalSprite;
    }
    #endregion

    #region Solve related
    public void SetAsUnresolved()
    {
        resolved = false;
    }


    public void SetAsResolved()
    {
        resolved = true;
    }
    #endregion

    public abstract void OnInteract();

    public void Interact()
    {
        OnInteract();
    }
}
