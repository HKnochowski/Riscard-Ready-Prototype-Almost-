using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap_Continent : HexMap {



    override public void GenerateMap()
    {
        base.GenerateMap();

        int numContinents = 3;
        int continentSpacing = NumColumns/numContinents;

        Random.InitState(0);
        for (int c = 0; c < numContinents; c++)
        {
            int numSplats = Random.Range(4, 8);
            for (int i = 0; i < numSplats; i++)
            {
                int range = Random.Range(5, 8);
                int y = Random.Range(range, NumRow - range);
                int x = Random.Range(0, 10) - y / 2 + (c * continentSpacing);

                ElevateArea(x, y, range);
            }
            
        }
        //ElevateArea(6, 2, 2);

        float noiseResolution = 0.1f;
        Vector2 NoiseOffset = new Vector2(Random.Range(0, 1), Random.Range(0f, 1f));
        float noiseScale = 2f;      //Większe wartosci tworzą więcej wysp
        for (int column = 0; column < (NumColumns); column++)
        {
            for (int row = 0; row < (NumRow); row++)
            {
                Hex h = GetHexAt(column, row);
               float n = 
                    Mathf.PerlinNoise(((float)column / Mathf.Max(NumColumns, NumRow) / noiseResolution) + NoiseOffset.x, 
                    ((float)row /Mathf.Max(NumColumns, NumRow)/ noiseResolution)+NoiseOffset.y) -0.5f;
                h.Elevation += n * noiseScale;
            }
        }

        noiseResolution = 0.1f;
        NoiseOffset = new Vector2(Random.Range(0, 1), Random.Range(0f, 1f));
        noiseScale = 2f;      //Większe wartosci tworzą więcej wysp
        for (int column = 0; column < (NumColumns); column++)
        {
            for (int row = 0; row < (NumRow); row++)
            {
                Hex h = GetHexAt(column, row);
                float n =
                     Mathf.PerlinNoise(((float)column / Mathf.Max(NumColumns, NumRow) / noiseResolution) + NoiseOffset.x,
                     ((float)row / Mathf.Max(NumColumns, NumRow) / noiseResolution) + NoiseOffset.y) - 0.5f;
                h.Moisture = n * noiseScale;
            }
        }

        UpdateHexVisuals();

        Unit unit = new Unit();
        SpawnUnitAt(unit, UnitSphere, 74, 9);

    }

    void ElevateArea(int q, int r, int range, float centerHeight = 0.5f)
    {
        Hex centerHex = GetHexAt(q, r);
        
        Hex[] areaHexes = GetHexesWithinRangeOf(centerHex, range);

       foreach(Hex h in areaHexes)
       {
            if (h.Elevation < 0)
                h.Elevation = 0;

            h.Elevation = centerHeight * Mathf.Lerp(1f, 0.25f, Mathf.Pow(Hex.Distance(centerHex,h)/range,2f ));
       }
    }
}


