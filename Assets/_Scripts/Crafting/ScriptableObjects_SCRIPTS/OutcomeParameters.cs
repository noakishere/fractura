using UnityEngine;
public abstract class OutcomeParameters { }

public class WeaponOutcomeParameters : OutcomeParameters 
{
    public string nameTest;
}

public class BuildOutcomeParameters : OutcomeParameters
{
    public Vector2 buildPosition;
    public GameObject buildingPrefab;
}