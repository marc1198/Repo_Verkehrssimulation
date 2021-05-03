using System;
using System.Collections.Generic;

// Klasse zur Haltung von Zellenkoordianten (X,Y)
public class Point
{
    public int X { get; set; }
    public int Y { get; set; }

    public Point(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    public override bool Equals(object obj)
    {
        if (obj == null) { return false; }
        if (obj is Point) 
        {
            Point p = obj as Point;
            return this.X == p.X && this.Y == p.Y;
        }
        return false;
    }
}

// Defintion der Zellenzustände
public enum CellType
{
    Empty, Road, None
}

public class Grid
{
    private CellType[,] _grid;
    private int _width, _height;

    public int Width { get { return _width; } }
    public int Height { get { return _height; } }

    private List<Point> _roadList = new List<Point>();

    public Grid(int width, int height)
    {
        _width = width;
        _height = height;
        _grid = new CellType[width, height];
    }

    // Hinzufügen von Indizes zum spezifischen Zugriff auf einzelene Zellen im Grid 
    public CellType this[int i, int j]
    {
        get { return _grid[i, j]; }
        set
        {
            if (value == CellType.Road) { _roadList.Add(new Point(i, j)); }
            else { _roadList.Remove(new Point(i, j)); }
            _grid[i, j] = value;
        }
    }

    // Kostendefinition eines Zelltyps für A*-Algorithmus
    public float GetCostOfEnteringCell(Point cell)
    {
        return 1;
    }

    // Rückgabe der Koordinaten von Nachbarzellen 
    public List<Point> GetAllAdjacentCells(int x, int y)
    {
        List<Point> adjacentCells = new List<Point>();
        if (x > 0) { adjacentCells.Add(new Point(x - 1, y)); }
        if (x < _width - 1) { adjacentCells.Add(new Point(x + 1, y)); }
        if (y > 0) { adjacentCells.Add(new Point(x, y - 1)); }
        if (y < _height - 1) { adjacentCells.Add(new Point(x, y + 1)); }
        return adjacentCells;
    }

    public CellType[] GetAllAdjacentCellTypes(int x, int y)
    {
        CellType[] neighbours = { CellType.None, CellType.None, CellType.None, CellType.None };
        if (x > 0) { neighbours[0] = _grid[x - 1, y]; }
        if (x < _width - 1) { neighbours[2] = _grid[x + 1, y]; }
        if (y > 0) { neighbours[3] = _grid[x, y - 1]; }
        if (y < _height - 1) { neighbours[1] = _grid[x, y + 1]; }
        return neighbours;
    }

    #region Fahrzeugwegfindung
    // Methode für die Fahzeugwegfindung 
    public static bool IsCellWakable(CellType cellType, bool aiAgent = false)
    {
        if (aiAgent) { return cellType == CellType.Road; }
        return cellType == CellType.Empty || cellType == CellType.Road;
    }

    public List<Point> GetAdjacentCells(Point cell, bool isAgent)
    {
        return GetWakableAdjacentCells((int)cell.X, (int)cell.Y, isAgent);
    }

    public List<Point> GetWakableAdjacentCells(int x, int y, bool isAgent)
    {
        List<Point> adjacentCells = GetAllAdjacentCells(x, y);
        for (int i = adjacentCells.Count - 1; i >= 0; i--)
            if(IsCellWakable(_grid[adjacentCells[i].X, adjacentCells[i].Y], isAgent)==false) 
                adjacentCells.RemoveAt(i);
        return adjacentCells;
    }

    public List<Point> GetAdjacentCellsOfType(int x, int y, CellType type)
    {
        List<Point> adjacentCells = GetAllAdjacentCells(x, y);
        for (int i = adjacentCells.Count - 1; i >= 0; i--)
            if (_grid[adjacentCells[i].X, adjacentCells[i].Y] != type)
                adjacentCells.RemoveAt(i);
        return adjacentCells;
    }
    #endregion

}

