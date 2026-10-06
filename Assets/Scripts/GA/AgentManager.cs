using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager
{
    public List<GameObject> agentObjects;
    private System.Random _Random;

    private float spread;
    private int numAgents;
    private GameObject agentPrefab;

    public AgentManager(float startX, float startZ, float spread, 
        int numAgents, GameObject agentPrefab, System.Random _Random,
        List<Agent> Agents)
    {
        this._Random = _Random;
        agentObjects = new List<GameObject>();

        this.spread = spread;
        this.agentPrefab = agentPrefab;

        for (int i = 0; i < numAgents; i++)
        {
            float randomX = (float)_Random.NextDouble() * spread;
            float randomZ = (float)_Random.NextDouble() * spread;

            float posX = startX + randomX;
            float posZ = startZ + randomZ;

            GameObject obj = GameObject.Instantiate(agentPrefab, new Vector3(posX, 1, posZ), new Quaternion());

            obj.GetComponent<Boid>().Position = new Vector3(posX, 1, posZ);
            obj.GetComponent<Boid>().Velocity = Vector3.zero;

            obj.GetComponent<Boid>().Speed                  = Agents[i].DNA[0].value;
            obj.GetComponent<Boid>().Size                   = Agents[i].DNA[1].value;
            obj.GetComponent<Boid>().AvoidanceRadius        = Agents[i].DNA[2].value;
            obj.GetComponent<Boid>().MoveCenterFactor       = Agents[i].DNA[3].value;
            obj.GetComponent<Boid>().FoodMotivationFactor   = Agents[i].DNA[4].value;
            obj.GetComponent<Boid>().CenterOfMassRadius     = Agents[i].DNA[5].value;
            obj.GetComponent<Boid>().Awareness              = (int)Agents[i].DNA[6].value;

            agentObjects.Add(obj);
        }
    }

    // when there are new offsprings, spawn these into the world space.
    public void Spawn(List<Agent> newborns, float startX, float startZ)
    {
        for (int i = 0; i < newborns.Count; i++)
        {
            float randomX = (float)_Random.NextDouble() * spread;
            float randomZ = (float)_Random.NextDouble() * spread;

            float posX = startX + randomX;
            float posZ = startZ + randomZ;

            GameObject obj = GameObject.Instantiate(agentPrefab, new Vector3(posX, 1, posZ), new Quaternion());

            obj.GetComponent<Boid>().Position = new Vector3(posX, 1, posZ);
            obj.GetComponent<Boid>().Velocity = Vector3.zero;

            obj.GetComponent<Boid>().Speed                  = newborns[i].DNA[0].value;
            obj.GetComponent<Boid>().Size                   = newborns[i].DNA[1].value;
            obj.GetComponent<Boid>().AvoidanceRadius        = newborns[i].DNA[2].value;
            obj.GetComponent<Boid>().MoveCenterFactor       = newborns[i].DNA[3].value;
            obj.GetComponent<Boid>().FoodMotivationFactor   = newborns[i].DNA[4].value;
            obj.GetComponent<Boid>().CenterOfMassRadius     = newborns[i].DNA[5].value;
            obj.GetComponent<Boid>().Awareness              = (int)newborns[i].DNA[6].value;

            agentObjects.Add(obj);
        }
    }

    

    // TODO handle the case where agents can walk to out of bounds of
    // the terrain


}
