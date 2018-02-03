using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using QPath;

public class Hex : IQPathTile {

    public Hex(HexMap hexmap, int q, int r)
    {
        this.HexMap = hexmap;
        this.Q = q;
        this.R = r;
        this.S = -(q + r);

    }

    public readonly int Q;  //Column
    public readonly int R;  //Row
    public readonly int S;

    public float Elevation;
    public float Moisture;

    public readonly HexMap HexMap;

    static readonly float WIDTH_MULTIPLIER = Mathf.Sqrt(3) / 2;

    float Radius = 1.05f;
    //bool allowWrapEastWest = true;
    //bool allowWrapNorthSouth = false;

    HashSet<Unit> units;



    public Vector3 Position()
    {
        return new Vector3(
            hexHorizontalSpacing() * (this.Q + this.R / 2f),
            hexVerticalSpacing() * this.R,
            0
         );
    }

    public float HexHeight()
    {
        return Radius * 2;
    }

    public float HexWidth()
    {
        return WIDTH_MULTIPLIER * HexHeight();
    }

    public float hexVerticalSpacing()
    {
        return HexHeight() * 0.75f;
    }

    public float hexHorizontalSpacing()
    {
        return HexWidth();
    }

    public Vector3 PositionFromCamera()
    {
        return HexMap.GetHexPosition(this);
    }

    public Vector3 PositionFromCamera(Vector3 cameraPosition, float NumRows, float NumColumns)
    {
        float mapHeight = NumRows * hexVerticalSpacing();
        float mapWidth = NumColumns * hexHorizontalSpacing();

        Vector3 position = Position();

        if (HexMap.allowWrapEastWest)
        {
            float howManyWidthsFormCamera = (position.x - cameraPosition.x) / mapWidth;

            if (howManyWidthsFormCamera > 0)
                howManyWidthsFormCamera += 0.5f;
            else
                howManyWidthsFormCamera -= 0.5f;

            int howManyWidthsToFix = (int)howManyWidthsFormCamera;

            position.x -= howManyWidthsToFix * mapWidth;
        }

        if (HexMap.allowWrapNorthSouth)
        {
            float howManyHeightsFormCamera = (position.y - cameraPosition.y) / mapHeight;

            if (howManyHeightsFormCamera > 0)
                howManyHeightsFormCamera += 0.5f;
            else
                howManyHeightsFormCamera -= 0.5f;

            int howManyHeightsToFix = (int)howManyHeightsFormCamera;

            position.y -= howManyHeightsToFix * mapHeight;
        }
        return position;

    }


    public static float Distance(Hex a, Hex b)
    {
        int dQ = Mathf.Abs(a.Q - b.Q);
        if (a.HexMap.allowWrapEastWest)
        {
            if (dQ > a.HexMap.NumColumns / 2)
                dQ = a.HexMap.NumColumns - dQ;
        }
        int dR = Mathf.Abs(a.R - b.R);
        if (a.HexMap.allowWrapNorthSouth)
        {
            if (dR > a.HexMap.NumRow / 2)
                dR = a.HexMap.NumRow - dR;
        }
        return
            Mathf.Max(
                dQ,
                dR,
                Mathf.Abs(a.S - b.S));
    }

    public void AddUnit(Unit unit)
    {
        if (units == null)
        {
            units = new HashSet<Unit>();
        }
        units.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        if (units != null)
        {
            units.Remove(unit);
        }
    }

    public Unit[] Units()
    {
        return units.ToArray();
    }

    public int BaseMovementCost()
    {
        return 1;
    }

    #region IQPathTile implementation
    public IQPathTile[] GetNeighbours()
    {
        throw new System.NotImplementedException();
    }

    public float AggregateCostToEnter(float costSoFar, IQPathTile sourceTile, IQPathUnit theUnit)
    {
        throw new System.NotImplementedException();
    }
    #endregion
}
