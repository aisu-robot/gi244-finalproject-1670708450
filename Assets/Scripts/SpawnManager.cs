using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject obstaclePrefab;
    public GameObject itemCubePrefab; // เพิ่มช่องสำหรับใส่ Prefab กล่องไอเทม

    [Header("Spawn Settings")]
    public float spawnDistance = 70f;
    public float startDelay = 2f;
    public float repeatRate = 0.3f;

    public float itemSpawnRate = 10f; // ตั้งเวลาให้กล่องไอเทมเกิด (เช่น สุ่มเกิดทุก 10 วินาที)

    private PlayerController playerController;

    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        // ลบ InvokeRepeating เดิมทิ้งไปครับ
    }

    // สร้างฟังก์ชันใหม่นี้เพื่อให้ GameManager มาสั่งเริ่มเสก
    public void StartSpawning()
    {
        InvokeRepeating(nameof(SpawnObstacle), startDelay, repeatRate);
        InvokeRepeating(nameof(SpawnItemCube), startDelay + 2f, itemSpawnRate);
    }

    void SpawnObstacle()
    {
        if (playerController.gameOver == false)
        {
            int spawnCount = Random.Range(7, 17);

            for (int i = 0; i < spawnCount; i++)
            {
                float randomZ = Random.Range(-10f, 10f);
                Vector3 randomSpawnPos = new Vector3(spawnDistance, 0, randomZ);
                Instantiate(obstaclePrefab, randomSpawnPos, obstaclePrefab.transform.rotation);
            }
        }
    }

    // ฟังก์ชันใหม่: สำหรับเสกกล่องไอเทม
    void SpawnItemCube()
    {
        if (playerController.gameOver == false)
        {
            // สุ่มตำแหน่งซ้าย-ขวา
            float randomZ = Random.Range(-5f, 5f);

            // เปลี่ยน 1.5f เป็น 4f เพื่อให้กล่องลอยสูงขึ้น (ปรับเลข 4f ได้ตามใจชอบเลยครับ)
            Vector3 randomSpawnPos = new Vector3(spawnDistance, 4f, randomZ);

            Instantiate(itemCubePrefab, randomSpawnPos, itemCubePrefab.transform.rotation);
        }
    }

    public void StartCrazyMode(float duration)
    {
        StartCoroutine(CrazyModeRoutine(duration));
    }

    private System.Collections.IEnumerator CrazyModeRoutine(float duration)
    {
        CancelInvoke(nameof(SpawnObstacle));
        InvokeRepeating(nameof(SpawnObstacle), 0f, 0.35f);
        yield return new WaitForSeconds(duration);
        CancelInvoke(nameof(SpawnObstacle));

        if (playerController.gameOver == false)
        {
            InvokeRepeating(nameof(SpawnObstacle), startDelay, repeatRate);
        }
    }
}