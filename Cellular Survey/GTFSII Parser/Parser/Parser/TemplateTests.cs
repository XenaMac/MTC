using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Metra.Module.ServiceAlerts.Models
{
    class TemplateTests
    {
        public AlertTemplate AlrtTmplt;
        public MetraTemplate MtrTmplt;
        public List<LineAlertInfo> LaiList, LaiList2, LaiList3;
        public List<String> StationList;
        public List<TrainAlertInfo> TaiList;
        public String DelayReason;
        public TemplateParser TmpltPrsr;

        public TemplateTests()
        {
            InitVars();
        }

        public void InitVars()
        {
            AlrtTmplt = new AlertTemplate();
            MtrTmplt = new MetraTemplate();
            TmpltPrsr = new TemplateParser();
            LaiList = new List<LineAlertInfo>();
            LaiList2 = new List<LineAlertInfo>(); 
            LaiList3 = new List<LineAlertInfo>();
            StationList = new List<String>();
            TaiList = new List<TrainAlertInfo>();

            LineAlertInfo lai = new LineAlertInfo();
            lai.LineName = "Metra Electric";
            lai.LineAbbr = "ME";
            lai.SubLineList = new List<String>();
            lai.SubLineList.Add("University Park");
            lai.SubLineList.Add("Blue Island");
            LaiList.Add(lai);
            LaiList3.Add(lai);

            lai = new LineAlertInfo();
            lai.LineName = "UP North";
            lai.LineAbbr = "UP-N";
            lai.SubLineList = null;
            LaiList2.Add(lai);
            LaiList3.Add(lai);

            TrainAlertInfo tai = new TrainAlertInfo();
            tai.TrainNum = "2134";
            tai.SchedOrigin = "Grayslake";
            tai.SchedOriginTime = "11:58:00";
            tai.SchedDestination = "Chicago Union Station";
            tai.SchedDestTime = "13:22:00";
            TaiList.Add(tai);

            tai = new TrainAlertInfo();
            tai.TrainNum = "2136";
            tai.SchedOrigin = "Fox Lake";
            tai.SchedOriginTime = "12:45:00";
            tai.SchedDestination = "Chicago Union Station";
            tai.SchedDestTime = "14:22:00";
            TaiList.Add(tai);

            tai = new TrainAlertInfo();
            tai.TrainNum = "2138";
            tai.SchedOrigin = "Grayslake";
            tai.SchedOriginTime = "13:58:00";
            tai.SchedDestination = "Chicago Union Station";
            tai.SchedDestTime = "15:22:00";
            TaiList.Add(tai);

            StationList.Add("Hubbard Woods");
            StationList.Add("Winnetka");
            StationList.Add("Indian Hill");

            DelayReason = "signal problems";
        }

        public void TestAllTemplates()
        {
            TestLineTemplates();
            TestTrainTemplates();
            TestTrainAutoTemplates();
            TestStationTemplates();
            TestADATemplates();
            TestElevatorTemplates();
            TestSystemTemplates();
        }

        public void TestLineTemplates()
        {
            Console.WriteLine("====================================================");
            Console.WriteLine("   LINES");
            for (int indx = 0; indx < 10; ++indx)
            {
                AlrtTmplt.Header = MtrTmplt.GetAlertTemplateHeader()[indx];
                AlrtTmplt.Text = MtrTmplt.GetAlertTemplateText()[indx];
                DelayReason = "signal problems";
                Console.WriteLine("====================================================");
                Console.WriteLine("--- No sublines ---");
                ParsedMessage pm = TmpltPrsr.ParseLineAlert(LaiList2, DelayReason, AlrtTmplt);
                Console.WriteLine("====================================================");
                Console.WriteLine(MtrTmplt.GetAlertTemplateName()[indx]);
                Console.WriteLine("--- header template ---");
                Console.WriteLine(AlrtTmplt.Header);
                Console.WriteLine("--- parsed header result ---");
                Console.WriteLine(pm.AlertSummary);
                Console.WriteLine("--- text template ---");
                Console.WriteLine(AlrtTmplt.Text);
                Console.WriteLine("--- parsed text result ---");
                Console.WriteLine(pm.AlertText);
                Console.WriteLine("====================================================");
                Console.WriteLine("--- no delay reason ---");
                DelayReason = "[ none ]";
                pm = TmpltPrsr.ParseLineAlert(LaiList2, DelayReason, AlrtTmplt);
                Console.WriteLine("====================================================");
                Console.WriteLine(MtrTmplt.GetAlertTemplateName()[indx]);
                Console.WriteLine("--- header template ---");
                Console.WriteLine(AlrtTmplt.Header);
                Console.WriteLine("--- parsed header result ---");
                Console.WriteLine(pm.AlertSummary);
                Console.WriteLine("--- text template ---");
                Console.WriteLine(AlrtTmplt.Text);
                Console.WriteLine("--- parsed text result ---");
                Console.WriteLine(pm.AlertText);
                DelayReason = "signal problems";
                Console.WriteLine("====================================================");
                Console.WriteLine("--- Multiple sublines ---");
                pm = TmpltPrsr.ParseLineAlert(LaiList, DelayReason, AlrtTmplt);
                Console.WriteLine("====================================================");
                Console.WriteLine(MtrTmplt.GetAlertTemplateName()[indx]);
                Console.WriteLine("--- header template ---");
                Console.WriteLine(AlrtTmplt.Header);
                Console.WriteLine("--- parsed header result ---");
                Console.WriteLine(pm.AlertSummary);
                Console.WriteLine("--- text template ---");
                Console.WriteLine(AlrtTmplt.Text);
                Console.WriteLine("--- parsed text result ---");
                Console.WriteLine(pm.AlertText);
                Console.WriteLine("====================================================");
                Console.WriteLine("--- no delay reason ---");
                DelayReason = "[ none ]";
                pm = TmpltPrsr.ParseLineAlert(LaiList, DelayReason, AlrtTmplt);
                Console.WriteLine("====================================================");
                Console.WriteLine(MtrTmplt.GetAlertTemplateName()[indx]);
                Console.WriteLine("--- header template ---");
                Console.WriteLine(AlrtTmplt.Header);
                Console.WriteLine("--- parsed header result ---");
                Console.WriteLine(pm.AlertSummary);
                Console.WriteLine("--- text template ---");
                Console.WriteLine(AlrtTmplt.Text);
                Console.WriteLine("--- parsed text result ---");
                Console.WriteLine(pm.AlertText);
                DelayReason = "signal problems";
                Console.WriteLine("====================================================");
                Console.WriteLine("--- Multiple lines and sublines ---");
                pm = TmpltPrsr.ParseLineAlert(LaiList3, DelayReason, AlrtTmplt);
                Console.WriteLine("====================================================");
                Console.WriteLine(MtrTmplt.GetAlertTemplateName()[indx]);
                Console.WriteLine("--- header template ---");
                Console.WriteLine(AlrtTmplt.Header);
                Console.WriteLine("--- parsed header result ---");
                Console.WriteLine(pm.AlertSummary);
                Console.WriteLine("--- text template ---");
                Console.WriteLine(AlrtTmplt.Text);
                Console.WriteLine("--- parsed text result ---");
                Console.WriteLine(pm.AlertText);
                Console.WriteLine("====================================================");
                Console.WriteLine("--- no delay reason ---");
                DelayReason = "[ none ]";
                pm = TmpltPrsr.ParseLineAlert(LaiList3, DelayReason, AlrtTmplt);
                Console.WriteLine("====================================================");
                Console.WriteLine(MtrTmplt.GetAlertTemplateName()[indx]);
                Console.WriteLine("--- header template ---");
                Console.WriteLine(AlrtTmplt.Header);
                Console.WriteLine("--- parsed header result ---");
                Console.WriteLine(pm.AlertSummary);
                Console.WriteLine("--- text template ---");
                Console.WriteLine(AlrtTmplt.Text);
                Console.WriteLine("--- parsed text result ---");
                Console.WriteLine(pm.AlertText);
            }
        }

        public void TestTrainTemplates()
        {
            Console.WriteLine("====================================================");
            Console.WriteLine("   TRAINS");
            for (int indx = 10; indx < 30; ++indx)
            {
                AlrtTmplt.Header = MtrTmplt.GetAlertTemplateHeader()[indx];
                AlrtTmplt.Text = MtrTmplt.GetAlertTemplateText()[indx];
                DelayReason = "signal problems";
                ParsedMessage pm = TmpltPrsr.ParseTrainAlert(TaiList, DelayReason, AlrtTmplt);
                Console.WriteLine("====================================================");
                Console.WriteLine(MtrTmplt.GetAlertTemplateName()[indx]);
                Console.WriteLine("--- header template ---");
                Console.WriteLine(AlrtTmplt.Header);
                Console.WriteLine("--- parsed header result ---");
                Console.WriteLine(pm.AlertSummary);
                Console.WriteLine("--- text template ---");
                Console.WriteLine(AlrtTmplt.Text);
                Console.WriteLine("--- parsed text result ---");
                Console.WriteLine(pm.AlertText);
                Console.WriteLine("====================================================");
                Console.WriteLine("--- no delay reason ---");
                DelayReason = "[ none ]";
                pm = TmpltPrsr.ParseTrainAlert(TaiList, DelayReason, AlrtTmplt);
                Console.WriteLine("====================================================");
                Console.WriteLine(MtrTmplt.GetAlertTemplateName()[indx]);
                Console.WriteLine("--- header template ---");
                Console.WriteLine(AlrtTmplt.Header);
                Console.WriteLine("--- parsed header result ---");
                Console.WriteLine(pm.AlertSummary);
                Console.WriteLine("--- text template ---");
                Console.WriteLine(AlrtTmplt.Text);
                Console.WriteLine("--- parsed text result ---");
                Console.WriteLine(pm.AlertText);
            }
        }

        public void TestTrainAutoTemplates()
        {
            Console.WriteLine("====================================================");
            Console.WriteLine("   TRAIN AUTO ALERTS");
            for (int indx = 30; indx < 31; ++indx)
            {
                AlrtTmplt.Header = MtrTmplt.GetAlertTemplateHeader()[indx];
                AlrtTmplt.Text = MtrTmplt.GetAlertTemplateText()[indx];
                DelayReason = "signal problems";
                ParsedMessage pm = TmpltPrsr.ParseTrainAutoAlert(TaiList, DelayReason, 15, AlrtTmplt);
                Console.WriteLine("====================================================");
                Console.WriteLine(MtrTmplt.GetAlertTemplateName()[indx]);
                Console.WriteLine("--- header template ---");
                Console.WriteLine(AlrtTmplt.Header);
                Console.WriteLine("--- parsed header result ---");
                Console.WriteLine(pm.AlertSummary);
                Console.WriteLine("--- text template ---");
                Console.WriteLine(AlrtTmplt.Text);
                Console.WriteLine("--- parsed text result ---");
                Console.WriteLine(pm.AlertText);
                Console.WriteLine("====================================================");
                Console.WriteLine("--- no delay reason ---");
                DelayReason = "[ none ]";
                pm = TmpltPrsr.ParseTrainAutoAlert(TaiList, DelayReason, 15, AlrtTmplt);
                Console.WriteLine("====================================================");
                Console.WriteLine(MtrTmplt.GetAlertTemplateName()[indx]);
                Console.WriteLine("--- header template ---");
                Console.WriteLine(AlrtTmplt.Header);
                Console.WriteLine("--- parsed header result ---");
                Console.WriteLine(pm.AlertSummary);
                Console.WriteLine("--- text template ---");
                Console.WriteLine(AlrtTmplt.Text);
                Console.WriteLine("--- parsed text result ---");
                Console.WriteLine(pm.AlertText);
            }
        }

        public void TestStationTemplates()
        {
            Console.WriteLine("====================================================");
            Console.WriteLine("   STATIONS");
            for (int indx = 32; indx < 33; ++indx)
            {
                AlrtTmplt.Header = MtrTmplt.GetAlertTemplateHeader()[indx];
                AlrtTmplt.Text = MtrTmplt.GetAlertTemplateText()[indx];
                ParsedMessage pm = TmpltPrsr.ParseStationAlert(StationList, AlrtTmplt);
                Console.WriteLine("====================================================");
                Console.WriteLine(MtrTmplt.GetAlertTemplateName()[indx]);
                Console.WriteLine("--- header template ---");
                Console.WriteLine(AlrtTmplt.Header);
                Console.WriteLine("--- parsed header result ---");
                Console.WriteLine(pm.AlertSummary);
                Console.WriteLine("--- text template ---");
                Console.WriteLine(AlrtTmplt.Text);
                Console.WriteLine("--- parsed text result ---");
                Console.WriteLine(pm.AlertText);
            }
        }

        public void TestADATemplates()
        {
            Console.WriteLine("====================================================");
            Console.WriteLine("   ADA");
            for (int indx = 33; indx < 35; ++indx)
            {
                AlrtTmplt.Header = MtrTmplt.GetAlertTemplateHeader()[indx];
                AlrtTmplt.Text = MtrTmplt.GetAlertTemplateText()[indx];
                ParsedMessage pm = TmpltPrsr.ParseAdaAlert(StationList, AlrtTmplt);
                Console.WriteLine("====================================================");
                Console.WriteLine(MtrTmplt.GetAlertTemplateName()[indx]);
                Console.WriteLine("--- header template ---");
                Console.WriteLine(AlrtTmplt.Header);
                Console.WriteLine("--- parsed header result ---");
                Console.WriteLine(pm.AlertSummary);
                Console.WriteLine("--- text template ---");
                Console.WriteLine(AlrtTmplt.Text);
                Console.WriteLine("--- parsed text result ---");
                Console.WriteLine(pm.AlertText);
            }
        }

        public void TestElevatorTemplates()
        {
            Console.WriteLine("====================================================");
            Console.WriteLine("   ELEVATORS");
            for (int indx = 33; indx < 35; ++indx)
            {
                AlrtTmplt.Header = MtrTmplt.GetAlertTemplateHeader()[indx];
                AlrtTmplt.Text = MtrTmplt.GetAlertTemplateText()[indx];
                ParsedMessage pm = TmpltPrsr.ParseElevatorAlert(StationList, AlrtTmplt);
                Console.WriteLine("====================================================");
                Console.WriteLine(MtrTmplt.GetAlertTemplateName()[indx]);
                Console.WriteLine("--- header template ---");
                Console.WriteLine(AlrtTmplt.Header);
                Console.WriteLine("--- parsed header result ---");
                Console.WriteLine(pm.AlertSummary);
                Console.WriteLine("--- text template ---");
                Console.WriteLine(AlrtTmplt.Text);
                Console.WriteLine("--- parsed text result ---");
                Console.WriteLine(pm.AlertText);
            }
        }

        public void TestSystemTemplates()
        {
            Console.WriteLine("====================================================");
            Console.WriteLine("   SYSTEM");
            for (int indx = 35; indx < 41; ++indx)
            {
                AlrtTmplt.Header = MtrTmplt.GetAlertTemplateHeader()[indx];
                AlrtTmplt.Text = MtrTmplt.GetAlertTemplateText()[indx];
                ParsedMessage pm = TmpltPrsr.ParseSystemAlert(AlrtTmplt);
                Console.WriteLine("====================================================");
                Console.WriteLine(MtrTmplt.GetAlertTemplateName()[indx]);
                Console.WriteLine("--- header template ---");
                Console.WriteLine(AlrtTmplt.Header);
                Console.WriteLine("--- parsed header result ---");
                Console.WriteLine(pm.AlertSummary);
                Console.WriteLine("--- text template ---");
                Console.WriteLine(AlrtTmplt.Text);
                Console.WriteLine("--- parsed text result ---");
                Console.WriteLine(pm.AlertText);
            }
        }
    }
}
