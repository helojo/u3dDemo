using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class BattleTestMgrImplGrid : BattleTestMgrImplBase
{
    public void DoBattleTest()
    {
    }

    public void DoGridTest()
    {
        if (BattleSceneStarter.G_isTestEnable)
        {
            BattleGridGame curGame = BattleState.GetInstance().CurGame as BattleGridGame;
            if (curGame != null)
            {
                <DoGridTest>c__AnonStorey14A storeya = new <DoGridTest>c__AnonStorey14A();
                BattleGridGameMapControl component = curGame.battleGameData.battleComObject.GetComponent<BattleGridGameMapControl>();
                List<int> list = StrParser.ParseDecIntList(BattleSceneStarter.Instance.TestGridPoint, -1);
                storeya.modelList = StrParser.ParseStringList(BattleSceneStarter.Instance.TestGridPointModel, "|");
                if (list.Count > 0)
                {
                    <DoGridTest>c__AnonStorey149 storey = new <DoGridTest>c__AnonStorey149 {
                        <>f__ref$330 = storeya,
                        index = 0
                    };
                    list.ForEach(new Action<int>(storey.<>m__10B));
                    VectorInt2 num = BattleGridGameMapControl.Index2Axis(list[0]);
                    component.SetPlayerToGrid(num.x, num.y);
                }
                else
                {
                    TestInitGrid(0, 0, null);
                    TestInitGrid(0, 1, null);
                    TestInitGrid(0, 2, null);
                    TestInitGrid(0, 3, null);
                    TestInitGrid(1, 3, null);
                    TestInitGrid(0, 4, null);
                    TestInitGrid(0, 5, null);
                    TestInitGrid(0, 6, null);
                    component.SetPlayerToGrid(0, 0);
                }
            }
        }
    }

    public override void Init()
    {
        BattleData battleGameData = base.battleGameData;
        battleGameData.OnMsgEnter = (System.Action) Delegate.Combine(battleGameData.OnMsgEnter, new System.Action(this.DoGridTest));
    }

    public override void InitTest()
    {
    }

    public static void RefreshGridModel()
    {
        BattleGridGame curGame = BattleState.GetInstance().CurGame as BattleGridGame;
        if (curGame != null)
        {
            <RefreshGridModel>c__AnonStorey148 storey = new <RefreshGridModel>c__AnonStorey148 {
                control = curGame.battleGameData.battleComObject.GetComponent<BattleGridGameMapControl>()
            };
            List<int> list = StrParser.ParseDecIntList(BattleSceneStarter.Instance.TestGridPoint, -1);
            storey.modelList = StrParser.ParseStringList(BattleSceneStarter.Instance.TestGridPointModel, "|");
            if (list.Count > 0)
            {
                <RefreshGridModel>c__AnonStorey147 storey2 = new <RefreshGridModel>c__AnonStorey147 {
                    <>f__ref$328 = storey,
                    index = 0
                };
                list.ForEach(new Action<int>(storey2.<>m__10A));
            }
        }
    }

    private static void TestInitGrid(int x, int y, string modelName)
    {
        BattleGridGame curGame = BattleState.GetInstance().CurGame as BattleGridGame;
        if (curGame != null)
        {
            BattleGridGameMapControl component = curGame.battleGameData.battleComObject.GetComponent<BattleGridGameMapControl>();
            component.SetGrid(x, y);
            component.GetGrid(x, y).Init(null, modelName, null);
        }
    }

    [CompilerGenerated]
    private sealed class <DoGridTest>c__AnonStorey149
    {
        internal BattleTestMgrImplGrid.<DoGridTest>c__AnonStorey14A <>f__ref$330;
        internal int index;

        internal void <>m__10B(int obj)
        {
            VectorInt2 num = BattleGridGameMapControl.Index2Axis(obj);
            string modelName = null;
            if (this.index < this.<>f__ref$330.modelList.Count)
            {
                modelName = this.<>f__ref$330.modelList[this.index];
            }
            BattleTestMgrImplGrid.TestInitGrid(num.x, num.y, modelName);
            this.index++;
        }
    }

    [CompilerGenerated]
    private sealed class <DoGridTest>c__AnonStorey14A
    {
        internal List<string> modelList;
    }

    [CompilerGenerated]
    private sealed class <RefreshGridModel>c__AnonStorey147
    {
        internal BattleTestMgrImplGrid.<RefreshGridModel>c__AnonStorey148 <>f__ref$328;
        internal int index;

        internal void <>m__10A(int obj)
        {
            VectorInt2 num = BattleGridGameMapControl.Index2Axis(obj);
            string modelInfoText = null;
            if (this.index < this.<>f__ref$328.modelList.Count)
            {
                modelInfoText = this.<>f__ref$328.modelList[this.index];
            }
            BattleGrid grid = this.<>f__ref$328.control.GetGrid(num.x, num.y);
            if (grid != null)
            {
                grid.ChangeModel(null, modelInfoText);
            }
            this.index++;
        }
    }

    [CompilerGenerated]
    private sealed class <RefreshGridModel>c__AnonStorey148
    {
        internal BattleGridGameMapControl control;
        internal List<string> modelList;
    }
}

