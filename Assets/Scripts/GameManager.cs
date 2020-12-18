using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Sprite[] sprits = null;

    [SerializeField]
    private RcpObject MyObject = null;

    [SerializeField]
    private Transform genomsParent = null;

    [SerializeField]
    private GameObject genomsObj = null;

    [SerializeField]
    private Generation generation = null;

    [SerializeField]
    private Text[] scoreText = null;

    [SerializeField]
    private Text text = null;

    private int generationCount = 0;

    private int nowScore = 0;
    private int bestScore = 0;

    private int[,] answerTable = { {0,1,1,1,0,0,0},
                                    {0,0,1,1,1,0,0},
                                    {0,0,0,1,1,1,0},
                                    {0,0,0,0,1,1,1},
                                    {1,0,0,0,0,1,1},
                                    {1,1,0,0,0,0,1},
                                    {1,1,1,0,0,0,0} };

    public void StepRCP()
    {
        //Debug.LogFormat("=====================");
        for (int j = 0; j < 1; j++)
        {

            int rand = UnityEngine.Random.Range(0, 7);

            while(MyObject.State == rand)
            {
                rand = UnityEngine.Random.Range(0, 7);
            }

            int live = 0;
            double[] input = { 0,0,0,0,0,0,0,0 };
            input[rand] = 1;

            //Debug.LogFormat("rand  {0}",rand);

            MyObject.SetRct(sprits[rand],rand);

            for (int i = 0; i < genomsParent.childCount; i++)
            {
                RcpObject tempRcp = genomsParent.GetChild(i).GetComponent<RcpObject>();
                if (tempRcp.Live)
                {
                    live++;
                }
            }

            if (live > 0)
            {
                for (int i = 0; i < genomsParent.childCount; i++)
                {
                    RcpObject tempRcp = genomsParent.GetChild(i).GetComponent<RcpObject>();

                    if (tempRcp.Live)
                    {
                        int answer = ConversionToAnswer(generation.Genomes[i].Answer(input));

                        if (answer > -1)
                        {
                            tempRcp.SetRct(sprits[answer], answer);
                            int score = CheckResult(rand, answer);

                            switch (score)
                            {
                                case 0:
                                    tempRcp.SetLive(false);
                                    break;
                                case 1:
                                    generation.Genomes[i].Score += score;
                                    break;
                            }
                        }
                        else
                        {
                            tempRcp.SetLive(false);
                            generation.Genomes[i].Score--;
                        }
                    }
                }
                //Debug.LogFormat("{0}   win {1}  draw {2}  lose {3}", rand, resultCount[0], resultCount[1], resultCount[2]);
                nowScore++;
                scoreText[1].text = string.Format("NowScore : {0}", nowScore);
            }
            else if(live == 0)
            {
                NextGeneration();
                if(nowScore > bestScore)
                {
                    bestScore = nowScore;
                }
                nowScore = 0;
                scoreText[0].text = string.Format("BestScore : {0}", bestScore);
            }
        }
    }

    public void Test(int inn)
    {
        /*
        double[] input = { (double)inn / 3, 0 };
        for (int i = 0; i < genomsParent.childCount; i++)
        {
            int answer = ConversionToAnswer(generation.Genomes[i].Answer(input));
            Debug.LogFormat("test  {0}   {1} ", i, answer);
        }
        Debug.LogFormat("===============");
        */
        double[] input = {0,0};
        for (int i = 1; i < 7; i++)
        {
            input[0] = i * 0.25;
            input[1] = i * 0.25;
            Debug.LogFormat("=============test  {0}   {1} ", i, generation.Genomes[inn].Answer(input));
        }
        Debug.LogFormat("===============");

    }

    private int CheckResult(int rand, int answer)
    {
        /*
        int score = 0;

        if(rand == 1 && answer == 3)
        {
            score = 2;
        }
        else if(rand == 2 && answer == 1)
        {
            score = 2;
        }
        else if(rand == 3 && answer == 2)
        {
            score = 2;
        }

        if(rand == answer)
        {
            score = 1;
        }

        return score;
        */
        return answerTable[answer, rand];
    }

    private int ConversionToAnswer(double value)
    {
        int answer = -1;

        if(value < -0.2)
        {
            answer = 0;
        }
        else if(value > 0.3)
        {
            answer = 6;
        }

        for (int i = 1; i < 6; i++)
        {
            if (value > -0.3 + (0.1 * i) && value < -0.2 + (0.1 * i))
            {
                answer = i;
            }
        }

        return answer;
    }

    public void NextGeneration()
    {
        if (nowScore > bestScore)
        {
            bestScore = nowScore;
        }
        nowScore = 0;
        scoreText[0].text = string.Format("BestScore : {0}", bestScore);
        scoreText[1].text = string.Format("NowScore : {0}", nowScore);

        generationCount++;

        text.text = string.Format("== {0}th Generation ==", generationCount);

        int rand = UnityEngine.Random.Range(0, 3);

        for (int i = 0; i < genomsParent.childCount; i++)
        {
            RcpObject tempRcp = genomsParent.GetChild(i).GetComponent<RcpObject>();
            tempRcp.SetLive(true);
            tempRcp.SetLive(true);
            tempRcp.SetRct(null, rand);
        }

        generation.SetBestGenomes();
        generation.Mutations();

        /*
        for (int i = 0; i < genomsParent.childCount; i++)
        {
            Debug.LogFormat("init      {0}   {1}",i, generation.Genomes[i].Score);
        }
        */
    }
}
