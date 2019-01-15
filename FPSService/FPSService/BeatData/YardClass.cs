using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Microsoft.SqlServer.Types;

namespace FPSService.BeatData
{
    public static class YardClass
    {
        //DEPRECATED - REPLACED WITH Yards.cs

        static Logging.EventLogger logger = new Logging.EventLogger();
        public static List<Yard> AllYards = new List<Yard>();

        public static void LoadYards()
        {
            /*
            SQL.SQLCode mySQL = new SQL.SQLCode();
            AllYards = mySQL.LoadYards();
             * */
        }

        public static string FindInYard(string ContractCompanyName, SqlGeography thisPoint)
        {
            string YardDescription = "Not In Yard";

            var yList = from y in AllYards
                        where y.TowTruckCompanyName == ContractCompanyName
                        select y;

            foreach (Yard y in yList)
            {
                if (y.Position.STBuffer(10).STIntersects(thisPoint))
                {
                    YardDescription = y.YardDescription;
                }
            }

            return YardDescription;
        }
        
    }

    public class Yard //defines an individual yard
    {
        public Guid YardID { get; set; }
        public string Location { get; set; }
        public string TowTruckCompanyName { get; set; }
        public SqlGeography Position { get; set; }
        public string YardDescription { get; set; }
    }
}