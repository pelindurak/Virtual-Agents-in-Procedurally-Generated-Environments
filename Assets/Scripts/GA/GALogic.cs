using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GALogic
{
    // how big is the dna?
    private int GeneSize;

    // how many parents are appointed when choosing the best 
    private int BestAgentsListSize;

    // ref to every agent
    public List<Agent> Agents;

    // fittest agents on each generation are selected
    private List<Agent> BestAgents;

    // number of generations since the beginning of the simulation
    private int NumGenerations = 0;

    // how many agents will be spawned initially
    private int NumAgentsAtSpawn = 0;

    // how likely is this species to mutate
    private float MutationChance;

    private System.Random _Random;

    // by default, mutation chance is none and the gene size is 5.
    // possible genes are added as enum in Gene.cs
    public GALogic(float mutationChance, int geneSize, 
        int numAgentsAtSpawn, int bestAgentsListSize, System.Random _Random,
        float[] maxValues)
    {
        this._Random = _Random;
        MutationChance = mutationChance;
        GeneSize = geneSize;
        NumAgentsAtSpawn = numAgentsAtSpawn;
        BestAgentsListSize = bestAgentsListSize;

        // declare both the agents (general population) and the best agents (highest fitness) lists
        Agents = new List<Agent>();
        BestAgents = new List<Agent>();

        // initialize the population with Agents that have random attributes
        Agents = CreateNewGeneration(numAgentsAtSpawn, maxValues);
    }

    /// <summary>
    /// Create a list of Agents that have randomized attributes
    /// </summary>
    /// <remarks>
    /// the "size" parameter is the size of the list to be returned
    /// </remarks>
    /// <param name="size">the size of the list to be generated</param>
    /// <returns></returns>
    public List<Agent> CreateNewGeneration(int size, float[] MaxValues)
    {
        List<Agent> agentList = new List<Agent>();
        for (int i = 0; i < size; i++)
        {
            agentList.Add(new Agent(GeneSize, _Random, MaxValues));
        }
        NumGenerations++; // first generation
        return agentList;
    }

    public List<Agent> CreateSubsequentGeneration(bool isRanking)
    {
        List<Agent> newList = new List<Agent>();

        if (isRanking)
        {
            BestAgents = ChooseBestAgentsRanking();
        }
        else
        {
            BestAgents = ChooseBestAgentsTournament();
        }
        
        for (int i = 0; i < Agents.Count; i++)
        {
            if (BestAgents.Count <= 0) break;
            Agent parent1 = FindGoodParent();

            if (BestAgents.Count <= 0) break;
            Agent parent2 = FindGoodParent();

            Agent child = MergeAgents(parent1, parent2, MutationChance);

            newList.Add(child);
        }

        // add the new children to the population.
        // we do this after breeding the parents.
        foreach (Agent agent in newList)
        {
            Agents.Add(agent); 
        }

        NumGenerations++;

        return newList;
    }

    /// <summary>
    /// Choose the agents with the highest fitness.
    /// "size" denotes how many agent will be chosen.
    /// </summary>
    public List<Agent> ChooseBestAgentsRanking()
    {
        List<Agent> agents = Agents;
        List<Agent> bestAgents = new List<Agent>();

        int listSize = agents.Count;

        agents.Sort();

        for (int i = 0; i < BestAgentsListSize; i++)
        {
            bestAgents.Add(agents[listSize - i - 1]);
        }

        return bestAgents;
    }

    public List<Agent> ChooseBestAgentsTournament()
    {
        List<Agent> agents = Agents;
        List<Agent> bestAgents = new List<Agent>();
        int matchSize = 3;
        while (bestAgents.Count < BestAgentsListSize)
        {
            for (int i = 0; i < agents.Count - matchSize + 1; i += matchSize)
            {
                Agent agent1 = agents[i];
                Agent agent2 = agents[i + 1];
                Agent agent3 = agents[i + 2];

                Agent winner = Showdown(agent1, agent2, agent3);

                bestAgents.Add(winner);

                if (bestAgents.Count > BestAgentsListSize) break;
            }
        }
        return bestAgents;
    }

    public Agent Showdown(Agent agent1, Agent agent2, Agent agent3)
    {
        Agent first = agent1.CompareTo(agent2) > 0 ? agent1 : agent2;
        return first.CompareTo(agent3) > 0 ? first : agent3;
    }

    public void EliminateWorstAgents()
    {
        List<Agent> agents = Agents;
        List<float> worstAgentsFitness = new List<float>();

        agents.Sort();

        for (int i = 0; i < BestAgentsListSize / 2; i++)
        {
            worstAgentsFitness.Add(agents[i].CalculateFitness());
        }


        worstAgentsFitness.Sort();

        int fSize = worstAgentsFitness.Count;
        float bestWorstFitness = worstAgentsFitness[fSize - 1];


        for (int i = 0; i < Agents.Count; i++)
        {
            float aFitness = Agents[i].CalculateFitness();
            if (aFitness <= bestWorstFitness)
            {
                Agents[i].IsAlive = false;
            }
        }
    }

    /// <summary>
    /// choose randomly from the list of BestAgents.
    /// Removes the returned parent object from the list.
    /// </summary>
    /// <returns>An Agent from BestAgents</returns>
    public Agent FindGoodParent()
    {
        int randomIndex = _Random.Next(BestAgents.Count);
        //int randomIndex = 0;
        //do
        //{
        //    randomIndex = _Random.Next(BestAgentsListSize);
        //}
        //while (randomIndex >= BestAgents.Count);

        Agent newParent = BestAgents[randomIndex];
        BestAgents.RemoveAt(randomIndex);
        return newParent;
    }

    /// <summary>
    /// simulates two parents having a child.
    /// </summary>
    /// <remarks>
    /// the "mutationChance" parameter determines the percent 
    /// chance of a random mutation.
    /// mutationChance is between 0 and 1.
    /// </remarks>
    /// <param name="parent1"></param>
    /// <param name="parent2"></param>
    /// <param name="mutationChance"></param>
    /// <returns>a child of given parents</returns>
    public Agent MergeAgents(Agent parent1, Agent parent2, float mutationChance)
    {
        Agent newAgent = new Agent(GeneSize, _Random, parent1.MaxValues);
        
        for (int i = 0; i < GeneSize; i++)
        {
            // introduce a change of mutation
            double chance = _Random.NextDouble();
            if (chance < mutationChance)
            {
                chance = (float)_Random.NextDouble();
                // mutation chance is low, no mutation
                // take a parent's gene by 50% chance
                newAgent.DNA[i] = chance < 0.5f ? parent1.DNA[i] : parent2.DNA[i];
            }
            else
            {
                // mutation happens
                newAgent.DNA[i] = newAgent.CreateRandomGene(i, parent1.MaxValues[i]);
            }
        }
        return newAgent;
    }
}
