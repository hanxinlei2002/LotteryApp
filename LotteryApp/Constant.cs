using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;

namespace LotteryApp
{
    public static class Constant
    {
        public static decimal Rounding = 1 - decimal.Parse(ConfigurationManager.AppSettings["Rounding"].Trim());
        
        public static DataTable GameList= new DataTable();
    }
}
