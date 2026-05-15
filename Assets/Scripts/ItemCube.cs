using System.Collections;
using UnityEngine;

public class ItemCube : MonoBehaviour
{
    [Header("ตั้งค่าไอเทม")]
    public float powerUpDuration = 5f;

    private PlayerController player;
    private SpawnManager spawnManager;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. ซ่อนกล่องและปิดการชน
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;

            // 2. ปิดสคริปต์ MoveLeft เพื่อไม่ให้กล่องไหลไปโดนระบบทำลายขยะลบทิ้ง
            MoveLeft moveLeftScript = GetComponent<MoveLeft>();
            if (moveLeftScript != null)
            {
                moveLeftScript.enabled = false;
            }

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
            player.moveSpeed *= 3f;

            yield return new WaitForSeconds(powerUpDuration);
            player.moveSpeed = oldSpeed; // คืนค่าความเร็วเดิม
        }
        else if (effectIndex == 2)
        {
            Debug.Log("ได้บัฟ: กระโดดสูงปรี๊ด!");
            float oldJump = player.jumpForce;
            player.jumpForce *= 2f;

            yield return new WaitForSeconds(powerUpDuration);
            player.jumpForce = oldJump; // คืนค่าแรงกระโดดเดิม
        }
        else if (effectIndex == 3)
        {
            Debug.Log("โดนคำสาป: สิ่งกีดขวางคลั่ง!");
            spawnManager.StartCrazyMode(powerUpDuration);

            // รอเวลาให้คำสาปหมด
            yield return new WaitForSeconds(powerUpDuration);
        }

        // เมื่อรันคำสั่งคืนค่าต่างๆ เสร็จเรียบร้อยแล้ว ค่อยทำลาย Object กล่องนี้ทิ้งจริงๆ
        Destroy(gameObject);
    }
}