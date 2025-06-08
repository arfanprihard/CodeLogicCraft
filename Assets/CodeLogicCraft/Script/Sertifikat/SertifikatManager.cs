using TMPro;
using UnityEngine;

public class SertifikatManager : MonoBehaviour
{
    public TMP_Text namePlayer;
    public TMP_Text nilai;
    public GameObject lockedSertifikat;
    public GameObject unlockedSertifikat;
    void Start()
    {
        int bintangLevelTerakhir = SaveLoadSystem.Instance.GetBintangLevelTerakhir();
        int totalBintang = SaveLoadSystem.Instance.GetTotalBintang();

        if (bintangLevelTerakhir > 0)
        {
            unlockedSertifikat.SetActive(true);
            lockedSertifikat.SetActive(false);
            namePlayer.text = SaveLoadSystem.Instance.GetNama();
            if (totalBintang >= 60)
            {
                nilai.text = "Sempurna\n(" + totalBintang + "/60 Bintang)";
            }
            else
            {
                nilai.text = "Baik\n(" + totalBintang + "/60 Bintang)";
            }

        }
        else
        {
            unlockedSertifikat.SetActive(false);
            lockedSertifikat.SetActive(true);
        }
    }
}
