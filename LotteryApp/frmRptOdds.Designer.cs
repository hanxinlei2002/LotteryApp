namespace LotteryApp
{
    partial class frmRptOdds
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRptOdds));
            this.dtGameDetailBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dsGame = new LotteryApp.dsGame();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            ((System.ComponentModel.ISupportInitialize)(this.dtGameDetailBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsGame)).BeginInit();
            this.SuspendLayout();
            // 
            // dtGameDetailBindingSource
            // 
            this.dtGameDetailBindingSource.DataMember = "dtGameDetail";
            this.dtGameDetailBindingSource.DataSource = this.dsGame;
            // 
            // dsGame
            // 
            this.dsGame.DataSetName = "dsGame";
            this.dsGame.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = this.dtGameDetailBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "LotteryApp.rptOdds.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(747, 592);
            this.reportViewer1.TabIndex = 0;
            // 
            // frmRptOdds
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(747, 592);
            this.Controls.Add(this.reportViewer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmRptOdds";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "赔率变化情况";
            this.Load += new System.EventHandler(this.frmRptOdds_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtGameDetailBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsGame)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource dtGameDetailBindingSource;
        private dsGame dsGame;

    }
}