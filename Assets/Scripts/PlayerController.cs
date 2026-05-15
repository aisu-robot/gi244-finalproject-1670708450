using UnityEngine;
using UnityEngine.InputSystem;
using TMPro; // <--- เพิ่มบรรทัดนี้เพื่อเรียกใช้ระบบ TextMeshPro

public class PlayerController : MonoBehaviour
{
    [Header("UI Score")]
    public TextMeshProUGUI scoreText; // ช่องสำหรับใส่ UI ข้อความ
    public int score = 0;             // ตัวแปรเก็บคะแนนปัจจุบัน

    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float xBoundary = 5f;

    [Header("Jump Settings")]
    public float jumpForce = 10f;
    public float doubleJumpForce = 8f;
    public float gravityModifier = 2f;

    [Header("Particles & Audio")]
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    public AudioClip jumpSfx;
    public AudioClip crashSfx;

    private Rigidbody rb;
    private bool isOnGround = true;
    private bool doubleJumpUsed = false;

    private Animator playerAnim;
    private AudioSource playerAudio;

    public bool gameOver = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
    }

    void Start()
    {
        Physics.gravity *= gravityModifier;
        gameOver = false;

        // รีเซ็ตคะแนนตอนเริ่มเกม
        score = 0;
        UpdateScoreText();
    }

    void Update()
    {
        if (gameOver)
        {
            Time.timeScale = 1f;
            return;
        }

        // ---------------- 1. ระบบเดิน (W A S D) ----------------
        float horizontalInput = 0;
        float verticalInput = 0;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) horizontalInput = -1;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) horizontalInput = 1;
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) verticalInput = 1;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) verticalInput = -1;

        transform.Translate(Vector3.right * horizontalInput * moveSpeed * Time.deltaTime);
        transform.Translate(Vector3.forward * verticalInput * moveSpeed * Time.deltaTime);

        if (transform.position.z < -xBoundary) transform.position = new Vector3(transform.position.x, transform.position.y, -xBoundary);
        if (transform.position.z > xBoundary) transform.position = new Vector3(transform.position.x, transform.position.y, xBoundary);

        // ---------------- 2. ระบบกระโดด และ Double Jump ----------------
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (isOnGround)
            {
                rb.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
                isOnGround = false;
                doubleJumpUsed = false;
                playerAnim.SetTrigger("Jump_trig");
                dirtParticle.Stop();
                playerAudio.PlayOneShot(jumpSfx);
            }
            else if (!doubleJumpUsed)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(doubleJumpForce * Vector3.up, ForceMode.Impulse);
                doubleJumpUsed = true;
                playerAnim.SetTrigger("Jump_trig");
                playerAudio.PlayOneShot(jumpSfx);
            }
        }

        // ---------------- 3. ระบบเร่งเวลาเกม ----------------
        if (Keyboard.current.leftShiftKey.isPressed) Time.timeScale = 1.5f;
        else Time.timeScale = 1f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            doubleJumpUsed = false;
            dirtParticle.Play();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over!");
            gameOver = true;
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
            explosionParticle.Play();
            dirtParticle.Stop();
            playerAudio.PlayOneShot(crashSfx);
        }
    }

    // ---------------- ฟังก์ชันจัดการคะแนน ----------------
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText(); // สั่งให้อัปเดตข้อความบนจอ
    }

    private void UpdateScoreText()
    {
        // ตรวจสอบว่ามี UI เชื่อมอยู่ไหม ป้องกันการ Error
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}