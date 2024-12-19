using UnityEngine;

public class SwingSkillWithPivot : MonoBehaviour
{
    public float hookDistance = 5f; // ระยะห่างของจุดโหนจากตัวละคร
    public KeyCode swingKey = KeyCode.E; // ปุ่มสำหรับใช้สกิลโหน
    public LineRenderer ropeRenderer; // ใช้แสดงเชือก (ถ้ามี)

    private HingeJoint2D hingeJoint; // Joint สำหรับการโหน
    private Rigidbody2D rb; // Rigidbody ของตัวละคร
    private GameObject pivotPoint; // จุดหมุน (Pivot Point)

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(swingKey))
        {
            CreatePivotPoint(); // สร้างจุดโหน
        }

        if (Input.GetKeyUp(swingKey))
        {
            ReleaseSwing(); // ปล่อยจุดโหน
        }

        UpdateRope(); // อัปเดตเชือก (ถ้ามี LineRenderer)
    }

    void CreatePivotPoint()
    {
        if (hingeJoint != null) return; // ห้ามสร้างซ้ำถ้ากำลังโหนอยู่

        // คำนวณตำแหน่งของ Pivot Point ที่ 45 องศา
        Vector2 direction = new Vector2(1, 1).normalized; // ทิศทาง 45 องศา
        direction *= Mathf.Sign(transform.localScale.x); // หันตามตัวละคร (ซ้าย/ขวา)
        Vector2 pivotPosition = (Vector2)transform.position + direction * hookDistance;

        // สร้าง Pivot Point
        pivotPoint = new GameObject("Pivot Point");
        pivotPoint.transform.position = pivotPosition;

        // เพิ่ม HingeJoint2D เชื่อมตัวละครกับ Pivot Point
        hingeJoint = gameObject.AddComponent<HingeJoint2D>();
        hingeJoint.connectedAnchor = pivotPosition; // ตำแหน่งจุดหมุน
        hingeJoint.enableCollision = false; // ปิดการชนกัน

        // เปิดใช้งาน LineRenderer ถ้ามี
        if (ropeRenderer != null)
        {
            ropeRenderer.enabled = true;
            ropeRenderer.positionCount = 2;
        }
    }

    void ReleaseSwing()
    {
        if (hingeJoint != null)
        {
            Destroy(hingeJoint); // ลบ Hinge Joint
        }

        if (pivotPoint != null)
        {
            Destroy(pivotPoint); // ลบ Pivot Point
        }

        if (ropeRenderer != null)
        {
            ropeRenderer.enabled = false; // ซ่อนเชือก
        }

        // ตัวละครจะยังคงเคลื่อนที่ต่อด้วยแรงโมเมนตัม (Physics)
    }

    void UpdateRope()
    {
        if (ropeRenderer != null && hingeJoint != null)
        {
            ropeRenderer.SetPosition(0, transform.position); // ตำแหน่งของ Player
            ropeRenderer.SetPosition(1, hingeJoint.connectedAnchor); // ตำแหน่งของ Pivot Point
        }
    }
}
