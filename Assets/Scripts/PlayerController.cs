using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f; // ความเร็วเดิน W A S D
    public float xBoundary = 2f;  // ระยะขอบถนนซ้ายขวา (กันเดินตกขอบ)

    [Header("Jump Settings")]
    public float jumpForce = 10f;
    public float doubleJumpForce = 8f; // แรงกระโดดครั้งที่ 2
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
    }

    void Update()
    {
        // ถ้ายอมแพ้/ตายแล้ว ให้คืนค่าเวลาเป็นปกติและหยุดทำงานด้านล่างทั้งหมด
        if (gameOver)
        {
            Time.timeScale = 1f;
            return;
        }

        // ---------------- 1. ระบบเดิน (W A S D) ----------------
        float horizontalInput = 0; // สำหรับรับค่า A, D (ซ้าย-ขวา)
        float verticalInput = 0;   // สำหรับรับค่า W, S (หน้า-หลัง)

        // เช็คปุ่ม ซ้าย-ขวา
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) horizontalInput = -1;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) horizontalInput = 1;

        // เช็คปุ่ม หน้า-หลัง
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) verticalInput = 1;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) verticalInput = -1;

        // สั่งขยับ ซ้าย-ขวา (แกน X)
        transform.Translate(Vector3.right * horizontalInput * moveSpeed * Time.deltaTime);

        // สั่งขยับ หน้า-หลัง (แกน Z)
        transform.Translate(Vector3.forward * verticalInput * moveSpeed * Time.deltaTime);

        // ---------------- การล็อกขอบเขต (ไม่ให้เดินตกโลก) ----------------
        // ล็อกแกนซ้าย-ขวา (แกน X)
        if (transform.position.z < -xBoundary)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -xBoundary);
        }
        if (transform.position.z > xBoundary)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, xBoundary);
        }

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
                // รีเซ็ตความเร็วตอนตกลงมาก่อน เพื่อให้กระโดดครั้งที่สองเด้งขึ้นเสมอ
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(doubleJumpForce * Vector3.up, ForceMode.Impulse);
                doubleJumpUsed = true;
                playerAnim.SetTrigger("Jump_trig");
                playerAudio.PlayOneShot(jumpSfx);
            }
        }

        // ---------------- 3. ระบบ Dash (เร่งเวลาเกม) ----------------
        // กดปุ่ม Shift ซ้ายค้างไว้เพื่อเร่งเวลา
        if (Keyboard.current.leftShiftKey.isPressed)
        {
            Time.timeScale = 1.5f; // ความเร็ว 1.5 เท่า (ปรับเลขให้เร็วขึ้นได้ตามต้องการ)
        }
        else
        {
            Time.timeScale = 1f; // ปล่อยปุ่มแล้วกลับมาความเร็วปกติ
        }
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
}