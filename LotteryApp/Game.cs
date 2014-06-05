using System;
using System.Collections.Generic;
using System.Text;

namespace LotteryApp
{
    public class Game
    {
        #region Model
        private int sn;
        private string league;
        private string home;
        private string visitor;
        private DateTime gameTime;
        private string score;
        private string result;
        private string finalSP;

        /// <summary>
        /// 编号
        /// </summary>
        public int SN
        {
            set { sn = value; }
            get { return sn; }
        }
        /// <summary>
        /// 联赛
        /// </summary>
        public string League
        {
            set { league = value; }
            get { return league; }
        }
        /// <summary>
        /// 主队
        /// </summary>
        public string Home
        {
            set { home = value; }
            get { return home; }
        }
        /// <summary>
        /// 客队
        /// </summary>
        public string Visitor
        {
            set { visitor = value; }
            get { return visitor; }
        }
        /// <summary>
        /// 比赛时间
        /// </summary>
        public DateTime GameTime
        {
            set { gameTime = value; }
            get { return gameTime; }
        }
        /// <summary>
        /// 比分
        /// </summary>
        public string Score
        {
            set { score = value; }
            get { return score; }
        }
        /// <summary>
        /// 彩果
        /// </summary>
        public string Result
        {
            set { result = value; }
            get { return result; }
        }
        /// <summary>
        /// 开奖SP
        /// </summary>
        public string FinalSP
        {
            set { finalSP = value; }
            get { return finalSP; }
        }
        #endregion Model
    }
}
