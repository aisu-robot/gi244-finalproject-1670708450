using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject obstaclePrefab;

    // 1. เพิ่มระยะเกิดให้ไกลขึ้น (เปลี่ยนเลขตรงนี้ได้เลย)
    public float spawnDistance = 70f;

    // 2. ปรับเวลาให้รัวขึ้น (ค่ายิ่งน้อย วัตถุยิ่งเกิดเร็วขึ้น)
    public float startDelay = 2f;
    public float repeatRate = 0.3f; // เปลี่ยนจาก 2 เป็น 0.8 วินาที

    private PlayerController playerController;

    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        // สั่งให้ทำซ้ำตามเวลาที่ตั้งไว้
        InvokeRepeating(nameof(SpawnObstacle), startDelay, repeatRate);
    }

    void SpawnObstacle()
    {
        if (playerController.gameOver == false)
        {
            // 3. สุ่มจำนวนที่จะเกิดในรอบนี้ (สุ่มเลข 1 ถึง 2)
            // หมายเหตุ: Random.Range(1, 3) หมายถึงจะสุ่มได้เลข 1 หรือ 2 (ไม่รวม 3)
            int spawnCount = Random.Range(7, 17);

            // ใช้ loop สั่งเสกวัตถุตามจำนวนที่สุ่มได้ข้างบน
            for (int i = 0; i < spawnCount; i++)
            {
                // สุ่มตำแหน่งซ้าย-ขวา (แกน Z)
                float randomZ = Random.Range(-10f, 10f);

                // สร้างตำแหน่งจุดเกิดใหม่ โดยอิงจากระยะ spawnDistance
                Vector3 randomSpawnPos = new Vector3(spawnDistance, 0, randomZ);

                // สั่งเสกวัตถุ
                Instantiate(obstaclePrefab, randomSpawnPos, obstaclePrefab.transform.rotation);
            }
        }
    }
}