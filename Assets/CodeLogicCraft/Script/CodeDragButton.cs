using UnityEngine;
using UnityEngine.EventSystems;

public class DragUIObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform deadZone;
    private RectTransform rectTransform;
    private Vector2 startPosition; // Menyimpan posisi awal

    private void Start()
    {
        // Ambil komponen RectTransform
        rectTransform = GetComponent<RectTransform>();

        // Simpan posisi awal
        startPosition = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Mulai Drag: " + rectTransform.anchoredPosition);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Pindahkan RectTransform sesuai posisi kursor (eventData.position sudah dalam Screen Space)
        rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Akhir Drag di: " + rectTransform.anchoredPosition);

        // Kembalikan ke posisi awal setelah drag selesai
        rectTransform.anchoredPosition = startPosition;
    }
}
