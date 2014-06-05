using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace LotteryApp
{
    public partial class frmRptOdds : Form
    {
        private Game GameForRpt = new Game();
        private DataTable DtGameDetail = new DataTable();
        public frmRptOdds(Game game,DataTable dtGameDetail)
        {
            InitializeComponent();
            GameForRpt = game;
            DtGameDetail = dtGameDetail;
            reportViewer1.LocalReport.ReportPath = "rptOdds.rdlc";
        }

        private void frmRptOdds_Load(object sender, EventArgs e)
        {
            ReportParameter[] parameters = new ReportParameter[4];
            parameters[0] = new ReportParameter("paraLeague", GameForRpt.League);
            parameters[1] = new ReportParameter("paraHome", GameForRpt.Home);
            parameters[2] = new ReportParameter("paraVisitor", GameForRpt.Visitor);
            parameters[3] = new ReportParameter("paraGameTime", GameForRpt.GameTime.ToString());


            reportViewer1.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local;

            reportViewer1.LocalReport.SetParameters(new ReportParameter[] { parameters[0] });
            reportViewer1.LocalReport.SetParameters(new ReportParameter[] { parameters[1] });
            reportViewer1.LocalReport.SetParameters(new ReportParameter[] { parameters[2] });
            reportViewer1.LocalReport.SetParameters(new ReportParameter[] { parameters[3] });

            ReportDataSource rds = new ReportDataSource("DataSet_dtGameDetail", DtGameDetail);
            reportViewer1.LocalReport.DataSources.Add(rds);

            this.reportViewer1.RefreshReport();
        }
    }
}
