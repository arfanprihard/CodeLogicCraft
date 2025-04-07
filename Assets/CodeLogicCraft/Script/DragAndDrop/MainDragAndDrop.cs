using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainDragAndDrop : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public Transform dropZone; // Assign Horizontal Layout Group di Inspector
    public Transform dropZoneMethod; // Assign Horizontal Layout Group di Inspector

    private GameObject clone;
    private RectTransform cloneRectTransform;
    public GameObject prefabButton;



    private Transform parentButton;
    private Transform parentLoop;

    private bool isDragging = false;
    private bool isInsideDropZone = false; // Menyimpan status apakah objek berada di dalam dropZone
    private bool isInsideDropZoneMethod = false; // Menyimpan status apakah objek berada di dalam dropZone
    private bool isInsideButton = false;
    private bool isInsideIfButton = false;
    private bool isInsideLoop = false;



    private bool isInsideLeftButton = false;
    private bool isInsideRightButton = false;



    private int index;
    private int indexIfButton;
    private int indexLoop;

    public GameObject prefabPlaceHolder;
    private GameObject clonePrefabPlaceHolder;
    private RectTransform clonePlaceHolderRectTransform;
    private Vector3 savedPositionPrefab;

    private Vector3 startDragPosition;
    private float dragThreshold = 10f; // Ambang batas jarak untuk mendeteksi drag

    private int maxDropZoneMainItems = 10;
    private int maxDropZoneMethodItems = 7;


    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");

        startDragPosition = eventData.position; // Simpan posisi awal
        Transform canvas = FindObjectOfType<Canvas>().transform;
        if (gameObject.CompareTag("Button"))
        {
            clone = gameObject;
            int indexClone = clone.transform.GetSiblingIndex();
            clonePrefabPlaceHolder = Instantiate(prefabPlaceHolder, transform.parent);
            clonePrefabPlaceHolder.name = prefabPlaceHolder.name;
            clonePrefabPlaceHolder.transform.SetParent(clone.transform.parent);
            clonePrefabPlaceHolder.transform.SetSiblingIndex(indexClone);
            cloneRectTransform = clone.GetComponent<RectTransform>();
            clonePlaceHolderRectTransform = clonePrefabPlaceHolder.GetComponent<RectTransform>();
            clone.transform.SetParent(canvas);
            cloneRectTransform.sizeDelta = new Vector2(85f, 85f);



            clone.tag = "Untagged";

        }
        ///////
        else
        {
            clone = Instantiate(prefabButton, transform.parent);
            clone.name = prefabButton.name;
            clone.transform.SetParent(null);
            clone.transform.position = transform.position;
            clone.transform.SetParent(transform.parent);
            cloneRectTransform = clone.GetComponent<RectTransform>();
            clone.transform.SetParent(canvas);
            clonePrefabPlaceHolder = Instantiate(prefabPlaceHolder, transform.parent);
            clonePrefabPlaceHolder.name = prefabPlaceHolder.name;
            clonePrefabPlaceHolder.transform.SetParent(null);
            clonePlaceHolderRectTransform = clonePrefabPlaceHolder.GetComponent<RectTransform>();
            savedPositionPrefab = clonePrefabPlaceHolder.transform.position;
        }



        MainDragAndDrop cloneScript = clone.GetComponent<MainDragAndDrop>();
        if (cloneScript != null)
        {
            cloneScript.dropZone = this.dropZone;
            cloneScript.prefabButton = this.prefabButton;
            cloneScript.dropZoneMethod = this.dropZoneMethod;

        }

        isDragging = true;
    }


    public void OnDrag(PointerEventData eventData)
    {
        // Debug.Log("OnDrag");
        // Debug.Log("Jumlah child Main = " + totalChildMain);

        if (isDragging && clone != null)
        {
            Debug.Log("OnDrag");

            cloneRectTransform.localScale = Vector3.one;
            isInsideDropZone = false; // Default false
            isInsideButton = false;
            isInsideDropZoneMethod = false;
            isInsideIfButton = false;
            isInsideLoop = false;

            // Mengikuti posisi kursor
            cloneRectTransform.position = eventData.position;


            CheckRaycastDropZone(eventData);
            CheckRaycastButton(eventData);



        }
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;

        int indexPlaceHolder = clonePrefabPlaceHolder.transform.GetSiblingIndex();


        // Hitung jarak antara posisi awal dan posisi akhir
        float distance = Vector3.Distance(startDragPosition, eventData.position);

        if (distance < dragThreshold)
        {
            // Jika button langsung di klik


            Debug.Log("Button clicked!");

            if (ApakahChildTidakPenuh(dropZone) && (gameObject.transform.parent.name != "Main" || gameObject.transform.parent.name != "Method"))
            {
                clone.transform.SetParent(dropZone);
                clone.tag = "Button";
            }
            else
            {
                Destroy(clone);
                Debug.Log("SLOT PENUH DER!!");
            }
        }
        else
        {
            CheckRaycastPlaceHolder(eventData);
        }

        Destroy(clonePrefabPlaceHolder);
        
    }


    private void CheckRaycastButton(PointerEventData eventData)
    {
        // Mengumpulkan semua objek di bawah pointer
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        // Reset status sebelum pengecekan
        isInsideButton = false;
        parentButton = null;
        isInsideLeftButton = false;
        isInsideRightButton = false;

        foreach (RaycastResult result in results)
        {
            GameObject hitObject = result.gameObject;

            // Jika mengenai PlaceHolder, hentikan proses
            if (hitObject.CompareTag("PlaceHolder"))
            {
                Debug.Log("Menemukan PlaceHolder - Tidak bisa menempatkan lagi");
                return;
            }

            // Validasi apakah berada di dalam parent Button
            if (hitObject.transform.parent?.parent != null && hitObject.transform.parent.parent.CompareTag("Button"))
            {
                Transform hitParent = hitObject.transform.parent.parent;

                // Mencari DropZone ("Main" atau "Method")
                Transform currentDropZone = null;
                Transform obj = hitParent;
                while (obj != null)
                {
                    if (obj.name == "Main" || obj.name == "Method")
                    {
                        currentDropZone = obj;
                        break;
                    }
                    obj = obj.parent;
                }

                // Menentukan posisi berdasarkan nama child
                string childName = hitObject.name.ToLower();
                isInsideButton = true;

                if (childName == "kiri")
                {
                    if (hitParent.parent.name == "isi")
                    {
                        Debug.Log("Arah: Kiri");
                        parentButton = hitParent.parent.parent.parent.parent;
                        index = hitParent.parent.parent.parent.GetSiblingIndex();
                    }
                    else
                    {
                        parentButton = hitParent.parent;
                        index = hitParent.GetSiblingIndex();
                    }

                    isInsideLeftButton = true;

                }
                else if (childName == "kanan")
                {
                    if (hitParent.parent.name == "isi")
                    {
                        Debug.Log("Arah: Kanan");
                        parentButton = hitParent.parent.parent.parent.parent;
                        index = hitParent.parent.parent.parent.GetSiblingIndex() + 1;
                    }
                    else
                    {
                        parentButton = hitParent.parent;
                        index = hitParent.GetSiblingIndex() + 1;
                    }

                    isInsideRightButton = true;

                }
                else if (childName == "isi" && hitObject.transform.childCount == 0 && transform.name != "LoopIn" && transform.name != "Percabangan")
                {
                    parentButton = hitObject.transform;
                    index = 0;
                    Debug.Log("Arah: Isi");
                }
                else if (childName == "isiloop")
                {
                    parentButton = hitParent.parent;
                    index = hitParent.GetSiblingIndex() + 1;
                    isInsideLeftButton = true;
                    Debug.Log("Arah: isiLoop");
                }

                Debug.Log($"Parent = {parentButton}, Index = {index}");

                if (currentDropZone != null && currentDropZone.name == "Method" && transform.name == "Method")
                {
                    break;
                }

                bool bisaDitempatkan = ApakahChildTidakPenuh(currentDropZone);
                Debug.Log("Apakah Tidak Penuh = " + bisaDitempatkan);

                // Menempatkan Placeholder jika memungkinkan
                if (bisaDitempatkan || (childName == "isiloop" && transform.name != "LoopIn") || childName == "isi")
                {
                    clonePrefabPlaceHolder.transform.SetParent(parentButton);
                    clonePrefabPlaceHolder.transform.SetSiblingIndex(index);
                }
                else if (childName == "isiloop" && transform.name == "LoopIn")
                {
                    if (currentDropZone != null)
                    {
                        int maxItems = currentDropZone.name == "Main" ? maxDropZoneMainItems : maxDropZoneMethodItems;
                        if (HitungSemuaButton(currentDropZone) < maxItems)
                        {
                            clonePrefabPlaceHolder.transform.SetParent(parentButton);
                            clonePrefabPlaceHolder.transform.SetSiblingIndex(index);
                        }
                        else
                        {
                            clonePrefabPlaceHolder.transform.SetParent(null);
                        }
                    }

                }
                else
                {
                    clonePrefabPlaceHolder.transform.SetParent(null);
                }

                break; // Hentikan setelah menemukan target yang sesuai
            }
        }
    }
    private void CheckRaycastDropZone(PointerEventData eventData)
    {
        // Mengumpulkan semua objek di bawah pointer
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        // Reset status sebelum pengecekan
        isInsideButton = false;
        isInsideDropZone = false;
        isInsideDropZoneMethod = false;
        parentButton = null;

        bool foundDropZone = false; // Menandai apakah DropZone ditemukan

        foreach (RaycastResult result in results)
        {
            GameObject hitObject = result.gameObject;

            // Jika menemukan PlaceHolder, hentikan proses
            if (hitObject.CompareTag("PlaceHolder"))
            {
                Debug.Log("Menemukan PlaceHolder - Tidak bisa menempatkan lagi");
                return;
            }

            // Jika menemukan DropZone
            if (hitObject.CompareTag("DropZone"))
            {
                Debug.Log("Mengenai DropZone");
                parentButton = hitObject.transform;
                foundDropZone = true;

                // Jika DropZone belum penuh dan placeholder belum ada di dalam
                if (ApakahChildTidakPenuh(parentButton)) //&& clonePrefabPlaceHolder.transform.parent != parentButton)
                {
                    isInsideDropZone = true;
                    Debug.Log($"Menemukan DropZone: {hitObject.name}, Total Child = {parentButton.childCount}");

                    // Menambahkan clonePrefabPlaceHolder ke DropZone
                    if (parentButton.name == "Method" && transform.name == "Method")
                    {
                        clonePrefabPlaceHolder.transform.SetParent(null);
                    }
                    else
                    {

                        clonePrefabPlaceHolder.transform.SetParent(parentButton);
                    }

                }
                else
                {
                    foundDropZone = false;
                }

                break; // Keluar dari loop setelah menemukan DropZone
            }
        }

        // Jika tidak menemukan DropZone, kembalikan placeholder ke posisi awal
        if (!foundDropZone)
        {
            clonePrefabPlaceHolder.transform.SetParent(null);
            clonePrefabPlaceHolder.transform.position = savedPositionPrefab;
        }
        else
        {
            clonePlaceHolderRectTransform.sizeDelta = new Vector2(85f, 85f);
        }
    }


    private void CheckRaycastPlaceHolder(PointerEventData eventData)
    {
        // Mengumpulkan semua objek di bawah pointer
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        // Reset status sebelum pengecekan
        isInsideButton = false;
        isInsideDropZone = false;
        isInsideDropZoneMethod = false;
        parentButton = null;

        bool isValidDrop = false;

        foreach (RaycastResult result in results)
        {
            GameObject hitObject = result.gameObject;


            if (hitObject.CompareTag("DropZone"))
            {
                foreach (Transform child in hitObject.GetComponentsInChildren<Transform>())
                {
                    if (child.CompareTag("PlaceHolder"))
                    {
                        Debug.Log("Ditemukan PlaceHolder di " + child.parent.name);
                        index = child.transform.GetSiblingIndex();
                        parentButton = child.transform.parent;

                        clone.transform.SetParent(parentButton);
                        clone.transform.SetSiblingIndex(index);
                        isValidDrop = true;
                        clone.tag = "Button";
                        break;
                    }
                }
                break;
            }
        }

        // Jika tidak ada yang valid, hancurkan clone
        if (!isValidDrop)
        {
            Destroy(clone);
        }
    }


    bool ApakahChildTidakPenuh(Transform layout)
    {
        int total = HitungSemuaButton(layout);
        bool apakahTidakPenuh = false;
        total += transform.name == "LoopIn" ? 1 : 0;
        Debug.Log("Total Child (Button) = " + total);

        if (layout != null)
        {
            if (layout.name == "Main")
            {
                apakahTidakPenuh = total < maxDropZoneMainItems;
            }
            else if (layout.name == "Method")
            {
                apakahTidakPenuh = total < maxDropZoneMethodItems;
            }
            else if (layout.name == "isi")
            {
                apakahTidakPenuh = total < 1;
            }
        }


        return apakahTidakPenuh;
    }

    public int HitungSemuaButton(Transform parent)
    {
        int jumlahButton = 0;
        if (parent != null)
        {
            foreach (Transform child in parent)
            {
                // Cek apakah aktif di hierarchy dan memiliki tag "Button"
                // Debug.Log("Apakah kena Place Holder dengan parent Loop = " + (child.CompareTag("PlaceHolder") && child.parent.name == "LoopIn"));

                if (child.CompareTag("Button") && child.parent.name != "isi" && child.gameObject.activeInHierarchy && child.name != "loopImg")
                {

                    jumlahButton += 1;


                }

                // Rekursif untuk memeriksa child dari child yang aktif
                jumlahButton += HitungSemuaButton(child);
            }
        }

        return jumlahButton;
    }

}
