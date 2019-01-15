using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPSService.BeatData
{
    public static class CallBoxes
    {
        public static List<esriCB> callBoxList = new List<esriCB>();

        public static void LoadCallBoxes()
        {
            SQL.SQLCode mySQL = new SQL.SQLCode();
            callBoxList.Clear();
            mySQL.loadCallBoxData();
            callBoxList.Clear();
        }

        #region " updater "

        public static void addCallBoxPolygonData(esriCB b)
        {
            for (int i = callBoxList.Count - 1; i >= 0; i--)
            {
                if (callBoxList[i].ID == b.ID)
                {
                    callBoxList.RemoveAt(i);
                }
            }
            SQL.SQLCode sql = new SQL.SQLCode();
            sql.updateCallBoxData(b);
            callBoxList.Add(b);
        }

        public static void deleteCallBoxPolygonData(Guid id)
        {
            for (int i = callBoxList.Count - 1; i >= 0; i--)
            {
                if (callBoxList[i].ID == id)
                {
                    callBoxList.RemoveAt(i);
                }
            }
            SQL.SQLCode sql = new SQL.SQLCode();
            sql.deleteCallBoxData(id);
        }

        #endregion
    }
}