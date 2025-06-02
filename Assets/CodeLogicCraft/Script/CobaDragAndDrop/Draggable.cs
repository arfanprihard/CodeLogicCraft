using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Button button;
    public Transform main;
    public Transform method;

    public GameObject prefabToClone;
    public GameObject prefabPlaceHolder;

    private bool isDragging = false;
    bool foundDropZone = false;
    private GameObject currentClone;
    private RectTransform cloneRectTransform;
    private GameObject clonePrefabPlaceHolder;

    private Canvas canvas;


    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        button.onClick.AddListener(OnButtonClicked);

    }

    void Update()
    {


    }

    private void OnButtonClicked()
    {
        if (!isDragging)
        {
            if (transform.parent.CompareTag("Bingkai"))
            {
                if (CanAcceptMoreItems(main, transform))
                {
                    SpawnClone(main);
                }
                else
                {
                    if (method != null)
                    {
                        if (CanAcceptMoreItems(method, transform))
                        {
                            SpawnClone(method);
                        }

                    }
                }

            }
            else
            {
                Debug.Log("Ke destroy");
                Destroy(gameObject);
            }
        }

    }

    void SpawnClone(Transform parent)
    {
        if (parent.name == "Method" && gameObject.name == "Method")
        {
            return;
        }
        if (prefabToClone != null && !isDragging)
        {
            currentClone = Instantiate(prefabToClone, parent, false);
            currentClone.name = prefabToClone.name;
            cloneRectTransform = currentClone.GetComponent<RectTransform>();
            cloneRectTransform.localScale = prefabToClone.transform.localScale;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {


        if (transform.parent.CompareTag("Bingkai"))
        {
            clonePrefabPlaceHolder = Instantiate(prefabPlaceHolder, canvas.transform);
            clonePrefabPlaceHolder.name = prefabPlaceHolder.name;

            SpawnClone(canvas.transform);

        }
        else
        {
            currentClone = gameObject;
            currentClone.transform.SetParent(canvas.transform);
            clonePrefabPlaceHolder = Instantiate(prefabPlaceHolder, canvas.transform);
            clonePrefabPlaceHolder.name = prefabPlaceHolder.name;
            // int indexClone = currentClone.transform.GetSiblingIndex();
            // clonePrefabPlaceHolder.transform.SetParent(currentClone.transform.parent);
            // clonePrefabPlaceHolder.transform.SetSiblingIndex(indexClone);
            cloneRectTransform = currentClone.GetComponent<RectTransform>();
            //clonePlaceHolderRectTransform = clonePrefabPlaceHolder.GetComponent<RectTransform>();

        }

        if (currentClone.name != "LoopIn")
        {
            transform.localScale = Vector3.one;
        }
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging && currentClone != null)
        {
            // Mengikuti posisi kursor
            cloneRectTransform.position = eventData.position;
            CheckRaycastDropZone(eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;

        if (foundDropZone)
        {
            Debug.Log("menemukan dropzone found");
            int indexPlaceHolder = clonePrefabPlaceHolder.transform.GetSiblingIndex();
            currentClone.transform.SetParent(clonePrefabPlaceHolder.transform.parent);
            currentClone.transform.SetSiblingIndex(indexPlaceHolder);
        }
        else
        {
            Debug.Log("Tidak menemukan dropzone found");
            Destroy(currentClone);
        }
        Destroy(clonePrefabPlaceHolder);
    }

    private void CheckRaycastDropZone(PointerEventData eventData)
    {
        // Mengumpulkan semua objek di bawah pointer
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foundDropZone = false;
        foreach (RaycastResult result in results)
        {
            GameObject hitObject = result.gameObject;
            if (currentClone != null && hitObject.transform.IsChildOf(currentClone.transform))
            {
                continue;
            }

            if (!hitObject.CompareTag("Untagged"))
            {
                if (hitObject.transform.parent.tag == "Bingkai" || hitObject.transform.parent.parent.tag == "Bingkai")
                {
                    continue;
                }

                foundDropZone = true;
                if (hitObject.CompareTag("PlaceHolder"))
                {
                    Debug.Log("Menemukan PlaceHolder - Tidak bisa menempatkan lagi");
                    return;
                }
                else if (hitObject.CompareTag("Button"))
                {
                    if (FindDropZone(hitObject.transform).name == "Method" && IsThereMethodButton(currentClone.transform))
                    {
                        continue;
                    }
                    if (hitObject.transform.parent.name == "Percabangan" && hitObject.transform.parent.childCount > 1)
                    {
                        Transform currentParent = hitObject.transform.parent;
                        Transform currentDropZone = FindDropZone(currentParent);

                        SetPlaceHolder(currentDropZone, currentParent, 0);
                    }
                    else
                    {
                        Transform currentParent = hitObject.transform;
                        Transform currentDropZone = FindDropZone(currentParent);

                        SetPlaceHolder(currentDropZone, currentParent, 0);
                    }


                    Debug.Log("Menemukan Button");
                    return;
                }
                else if (hitObject.CompareTag("DropZone"))
                {
                    if (hitObject.name == "Method" && IsThereMethodButton(currentClone.transform))
                    {
                        continue;
                    }
                    if (CanAcceptMoreItems(hitObject.transform, currentClone.transform))
                    {
                        clonePrefabPlaceHolder.transform.SetParent(hitObject.transform);
                        clonePrefabPlaceHolder.transform.localScale = Vector3.one;
                        currentClone.transform.localScale = Vector3.one;
                    }
                    Debug.Log("Menemukan DropZone");
                    return;
                }
                else if (hitObject.CompareTag("LoopButton"))
                {
                    if (FindDropZone(hitObject.transform).name == "Method" && IsThereMethodButton(currentClone.transform))
                    {
                        continue;
                    }
                    Transform currentParent = hitObject.transform;
                    Transform currentDropZone = FindDropZone(currentParent);
                    RectTransform rt = hitObject.GetComponent<RectTransform>();
                    if (rt == null) continue;

                    Vector2 localPoint;
                    if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, eventData.position, eventData.pressEventCamera, out localPoint))
                    {
                        float halfWidth = rt.rect.width / 2f;
                        float regionThreshold = 15f;

                        Debug.Log($"Local X: {localPoint.x}, HalfWidth: {halfWidth}");

                        if (localPoint.x < -halfWidth + regionThreshold)
                        {
                            Debug.Log("Kena kiri");

                            SetPlaceHolder(currentDropZone, currentParent, 0);
                            return;
                        }
                        else if (localPoint.x > halfWidth - regionThreshold)
                        {
                            Debug.Log("Kena kanan");

                            SetPlaceHolder(currentDropZone, currentParent, 1);
                            return;

                        }
                        else
                        {
                            Debug.Log("Kena tengah");
                            if (CanAcceptMoreItems(currentDropZone, currentClone.transform))
                            {
                                clonePrefabPlaceHolder.transform.SetParent(currentParent);
                                clonePrefabPlaceHolder.transform.SetSiblingIndex(currentParent.childCount - 2);
                                clonePrefabPlaceHolder.transform.localScale = Vector3.one;
                                currentClone.transform.localScale = Vector3.one;
                                return;
                            }
                            // else
                            // if (hitObject.transform.GetChild(0).gameObject.activeInHierarchy)
                            // {
                            //     if (CountTaggedActiveChildren(currentClone.transform) == 1)
                            //     {
                            //         clonePrefabPlaceHolder.transform.SetParent(currentParent);
                            //         clonePrefabPlaceHolder.transform.SetSiblingIndex(currentParent.childCount - 2);
                            //         clonePrefabPlaceHolder.transform.localScale = Vector3.one;
                            //         currentClone.transform.localScale = Vector3.one;
                            //     }
                            //     return;
                            // }
                            return;
                        }
                    }
                }
                else if (hitObject.CompareTag("PercabanganButton"))
                {
                    if (FindDropZone(hitObject.transform).name == "Method" && IsThereMethodButton(currentClone.transform))
                    {
                        continue;
                    }
                    RectTransform rt = hitObject.GetComponent<RectTransform>();
                    if (rt == null) continue;

                    Vector2 localPoint;
                    if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, eventData.position, eventData.pressEventCamera, out localPoint))
                    {
                        float halfWidth = rt.rect.width / 2f;
                        float regionThreshold = 10f;

                        if (localPoint.x < -halfWidth + regionThreshold)
                        {
                            Debug.Log("Kena kiri");

                            Transform currentParent = hitObject.transform;
                            Transform currentDropZone = FindDropZone(currentParent);

                            SetPlaceHolder(currentDropZone, currentParent, 0);
                            return;
                        }
                        else if (localPoint.x > halfWidth - regionThreshold)
                        {
                            Debug.Log("Kena kanan");
                            Transform currentParent = hitObject.transform;
                            Transform currentDropZone = FindDropZone(currentParent);

                            SetPlaceHolder(currentDropZone, currentParent, 1);
                            return;
                        }
                        else
                        {
                            if (currentClone.name != "LoopIn" && currentClone.name != "Percabangan")
                            {
                                Debug.Log("Kena tengah");
                                if (hitObject.transform.childCount <= 1)
                                {
                                    clonePrefabPlaceHolder.transform.SetParent(hitObject.transform);
                                    clonePrefabPlaceHolder.transform.localScale = new Vector3(0.65f, 0.65f, 1f);
                                    currentClone.transform.localScale = new Vector3(0.65f, 0.65f, 1f);
                                }
                                return;
                            }
                            else
                            {
                                Transform currentParent = hitObject.transform;
                                Transform currentDropZone = FindDropZone(currentParent);

                                SetPlaceHolder(currentDropZone, currentParent, 0);
                                return;
                            }
                        }
                    }
                }
            }
            else
            {
                foundDropZone = false;
            }
        }
        if (!foundDropZone)
        {
            clonePrefabPlaceHolder.transform.SetParent(null);
        }
    }

    private Transform FindDropZone(Transform child)
    {
        Transform current = child;

        while (current != null)
        {
            if (current.CompareTag("DropZone"))
            {
                return current;
            }

            current = current.parent;
        }

        // Tidak ditemukan
        return null;
    }

    private void SetPlaceHolder(Transform dropZone, Transform currentParent, int plusIndex)
    {
        if (CanAcceptMoreItems(dropZone, currentClone.transform))
        {
            int indexParent = currentParent.GetSiblingIndex() + plusIndex;
            currentParent = currentParent.parent;
            clonePrefabPlaceHolder.transform.SetParent(currentParent);
            clonePrefabPlaceHolder.transform.SetSiblingIndex(indexParent);
            clonePrefabPlaceHolder.transform.localScale = Vector3.one;
            currentClone.transform.localScale = Vector3.one;
        }
        else
        {
            foundDropZone = false;
        }
    }


    public int CountTaggedActiveChildren(Transform parent)
    {
        string[] tags = new string[] { "Button", "LoopButton", "Percabangan" };

        // Jika parent bernama "Percabangan", maka abaikan dia dan semua child-nya
        if (parent.name == "Percabangan")
            return 1;

        int counter = 0;

        // Cek tag pada parent itu sendiri
        if (parent.gameObject.activeInHierarchy)
        {
            foreach (string tag in tags)
            {
                if (parent.CompareTag(tag))
                {

                    counter++;

                    break;
                }
            }
        }

        // Lanjut cek semua child-nya secara rekursif
        foreach (Transform child in parent)
        {
            counter += CountTaggedActiveChildren(child);
        }

        return counter;
    }
    private bool IsThereMethodButton(Transform parent)
    {
        if (parent.name == "Method")
        {
            return true;
        }
        foreach (Transform child in parent.GetComponentsInChildren<Transform>(true))
        {
            if (child.name == "Method")
            {
                return true;
            }
        }
        return false;
    }


    private int maxButtonMain123 = 14;
    private int maxButtonMain4 = 10;
    private int maxButtonMethod = 7;
    public bool CanAcceptMoreItems(Transform dropZone, Transform dragItem)
    {
        if (dropZone.name == "Main")
        {
            int tingkatanKesulitanSekarang = PlayerPrefs.GetInt("TingkatKesulitan");
            if (tingkatanKesulitanSekarang >= 4)
            {
                int totalButtonDropZone = CountTaggedActiveChildren(dropZone);
                int totalButtonDragItem = CountTaggedActiveChildren(dragItem);
                Debug.Log("Total Button DropZone = " + totalButtonDropZone);
                Debug.Log("Total Button DragItem = " + totalButtonDragItem);
                Debug.Log("Total Button main = " + (totalButtonDropZone + totalButtonDragItem));
                return totalButtonDropZone + totalButtonDragItem <= maxButtonMain4;
            }
            else if (tingkatanKesulitanSekarang >= 1 && tingkatanKesulitanSekarang <= 3)
            {
                int totalButtonDropZone = CountTaggedActiveChildren(dropZone);
                int totalButtonDragItem = CountTaggedActiveChildren(dragItem);
                Debug.Log("Total Button DropZone = " + totalButtonDropZone);
                Debug.Log("Total Button DragItem = " + totalButtonDragItem);
                Debug.Log("Total Button main = " + (totalButtonDropZone + totalButtonDragItem));
                return totalButtonDropZone + totalButtonDragItem <= maxButtonMain123;
            }
        }
        else if (dropZone.name == "Method")
        {
            int totalButtonDropZone = CountTaggedActiveChildren(dropZone);
            int totalButtonDragItem = CountTaggedActiveChildren(dragItem);
            return totalButtonDropZone + totalButtonDragItem <= maxButtonMethod;
        }
        return false;
    }
    public bool GetIsDragging()
    {
        return isDragging;
    }

}
