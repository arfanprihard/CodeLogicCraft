using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private CanvasGroup canvasGroup;
    private Transform parentToReturnTo = null; // Menyimpan parent asli

    void Start()
    {
        // Pastikan objek memiliki CanvasGroup
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Simpan parent asli
        parentToReturnTo = transform.parent;

        // Lepaskan dari parent sementara
        transform.SetParent(transform.root);

        // Nonaktifkan raycast blocking agar objek di bawahnya bisa dideteksi
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Gerakkan objek mengikuti mouse
        transform.position = eventData.position;

        // Cari objek di bawah pointer
        GameObject target = eventData.pointerCurrentRaycast.gameObject;

        if (target != null)
        {
            Debug.Log("Objek yang terkena raycast: " + target.name);

            // Cek tag "looping" atau "dropzone" pada objek yang dideteksi atau parent-nya
            Transform parent = target.transform;
            while (parent != null)
            {
                if (parent.CompareTag("looping")) // Cek tag "looping"
                {

                    Debug.Log("MajuBt berhasil menjadi child dari LoopIn!");
                    return; // Keluar dari fungsi setelah berhasil
                }
                else if (parent.CompareTag("dropzone")) // Cek tag "dropzone"
                {
                    Debug.Log("MajuBt berhasil menjadi child dari Main!");
                    return; // Keluar dari fungsi setelah berhasil
                }
                parent = parent.parent; // Cek parent selanjutnya
            }

            Debug.Log("Objek yang terkena raycast bukan 'looping' atau 'dropzone'.");
        }
        else
        {
            Debug.Log("Tidak ada objek yang terkena raycast.");
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Kembalikan raycast blocking
        canvasGroup.blocksRaycasts = true;

        // Jika tidak ada objek yang valid, kembalikan ke parent asli
        if (transform.parent == transform.root)
        {
            transform.SetParent(parentToReturnTo);
            Debug.Log("MajuBt dikembalikan ke parent asli.");
        }
    }
}