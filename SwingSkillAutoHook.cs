using UnityEngine;

public class SwingSkillAutoHook : MonoBehaviour
{
    public float hookDistance = 5f; // ระยะที่จุดโหนจะถูกสร้าง
    public KeyCode swingKey = KeyCode.E; // ปุ่มสำหรับใช้สกิลโหน
    public LineRenderer ropeRenderer; // ใช้แสดงผลเชือก
    public LayerMask hookableLayers; // เลเยอร์ที่ใช้ตรวจสอบจุดโหน

    private HingeJoint2D hingeJoint; // Joint สำหรับการโหน
    private Rigidbody2D rb; // Rigidbody ของตัวละคร
    private GameObject currentHook; // จุดโหนปัจจุบัน

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(swingKey))
        {
            CreateHook(); // สร้างจุดโหน
        }

        if (Input.GetKeyUp(swingKey))
        {
            ReleaseHook(); // ปล่อยจุดโหน
        }

        UpdateRope(); // อัปเดตเชือก (ถ้ามี LineRenderer)
    }

    void CreateHook()
    {
        if (hingeJoint != null) return; // ห้ามสร้างซ้ำถ้ากำลังโหนอยู่

        // คำนวณตำแหน่งจุดโหนในทิศทางของตัวละคร
        Vector2 hookPoint = (Vector2)transform.position + GetHookDirection() * hookDistance;

        // ตรวจสอบว่าจุดโหนอยู่ในพื้นที่ Hookable
        RaycastHit2D hit = Physics2D.Raycast(transform.position, GetHookDirection(), hookDistance, hookableLayers);
        if (hit.collider != null)
        {
            hookPoint = hit.point; // ปรับตำแหน่งจุดโหนให้ตรงกับพื้นที่ที่พบ
        }

        // สร้างวัตถุจุดโหน
        currentHook = new GameObject("Hook Point");
        currentHook.transform.position = hookPoint;

        // เพิ่ม HingeJoint2D เชื่อมตัวละครกับจุดโหน
        hingeJoint = gameObject.AddComponent<HingeJoint2D>();
        hingeJoint.connectedAnchor = hookPoint; // จุดที่เชื่อมต่อ
        hingeJoint.enableCollision = false; // ปิดการชนกัน

        // เปิดใช้งาน LineRenderer ถ้ามี
        if (ropeRenderer != null)
        {
            ropeRenderer.enabled = true;
            ropeRenderer.positionCount = 2;
        }
    }

    void ReleaseHook()
    {
        if (hingeJoint != null)
        {
            Destroy(hingeJoint); // ลบ Joint
        }

        if (currentHook != null)
        {
            Destroy(currentHook); // ลบจุดโหน
        }

        if (ropeRenderer != null)
        {
            ropeRenderer.enabled = false; // ซ่อนเชือก
        }
    }

    void UpdateRope()
    {
        if (ropeRenderer != null && hingeJoint != null)
        {
            ropeRenderer.SetPosition(0, transform.position); // ตำแหน่งของ Player
            ropeRenderer.SetPosition(1, hingeJoint.connectedAnchor); // ตำแหน่งของ Hook Point
        }
    }

    Vector2 GetHookDirection()
    {
        // ทิศทางการโหน (เช่น ตามแกน X หรือ Y)
        return transform.localScale.x > 0 ? Vector2.right : Vector2.left; // หันขวา = ขวา, หันซ้าย = ซ้าย
    }
}
