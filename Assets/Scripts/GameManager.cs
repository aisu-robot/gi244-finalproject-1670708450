using UnityEngine;
using UnityEngine.SceneManagement; // ต้องใช้ตัวนี้เพื่อสั่งรีเซ็ตฉาก

public class GameManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject startPanel;
    public GameObject gameOverPanel;

    private SpawnManager spawnManager;

    void Start()
    {
        // 1. หยุดเวลาเกมไว้ตอนเริ่ม เพื่อรอคนกดปุ่ม Start
        Time.timeScale = 0;

        // 2. เปิดหน้า Start และปิดหน้า Game Over
        startPanel.SetActive(true);
        gameOverPanel.SetActive(false);

        // 3. หาตัวเสกของในฉากเตรียมไว้
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    // ฟังก์ชันสำหรับปุ่ม Start
    public void StartGame()
    {
        Time.timeScale = 1; // ให้เวลาเกมเดินปกติ
        startPanel.SetActive(false); // ซ่อนเมนู

        spawnManager.StartSpawning(); // สั่งให้เริ่มเสกของ 
    }

    // ฟังก์ชันสำหรับเรียกหน้า Game Over
    public void ShowGameOver()
    {
        Time.timeScale = 0; // หยุดเวลาเกม
        gameOverPanel.SetActive(true); // โชว์หน้า Game Over
    }

    // ฟังก์ชันสำหรับปุ่ม Retry หรือ Main Menu
    public void RetryGame()
    {
        // โหลดฉากเดิมซ้ำเพื่อรีเซ็ตทุกอย่าง
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // ฟังก์ชันสำหรับปุ่ม Exit
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("ออกจากเกมแล้ว!"); // แสดงข้อความเทสใน Editor
    }
}