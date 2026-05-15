using System.Collections;
using UnityEngine;

public class ItemCube : MonoBehaviour
{
    [Header("ตั้งค่าไอเทม")]
    public float powerUpDuration = 5f; // เวลาที่บัฟ/คำสาป จะแสดงผล (วินาที)

    private PlayerController player;
    private SpawnManager spawnManager;

    void Start()
    {
        // ค้นหา Player และ SpawnManager ในฉาก
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // ตรวจสอบว่าคนที่มาชนคือผู้เล่นใช่ไหม
        if (other.CompareTag("Player"))
        {
            // ซ่อนกล่องและปิดการชนทันที เพื่อให้ดูเหมือนว่าถูกเก็บไปแล้ว
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;

            // สุ่มเลข 1, 2, หรือ 3
            int randomEffect = Random.Range(1, 4);

            // เริ่มการทำงานของไอเทม
            StartCoroutine(ApplyEffectRoutine(randomEffect));
        }
    }

    IEnumerator ApplyEffectRoutine(int effectIndex)
    {
        if (effectIndex == 1)
        {
            Debug.Log("ได้บัฟ: เดินเร็ว 3 เท่า!");
            float oldSpeed = player.moveSpeed;
            player.moveSpeed *= 3f; // เพิ่มสปีด 3 เท่า

            yield return new WaitForSeconds(powerUpDuration);

            player.moveSpeed = oldSpeed; // คืนค่าความเร็วเดิม
        }
        else if (effectIndex == 2)
        {
            Debug.Log("ได้บัฟ: กระโดดสูงปรี๊ด!");
            float oldJump = player.jumpForce;
            player.jumpForce *= 2f; // เพิ่มแรงกระโดด 2 เท่า

            yield return new WaitForSeconds(powerUpDuration);

            player.jumpForce = oldJump; // คืนค่าแรงกระโดดเดิม
        }
        else if (effectIndex == 3)
        {
            Debug.Log("โดนคำสาป: สิ่งกีดขวางคลั่ง!");
            // โยนหน้าที่ไปให้ SpawnManager จัดการเสกของรัวๆ
            spawnManager.StartCrazyMode(powerUpDuration);
        }

        // รอจนกว่าเวลาบัฟจะหมดจริงๆ แล้วค่อยทำลาย Object กล่องนี้ทิ้ง
        yield return new WaitForSeconds(powerUpDuration);
        Destroy(gameObject);
    }
}