using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LotteryApp
{
    public partial class frmHistory : Form
    {
        public frmHistory()
        {
            InitializeComponent();
        }

        private void frmHistory_Load(object sender, EventArgs e)
        {
            dtpStart.Value = DateTime.Today.AddDays(-7);
            dtpEnd.Value = DateTime.Today;
        }

        private void Init()
        {
            try
            {
                DataTable dt = new DataTable();
                DAGame daGame = new DAGame();
                Game myGame = new Game();
                int result = daGame.GetGameBySearch(dtpStart.Value, dtpEnd.Value, txtTeam.Text.Trim(), ref dt);
                if (result == -1)
                {
                    MessageBox.Show("查询失败，请重新运行本程序再试一次。");
                    return;
                }

                dgvGame.DataSource = dt;
                dgvGame.Columns[0].HeaderText = "比赛时间";
                dgvGame.Columns[0].Width = 150;
                dgvGame.Columns[1].Width = 90;
                dgvGame.Columns[2].Width = 110;
                dgvGame.Columns[3].Width = 110;
                dgvGame.Columns[1].HeaderText = "联赛";
                dgvGame.Columns[2].HeaderText = "主队";
                dgvGame.Columns[3].HeaderText = "客队";
                dgvGame.Columns[4].Visible = false; ;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Init();
        }

        private void dgvGame_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataTable dtGameList = new DataTable();
                dtGameList.Columns.Add("ID");
                dtGameList.Columns.Add("NAME");
                DataRow dr = dtGameList.NewRow();


                dr[0] = dgvGame.Rows[e.RowIndex].Cells["sn"].Value.ToString();
                dr[1] = "1," + dgvGame.Rows[e.RowIndex].Cells["league"].Value.ToString()
                        + "---" + dgvGame.Rows[e.RowIndex].Cells["home"].Value.ToString() + " VS "
                        + dgvGame.Rows[e.RowIndex].Cells["visitor"].Value.ToString();

                dtGameList.Rows.Add(dr);

                Constant.GameList = dtGameList;
                this.Dispose();
                this.Close();
            }
        }

        #region 行号

        private void dgvGame_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var dgv = (DataGridView)sender;
            if (dgv.RowHeadersVisible)
            {
                Rectangle rect = new Rectangle(e.RowBounds.Left, e.RowBounds.Top,
                                               dgv.RowHeadersWidth, e.RowBounds.Height);
                rect.Inflate(-2, -2);
                TextRenderer.DrawText(e.Graphics,
                   (e.RowIndex + 1).ToString(),
                   e.InheritedRowStyle.Font,
                   rect, e.InheritedRowStyle.ForeColor,
                   TextFormatFlags.Right | TextFormatFlags.VerticalCenter
                   );
            }
        }

        #endregion

        private void btnAnalysis_Click(object sender, EventArgs e)
        {
            DataTable dtGameList = new DataTable();
            dtGameList.Columns.Add("ID");
            dtGameList.Columns.Add("NAME");
            DataRow dr = dtGameList.NewRow();

            for (int i = 0; i < dgvGame.Rows.Count; i++)
            {
                dr = dtGameList.NewRow();
                dr[0] = dgvGame.Rows[i].Cells["sn"].Value.ToString();
                dr[1] = (i + 1).ToString() + "," + dgvGame.Rows[i].Cells["league"].Value.ToString()
                    + "---" + dgvGame.Rows[i].Cells["home"].Value.ToString() + " VS "
                    + dgvGame.Rows[i].Cells["visitor"].Value.ToString();

                dtGameList.Rows.Add(dr);
            }
            Constant.GameList = dtGameList;
            this.Dispose();
            this.Close();
        }
    }
}
