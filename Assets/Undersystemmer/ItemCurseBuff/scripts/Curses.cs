using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
// Enum til at definere curse typer
public enum CurseType
{
    WheelchairBound,
    Blindness,
    EnergyDepletion
}

// Base ScriptableObject for curses
[CreateAssetMenu(fileName = "NewCurse", menuName = "Curses/New Curse", order = 1)]
public class Curse : ScriptableObject
{
    [SerializeField] private string curseName;
    [SerializeField] private CurseType curseType;
    [SerializeField] private float duration; // i sekunder
    [SerializeField] private float intensity; // 0-1 for procentbaserede effekter

    public string CurseName => curseName;
    public CurseType CurseType => curseType;
    public float Duration => duration;
    public float Intensity => intensity;

    // Virtuel metode som kan overrides af specifikke curses
    public virtual void ApplyCurse(GameObject target)
    {
        switch (curseType)
        {
            case CurseType.WheelchairBound:
                ApplyWheelchairBound(target);
                break;
            case CurseType.Blindness:
                ApplyBlindness(target);
                break;
            case CurseType.EnergyDepletion:
                ApplyEnergyDepletion(target);
                break;
        }
    }


    protected virtual void ApplyWheelchairBound(GameObject target)
    {
        // Eksempel: Reducer bevægelseshastighed
        PlayerMovement movement = target.GetComponent<PlayerMovement>();
        if (movement != null)
        {
            movement.Speed *= 0.9f; // Reducer hastighed til 80%
        }
    }

    protected virtual void ApplyBlindness(GameObject target)
    {
        // Eksempel: Tilføj en vignette eller mørk effekt
        // Intensitet bestemmer hvor blind spilleren er (0-1)
    }

    protected virtual void ApplyEnergyDepletion(GameObject target)
    {
        // Eksempel: Reducer energi regeneration
        PlayerEnergy energy = target.GetComponent<PlayerEnergy>();
        if (energy != null)
        {
            energy.DepletionRate += intensity;
        }
    }
}

// Eksempel på en curse manager (ikke MonoBehaviour)
public class CurseManager
{
    private Curse activeCurse;
    private float curseTimer;
    private GameObject targetPlayer;

    public void ApplyRandomCurse(GameObject target)
    {
        targetPlayer = target;

        // Eksempel på at vælge en tilfældig curse fra en liste
        Curse[] availableCurses = Resources.LoadAll<Curse>("Curses");
        if (availableCurses.Length > 0)
        {
            activeCurse = availableCurses[Random.Range(0, availableCurses.Length)];
            curseTimer = activeCurse.Duration;
            activeCurse.ApplyCurse(target);
        }
    }


    public bool HasActiveCurse()
    {
        return activeCurse != null;
    }

    public string GetActiveCurseName()
    {
        return activeCurse != null ? activeCurse.CurseName : "Ingen aktiv curse";
    }
}
*/