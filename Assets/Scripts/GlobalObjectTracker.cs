using System;
using UnityEngine;

public class GlobalObjectTracker : MonoBehaviour
{
    public GameObject Terrain;
    public GameObject TournamentBoidsController;
    public GameObject RankingBoidsController;

    [NonSerialized] public GameObject BoidsController;
    public bool IsRanking;


    public void SetRanking(bool q)
    {
        IsRanking = q;
    }

    void Awake()
    {
        //TournamentBoidsController.SetActive(!IsRanking);
        //RankingBoidsController.SetActive(IsRanking);

        BoidsController = IsRanking ? RankingBoidsController : TournamentBoidsController;
    }
}
