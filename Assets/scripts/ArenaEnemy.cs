using FastBuf;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class ArenaEnemy : MonoBehaviour
{
    private Action<GameObject> fight_ui_event_handler;

    public void Invalidate(ArenaLadderEnemy enemy, Action<GameObject> handler = null)
    {
        if (enemy != null)
        {
            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(enemy.head_entry);
            if (_config != null)
            {
                UILabel component = base.transform.FindChild("Name").GetComponent<UILabel>();
                UILabel label2 = base.transform.FindChild("FightPower").GetComponent<UILabel>();
                UILabel label3 = base.transform.FindChild("Rank").GetComponent<UILabel>();
                UILabel label4 = base.transform.FindChild("Level").GetComponent<UILabel>();
                UITexture texture = base.transform.FindChild("Icon").GetComponent<UITexture>();
                UISprite frame = base.transform.FindChild("QualityBorder").GetComponent<UISprite>();
                UISprite sprite2 = base.transform.FindChild("QualityBorder/QIcon").GetComponent<UISprite>();
                component.text = enemy.name;
                label2.text = enemy.team_power.ToString();
                label3.text = enemy.order.ToString();
                label4.text = enemy.level.ToString();
                texture.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                CommonFunc.SetPlayerHeadFrame(frame, sprite2, enemy.head_frame_entry);
                Transform transform = base.transform.FindChild("PkBtn");
                GUIDataHolder.setData(transform.gameObject, enemy);
                this.fight_ui_event_handler = handler;
                UIEventListener.Get(transform.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickFightButton);
                GUIDataHolder.setData(texture.gameObject, enemy);
                UIEventListener.Get(texture.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickEnemyIcon);
            }
        }
    }

    private void OnClickEnemyIcon(GameObject go)
    {
        <OnClickEnemyIcon>c__AnonStorey16B storeyb = new <OnClickEnemyIcon>c__AnonStorey16B {
            enemy = GUIDataHolder.getData(go) as ArenaLadderEnemy
        };
        if (storeyb.enemy != null)
        {
            GUIMgr.Instance.DoModelGUI<TargetInfoPanel>(new Action<GUIEntity>(storeyb.<>m__178), null);
        }
    }

    private void OnClickFightButton(GameObject go)
    {
        if (this.fight_ui_event_handler != null)
        {
            this.fight_ui_event_handler(go);
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickEnemyIcon>c__AnonStorey16B
    {
        internal ArenaLadderEnemy enemy;

        internal void <>m__178(GUIEntity e)
        {
            (e as TargetInfoPanel).UpdateData(this.enemy);
        }
    }
}

