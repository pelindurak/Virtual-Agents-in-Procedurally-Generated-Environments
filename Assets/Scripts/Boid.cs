using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    #region Attributes
    [Header("Attributes")]

    // world traversal speed. Speed affects the total energy spent.
    [NonSerialized] public float Speed;

    // How much energy can an agent store. Affects the energy spent.
    [NonSerialized] public float Size;

    // how much an individual keeps distance from the others. 
    [NonSerialized] public float AvoidanceRadius;

    // how much an individual tends to follow the flock
    [NonSerialized] public float MoveCenterFactor;

    // how much an individual tends to follow the food
    [NonSerialized] public float FoodMotivationFactor;

    // what the animal considers the "center of mass" of the group
    [NonSerialized] public float CenterOfMassRadius;

    // how far an agent gazes for food.
    [NonSerialized] public int Awareness;
    #endregion

    public Vector3 Position = Vector3.zero;
    public Vector3 Velocity = Vector3.zero;


    void Start()
    {
        Position = transform.position;
        Velocity = Vector3.zero;
        GameObject.FindGameObjectWithTag("GlobalObjectTracker")
            .GetComponent<GlobalObjectTracker>()
            .BoidsController.GetComponent<BoidsMovement>()
            .Boids.Add(this);
    }

    void LateUpdate()
    {
        transform.position = Position;
    }
}
