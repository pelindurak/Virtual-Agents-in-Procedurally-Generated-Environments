using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;  

public class GAScript : MonoBehaviour
{
    public float startX;
    public float startZ;
    public float spread;

    public const int _GeneSize = 7;

    // time between new generations
    [SerializeField] private float SecondsTilNextGen = 1;
    private float _Timer;

    // how many agents will be spawned initially
    [SerializeField] private int NumAgentsAtSpawn = 0;


    [SerializeField] private int MaxAgentsSpawn = 20;

    // how many parents are appointed when choosing the best 
    [SerializeField] private int BestAgentsListSize = 2;

    [SerializeField] private GameObject AgentPrefab;

    // how likely is this species to mutate
    [SerializeField] private float MutationChance = 0.1f;

    private List<Agent> Agents;

    private System.Random _Random;

    private AgentManager _AgentManager; 
    private GALogic _GALogic;
    private BoidsMovement _Movement;

    public float[] MaxValues = new float[7];

    [SerializeField] private bool IsRanking;

    [SerializeField] private TMP_Text GenText;
    private int GenNum = 0;

    private HandleTextFile _HandleTextFile;


    private void Awake()
    {
        _Random = new System.Random();
        _Timer = SecondsTilNextGen;

        _GALogic = new GALogic(MutationChance, _GeneSize,
        NumAgentsAtSpawn, BestAgentsListSize, _Random, 
        MaxValues);


        Agents = _GALogic.Agents;

        _AgentManager = new AgentManager(startX, startZ, spread,
            NumAgentsAtSpawn, AgentPrefab, _Random, Agents);

        _HandleTextFile = new HandleTextFile();

    }

    void Start()
    {
        GenNum = 1;
        GenText.text = $"Gen {GenNum.ToString()}";

        _Movement = GameObject.FindGameObjectWithTag("GlobalObjectTracker")
            .GetComponent<GlobalObjectTracker>()
            .BoidsController.GetComponent<BoidsMovement>();

    }


    void Update()
    {
        _Movement.MoveBoids();

        if (NextGenTimer() && Agents.Count <= MaxAgentsSpawn)
        {

            GenNum++;
            GenText.text = $"Gen {GenNum.ToString()}";

            _Timer = SecondsTilNextGen;


            //_GALogic.EliminateWorstAgents();

            List<Agent> newborns = _GALogic.CreateSubsequentGeneration(IsRanking);
            _AgentManager.Spawn(newborns, startX, startZ);
        }

        if (NextGenTimer())
        {
            float avg = AverageFitness(Agents);
            _HandleTextFile.WriteText(avg + "\n");

        }
    }

    public float AverageFitness(List<Agent> agents)
    {
        float sum = 0f;

        for (int i = 0; i < agents.Count; i++)
        {
            sum += agents[i].CalculateFitness();
        }
        sum /= Agents.Count;

        return sum;
    }

    /// <summary>
    /// timer function that needs to be called in Update().
    /// keeps track of the time until the next generation is born.
    /// </summary>
    /// <returns>true if the timer has run out, false otherwise</returns>
    private bool NextGenTimer()
    {
        _Timer -= Time.deltaTime;
        return _Timer < 0;
    }
}
