using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BoidsMovement : MonoBehaviour
{
    [SerializeField] private float VelocityFactor = 1f; // general speed up/down te velocity
    [SerializeField] private float MaxSpeed = 10f;
    [SerializeField] private float EatRate = 0.1f;

    // timer to check resources on the map since calculating resource
    // values every frame is bad for the performance
    [SerializeField] private float ResourceLookupTimer = 1f; 
    private float _Timer = 0f;

    [SerializeField] public List<Boid> Boids;

    private GlobalObjectTracker objectTracker;
    private MapGeneration _MapGeneration;

    private Vector3 v4 = Vector3.zero;

    void Start()
    {
        _MapGeneration = GameObject.FindGameObjectWithTag("GlobalObjectTracker")
            .GetComponent<GlobalObjectTracker>()
            .Terrain.GetComponent<MapGeneration>();
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //    MoveBoids();
    //}

    // Adapted from the pseudocode:
    // https://vergenet.net/~conrad/boids/pseudocode.html
    public void MoveBoids()
    {
        Vector3 v1, v2, v3;
        foreach (Boid b in Boids)
        {
            v1 = FlyTowardsNeighbours(b);
            v2 = AvoidOthers(b);
            v3 = MatchVelocity(b);
            if (LookupTimer())
            {
                _Timer = ResourceLookupTimer; // reset the timer
                v4 = GoTowardsFood(b);
                BoidsEat(EatRate);
            }
            b.Velocity += v1 + v2 + v3 + v4;
            b.Velocity *= b.Speed;
            b.Velocity = Vector3.ClampMagnitude(b.Velocity, MaxSpeed) * VelocityFactor;
            b.Position += b.Velocity * Time.deltaTime;
            b.Position.y = 1f;
        }
    }

    public void BoidsEat(float rate)
    {
        foreach (Boid b in Boids)
        {
            _MapGeneration.DepleteResources(b.Position.x, b.Position.z, rate);
        }
    }

    Vector3 GoTowardsFood(Boid bb)
    {
        Vector3 v1 = Vector3.zero;

        int xPos = Mathf.RoundToInt(bb.Position.x);
        int zPos = Mathf.RoundToInt(bb.Position.z);

        int resX, resZ;
        float val;

        (resX, resZ, val) = _MapGeneration.GetMapResourceLocation(xPos, zPos, bb.Awareness);

        Vector3 foodPos = new Vector3(resX, 1, resZ);

        v1 = (foodPos - bb.Position) * val * bb.FoodMotivationFactor;


        return v1;
    }

    //Rule 1: Boids try to fly towards the centre of mass of neighbouring boids.
    Vector3 FlyTowardsNeighbours(Boid bb)
    {
        return (CenterOfMass(bb) - bb.Position) * bb.MoveCenterFactor;
    }

    // Rule 2: Boids try to keep a small distance away from other objects (including other boids). 
    Vector3 AvoidOthers(Boid bb)
    {
        Vector3 v1 = Vector3.zero;
        foreach (Boid b in Boids)
        {
            if (b == bb) continue;
            if (DistanceBetween(b, bb) < bb.AvoidanceRadius)
            {
                v1 -= (b.Position - bb.Position);
            }
        }
        return v1;
    }

    // Rule 3: Boids try to match velocity with near boids. 
    Vector3 MatchVelocity(Boid bb)
    {
        if (Boids.Count <= 0) return Vector3.zero;
        else if (Boids.Count == 1) return bb.Velocity;

        Vector3 v1 = Vector3.zero;

        foreach (Boid b in Boids)
        {
            if (b == bb) continue;
            v1 += b.Velocity;
        }

        v1 = v1 / (Boids.Count - 1);

        return (v1 - bb.Velocity);
    }

    // returns the center of mass given a Boid bb
    // the result excludes the given boid's position.
    Vector3 CenterOfMass(Boid bb)
    {
        if (Boids.Count <= 0) return Vector3.zero;
        else if (Boids.Count == 1) return bb.Position;

        Vector3 cm = Vector3.zero;

        foreach (Boid b in Boids)
        {
            if (b.Position == bb.Position) continue;
            if (DistanceBetween(b, bb) < bb.CenterOfMassRadius)
            {
                cm += b.Position;
            }
        }

        return cm / (Boids.Count - 1);
    }

    float DistanceBetween(Vector3 v1, Vector3 v2)
    {
        return (v1 - v2).magnitude;
    }

    float DistanceBetween(Boid b1, Boid b2)
    {
        return (b1.Position - b2.Position).magnitude;
    }

    private bool LookupTimer()
    {
        _Timer -= Time.deltaTime;
        return _Timer < 0;
    }

}
