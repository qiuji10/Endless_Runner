using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState { OnAir, OnRun, OnRoll, Whatever }

public class PlayerCore : MonoBehaviour
{
    public enum LaneState { Lane1, Lane2, Lane3 }

    [Header("Enum Settings")]
    public LaneState laneState = LaneState.Lane2;
    public PlayerState playerState = PlayerState.OnRun;
    public ForceMode forceMode;

    [Header("Audio Settings")]
    public AudioData playerSFX;

    [Header("Swipe Settings")]
    [SerializeField] float swipeDeadZone;
    [SerializeField] float swipeDuration;
    [SerializeField] float jumpForce;
    private float firstTapTime;
    private bool isJump;

    [Header("DoubleTap Settings")]
    [SerializeField] private float durationBetweenTaps;
    private bool hasFirstTapped;

    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    private float newXPos;

    [Header("PowerUpSettings")]
    [SerializeField] private float shieldTimeout;
    [SerializeField] private float jumpTimeout;
    [SerializeField] private float bonusTimeout;
    public Image Jump, Shield, Bonus;
    [SerializeField] private GameObject shieldObj;

    private bool isShieldPU, isJumpPU, isBonusPU;
    private float shieldTimer, jumpTimer, bonusTimer;

    private Rigidbody rb;
    private Animator anim;
    private GameManager gameManager;

    public bool IsShield { get { return isShieldPU; } set { isShieldPU = value; } }
    public float ShieldTimer { get { return shieldTimer; } set { shieldTimer = value; } }
    public Animator Anim { get { return anim; } set { anim = value; } }

    Vector3 firstTouchPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
        GameManager.OnGameStart += StartAnimation;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStart -= StartAnimation;
    }

    void Update()
    {
        GetInput();
        PowerUpTimer();
        transform.position = Vector3.Lerp(transform.position, new Vector3(newXPos, transform.position.y, transform.position.z), speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            isJump = false;
            playerState = PlayerState.OnRun;
        }
    }

    private void GetInput()
    {
        if (!gameManager.isPlaying)
            return;

        if (Time.time > firstTapTime + durationBetweenTaps)
        {
            hasFirstTapped = false;
        }

        if (Input.GetMouseButtonDown(0) && !hasFirstTapped)
        {
            hasFirstTapped = true;
            firstTapTime = Time.time;
            firstTouchPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonDown(0) && Time.time - firstTapTime <= durationBetweenTaps && !isShieldPU && gameManager.Data.playerData.powerupShield >= 1)
        {
            AudioManager.instance.PlaySFX(playerSFX, "pu_shield");
            isShieldPU = true;
            Shield.gameObject.SetActive(true);
            gameManager.Data.playerData.powerupShield--;
            gameManager.Data.SaveGame();
            shieldObj.SetActive(true);
            hasFirstTapped = false;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 swipeDelta = Input.mousePosition - firstTouchPos;

            if (swipeDelta != Vector2.zero && Time.time - firstTapTime <= swipeDuration)
            {
                Swipe(swipeDelta);
            }
        }
    }

    private void PowerUpTimer()
    {
        if (isJumpPU)
        {
            jumpTimer += Time.deltaTime;
            Jump.fillAmount -= 1.0f / jumpTimeout * Time.deltaTime;
            if (jumpTimer >= jumpTimeout)
            {
                Jump.gameObject.SetActive(false);
                jumpForce = 3.6f;
                isJumpPU = false;
                jumpTimer = 0;
            }
        }
        
        if (isShieldPU)
        {
            shieldTimer += Time.deltaTime;
            Shield.fillAmount -= 1.0f / shieldTimeout * Time.deltaTime;
            if (shieldTimer >= shieldTimeout)
            {
                Shield.fillAmount = 1;
                DisableShield();
                shieldTimer = 0;
            }
        }
        
        if (isBonusPU)
        {
            bonusTimer += Time.deltaTime;
            Bonus.fillAmount -= 1.0f / bonusTimeout * Time.deltaTime;
            if (bonusTimer >= bonusTimeout)
            {
                Bonus.gameObject.SetActive(false);
                gameManager.bonusAdd = 0;
                isBonusPU = false;
                bonusTimer = 0;
            }
        }
    }

    private void Swipe(Vector2 delta)
    {
        float xAbs = Mathf.Abs(delta.x);
        float yAbs = Mathf.Abs(delta.y);

        //Horizontal Swipe
        if (xAbs > yAbs)
        {
            AudioManager.instance.PlaySFX(playerSFX, "left_right");
            //do left or right
            if (delta.x > 0)
            {
                GoRight();
            }
            else if (delta.x < 0)
            {
                GoLeft();
            }
        }
        //Vertical swipe
        else
        {
            //up or down
            if (delta.y > 0 && !isJump)
            {
                AudioManager.instance.PlaySFX(playerSFX, "jump");
                playerState = PlayerState.OnAir;
                anim.SetTrigger("onAir");
                isJump = true;
                rb.AddForce(Vector3.up * jumpForce, forceMode);
            }

            else if (delta.y < 0)
            {
                AudioManager.instance.PlaySFX(playerSFX, "roll");
                playerState = PlayerState.OnRoll;
                anim.SetTrigger("onRoll");
                rb.AddForce(Vector3.down * jumpForce/2, forceMode);
            }
        }
    }

    private void GoRight()
    {
        if (laneState == LaneState.Lane1)
        {
            laneState = LaneState.Lane2;
            newXPos = 0;
        }
        else if (laneState == LaneState.Lane2)
        {
            laneState = LaneState.Lane3;
            newXPos = 0.8f;
        }
    }

    private void GoLeft()
    {
        if (laneState == LaneState.Lane3)
        {
            laneState = LaneState.Lane2;
            newXPos = 0;
        }
        else if (laneState == LaneState.Lane2)
        {
            laneState = LaneState.Lane1;
            newXPos = -0.8f;
        }
    }

    public void ResetState()
    {
        playerState = PlayerState.OnRun;
    }

    public void StartAnimation()
    {
        anim.SetTrigger("StartGame");
    }

    public void DisableShield()
    {
        Shield.gameObject.SetActive(false);
        isShieldPU = false;
        shieldTimer = 0;
        shieldObj.SetActive(false);
    }

    public void SetJumpForce(float force)
    {
        AudioManager.instance.PlaySFX(playerSFX, "pu_jump");
        Jump.gameObject.SetActive(true);
        isJumpPU = true;
        jumpForce = force;
        gameManager.Data.playerData.powerupJump--;
        gameManager.Data.SaveGame();
        gameManager.PupButton.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void SetBonusScore(float score)
    {
        AudioManager.instance.PlaySFX(playerSFX, "pu_bonus");
        Bonus.gameObject.SetActive(true);
        isBonusPU = true;
        gameManager.bonusAdd = score;
        gameManager.Data.playerData.powerupBonus--;
        gameManager.Data.SaveGame();
        gameManager.PupButton.transform.GetChild(1).gameObject.SetActive(false);
    }
}
