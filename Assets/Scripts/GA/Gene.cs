

public struct Gene
{
    public string name;
    public float value;
    public float maxValue;

    public Gene(string name = "unnamed", float value = 1f, float maxValue = 10f) 
    { 
        this.name = name;
        this.value = value;
        this.maxValue = maxValue;
    }

    public override string ToString()
    {
        return $"name: {name}, value: {value}, maxValue: {maxValue}";
    }
}

public enum GeneName
{
    Speed,                  // 0 world traversal speed. Speed affects the total energy spent.

    Size,                   // 1 How much energy can an agent store. Affects the energy spent.

    AvoidanceRadius,        // 2 how much an individual keeps distance from the others. 

    MoveCenterFactor,       // 3 how much an individual tends to follow the flock

    FoodMotivationFactor,   // 4 how much resources affect the velocity of an agent

    CenterOfMassRadius,     // 5 what the animal considers the "center of mass" of the group

    Awareness,              // 6 how far an agent gazes for food.
}
