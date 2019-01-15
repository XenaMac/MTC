﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FPSService.Admin
{
    public partial class CurrentBeats : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Logon"] == null)
            {
                Response.Redirect("Logon.aspx");
            }
            string logon = Session["Logon"].ToString();
            if (logon != "true")
            {
                Response.Redirect("Logon.aspx");
            }
            LoadBeats();
            LoadBeatSegments();
        }

        private void LoadBeats()
        {
            /*
            List<BeatData.Beat> theseBeats = BeatData.Beats.AllBeats;
            theseBeats = theseBeats.OrderBy(b => b.BeatNumber).ToList<BeatData.Beat>();
            gvBeats.DataSource = theseBeats;
            gvBeats.DataBind();
            gvBeats.Columns[0].Visible = false;
             * */
            List<BeatData.esriBeat> beats = BeatData.EsriBeats.beatPolygons;
            beats = beats.OrderBy(b => b.BeatID).ToList<BeatData.esriBeat>();
            gvBeats.DataSource = beats;
            gvBeats.DataBind();
            gvBeats.Columns[2].Visible = false;
        }

        private void LoadBeatSegments()
        {
            /*
            List<BeatData.BeatSegment> theseBeatSegments = BeatData.Beats.AllBeatSegments;
            theseBeatSegments = theseBeatSegments.OrderBy(b => b.BeatSegmentNumber).ToList<BeatData.BeatSegment>();
            gvBeatSegments.DataSource = theseBeatSegments;
            gvBeatSegments.DataBind();
            gvBeatSegments.Columns[0].Visible = false;
            gvBeatSegments.Columns[4].Visible = false;
             * */
            List<BeatData.esriSegment> segments = BeatData.EsriSegments.segPolygons;
            segments = segments.OrderBy(b => b.BeatID).ThenBy(s => s.SegmentID).ToList<BeatData.esriSegment>();
            gvBeatSegments.DataSource = segments;
            gvBeatSegments.DataBind();
            gvBeatSegments.Columns[3].Visible = false;
        }

        protected void btnReloadBeats_Click(object sender, EventArgs e)
        {
            try
            {
                //BeatData.Beats.LoadBeats();
                //BeatData.Beats.LoadBeatSegments();
                BeatData.EsriBeats.LoadBeats();
                BeatData.EsriSegments.LoadSegments();
                ClientScript.RegisterClientScriptBlock(GetType(), "Javascript", "<script>alert('All beat data reloaded')</script>");
                LoadBeats();
                LoadBeatSegments();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "Javascript", "<script>alert('" + ex.Message + "')</script>");
            }
        }

        protected void lbtnLogOff_Click(object sender, EventArgs e)
        {
            Session["Logon"] = null;
            Response.Redirect("Logon.aspx");
        }
    }
}