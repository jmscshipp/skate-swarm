using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrickPopupUI : MonoBehaviour
{
    [SerializeField]
    private Sprite defaultGraphic;
    [SerializeField]
    private Sprite failedGraphic;
    [SerializeField]
    private Sprite successGraphic;

    private Image popupImage;
    private GameObject player;

    private bool popupVisible = false;
    private float trickOpportunityTimer;
    private float trickOpportunityTime;
    private bool succeeded = false;

    private static TrickPopupUI instance;

    private void Awake()
    {
        // setting up singleton
        if (instance != null && instance != this)
            Destroy(this);
        instance = this;
    }

    public static TrickPopupUI Instance()
    {
        return instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        popupImage = GetComponentInChildren<Image>();
        popupImage.enabled = false;
        trickOpportunityTime = BalanceSettings.trickOpportunityTime;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x,
            player.transform.position.y -1.6f, player.transform.position.z);

        if (popupVisible)
        {
            trickOpportunityTimer += Time.deltaTime;

            if (trickOpportunityTimer > trickOpportunityTime + 0.4f)
                ClosePopup();
            else if (trickOpportunityTimer > trickOpportunityTime && !succeeded)
            {
                popupImage.sprite = failedGraphic;
            }
            else if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                succeeded = true;
                popupImage.sprite = successGraphic;
                Skateboard.Instance().KickFlip();
            }
        }
    }

    public void ActivateTrickPopup()
    {
        trickOpportunityTimer = 0f;
        popupImage.enabled = true;
        popupImage.sprite = defaultGraphic;
        succeeded = false;
        popupVisible = true;
    }
    public void ClosePopup()
    {
        popupImage.enabled = false;
        popupVisible = false;
    }
}
