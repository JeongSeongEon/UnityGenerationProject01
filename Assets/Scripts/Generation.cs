using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation : MonoBehaviour
{
    private List<Genome> genomes = new List<Genome>();
    private int population = 50;
    private int keep_best = 10;
    private int lucky_few = 10;
    private float chance_of_mutation = 0.2f;
    private float mutationFloat = 1f;

    private void Start()
    {
        for (int i = 0; i < population; i++)
        {
            NewGenomes();
        }
        //SetBestGenomes();
        //Mutations();
    }
    public List<Genome> Genomes
    {
        get { return genomes; }
    }

    public Genome NewGenomes()
    {
        Genome newGenome = new Genome();
        newGenome.Initialize();
        genomes.Add(newGenome);
        return newGenome;
    }

    public void SetBestGenomes()
    {
        genomes.Sort(delegate (Genome aa, Genome bb)
        {
            if (aa.Score > bb.Score) return -1;
            else if (aa.Score < bb.Score) return 1;
            return 0;
        });

        while(genomes.Count > keep_best)
        {
            genomes.RemoveAt(keep_best);
        }


        /*
        if (genomes[0].Score == 0)
        {
            while (genomes.Count > 0)
            {
                genomes.RemoveAt(0);
            }

            for (int i = 0; i < keep_best; i++)
            {
                Genome newGenome = this.gameObject.AddComponent<Genome>();
                newGenome.Initialize();
                genomes.Add(newGenome);
            }
        }
        */
    }

    private void DebugGenome(string str, Genome ge)
    {
        Debug.LogFormat(string.Format("{0}   {1}  : {2}  {3}  {4}  {5}", str, "WW1", ge.WW1[0, 0], ge.WW2[0, 0], ge.WW3[0, 0], ge.WW4[0, 0]));
    }

    private Genome CopyGenome(Genome old)
    {
        Genome temp = new Genome(old);
        //temp.WW1 = old.WW1;
        //temp.WW2 = old.WW2;
        //temp.WW3 = old.WW3;
        //temp.WW4 = old.WW4;

        return temp;
    }
    
    public void Mutations()
    {
        while (genomes.Count < population - lucky_few)
        {
            //DebugTest("00000  ");
            int rand1 = Random.Range(0, keep_best);
            int rand2 = Random.Range(0, keep_best);
            var temp1 = CrossOver(genomes[rand1], genomes[rand2], rand1, rand2, genomes.Count);
            //DebugTest("11111  ");
            //DebugGenome(string.Format("top1 aaaaa  "), temp1);//여기까지는 결과값 가지고있다
            var temp2 = Mutate(temp1);
            //DebugTest("22222  ");
            //DebugGenome(string.Format("top1 bbbbb  "), temp2);//여기서 바뀜
            genomes.Add(temp1);
            //DebugTest("33333  ");
            //DebugGenome(string.Format("top1 ccccc  "), genomes[genomes.Count-1]);
        }

        while (genomes.Count < population)
        {
            genomes.Add(Mutate(genomes[Random.Range(0, genomes.Count)]));
        }


        //ShuffleGenome();
        for (int i = 0; i < genomes.Count; i++)
        {
            genomes[i].Score = 0;
        }

        
    }

    private void DebugTest(string str)
    {
        for (int l = 0; l < genomes.Count; l++)
        {
            int sum = 0;
            for (int i = 0; i < genomes[0].WW1.GetLength(1); i++)
            {
                for (int j = 0; j < genomes[0].WW1.GetLength(0); j++)
                {
                    if (genomes[0].WW1[j, i] == genomes[l].WW1[j, i])
                    {
                        sum++;
                    }
                }
            }

            for (int i = 0; i < genomes[0].WW2.GetLength(1); i++)
            {
                for (int j = 0; j < genomes[0].WW2.GetLength(0); j++)
                {
                    if (genomes[0].WW2[j, i] == genomes[l].WW2[j, i])
                    {
                        sum++;
                    }
                }
            }

            for (int i = 0; i < genomes[0].WW3.GetLength(1); i++)
            {
                for (int j = 0; j < genomes[0].WW3.GetLength(0); j++)
                {
                    if (genomes[0].WW3[j, i] == genomes[l].WW3[j, i])
                    {
                        sum++;
                    }
                }
            }

            for (int i = 0; i < genomes[0].WW4.GetLength(1); i++)
            {
                for (int j = 0; j < genomes[0].WW4.GetLength(0); j++)
                {
                    if (genomes[0].WW4[j, i] == genomes[l].WW4[j, i])
                    {
                        sum++;
                    }
                }
            }
            Debug.LogFormat("same  {0}  {1}   {2}%", str, l, (float)sum / 164f * 100);
        }
        Debug.LogFormat("=========");
    }

    public Genome CrossOver(Genome g1, Genome g2, int aa, int bb, int count)
    {
        Genome newG1 = CopyGenome(g1);

        Genome newG2 = CopyGenome(g2);

        //DebugGenome(string.Format("newG1  {0}  {1}  ",aa, count), newG1);
        //DebugGenome(string.Format("newG2  {0}  {1}  ",bb, count), newG2);

        for (int i = 0; i < g1.WW1.GetLength(1); i++)
        {
            for (int j = 0; j < g1.WW1.GetLength(0); j++)
            {
                newG1.WW1[j, i] = Random.Range(0, 2) == 0 ? newG1.WW1[j, i] : newG2.WW1[j, i];
                //newG1.WW1[j, i] = UnityEngine.Random.Range(-1.5f, 1.5f);
            }
        }

        for (int i = 0; i < g1.WW2.GetLength(1); i++)
        {
            for (int j = 0; j < g1.WW2.GetLength(0); j++)
            {
                newG1.WW2[j, i] = Random.Range(0, 2) == 0 ? newG1.WW2[j, i] : newG2.WW2[j, i];
                //newG1.WW2[j, i] = UnityEngine.Random.Range(-1.5f, 1.5f);
            }
        }

        for (int i = 0; i < g1.WW3.GetLength(1); i++)
        {
            for (int j = 0; j < g1.WW3.GetLength(0); j++)
            {
                newG1.WW3[j, i] = Random.Range(0, 2) == 0 ? newG1.WW3[j, i] : newG2.WW3[j, i];
                //newG1.WW3[j, i] = UnityEngine.Random.Range(-1.5f, 1.5f);
            }
        }

        for (int i = 0; i < g1.WW4.GetLength(1); i++)
        {
            for (int j = 0; j < g1.WW4.GetLength(0); j++)
            {
                newG1.WW4[j, i] = Random.Range(0, 2) == 0 ? newG1.WW4[j, i] : newG2.WW4[j, i];
                //newG1.WW4[j, i] = UnityEngine.Random.Range(-1.5f, 1.5f);
            }
        }

        //DebugGenome(string.Format("return           "), newG1);
        Debug.LogFormat(string.Format("==========================="));
        return newG1;
    }

    public Genome Mutate(Genome g1)
    {
        Genome temp = CopyGenome(g1);
        /*
        Genome temp = gameObject.AddComponent<Genome>();
        temp.WW1 = g1.WW1;
        temp.WW2 = g1.WW2;
        temp.WW3 = g1.WW3;
        temp.WW4 = g1.WW4;
        */
        if (Random.Range(0f, 1f) < chance_of_mutation)
        {
            for (int i = 0; i < temp.WW1.GetLength(1); i++)
            {
                for (int j = 0; j < temp.WW1.GetLength(0); j++)
                {
                    if (Random.Range(0f, 1f) < 0.5)
                    {
                        temp.WW1[j, i] += temp.WW1[j, i] * (mutationFloat * Random.Range(0f, 1f));
                    }
                    else
                    {
                        temp.WW1[j, i] -= temp.WW1[j, i] * (mutationFloat * Random.Range(0f, 1f));
                    }
                    //Debug.LogFormat("Mutate  W1  {0}   {1}", temp.WW1[j, i], g1.WW1[j, i]);
                }
            }
        }

        if (Random.Range(0f, 1f) < chance_of_mutation)
        {
            for (int i = 0; i < temp.WW2.GetLength(1); i++)
            {
                for (int j = 0; j < temp.WW2.GetLength(0); j++)
                {
                    if (Random.Range(0f, 1f) < 0.5)
                    {
                        temp.WW2[j, i] += temp.WW2[j, i] * (mutationFloat * Random.Range(0f, 1f));
                    }
                    else
                    {
                        temp.WW2[j, i] -= temp.WW2[j, i] * (mutationFloat * Random.Range(0f, 1f));
                    }
                    //Debug.LogFormat("Mutate  W2  {0}   {1}", temp.WW2[j, i], g1.WW2[j, i]);
                }
            }
        }

        if (Random.Range(0f, 1f) < chance_of_mutation)
        {
            for (int i = 0; i < temp.WW3.GetLength(1); i++)
            {
                for (int j = 0; j < temp.WW3.GetLength(0); j++)
                {
                    if (Random.Range(0f, 1f) < 0.5)
                    {
                        temp.WW3[j, i] += temp.WW3[j, i] * (mutationFloat * Random.Range(0f, 1f));
                    }
                    else
                    {
                        temp.WW3[j, i] -= temp.WW3[j, i] * (mutationFloat * Random.Range(0f, 1f));
                    }
                    //Debug.LogFormat("Mutate  W3  {0}   {1}", temp.WW3[j, i], g1.WW3[j, i]);
                }
            }
        }

        if (Random.Range(0f, 1f) < chance_of_mutation)
        {
            for (int i = 0; i < temp.WW4.GetLength(1); i++)
            {
                for (int j = 0; j < temp.WW4.GetLength(0); j++)
                {
                    if (Random.Range(0f, 1f) < 0.5)
                    {
                        temp.WW4[j, i] += temp.WW4[j, i] * (mutationFloat * Random.Range(0f, 1f));
                    }
                    else
                    {
                        temp.WW4[j, i] -= temp.WW4[j, i] * (mutationFloat * Random.Range(0f, 1f));
                    }
                    //Debug.LogFormat("Mutate  W4  {0}   {1}", temp.WW4[j, i], g1.WW4[j, i]);
                }
            }
        }

        return temp;
    }

    public void ShuffleGenome()
    {
        int random1 = 0;
        int random2 = 0;
        Genome temp = null;


        for (int i = 0; i < genomes.Count * 2; i++)
        {
            random1 = UnityEngine.Random.Range(0, genomes.Count);
            random2 = UnityEngine.Random.Range(0, genomes.Count);

            temp = genomes[random1];
            genomes[random1] = genomes[random2];
            genomes[random2] = temp;
        }
    }
}
