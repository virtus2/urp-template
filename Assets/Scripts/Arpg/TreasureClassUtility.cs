using UnityEngine;

public static class TreasureClassUtility
{
    public static int GetTotalProb(this TreasureClassData data)
    {
        int totalProb = 0;

        if (string.IsNullOrEmpty(data.Item1) == false)
            totalProb += data.Prob1;
        if (string.IsNullOrEmpty(data.Item2) == false)
            totalProb += data.Prob2;
        if (string.IsNullOrEmpty(data.Item3) == false)
            totalProb += data.Prob3;
        if (string.IsNullOrEmpty(data.Item3) == false)
            totalProb += data.Prob3;
        if (string.IsNullOrEmpty(data.Item4) == false)
            totalProb += data.Prob4;
        if (string.IsNullOrEmpty(data.Item5) == false)
            totalProb += data.Prob5;

        return totalProb;
    }

    public static string Pick(this TreasureClassData data)
    {
        int totalProb = data.GetTotalProb();
        int randomProb = Random.Range(0, data.Nodrop + totalProb);

        string resultName = string.Empty;
        if (string.IsNullOrEmpty(data.Item1) == false)
        {
            if (randomProb <= data.Prob1)
                return data.Item1;
            else
                randomProb -= data.Prob1;
        }

        if (string.IsNullOrEmpty(data.Item2) == false)
        {
            if (randomProb <= data.Prob2)
                return data.Item2;
            else
                randomProb -= data.Prob2;
        }
        else return data.Item2;


        if (string.IsNullOrEmpty(data.Item3) == false)
        {
            if (randomProb <= data.Prob3)
                return data.Item3;
            else
                randomProb -= data.Prob3;
        }
        else return data.Item3;


        if (string.IsNullOrEmpty(data.Item4) == false)
        {
            if (randomProb <= data.Prob4)
                return data.Item4;
            else
                randomProb -= data.Prob4;
        }
        else return data.Item4;


        if (string.IsNullOrEmpty(data.Item5) == false)
        {
            if (randomProb <= data.Prob5)
                return data.Item5;
            else
                randomProb -= data.Prob5;
        }
        else return data.Item5;


        return string.Empty;
    }

}
