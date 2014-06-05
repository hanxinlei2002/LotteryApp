using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SQLite;
using System.Collections;

namespace LotteryApp
{
    public class DAGame
    {
        public int InsertGameInfo(Game game, DataTable dtGameDetail)
        {
            SQLiteDBHelper sqliteHelper = new SQLiteDBHelper();
            StringBuilder strSql = new StringBuilder();
            //Hashtable sqlStringList = new Hashtable();
            Dictionary<StringBuilder, SQLiteParameter[]> sqlStringList = new Dictionary<StringBuilder, SQLiteParameter[]>();

            strSql = new StringBuilder();
            strSql.Append("insert into game (");
            strSql.Append("sn,league,home,visitor,gametime,score,result,finalSP)");
            strSql.Append(" values (");
            strSql.Append("@sn,@league,@home,@visitor,@gametime,@score,@result,@finalSP)");
            SQLiteParameter[] parameters = {
                    sqliteHelper.MakeSQLiteParameter("@sn", DbType.Int32,game.SN),
                    sqliteHelper.MakeSQLiteParameter("@league", DbType.String,16,game.League),
                    sqliteHelper.MakeSQLiteParameter("@home", DbType.String,32,game.Home),
                    sqliteHelper.MakeSQLiteParameter("@visitor", DbType.String,32,game.Visitor),
                    sqliteHelper.MakeSQLiteParameter("@gametime", DbType.DateTime,game.GameTime),
                    sqliteHelper.MakeSQLiteParameter("@score", DbType.String,game.Score),
                    sqliteHelper.MakeSQLiteParameter("@result", DbType.String,game.Result),
                    sqliteHelper.MakeSQLiteParameter("@finalSP", DbType.String,game.FinalSP)
                    };
            sqlStringList.Add(strSql, parameters);

            for (int i = 0; i <= dtGameDetail.Rows.Count - 1; i++)
            {
                strSql = new StringBuilder();
                strSql.Append("insert into gamedetail (");
                strSql.Append("gamesn,changetime,win,draw,lose,changeminute)");
                strSql.Append(" values (");
                strSql.Append("@gamesn,@changetime,@win,@draw,@lose,@changeminute)");
                SQLiteParameter[] para = {
                    sqliteHelper.MakeSQLiteParameter("@gamesn", DbType.Int32,game.SN),
                    sqliteHelper.MakeSQLiteParameter("@changetime", DbType.DateTime,dtGameDetail.Rows[i][0]),
                    sqliteHelper.MakeSQLiteParameter("@win", DbType.Decimal,dtGameDetail.Rows[i][4]),
                    sqliteHelper.MakeSQLiteParameter("@draw", DbType.Decimal,dtGameDetail.Rows[i][5]),
                    sqliteHelper.MakeSQLiteParameter("@lose", DbType.Decimal,dtGameDetail.Rows[i][6]),
                    sqliteHelper.MakeSQLiteParameter("@changeminute", DbType.Int32,dtGameDetail.Rows[i][7])
                    };
                sqlStringList.Add(strSql, para);
            }

            int result = sqliteHelper.ExecuteSqlTran(sqlStringList);

            return result;
        }


        public int DeleteAllGame()
        {
            SQLiteDBHelper sqliteHelper = new SQLiteDBHelper();
            StringBuilder strSql = new StringBuilder();
            Dictionary<StringBuilder, SQLiteParameter[]> sqlStringList = new Dictionary<StringBuilder, SQLiteParameter[]>();

            strSql.Append("delete from game");
            sqlStringList.Add(strSql, null);

            strSql = new StringBuilder();
            strSql.Append("delete from gamedetail");
            sqlStringList.Add(strSql, null);

            int result = sqliteHelper.ExecuteSqlTran(sqlStringList);

            return result;
        }

        public int GetMaxGameSN()
        {
            DataTable dt = new DataTable();
            SQLiteDBHelper sqliteHelper = new SQLiteDBHelper();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select max(sn) from game");

            int result = sqliteHelper.ExecuteDataTable(strSql, null, ref dt);

            if (result != -1)
            {
                if (dt.Rows[0][0].ToString().Equals(string.Empty))
                {
                    result = 0;
                }
                else
                {
                    result = int.Parse(dt.Rows[0][0].ToString());
                }
            }
            return result;
        }

        public int GetGameByKey(int gameSN, ref DataTable dt)
        {
            SQLiteDBHelper sqliteHelper = new SQLiteDBHelper();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from game ");
            strSql.Append("where sn = @sn order by gametime desc");

            SQLiteParameter[] para = {
                    sqliteHelper.MakeSQLiteParameter("@sn", DbType.Int32,gameSN)
                                         };
            int result = sqliteHelper.ExecuteDataTable(strSql, para, ref dt);

            return result;
        }

        public int GetGameBySearch(DateTime start, DateTime end, string team, ref DataTable dt)
        {
            SQLiteDBHelper sqliteHelper = new SQLiteDBHelper();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select gametime,league,home,visitor,sn from game ");
            strSql.Append("where 1 = 1 ");
            if (start != null)
            {
                strSql.Append("and gametime >= @start ");
            }
            if (end != null)
            {
                strSql.Append("and gametime <= @end ");
            }
            if (team != "")
            {
                strSql.Append("and home like @home or visitor like @visitor ");
            }
            strSql.Append(" order by gametime desc");
            SQLiteParameter[] para = {
                    sqliteHelper.MakeSQLiteParameter("@start", DbType.DateTime,start),
                    sqliteHelper.MakeSQLiteParameter("@end", DbType.DateTime,end),
                    sqliteHelper.MakeSQLiteParameter("@home", DbType.String,32,"%"+team+"%"),
                    sqliteHelper.MakeSQLiteParameter("@visitor", DbType.String,32,"%"+team+"%")
                                         };
            int result = sqliteHelper.ExecuteDataTable(strSql, para, ref dt);

            return result;
        }
        
        public int GetGameDetailByKey(int gameSN, ref DataTable dt)
        {
            SQLiteDBHelper sqliteHelper = new SQLiteDBHelper();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select changetime,win,draw,lose,changeminute from gamedetail ");
            strSql.Append("where gamesn = @gamesn ");

            SQLiteParameter[] para = {
                    sqliteHelper.MakeSQLiteParameter("@gamesn", DbType.Int32,gameSN)
                                         };
            int result = sqliteHelper.ExecuteDataTable(strSql, para, ref dt);

            return result;
        }


    }
}
