using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Genome
{
    static int inputSize = 8;
    static int hiddenSize1 = 8;
    static int hiddenSize2 = 8;
    static int hiddenSize3 = 4;
    static int outputSize = 1;

    private double[,] W1 = new double[inputSize, hiddenSize1];
    private double[,] W2 = new double[hiddenSize1, hiddenSize2];
    private double[,] W3 = new double[hiddenSize2, hiddenSize3];
    private double[,] W4 = new double[hiddenSize3, outputSize];

    private int score = 0;

    public int Score
    {
        get { return score; }
        set { score = value; }
    }

    public double[,] WW1
    {
        get { return W1; }
        set { W1 = value; }
    }

    public double[,] WW2
    {
        get { return W2; }
        set { W2 = value; }
    }

    public double[,] WW3
    {
        get { return W3; }
        set { W3 = value; }
    }

    public double[,] WW4
    {
        get { return W4; }
        set { W4 = value; }
    }

    public Genome(Genome c)
    {
        W1 = c.WW1.Clone() as double[,];
        W2 = c.WW2.Clone() as double[,];
        W3 = c.WW3.Clone() as double[,];
        W4 = c.WW4.Clone() as double[,];

        score = 0;
    }

    public Genome()
    {
        Initialize();
        score = 0;
    }

    public void Initialize()
    {
        SetValue(W1);
        SetValue(W2);
        SetValue(W3);
        SetValue(W4);

        /*
        double[] input = new double[] {1f,0f};

        Answer(input);
        */
    }

    private void SetValue(double[,] arr)
    {
        for (int i = 0; i < arr.GetLength(0); i++)
        {
            for (int j = 0; j < arr.GetLength(1); j++)
            {
                arr[i, j] = UnityEngine.Random.Range(-1.5f, 1.5f);
                //Debug.LogFormat("fdas  {0}", arr[i, j]);
            }
        }
    }

    public double Answer(double[] input)
    {
        double answer = -1;


        if (input.Length == inputSize)
        {
            double[] z1 = new double[W1.GetLength(1)];
            for (int i = 0; i < z1.Length; i++)
            {
                for (int j = 0; j < W1.GetLength(0); j++)
                {
                    z1[i] += input[j] * W1[j, i];
                }

                z1[i] = Math.Tanh(z1[i]);
            }

            double[] z2 = new double[W2.GetLength(1)];
            for (int i = 0; i < z2.Length; i++)
            {
                for (int j = 0; j < W2.GetLength(0); j++)
                {
                    z2[i] += z1[j] * W2[j, i];
                }

                z2[i] = Math.Tanh(z2[i]);
            }

            double[] z3 = new double[W3.GetLength(1)];
            for (int i = 0; i < z3.Length; i++)
            {
                for (int j = 0; j < W3.GetLength(0); j++)
                {
                    z3[i] += z2[j] * W3[j, i];
                }

                z3[i] = Math.Tanh(z3[i]);
            }

            double[] z4 = new double[W4.GetLength(1)];
            for (int i = 0; i < W4.GetLength(0); i++)
            {
                z4[0] += z3[i] * W4[i, 0];
            }
            answer = Math.Tanh(z4[0]);
            //Debug.LogFormat("output   {0}", z4[0]);
        }

        return answer;
    }
}
