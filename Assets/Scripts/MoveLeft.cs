using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public float speed = 30f;
    private PlayerController playerController;
    private float leftBound = -15f;

    // ตัวแปรเช็คว่าถังใบนี้แจกคะแนนไปหรือยัง (กันมันบวกคะแนนรัวๆ)
    private bool isScored = false;

    void Start()
    {
        // เชื่อมต่อเพื่อขอดึงฟังก์ชัน บวกคะแนน จาก Player
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        if (playerController.gameOver == false)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }

        // ---------------- 1. ระบบเช็คการหลบพ้น (ได้คะแนน) ----------------
        // ตรวจสอบว่า 1.เป็นสิ่งกีดขวาง 2.ยังไม่เคยแจกคะแนน และ 3.ไหลผ่าน (แกน X) ตัวผู้เล่นไปแล้วหรือยัง
        if (gameObject.CompareTag("Obstacle") && !isScored)
        {
            if (transform.position.x < playerController.transform.position.x)
            {
                playerController.AddScore(1); // ส่งคำสั่งไปบวก 1 คะแนน!
                isScored = true; // ล็อกล็อกไว้ไม่ให้แจกคะแนนซ้ำ
            }
        }

        // ---------------- 2. ระบบทำลายวัตถุขยะ ----------------
        if (transform.position.x < leftBound && !gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}