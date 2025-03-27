[System.Serializable]
public class GameData
{
    public int[][] bintangPerLevel; // [tingkat_kesulitan][level]

    public GameData(int tingkatKesulitan, int levelPerKesulitan)
    {
        bintangPerLevel = new int[tingkatKesulitan][];
        for (int i = 0; i < tingkatKesulitan; i++)
        {
            bintangPerLevel[i] = new int[levelPerKesulitan]; // Default 0 bintang
        }
    }

    public int GetTotalBintang()
    {
        int total = 0;
        foreach (int[] levels in bintangPerLevel)
        {
            foreach (int bintang in levels)
            {
                total += bintang;
            }
        }
        return total;
    }
}
