using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LotteryApp
{
    public partial class frmImport : Form
    {
        private Game GameForRpt = new Game();
        private DataTable DtGameDetail = new DataTable();
        private DataTable DtForRpt = new DataTable();
        private string arrowRise = "↑";
        private string arrowFall = "↓";
        private string arrowSteady = "→";
        private string compareLarge = "大";
        private string compareMedium = "中";
        private string compareSmall = "小";

        public frmImport()
        {
            InitializeComponent();
        }

        private void frmImport_Load(object sender, EventArgs e)
        {
            lblHome.Text = "";
            lblVisitor.Text = "";
            lblLeague.Text = "";
            lblGameTime.Text = "";
            lblScore.Text = "";
            lblResult.Text = "";
            lblFinalSP.Text = "";
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text = openFileDialog.FileName;
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (txtFilePath.Text.Equals(string.Empty))
            {
                MessageBox.Show("请先选择文件。");
                return;
            }
            int sheetsCount = ExcelUtils.GetSheetsCount(txtFilePath.Text);
            DataTable dtGame = new DataTable(); ;
            DataTable dtGameDetail = new DataTable();
            string errorMsg = string.Empty;
            DAGame daGame = new DAGame();
            Game myGame = new Game();

            DataTable dtGameList = new DataTable();
            dtGameList.Columns.Add("ID");
            dtGameList.Columns.Add("NAME");
            DataRow dr = dtGameList.NewRow();

            for (int i = 0; i <= sheetsCount - 1; i++)
            {
                //比赛信息
                dtGame = new DataTable();
                dtGame.Columns.Add("League");
                dtGame.Columns.Add("Home");
                dtGame.Columns.Add("Visitor");
                dtGame.Columns.Add("Gametime");
                dtGame.Columns.Add("Score");
                dtGame.Columns.Add("Result");
                dtGame.Columns.Add("FinalSP");

                //赔率信息
                dtGameDetail = new DataTable();
                dtGameDetail.Columns.Add("Changetime");
                dtGameDetail.Columns.Add("B");
                dtGameDetail.Columns.Add("C");
                dtGameDetail.Columns.Add("D");
                dtGameDetail.Columns.Add("Win");
                dtGameDetail.Columns.Add("Draw");
                dtGameDetail.Columns.Add("Lose");
                dtGameDetail.Columns.Add("Changeminute");

                //读取比赛信息
                try
                {
                    ExcelUtils.headRowCount = 1;
                    ExcelUtils.dataRowCount = 1;
                    ExcelUtils.dataColCount = 7;
                    ExcelUtils.dtExcel = dtGame;
                    ExcelUtils.indexDatatimeCol = "3,4";
                    dtGame = ExcelUtils.TranslateToTable(txtFilePath.Text, i);
                }
                catch
                {
                    errorMsg += (i + 1).ToString() + "，";
                    continue;
                }

                //读取赔率信息
                try
                {
                    ExcelUtils.headRowCount = 4;
                    ExcelUtils.dataRowCount = 0;
                    ExcelUtils.dataColCount = 8;
                    ExcelUtils.dtExcel = dtGameDetail;
                    ExcelUtils.indexDatatimeCol = "0";
                    dtGameDetail = ExcelUtils.TranslateToTable(txtFilePath.Text, i);
                }
                catch
                {
                    errorMsg += (i + 1).ToString() + "，";
                    continue;
                }

                myGame = new Game();
                int result = daGame.GetMaxGameSN();
                if (result == -1)
                {
                    errorMsg += (i + 1).ToString() + "，";
                    continue;
                }

                myGame.SN = result + 1;
                myGame.League = dtGame.Rows[0]["League"].ToString();
                myGame.Home = dtGame.Rows[0]["Home"].ToString();
                myGame.Visitor = dtGame.Rows[0]["Visitor"].ToString();
                myGame.GameTime = DateTime.Parse(dtGame.Rows[0]["Gametime"].ToString());
                myGame.Score = dtGame.Rows[0]["Score"].ToString();
                myGame.Result = dtGame.Rows[0]["Result"].ToString();
                myGame.FinalSP = dtGame.Rows[0]["FinalSP"].ToString();

                result = daGame.InsertGameInfo(myGame, dtGameDetail);
                if (result == -1)
                {
                    errorMsg += (i + 1).ToString() + "，";
                    continue;
                }

                dr = dtGameList.NewRow();
                dr[0] = myGame.SN;
                dr[1] = (i + 1).ToString() + "," + myGame.League + "---" + myGame.Home + " VS " + myGame.Visitor;
                dtGameList.Rows.Add(dr);
            }

            SetCmbGame(dtGameList);

            if (!string.IsNullOrEmpty(errorMsg))
            {
                MessageBox.Show("第 "+errorMsg.TrimEnd('，')+" 个Sheet导入失败。");
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            int timeStart = 0;
            int timeEnd = 0;
            #region 验证过滤时间范围的输入
            try
            {
                timeStart = int.Parse(txtTimeStart.Text);
            }
            catch
            {
                MessageBox.Show("请输入分钟0-59。");
                return;
            }
            if (timeStart < 0 || timeStart > 59)
            {
                MessageBox.Show("请输入分钟0-59。");
                return;
            }

            try
            {
                timeEnd = int.Parse(txtTimeEnd.Text);
            }
            catch
            {
                MessageBox.Show("请输入分钟0-59。");
                return;
            }

            if (timeEnd < 0 || timeEnd > 59)
            {
                MessageBox.Show("请输入分钟0-59。");
                return;
            }

            if (timeStart >= timeEnd)
            {
                MessageBox.Show("过滤时间范围输入错误，应先小后大。");
                return;
            }
            #endregion
            SetRptData();
            frmRptOdds frmRptOdds = new frmRptOdds(GameForRpt, DtForRpt);
            frmRptOdds.ShowDialog();

        }

        private void tsmHistory_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmHistory frm = new frmHistory();
            frm.ShowDialog();
            SetCmbGame(Constant.GameList);
            this.Show();
        }

        private void tsmClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void tsmClearDB_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("此操作将清空所有已经导入的数据，您确定吗？", "", MessageBoxButtons.OKCancel);

            if (result == DialogResult.OK)
            {
                DAGame daGame = new DAGame();

                if (daGame.DeleteAllGame() == 0)
                {
                    MessageBox.Show("清空数据库成功。");
                }
                else
                {
                    MessageBox.Show("清空数据库失败。");
                }
            }

        }

        private void cmbGame_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbGame.SelectedIndex > -1)
            {
                ShowGame(int.Parse(cmbGame.SelectedValue.ToString()));
            }
        }
        
        private void ShowGame(int gameSN)
        {
            DataTable dtGame = new DataTable();
            DataTable dtGameDetail = new DataTable();
            DAGame daGame = new DAGame();
            int result = daGame.GetGameByKey(gameSN, ref dtGame);
            if (result == -1)
            {
                MessageBox.Show("查询失败，请重新运行本程序再试一次。");
                return;
            }

            result = daGame.GetGameDetailByKey(gameSN, ref dtGameDetail);
            if (result == -1)
            {
                MessageBox.Show("查询失败，请重新运行本程序再试一次。");
                return;
            }
            if (dtGame.Rows.Count > 0)
            {
                GameForRpt.SN = gameSN;
                GameForRpt.League = dtGame.Rows[0]["League"].ToString();
                GameForRpt.Home = dtGame.Rows[0]["Home"].ToString();
                GameForRpt.Visitor = dtGame.Rows[0]["Visitor"].ToString();
                GameForRpt.GameTime = DateTime.Parse(dtGame.Rows[0]["Gametime"].ToString());
                GameForRpt.Score = dtGame.Rows[0]["Score"].ToString();
                GameForRpt.Result = dtGame.Rows[0]["Result"].ToString();
                GameForRpt.FinalSP = dtGame.Rows[0]["FinalSP"].ToString(); 
            }
            else
            {
                MessageBox.Show("查询失败，请重新运行本程序再试一次。");
                return;
            }

            DtGameDetail = dtGameDetail;

            #region 显示在窗体中

            lblLeague.Text = GameForRpt.League;
            lblHome.Text = GameForRpt.Home;
            lblVisitor.Text = GameForRpt.Visitor;
            lblGameTime.Text = GameForRpt.GameTime.ToString("yyyy-MM-dd HH:mm");
            lblScore.Text = DateTime.Parse(GameForRpt.Score).ToString("HH:mm").TrimStart('0');
            lblResult.Text = GameForRpt.Result;
            lblFinalSP.Text = GameForRpt.FinalSP;

            dgvDetail.DataSource = dtGameDetail;
            dgvDetail.Columns[0].HeaderText = "变化时间";
            dgvDetail.Columns[0].Width = 170;
            dgvDetail.Columns[1].HeaderText = "胜";
            dgvDetail.Columns[2].HeaderText = "平";
            dgvDetail.Columns[3].HeaderText = "负";
            dgvDetail.Columns[4].HeaderText = "分";
            dgvDetail.Columns[4].Visible = false;
            dgvDetail.Columns[0].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";
            #endregion
        }

        private void SetCmbGame(DataTable dtGameList)
        {
            cmbGame.ValueMember = "ID";
            cmbGame.DisplayMember = "NAME";
            cmbGame.DataSource = dtGameList;
        }

        private void SetRptData()
        {
            int timeStart = int.Parse(txtTimeStart.Text);
            int timeEnd = int.Parse(txtTimeEnd.Text);

            DataRow[] arrayDR = DtGameDetail.Select("Changeminute >= " + timeStart.ToString() + " and Changeminute <= " + timeEnd.ToString(), "Changetime DESC");

            //每个小时只取最新的数据
            DataTable dtTemp = new DataTable();
            dtTemp = DtGameDetail.Clone();
            string tempTime1 = "";
            string tempTime2 = "";
            for (int i = 0; i < arrayDR.Length; i++)
            {
                tempTime2 = DateTime.Parse(arrayDR[i]["Changetime"].ToString()).ToString("yyyy-MM-dd HH");
                if (tempTime1 != tempTime2)
                {
                    dtTemp.Rows.Add(arrayDR[i].ItemArray);
                    tempTime1 = tempTime2;
                }
            }

            DataTable dtForRpt = new DataTable();
            dtForRpt.Columns.Add("ChangeTime");
            dtForRpt.Columns.Add("OddsArrow");
            dtForRpt.Columns.Add("Odds");
            dtForRpt.Columns.Add("OddsCompare");
            DataRow dr = dtForRpt.NewRow();

            string oddsArrow = string.Empty;
            string odds = string.Empty;
            string oddsCompare = string.Empty;
            decimal win = 0;
            decimal draw = 0;
            decimal lose = 0;

            #region 循环处理赔率数据
            for (int i = 0; i < dtTemp.Rows.Count; i++)
            {
                oddsArrow = string.Empty;
                odds = string.Empty;
                oddsCompare = string.Empty;
                win = Decimal.Parse(dtTemp.Rows[i]["Win"].ToString());
                draw = Decimal.Parse(dtTemp.Rows[i]["Draw"].ToString());
                lose = Decimal.Parse(dtTemp.Rows[i]["Lose"].ToString());
                //过滤掉赔率大于9的数据
                if (win > 9 || draw > 9 || lose > 9)
                {
                    continue;
                }

                if (i != dtTemp.Rows.Count - 1)
                {
                    //比较胜的赔率
                    if (win > Decimal.Parse(dtTemp.Rows[i + 1]["Win"].ToString()))
                    {
                        oddsArrow += arrowRise;
                    }
                    else if (win == Decimal.Parse(dtTemp.Rows[i + 1]["Win"].ToString()))
                    {
                        oddsArrow += arrowSteady;
                    }
                    else
                    {
                        oddsArrow += arrowFall;
                    }
                    //比较平的赔率
                    if (draw > Decimal.Parse(dtTemp.Rows[i + 1]["Draw"].ToString()))
                    {
                        oddsArrow += arrowRise;
                    }
                    else if (draw == Decimal.Parse(dtTemp.Rows[i + 1]["Draw"].ToString()))
                    {
                        oddsArrow += arrowSteady;
                    }
                    else
                    {
                        oddsArrow += arrowFall;
                    }
                    //比较负的赔率
                    if (lose > Decimal.Parse(dtTemp.Rows[i + 1]["Lose"].ToString()))
                    {
                        oddsArrow += arrowRise;
                    }
                    else if (lose == Decimal.Parse(dtTemp.Rows[i + 1]["Lose"].ToString()))
                    {
                        oddsArrow += arrowSteady;
                    }
                    else
                    {
                        oddsArrow += arrowFall;
                    }
                }

                odds = Math.Floor(win + Constant.Rounding).ToString() + "-"
                    + Math.Floor(draw + Constant.Rounding).ToString() + "-"
                    + Math.Floor(lose + Constant.Rounding).ToString();

                //判断胜的赔率大小
                if (win > Math.Max(draw, lose))
                {
                    oddsCompare += compareLarge;
                }
                else if (win < Math.Min(draw, lose))
                {
                    oddsCompare += compareSmall;
                }
                else
                {
                    oddsCompare += compareMedium;
                }
                //判断平的赔率大小
                if (draw > Math.Max(win, lose))
                {
                    oddsCompare += compareLarge;
                }
                else if (draw < Math.Min(win, lose))
                {
                    oddsCompare += compareSmall;
                }
                else
                {
                    oddsCompare += compareMedium;
                }
                //判断负的赔率大小
                if (lose > Math.Max(draw, win))
                {
                    oddsCompare += compareLarge;
                }
                else if (lose < Math.Min(draw, win))
                {
                    oddsCompare += compareSmall;
                }
                else
                {
                    oddsCompare += compareMedium;
                }

                dr = dtForRpt.NewRow();
                dr["ChangeTime"] = dtTemp.Rows[i]["Changetime"];
                dr["OddsArrow"] = oddsArrow;
                dr["Odds"] = odds;
                dr["OddsCompare"] = oddsCompare;
                dtForRpt.Rows.Add(dr);
            }
            #endregion

            DtForRpt = dtForRpt;
        }

        #region 行号
        private void dgvDetail_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
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
        
    }
}
