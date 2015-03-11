using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleGridPath
{
    [CompilerGenerated]
    private static Comparison<GridPoint> <>f__am$cache3;
    private static int FighterGridSTEP = 100;
    private static int numberOfGrid = BattleGridGameMapControl.numberOfGrid;
    private static int STEP = 2;

    private static int CalcG(GridPoint point)
    {
        int num = !point.IsPassGrid ? STEP : FighterGridSTEP;
        int num2 = (point.parent == null) ? 0 : point.parent.G;
        return (num + num2);
    }

    private static int CalcH(GridPoint end, GridPoint point)
    {
        GridAlterPoint point2 = new GridAlterPoint();
        GridAlterPoint point3 = new GridAlterPoint();
        point2.FromPoint(point);
        point3.FromPoint(end);
        float num = point2.X - point3.X;
        float num2 = point2.Y - point3.Y;
        return (Mathf.RoundToInt(Mathf.Sqrt((num * num) + (num2 * num2))) * STEP);
    }

    private static List<int> GetIndexPathByPoint(GridPoint point)
    {
        List<int> list = new List<int>();
        while (point != null)
        {
            list.Add(BattleGridGameMapControl.Axis2Index(point.X, point.Y));
            point = point.parent;
        }
        list.Reverse();
        return list;
    }

    public static List<int> GetPath(List<BattleGrid> allGrids, bool notOnlyFindShowGrid, int srcX, int srcY, int destX, int destY)
    {
        <GetPath>c__AnonStoreyF5 yf = new <GetPath>c__AnonStoreyF5 {
            notOnlyFindShowGrid = notOnlyFindShowGrid,
            allPoint = new List<GridPoint>(new GridPoint[numberOfGrid * numberOfGrid])
        };
        allGrids.ForEach(new Action<BattleGrid>(yf.<>m__7E));
        yf.openList = new List<GridPoint>();
        List<GridPoint> list = new List<GridPoint>();
        GridPoint item = GetPoint(srcX, srcY, yf.allPoint);
        yf.endPoint = GetPoint(destX, destY, yf.allPoint);
        if ((item != null) && (yf.endPoint != null))
        {
            yf.openList.Add(item);
            while (yf.openList.Count != 0)
            {
                <GetPath>c__AnonStoreyF6 yf2 = new <GetPath>c__AnonStoreyF6 {
                    <>f__ref$245 = yf
                };
                if (<>f__am$cache3 == null)
                {
                    <>f__am$cache3 = (x, y) => x.F - y.F;
                }
                yf.openList.Sort(<>f__am$cache3);
                yf2.tempStart = yf.openList[0];
                list.Add(yf2.tempStart);
                yf.openList.RemoveAt(0);
                if (yf2.tempStart == null)
                {
                }
                yf2.tempStart.isClose = true;
                GetSurroundPoints(yf2.tempStart, yf.allPoint).ForEach(new Action<GridPoint>(yf2.<>m__80));
                if (yf.openList.Contains(yf.endPoint))
                {
                    return GetIndexPathByPoint(yf.endPoint);
                }
            }
        }
        return null;
    }

    private static GridPoint GetPoint(int x, int y, List<GridPoint> allPoint)
    {
        if (((x < numberOfGrid) && (y < numberOfGrid)) && ((x >= 0) && (y >= 0)))
        {
            return allPoint[x + (numberOfGrid * y)];
        }
        return null;
    }

    private static List<GridPoint> GetSurroundPoints(GridPoint point, List<GridPoint> allPoint)
    {
        List<GridPoint> list = new List<GridPoint>();
        foreach (VectorInt2 num in point.battleGrid.GetSurroundPoints())
        {
            GridPoint item = GetPoint(point.X + num.x, point.Y + num.y, allPoint);
            if ((item != null) && !item.isClose)
            {
                list.Add(item);
            }
        }
        return list;
    }

    private static void NotFoundPoint(GridPoint start, GridPoint end, GridPoint point, List<GridPoint> openList)
    {
        point.parent = start;
        point.G = CalcG(point);
        point.H = CalcH(end, point);
        point.CalcF();
        openList.Add(point);
    }

    [CompilerGenerated]
    private sealed class <GetPath>c__AnonStoreyF5
    {
        internal List<BattleGridPath.GridPoint> allPoint;
        internal BattleGridPath.GridPoint endPoint;
        internal bool notOnlyFindShowGrid;
        internal List<BattleGridPath.GridPoint> openList;

        internal void <>m__7E(BattleGrid obj)
        {
            if ((obj != null) && (this.notOnlyFindShowGrid || obj.isShow))
            {
                BattleGridPath.GridPoint point = new BattleGridPath.GridPoint {
                    X = obj.X,
                    Y = obj.Y,
                    IsPassGrid = obj.IsMovoPassGrid(),
                    battleGrid = obj
                };
                this.allPoint[obj.GetIndex()] = point;
            }
        }
    }

    [CompilerGenerated]
    private sealed class <GetPath>c__AnonStoreyF6
    {
        internal BattleGridPath.<GetPath>c__AnonStoreyF5 <>f__ref$245;
        internal BattleGridPath.GridPoint tempStart;

        internal void <>m__80(BattleGridPath.GridPoint obj)
        {
            if (!this.<>f__ref$245.openList.Contains(obj))
            {
                BattleGridPath.NotFoundPoint(this.tempStart, this.<>f__ref$245.endPoint, obj, this.<>f__ref$245.openList);
            }
        }
    }

    private class GridAlterPoint
    {
        public float X;
        public float Y;

        public void FromPoint(BattleGridPath.GridPoint point)
        {
            float num = 1f / Mathf.Sqrt(3f);
            this.X = (num * 1.5f) * point.X;
            this.Y = point.Y;
            if ((point.X % 2) != 0)
            {
                this.Y += 0.5f;
            }
        }
    }

    private class GridPoint
    {
        public BattleGrid battleGrid;
        public int F;
        public int G;
        public int H;
        public bool isClose;
        public bool IsPassGrid;
        public BattleGridPath.GridPoint parent;
        public int X;
        public int Y;

        public void CalcF()
        {
            this.F = this.G + this.H;
        }

        public void Clear()
        {
            this.F = this.G = this.H = 0;
            this.parent = null;
        }
    }
}

