using System;
using UnityEngine;

public class Agent : MonoBehaviour, IComparable<Agent>
{
    [SerializeField] public Gene[] DNA;
    public float[] MaxValues = new float[7];
    public bool IsAlive = true;

    private System.Random _Random;

    public Agent(int geneSize, System.Random _Random, float[] maxValues)
    {
        this._Random = _Random;
        DNA = new Gene[geneSize];
        MaxValues = maxValues;
        CreateRandomDNA();
    }

    void Update()
    {
        if (!IsAlive) Destroy(this);
    }

    public void CreateRandomDNA()
    {
        string debug = "CreateRandomDNA: ";
        for (int i = 0; i < DNA.Length; i++)
        {
            DNA[i] = CreateRandomGene(i, MaxValues[i]);
            debug += DNA[i].ToString() + "\n";
        }        
        Debug.Log(debug);
    }

    public Gene CreateRandomGene(int index, float maxValue)
    {
        float value = (float)_Random.NextDouble() * maxValue;
        string geneName = ((GeneName)index).ToString();
        Gene gene = new Gene(geneName, value, maxValue);
        return gene;
    }

    public float CalculateFitness()
    {
        // speed + size * size
        float energySpent = DNA[0].value + DNA[1].value * DNA[1].value;

        // speed * MoveCenterFactor + size + speed * FoodMotivationFactor + Awareness
        float advantage = DNA[0].value * DNA[3].value + DNA[1].value + DNA[0].value * DNA[4].value + DNA[6].value;

        return advantage - energySpent; 
    }

    public int CompareTo(Agent other)
    {
        if (other == null) return 1;
        else if (this.CalculateFitness() > other.CalculateFitness())
        {
            return 1;
        }
        else if (this.CalculateFitness() < other.CalculateFitness())
        {
            return -1;
        }
        else
        {
            return 0; 
        }
    }
}
