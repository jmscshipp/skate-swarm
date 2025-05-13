using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Tooltip("progression of push decay over time")]
    private AnimationCurve pushDecayCurve;
    private float movementSpeed;
    private PushQueue pushQueue;

    private Rigidbody rb;

    [SerializeField]
    private Transform UICamera;
    [SerializeField]
    private Transform directionalArrowUI;
    [SerializeField]
    private Image pushUI;
    private Vector3 screenCenter;

    // amount the player is raised about the ground
    private float playerDistanceFromGround = 0.132f;
    // used with ground detection ray cast
    [SerializeField]
    private LayerMask groundMask;

    [SerializeField]
    private AnimationCurve jumpCurve;
    private float jumpTimer;
    private bool trickPrepped = false;
    private bool airBorne = false;
    private Vector3 jumpPeak;
    private float gravityTimer;
    private bool trickCompletedSuccesfully;
    private PlayerAttack playerAttack;

    [SerializeField]
    private MomentumUI momentumUI;
    private float momentum = 0f;
    private MeshRenderer[] renderers;
    private float health = 100f;
    [SerializeField]
    private Material defaultMat;
    [SerializeField]
    private Material allWhiteMat;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance().PlayMusic("mainTheme");
        UnityEngine.Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        movementSpeed = BalanceSettings.movementSpeed;

        rb = GetComponent<Rigidbody>();
        playerAttack = GetComponent<PlayerAttack>();
        renderers = GetComponentsInChildren<MeshRenderer>();
        pushQueue = new PushQueue(pushUI);
        pushQueue.Push();
    }

    // Update is called once per frame
    void Update()
    {
        // mouse visiblity stuff for windows version
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.Cursor.visible = !UnityEngine.Cursor.visible;
        }

        health = Mathf.Clamp(health + Time.deltaTime, 0f, 100f);
        // update direction arrow UI to follow player pos
        directionalArrowUI.transform.position = new Vector3(transform.position.x,
            transform.position.y + 0.35f, transform.position.z);

        // calculating the angle between the mouse and player
        //Vector3 playerPos = Camera.main.WorldToScreenPoint(transform.position);
        float moveDir = Mathf.Atan2(screenCenter.x - Input.mousePosition.x,
            screenCenter.y - Input.mousePosition.y) * Mathf.Rad2Deg;

        // rotate arrow UI in that direction
        directionalArrowUI.transform.localRotation = Quaternion.Euler(
            new Vector3(0f, 0f, 180f - moveDir));
            
        // rotate player to face matching way (needs different offset because
        // of isographic weirdness)
        transform.localRotation = Quaternion.Euler(new Vector3(0f, 405f + moveDir, 0f));


        // pushing
        if (Input.GetKey(KeyCode.Mouse0) && !airBorne)
            pushQueue.Push();
        // prep trick
        else if (Input.GetKeyDown(KeyCode.Mouse1) && !trickPrepped 
            && momentum >= BalanceSettings.momentumBuildUpTime)
        {
            StartCoroutine(PrepTrick());
        }

        pushQueue.Update(Time.deltaTime);

        if (airBorne)
        {
            jumpTimer += Time.deltaTime;
            // height of jump
            if (jumpTimer >= 1)
            {
                // save jump peak pos for lerping gravity down to ground
                jumpPeak = transform.position;
                gravityTimer += Time.deltaTime * 3f;
            }
        }
        else if (rb.velocity.magnitude > .1f)
        {
            momentum = Mathf.Clamp(momentum + Time.deltaTime, 0f, BalanceSettings.momentumBuildUpTime);
        }
        else
        {
            momentum = Mathf.Clamp(momentum - Time.deltaTime, 0f, BalanceSettings.momentumBuildUpTime);
        }
        momentumUI.UpdateMomentumMeter(momentum);
    }

    private void FixedUpdate()
    {
        SkatingSound.Instance().SetVolume(rb.velocity.magnitude / 3f);

        // move based on push and angle from mouse position
        rb.velocity = transform.forward * Time.fixedDeltaTime * movementSpeed * 
            pushDecayCurve.Evaluate(pushQueue.GetPushProgress());

        if (airBorne && jumpTimer < 1f)
        {
            rb.velocity = new Vector3(rb.velocity.x, 
                jumpCurve.Evaluate(jumpTimer / 1f) * 2f, rb.velocity.z);
            return;
        }

        // get y pos of ground
        float groundYPos = 0f;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down),
            out hit, Mathf.Infinity, groundMask))
        {
            groundYPos = hit.point.y;
        }

        // snap player to whatever elevation is below them
        if (!airBorne)
        {
            transform.position = new Vector3(transform.position.x, groundYPos +
                   playerDistanceFromGround, transform.position.z);
        }
        // lower player to the ground with gravity
        else
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(
                groundYPos + playerDistanceFromGround, jumpPeak.y, 
                jumpCurve.Evaluate(gravityTimer)),
                transform.position.z);
        }

        // check if we've returned to the ground after doing a trick
        if (airBorne && Mathf.Abs(transform.position.y - groundYPos) < 0.15f)
        {
            if (trickCompletedSuccesfully)
                playerAttack.Attack();
            AudioManager.Instance().PlaySound("landing");

            trickPrepped = false;
            airBorne = false;
        }

        SkatingSound.Instance().TurnOnSound(true);
    }

    private IEnumerator PrepTrick()
    {
        gravityTimer = 0f;
        jumpTimer = 0f;
        momentum = 0f;
        pushQueue.Push();
        airBorne = true;
        trickPrepped = true;
        trickCompletedSuccesfully = false;
        AudioManager.Instance().PlaySound("jump");
        SkatingSound.Instance().TurnOnSound(false);

        yield return new WaitForSeconds(0.3f);
        TrickPopupUI.Instance().ActivateTrickPopup();
    }

    public void CompleteTrick()
    {
        AudioManager.Instance().PlaySound("flip");
        pushQueue.Push();
        trickCompletedSuccesfully = true;
    }

    public IEnumerator TakeDamage()
    {
        if (airBorne)
            yield break;

        health -= 20f;
        AudioManager.Instance().PlaySound("hurt");

        // flash white
        foreach (MeshRenderer renderer in renderers)
            renderer.material = allWhiteMat;
        yield return new WaitForSeconds(0.1f);
        // flash skin
        foreach (MeshRenderer renderer in renderers)
            renderer.material = defaultMat;
        yield return new WaitForSeconds(0.1f);

        if (health <= 5f)
        {
            AudioManager.Instance().PlaySound("death");
            // flash white
            foreach (MeshRenderer renderer in renderers)
                renderer.material = allWhiteMat;
            yield return new WaitForSeconds(0.1f);
            foreach (MeshRenderer renderer in renderers)
                renderer.enabled = false;
            yield return new WaitForSeconds(0.5f);
            EnemyDestinations.Instance().Reset();
            SceneManager.LoadScene(0);
        }
    }
}