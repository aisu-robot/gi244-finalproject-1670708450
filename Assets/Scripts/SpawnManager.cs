using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public Vector3 spawnPos = new(25, 0, 0);

    public float startDelay = 2;
    public float repeatRate = 2;

    private PlayerController playerController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 1. แก้ไขตรงนี้: ต้องใส่ playerController = ด้านหน้า เพื่อเก็บค่าที่ค้นหาเจอลงในตัวแปร
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        InvokeRepeating(nameof(SpawnObstacle), startDelay, repeatRate);
    }

    void SpawnObstacle()
    {
        // 2. เช็คว่าถ้าเกมยังไม่จบ (gameOver == false) ถึงจะทำการเสกวัตถุ
        if (playerController.gameOver == false)
        {
            // 3. สุ่มตำแหน่งซ้าย-ขวา (แกน Z) 
            // สมมติว่าความกว้างถนนคือ -4 ถึง 4 (สามารถปรับตัวเลขนี้ให้เข้ากับถนนจริงในเกมคุณได้ครับ)
            float randomZ = Random.Range(-4f, 4f);

            // 4. สร้างตำแหน่งจุดเกิดใหม่ (X=25 คือระยะไกลสุด, Y=0 คือติดพื้น, Z=ตำแหน่งสุ่มซ้ายขวา)
            Vector3 randomSpawnPos = new Vector3(25, 0, randomZ);

            // เสกวัตถุออกมาตามตำแหน่งที่สุ่มได้
            Instantiate(obstaclePrefab, randomSpawnPos, obstaclePrefab.transform.rotation);
        }
    }
}
