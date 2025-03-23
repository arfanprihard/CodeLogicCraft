using UnityEngine;

public class DragDetectDirection : MonoBehaviour
{
    private bool isDragging = false;  // Menandakan apakah sedang drag
    private Vector3 offset;           // Menyimpan jarak antara mouse dan objek
    public GameObject kotak2;         // Referensi ke Kotak 2

    void Update()
    {
        // Menggerakkan objek selama drag
        if (isDragging)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.WorldToScreenPoint(transform.position).z;
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePos) + offset;
            transform.position = targetPos;
        }
    }

    void OnMouseDown()
    {
        // Mulai drag dan hitung offset antara posisi mouse dan objek
        isDragging = true;
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        offset = transform.position - Camera.main.ScreenToWorldPoint(mousePos);
    }

    void OnMouseUp()
    {
        isDragging = false; // Selesai drag

        if (IsInsideKotak2())
        {
            float jarakX = kotak2.transform.position.x - transform.position.x;

            if (jarakX > 0 && jarakX <= 10) // Jika dalam 10 unit di sebelah kiri
            {
                Debug.Log("Arah Kiri");
            }
            else
            {
                Debug.Log("Bukan di Arah Kiri");
            }
        }
    }

    // Cek apakah kotak 1 berada di dalam kotak 2
    private bool IsInsideKotak2()
    {
        if (kotak2 == null) return false;

        // Ambil ukuran collider kedua kotak
        Collider2D kotak1Collider = GetComponent<Collider2D>();
        Collider2D kotak2Collider = kotak2.GetComponent<Collider2D>();

        return kotak1Collider.bounds.Intersects(kotak2Collider.bounds);
    }
}
