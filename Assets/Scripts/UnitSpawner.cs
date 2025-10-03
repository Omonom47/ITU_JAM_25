using UnityEngine;
using Grid = Model.Grid;

public class UnitSpawner : MonoBehaviour
{
    private Grid _grid;
    
    public void SetGrid(Grid grid)
    {
        _grid = grid;
    }
}