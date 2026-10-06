# Adaptability of Virtual Agents using Genetic Algorithms in Procedurally Generated Environments

A Unity3D simulation exploring how **genetic algorithms** can shape the foraging and movement behaviour of agent-based herds in procedurally generated environments.

---

## Overview

This project investigates whether different **parent selection methods** in a genetic algorithm produce meaningfully different flocking and foraging outcomes for virtual animal agents. It combines two well-established techniques:

- **Boids algorithm** (Reynolds, 1987) — for emergent herd/flocking movement
- **Genetic algorithms** (Holland, 1992) — for evolving agent behaviour across generations

The agents inhabit a terrain generated at runtime using **Perlin Noise**, where each cell holds a resource value between 0 and 1 (representing food, water, or similar). Agents have limited awareness of their surroundings and must forage while maintaining herd cohesion.

The simulation compares two parent selection strategies:

| Method | Description |
|---|---|
| **Tournament Selection** | Agents compete against each other in small groups; winners are selected as parents |
| **Rank Selection** | All agents are sorted by fitness; top-ranked individuals are selected as parents |

---

## Features

- Procedurally generated terrain using Perlin Noise (configurable scale, offset, and size)
- Agent-based movement using a modified Boids algorithm with per-agent gene values
- Genetic algorithm with crossover and mutation across generations
- Two selectable parent selection methods: Tournament and Ranking
- Runtime heatmap visualisation of agent traversal paths
- Fitness tracking exported to text files for external graphing and analysis
- Configurable simulation parameters exposed in the Unity Editor

---

## Agent Genes (DNA)

Each agent's behaviour is defined by the following genes, which are inherited and mutated across generations:

| Gene | Effect |
|---|---|
| `Speed` | World traversal speed; higher speed drains energy faster |
| `Size` | Maximum energy an agent can store |
| `AvoidanceRadius` | How far an agent stays from its neighbours (repulsion force) |
| `MoveCenterFactor` | Strength of attraction towards the group's centre of mass |
| `FoodMotivationFactor` | How strongly resource-rich cells influence movement |
| `CenterOfMassRadius` | The radius within which other agents are considered part of the group |
| `Awareness` | How far an agent can detect resources around itself |

---

## How to Run

1. Open the project in **Unity version 2022.3.62f3**
2. Open the `Main` scene
3. In the Inspector, configure the `GAController` objects:
   - Number of agents to spawn initially
   - Number of best agents to use as parents (`BestAgentsListSize`)
   - Seconds between generations (`SecondsTilNextGeneration`)
   - Spread value for initial spawn distribution
   - Selection method (Tournament or Ranking)
4. Press **Play** to run the simulation
5. Fitness values are written to a text file at runtime for post-hoc analysis

---

## Key Findings

- Both Tournament and Rank selection produce valid flocking and foraging behaviour compatible with Boids-based herd movement
- At **high speeds**, Tournament-selected agents tend to follow more linear paths, while Rank-selected agents disperse more evenly across the terrain
- At **high food motivation**, agent behaviour is more strongly shaped by resource gradients, with both methods producing more directed traversal
- **Regular-sized groups** (tighter flocking parameters) generally achieved higher average fitness than large, spread-out groups
- Neither method decisively outperformed the other; pseudorandom variation in gene mutation likely contributes to observed differences
- Rank selection has better worst-case complexity — O(n log n) vs O(n²) for Tournament — which becomes relevant as population size grows

---

## Computational Considerations

Both the Boids algorithm (O(n²)) and Tournament selection (O(n²) worst case) become expensive as agent count increases. This project is a **proof of concept** and is not optimised for large-scale or real-time game deployment without further engineering.

---

## Selected references

- Reynolds, C.W. (1987). Flocks, herds and schools: A distributed behavioral model. *SIGGRAPH '87*
- Holland, J.H. (1992). Genetic algorithms. *Scientific American*, 267(1), pp.66–73
- Grünbaum, D. (1998). Schooling as a strategy for taxis in a noisy environment. *Evolutionary Ecology*, 12, pp.503–522
- Katoch, S. et al. (2021). A review on genetic algorithm: past, present, and future. *Multimedia Tools and Applications*, 80, pp.8091–8126

---

## Future Work

- Introduce impassable terrain, predators, and weather conditions
- Extend the gene pool with additional behavioural parameters
- Optimise selection and Boids calculations for larger agent populations
- Package as a reusable plugin for integration into serious or educational games
