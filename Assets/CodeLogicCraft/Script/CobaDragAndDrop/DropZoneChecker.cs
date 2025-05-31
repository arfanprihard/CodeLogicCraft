using UnityEngine;

public class DropZoneChecker : MonoBehaviour
{
    private int maxButtonMain123 = 14;
    private int maxButtonMain4 = 10;
    private int maxButtonMethod = 7;
    public Transform rootParent;

    void Start()
    {

    }

    public int CountTaggedActiveChildren(Transform parent)
    {
        string[] tags = new string[] { "Button", "LoopButton", "Percabangan" };
        // Jika parent bernama "Percabangan", maka abaikan dia dan semua child-nya
        if (parent.name == "Percabangan")
            return 0;

        int counter = 0;

        foreach (Transform child in parent)
        {
            if (!child.gameObject.activeInHierarchy)
                continue;

            foreach (string tag in tags)
            {
                if (child.CompareTag(tag))
                {
                    counter++;
                    break;
                }
            }

            // Rekursif ke dalam child-child
            counter += CountTaggedActiveChildren(child);
        }

        return counter;
    }

    public bool isDropZoneFull(Transform dropZone)
    {
        if (dropZone.name == "Main")
        {
            int tingkatanKesulitanSekarang = PlayerPrefs.GetInt("TingkatKesulitan");
            if (tingkatanKesulitanSekarang <= 3)
            {
                if (dropZone != null)
                {
                    int jumlahButton = CountTaggedActiveChildren(rootParent);

                }
            }
            else
            {

            }
        }
        return false;
    }
    void Update()
    {
        // string nameObj = transform.name;
        // Transform obj = transform;
        // bool foundDropZone = false;

        // while (obj != null) // Loop selama masih ada parent
        // {
        //     if (obj.tag == "DropZone")
        //     {
        //         if (nameObj == "LoopIn")
        //         {
        //             gameObject.tag = "LoopButton";
        //         }
        //         else if (nameObj == "Percabangan")
        //         {
        //             gameObject.tag = "PercabanganButton";
        //         }
        //         else
        //         {
        //             gameObject.tag = "Button";
        //         }

        //         foundDropZone = true;
        //         break;
        //     }

        //     obj = obj.parent; // lanjut ke atas
        // }

        // if (!foundDropZone)
        // {
        //     gameObject.tag = "Untagged";
        // }
    }

}
