using QPath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap : MonoBehaviour, IQPathWorld
{

    // Use this for initialization
    void Start()
    {
        GenerateMap();
    }

     void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (units != null)
            {
                foreach (Unit u in units)
                {
                    u.DoTurn();
                }
            }
        }
    }

    public GameObject hexPrefab;

    public Mesh MeshWater;
    public Mesh MeshFlat;
    public Mesh MeshHill;
    public Mesh MeshMountain;

    public GameObject ForestPrefab;

    public Material MatOcean;
    public Material MatPlain;
    public Material MatGrassLand;
    public Material MatMountains;
    public Material MatDesert;

    public GameObject UnitSphere;

    [System.NonSerialized] public float HeightMountain = 1f;
    [System.NonSerialized] public float HeightHill = 0.6f;
    [System.NonSerialized] public float HeightFlat = 0.0f;

    [System.NonSerialized] public float MoistureJungle = 1f;
    [System.NonSerialized] public float MoistureForest = 0.5f;
    [System.NonSerialized] public float MoistureGrasslands = 0f;
    [System.NonSerialized] public float MoisturePlains = -0.75f;


    [System.NonSerialized] public int NumRow = 50;
    [System.NonSerialized] public int NumColumns = 80;


    [System.NonSerialized] public bool allowWrapEastWest = true;
    [System.NonSerialized] public bool allowWrapNorthSouth = true;
    [System.NonSerialized] public bool allowChangeCameraAngle = false;

    private Hex[,] hexes;
    private Dictionary<Hex, GameObject> hexToGameObjectMap;

    private HashSet<Unit> units;
    private Dictionary<Unit, GameObject> unitToGameObjectMap;

    public Hex GetHexAt(int x, int y)
    {
        if (hexes == null)
        {
            Debug.LogError("Tablica hexów jeszcze nie powstała");
            return null;
        }

        if (allowWrapEastWest)
        {
            x = x % NumColumns;
            if (x < 0)
            {
                x += NumColumns;
            }
        }
        if (allowWrapNorthSouth)
        {
            y = y % NumRow;
            if (y < 0)
            {
                y += NumRow;
            }
        }

        try
        {
            return hexes[x, y];
        }
        catch
        {
            Debug.LogError("GetHexAt: " + x + ", " +y);
            return null;
        }
    }


    public Vector3 GetHexPosition(int q, int r)
    {
        Hex hex = GetHexAt(q, r);

        return GetHexPosition(hex);
    }

    public Vector3 GetHexPosition(Hex hex)
    {
        
        return hex.PositionFromCamera(Camera.main.transform.position, NumRow, NumColumns);
    }


    // Update is called once per frame
    virtual public void GenerateMap()
    {
        hexes = new Hex[NumColumns, NumRow];
        hexToGameObjectMap = new Dictionary<Hex, GameObject>();

        for (int column = 0; column < (NumColumns); column++)
        {
            for (int row = 0; row < (NumRow); row++)
            {

                Hex h = new Hex(this, column, row);
                h.Elevation = -1;

                hexes[column, row] = h;

                Vector3 pos = h.PositionFromCamera(
                    Camera.main.transform.position,
                    NumRow,
                    NumColumns
                    );

                GameObject hex_go = (GameObject)Instantiate(
                    hexPrefab,
                    h.Position(),
                    Quaternion.identity,
                    this.transform
                    );

                hexToGameObjectMap[h] = hex_go;

                hex_go.name = string.Format("{0},{1}", column, row);
                hex_go.GetComponent<HexComponent>().Hex = h;
                hex_go.GetComponent<HexComponent>().HexMap = this;

                hex_go.GetComponentInChildren<TextMesh>().text = string.Format("{0},{1}", column, row);

                hex_go.name = "Hex_" + column + "_" + row;

                hex_go.transform.SetParent(this.transform, true);

                
            }
        }
        UpdateHexVisuals();
    }

    public void UpdateHexVisuals()
    {
        for (int column = 0; column < (NumColumns); column++)
        {
            for (int row = 0; row < (NumRow); row++)
            {
                Hex h = hexes[column, row];
                GameObject hexGO = hexToGameObjectMap[h];

                MeshRenderer mr = hexGO.GetComponentInChildren<MeshRenderer>();
                MeshFilter mf = hexGO.GetComponentInChildren<MeshFilter>();
                               
                if (h.Elevation >= HeightFlat && h.Elevation < HeightMountain)
                {
                    if (h.Moisture >= MoistureJungle)
                    {
                        mr.material = MatGrassLand;
                        // Spawn Jungle
                        //Vector3 p = hexGO.transform.position;
                        //if (h.Elevation >= HeightHill)
                        //{
                        //    p.y += 0.25f;
                        //}
                        //GameObject.Instantiate(JunglePrefab, hexGO.transform.position, Quaternion.identity, hexGO.transform);
                    }
                    else if (h.Moisture >= MoistureForest)
                    {
                        mr.material = MatGrassLand;
                        // Spawn Forest
                        //Vector3 p = hexGO.transform.position;
                        //if (h.Elevation >= HeightHill)
                        //{
                        //    p.y += 0.25f;
                        //}
                        // GameObject.Instantiate(ForestPrefab, hexGO.transform.position, Quaternion.identity, hexGO.transform);
                    }
                    else if (h.Moisture >= MoistureGrasslands)
                    {
                        mr.material = MatGrassLand;
                    }
                    else if (h.Moisture >= MoisturePlains)
                    {
                        mr.material = MatPlain;
                    }
                    else
                    {
                        mr.material = MatDesert;
                    }
                }

                if (h.Elevation >= HeightMountain)
                {
                    mr.material = MatMountains;
                    mf.mesh = MeshMountain;
                }
                else if (h.Elevation >= HeightHill)
                {
                    mf.mesh = MeshHill;
                }
                else if (h.Elevation >= HeightFlat)
                {
                    mf.mesh = MeshFlat;
                }
                else
                {
                    mr.material = MatOcean;
                    mf.mesh = MeshWater;
                }
            }
        }
    }

    public Hex[] GetHexesWithinRangeOf(Hex centerHex, int range)
    {
        List<Hex> results = new List<Hex>();

        for (int dx = - range; dx < range-1; dx++)
        {
            for (int dy = Mathf.Max(- range+1, -dx-range); dy < Mathf.Min(range, -dx+range-1); dy++)
            {
                results.Add(GetHexAt(centerHex.Q + dx, centerHex.R + dy));
            }
        }
        return results.ToArray();
    }

    public void SpawnUnitAt( Unit unit, GameObject prefab, int q, int r)
    {
        if (units == null)
	    {
            units = new HashSet<Unit>();
            unitToGameObjectMap = new Dictionary<Unit, GameObject>();
        }

        Hex myHex = GetHexAt(q, r);
        GameObject myHexGO= hexToGameObjectMap[myHex];
        unit.SetHex(myHex);

        GameObject unitGO = (GameObject)Instantiate(prefab, myHexGO.transform.position, Quaternion.identity, myHexGO.transform);
        unit.OnUnitMoved += unitGO.GetComponent<UnitView>().OnUnitMoved;


        units.Add(unit);
        unitToGameObjectMap.Add(unit, unitGO);
    }
}
