using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace UTSOFTMAIN
{
    class DBHelper
    {
        #region -- 直接链接数据库 --
        public static string user = "";
        public static string grade = "";
        public static string constr = "Data Source=122.114.36.252,2658\\SQL2008;Network Library = DBMSSOCN;Initial Catalog=UTERP;User ID=utcrm;Password = 1qazse4;Pooling=true;Max Pool Size=50;Min Pool Size=0;";
        public static int test()
        {
            int flag = 0;
            SqlConnection conn = new SqlConnection(constr);

            try
            {
                conn.Open();
            }
            catch //(Exception ex)
            {
                //frmUTSOFTMAIN.ErrMsg = ex.Message;
                flag = -1;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return flag;
        }

        public static DataTable ExecuteQuery(string sql)
        {
            SqlConnection conn = new SqlConnection(constr);
            DataSet dataset = new DataSet();
            //DataTable table = new DataTable();
            //frmUTSOFTMAIN.ErrMsg = "";
            try
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = sql;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);//将SqlCommand与SqlDataAdapter绑定
                adapter.Fill(dataset);
                //table = dataset.Tables[0];
            }
            catch
            {
                conn.Close();
                conn.Dispose();
                dataset.Dispose();
                return null;
            }
            conn.Close();
            conn.Dispose();
            dataset.Dispose();
            return dataset.Tables[0];
        }


        public static int ExecuteUpdate(string sql)
        {
            //frmUTSOFTMAIN.ErrMsg = "";
            SqlConnection conn = new SqlConnection(constr);
            SqlCommand com = new SqlCommand(sql, conn);
            int i = 0;
            try
            {
                conn.Open();

                i = com.ExecuteNonQuery();
            }
            catch { }//(Exception ex) { frmUTSOFTMAIN.ErrMsg = ex.Message; }
            finally
            {
                conn.Close();
                conn.Dispose();
                com.Dispose();
            }
            return i;
        }
        #endregion

        #region -- 通过官网端口链接 --
        //public static DataTable ExecuteQuery(string sql)
        //{
        //    string sqlstr = "http://www.utjiaoyu.com:8089/soft/?name=[SQLSERVER_SELECT]" + MyUrlEnCode(sql);
        //    string result = gethtmltext(sqlstr);
        //    if (result == "")
        //        return new DataTable();
        //    DataTable dt = DeserializeDataTable(result);
        //    return dt;
        //}

        //public static int ExecuteUpdate(string sql)
        //{
        //    string sqlstr = "http://www.utjiaoyu.com:8089/soft/?name=[SQLSERVER_UPDATE]" + MyUrlEnCode(sql);
        //    string result = gethtmltext(sqlstr);
        //    if (result == "")
        //        return -1;
        //    int i = int.Parse(result);
        //    return i;
        //}

        //public static string gethtmltext(string url)
        //{
        //    string all = "";
        //    string result = "";
        //    try
        //    {
        //        WebRequest wr = WebRequest.Create(url);
        //        Stream s = wr.GetResponse().GetResponseStream();
        //        StreamReader sr = new StreamReader(s, Encoding.UTF8);
        //        all = sr.ReadToEnd(); //读取网站的数据
        //        sr.Close();
        //        sr.Dispose();
        //        s.Close();
        //        s.Dispose();
        //        all = all.Replace("\r\n", "");
        //        Regex rg = new Regex("<body>(.*?)</body>");
        //        result = rg.Match(all).Groups[1].Value;
        //    }
        //    catch
        //    {

        //    }

        //    return result;
        //}


        ///// <summary> 
        ///// 反序列化DataTable 
        ///// </summary> 
        ///// <param name="pXml">序列化的DataTable</param> 
        ///// <returns>DataTable</returns> 
        //public static DataTable DeserializeDataTable(string pXml)
        //{
        //    if (pXml.Length == 0)
        //        return null;
        //    StringReader strReader = new StringReader(pXml);
        //    XmlReader xmlReader = XmlReader.Create(strReader);
        //    XmlSerializer serializer = new XmlSerializer(typeof(DataSet));
        //    DataTable dt = (serializer.Deserialize(xmlReader) as DataSet).Tables[0];
        //    return dt;
        //}

        ////Url编码
        //public static string MyUrlEnCode(string str)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    for (int i = 0; i < str.Length; i++)
        //    {
        //        if (str[i] == '%')
        //            sb.Append("[BFH]");
        //        else if (str[i] == '+')
        //            sb.Append("[JAH]");
        //        else if (str[i] == '-')
        //            sb.Append("[JNH]");
        //        else if (str[i] == '&')
        //            sb.Append("[ANH]");
        //        else if (str[i] == '/')
        //            sb.Append("[XEH]");
        //        else
        //            sb.Append(str[i]);
        //    }
        //    return HttpUtility.UrlEncode(sb.ToString(), System.Text.Encoding.GetEncoding("GB2312"));
        //}
        #endregion
    }

    class MySql
    {
        //public static string mysqlConnectStr = 
        //"Database=utjiaoyu2017;Data Source=122.114.36.252;User Id=utjiaoyu2017;Password='uP47F+2!h]Iv';pooling=false;CharSet=utf8;port=3306";

        //public static string mysqlConnectStr =
        //"Database=utjiaoyu;Data Source=101.37.148.206;User Id=root;Password='%DXtjyWc6i#Rqia';pooling=false;CharSet=utf8;port=3306";

        public static string mysqlConnectStr =
        "Database=utjiaoyu;Data Source=101.37.148.206;User Id=utjiaoyu;Password='5Blfsjk39H0jf';pooling=false;CharSet=utf8;port=3306";

        //public static string mysqlConnectStr =
        //    "Server=101.37.148.206;UserId=root;Password=%DXtjyWc6i#Rqia;Database=utjiaoyu;pooling=false;CharSet=utf8;port=3306";
        //"Database=utjiaoyu;Data Source=101.37.148.206;User Id=root;Password='%DXtjyWc6i#Rqia';pooling=false;CharSet=utf8;port=3306";
        //public static string mysqlConnectStr = 
        //"Provider =101.37.148.206;Data Source = utjiaoyu; User Id = root; Password=%DXtjyWc6i#Rqia";

        #region -- 直接链接数据库 --
        #region  建立MySql数据库连接
        /// <summary>
        /// 建立数据库连接.%DXtjyWc6i#Rqia
        /// </summary>
        /// <returns>返回MySqlConnection对象</returns>
        public static MySqlConnection getmysqlcon()
        {
            //http://sosoft.cnblogs.com/
            MySqlConnection myCon = new MySqlConnection(mysqlConnectStr);
            return myCon;
        }
        #endregion

        #region  执行MySqlCommand命令
        /// <summary>
        /// 执行MySqlCommand
        /// </summary>
        /// <param name="M_str_sqlstr">SQL语句</param>
        public static int ExecuteUpdate(string M_str_sqlstr)
        {
            int result = -1;
            try
            {
                MySqlConnection mysqlcon = getmysqlcon();
                mysqlcon.Open();
                MySqlCommand command = new MySqlCommand("SET NAMES utf8", mysqlcon);
                command.ExecuteNonQuery();
                MySqlCommand mysqlcom = new MySqlCommand(M_str_sqlstr, mysqlcon);
                result = mysqlcom.ExecuteNonQuery();

                mysqlcom.Dispose();
                mysqlcon.Close();
                mysqlcon.Dispose();

            }
            catch //(Exception ex)
            {
                //frmUTSOFTMAIN.ErrMsg = ex.Message;
                result = -1;
            }
            return result;
        }
        #endregion

        public static int mysqltest()
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(mysqlConnectStr);
                conn.Open();
                return 0;
            }
            catch
            {
                return -1;
            }
        }

        public static DataTable ExecuteQuery(string sqlstr)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(mysqlConnectStr);
                DataSet data = new DataSet();
                MySqlDataAdapter da = new MySqlDataAdapter(sqlstr, conn);
                MySqlCommandBuilder cb = new MySqlCommandBuilder(da); // 此处必须有，否则无法更新
                da.Fill(data);
                return data.Tables[0];
            }
            catch//(Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return null;
            }
        }

        public static void updateChange(MySqlDataAdapter da, DataTable data)
        {
            DataTable changes = data.GetChanges();
            da.Update(changes);
            data.AcceptChanges();
        }
        #endregion

        #region -- 通过官网端口链接 --
        //public static DataTable ExecuteQuery(string sql)
        //{
        //    string sqlstr = "http://www.utjiaoyu.com:8089/soft/?name=[MYSQL_SELECT]" + MyUrlEnCode(sql);
        //    string result = gethtmltext(sqlstr);
        //    DataTable dt = DeserializeDataTable(result);
        //    return dt;
        //}

        //public static int ExecuteUpdate(string sql)
        //{
        //    string sqlstr = "http://www.utjiaoyu.com:8089/soft/?name=[MYSQL_UPDATE]" + MyUrlEnCode(sql);
        //    string result = gethtmltext(sqlstr);
        //    int i = 0;
        //    try
        //    {
        //        i = int.Parse(result);
        //    }
        //    catch { }
        //    return i;
        //}

        //public static string gethtmltext(string url)
        //{
        //    string all = "";
        //    string result = "";
        //    try
        //    {
        //        WebRequest wr = WebRequest.Create(url);
        //        Stream s = wr.GetResponse().GetResponseStream();
        //        StreamReader sr = new StreamReader(s, Encoding.UTF8);
        //        all = sr.ReadToEnd(); //读取网站的数据
        //        sr.Close();
        //        sr.Dispose();
        //        s.Close();
        //        s.Dispose();
        //        all = all.Replace("\r\n", "");
        //        Regex rg = new Regex("<body>(.*?)</body>");
        //        result = rg.Match(all).Groups[1].Value;
        //    }
        //    catch
        //    {
        //    }

        //    return result;
        //}

        ///// <summary> 
        ///// 序列化DataTable 
        ///// </summary> 
        ///// <param name="pDt">包含数据的DataTable</param> 
        ///// <returns>序列化的DataTable</returns> 
        //private static string SerializeDataTableXml(DataTable pDt)
        //{
        //    if (pDt.Rows.Count == 0)
        //        return "";
        //    DataSet ds = new DataSet();//datatable不可直接序列化
        //    ds.Tables.Add(pDt);
        //    StringBuilder sb = new StringBuilder();
        //    try
        //    {
        //        XmlWriter writer = XmlWriter.Create(sb);
        //        XmlSerializer serializer = new XmlSerializer(typeof(DataSet));
        //        serializer.Serialize(writer, ds);
        //        writer.Close();
        //    }
        //    catch { }
        //    return sb.ToString();
        //}

        ///// <summary> 
        ///// 反序列化DataTable 
        ///// </summary> 
        ///// <param name="pXml">序列化的DataTable</param> 
        ///// <returns>DataTable</returns> 
        //public static DataTable DeserializeDataTable(string pXml)
        //{
        //    if (pXml == "")
        //        return null;
        //    StringReader strReader = new StringReader(pXml);
        //    XmlReader xmlReader = XmlReader.Create(strReader);
        //    XmlSerializer serializer = new XmlSerializer(typeof(DataSet));
        //    DataSet ds = serializer.Deserialize(xmlReader) as DataSet;
        //    DataTable dt = ds.Tables[0];
        //    return dt;
        //}

        ////Url编码
        //public static string MyUrlEnCode(string str)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    for (int i = 0; i < str.Length; i++)
        //    {
        //        if (str[i] == '%')
        //            sb.Append("[BFH]");
        //        else if (str[i] == '+')
        //            sb.Append("[JAH]");
        //        else if (str[i] == '-')
        //            sb.Append("[JNH]");
        //        else if (str[i] == '&')
        //            sb.Append("[ANH]");
        //        else if (str[i] == '/')
        //            sb.Append("[XEH]");
        //        else
        //            sb.Append(str[i]);
        //    }
        //    return HttpUtility.UrlEncode(sb.ToString(), System.Text.Encoding.GetEncoding("GB2312"));
        //}
        #endregion
    }
}
