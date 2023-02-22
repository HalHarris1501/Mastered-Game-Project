using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Testing : MonoBehaviour
{
    private CustomGrid<HeatmapGridObject> grid;

    // Start is called before the first frame update
    private void Start()
    {
        grid = new CustomGrid<HeatmapGridObject>(150, 100, 1f, new Vector3(-10, -10), (CustomGrid<HeatmapGridObject> g, int x, int y) => new HeatmapGridObject(g, x, y));
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 position = UtilsClass.GetMouseWorldPosition();
            HeatmapGridObject heatmapGridObject = grid.GetGridObject(position);
            if(heatmapGridObject != null)
            {
                heatmapGridObject.AddValue(5);
            }
        }

        if(Input.GetMouseButtonDown(1))
        {
            Debug.Log(grid.GetGridObject(UtilsClass.GetMouseWorldPosition())); 
        }
    }
}

public class HeatmapGridObject
{
    private const int MIN = 0;
    private const int MAX = 100;

    private CustomGrid<HeatmapGridObject> grid;
    private int x;
    private int y;
    private int value;

    public HeatmapGridObject(CustomGrid<HeatmapGridObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public void AddValue(int addValue)
    {
        value += addValue;
        value = Mathf.Clamp(value, MIN, MAX);
        grid.TriggerGridObjectChanged(x, y);
    }

    public float GetNormalizedValue()
    {
        return (float)value / MAX;
    }

    public override string ToString()
    {
        return value.ToString();
    }
}
