using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QPath;

public class Unit : IQPathUnit
{

    public string Name = "Kula";
    public int HitPoints = 100;
    public int Strenght = 8;
    public int Movement = 2;
    public int MovementRemaining = 2;

    public Hex Hex { get; protected set; }

    public delegate void UnitMovedDelegate(Hex oldHex, Hex newHex);
    public event UnitMovedDelegate OnUnitMoved;

    Queue<Hex> hexPath;

    const bool MOVEMENT_RULES_LIKE_CIV6 = false;

    public void SetHex(Hex newHex)
    {
        Hex oldHex = Hex;
        if (Hex != null)
        {
            Hex.RemoveUnit(this);
        }
        Hex = newHex;

        Hex.AddUnit(this);

        if (OnUnitMoved != null)
        {
            OnUnitMoved(oldHex, newHex);
        }
    }

    public void SetHexPath( Hex[] hexPath)
    {
        this.hexPath = new Queue<Hex>(hexPath);
    }

    public void DoTurn()
    {
        Debug.Log("Do turn");

        if (hexPath == null || hexPath.Count == 0)
        {
            return;
        }

        
        Hex newHex = hexPath.Dequeue();
        
        SetHex( newHex);
    }

    public int MovementCostToEnterHex(Hex hex)
    {
        return hex.BaseMovementCost();
    }

    public float AggregateTurnsToEnterHex(Hex hex, float turnsToDate)
    {
        float baseTurnsToEnterHex = MovementCostToEnterHex(hex) / Movement;

        if (baseTurnsToEnterHex > 1)
        {
            baseTurnsToEnterHex = 1;
        }
        float turnsRemaining = MovementRemaining / Movement;

        float turnsToDateWhole = Mathf.Floor(turnsToDate);
        float turnsToDateFraction = turnsToDate - turnsToDateWhole;

        if (turnsToDateFraction < 0.01f || turnsToDateFraction > 0.99f)
        {
            if (turnsToDateFraction <0.01f)
            {
                turnsToDateFraction = 0; 
            }
            if (turnsToDateFraction > 0.99f)
            {
                turnsToDateWhole += 1;
                turnsToDateFraction = 0;
            }
        }

        float turnsUsedAfterThisMove = turnsToDateFraction + baseTurnsToEnterHex;

        if (turnsUsedAfterThisMove >1)
        {
            if (MOVEMENT_RULES_LIKE_CIV6)
            {
                if (turnsToDateFraction == 0)
                {

                }
                else
                {
                    turnsToDateWhole += 1;
                    turnsToDateFraction = 0;
                }

                turnsUsedAfterThisMove = baseTurnsToEnterHex;
            }
            else
            {
                //Zawsze możey wejść na hex, niezależnie od tego czy mamy  wystarczającą ilość pkt ruchu
                turnsUsedAfterThisMove = 1;
            }
        }

        return turnsToDateWhole + turnsUsedAfterThisMove;

    }

    public float CostToEnterHex(IQPathTile sourceTile, IQPathTile destinationTile)
    {
        return 1;
    }


}
