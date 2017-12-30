using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace UTSOFTMAIN
{
    class PublicMethod
    {
        /// <summary>
        /// 获取预定报名类基数
        /// </summary>
        /// <param name="CompanyNames">所属公司</param>
        /// <param name="DepartmentName">所属部门</param>
        /// <param name="GroupName">所属组</param>
        /// <param name="jxCardinalNumStr">基数类型</param>
        /// <param name="jxTime">绩效时间</param>
        /// <param name="sfzStr">身份证号码</param>
        /// <param name="enterType">主类型</param>
        /// <param name="enterTypeSmall">子类型</param>
        /// <param name="value">加减值</param>
        /// <returns></returns>
        public static float getYDBMJXCardinalNum(string CompanyNames, string DepartmentName, string GroupName, string jxCardinalNumStr, DateTime jxTime,
            string sfzStr, string enterType, string enterTypeSmall, float value)
        {
            string sqljxCardinalNum = "";

            if (jxCardinalNumStr == "本部门")
            {
                sqljxCardinalNum = string.Format("select coalesce(sum(a.Value),0) from Pub_YDBM_JXManager a" +
                    " left join Pub_VIPMessage b on a.ForID = b.id" +
                    " left join Users c on a.StaffID = c.StaffID" +
                    " where c.CompanyNames = '{0}' and c.DepartmentName = '{1}' and" +
                    " datepart(yy,b.Jointime) ={2} and datepart(mm,b.Jointime) ={3} and b.EnterType = '{4}'" +
                    " and a.EnterTypeSmall = '{5}' and a.StaffID = a.AmountManager",
                    CompanyNames, DepartmentName, jxTime.Year, jxTime.Month, enterType, enterTypeSmall);
            }
            else if (jxCardinalNumStr == "本组")
            {
                sqljxCardinalNum = string.Format("select coalesce(sum(a.Value),0) from Pub_YDBM_JXManager a" +
                    " left join Pub_VIPMessage b on a.ForID = b.id" +
                    " left join Users c on a.StaffID = c.StaffID" +
                    " where c.CompanyNames = '{0}' and c.DepartmentName = '{1}' and EnterTypeSmall = '{2}'" +
                    " and datepart(yy,b.Jointime) ={3} and datepart(mm,b.Jointime) ={4} and b.EnterType = '{5}'" +
                    " and a.EnterTypeSmall = '{6}' and a.StaffID = a.AmountManager and c.IsPartManager = 0",
                    CompanyNames, DepartmentName, GroupName, jxTime.Year, jxTime.Month, enterType, enterTypeSmall);
            }
            else//本人
            {
                sqljxCardinalNum = string.Format("select coalesce(sum(a.Value),0) from Pub_YDBM_JXManager a" +
                    " left join Pub_VIPMessage b on a.ForID = b.id" +
                    " left join Users c on a.StaffID = c.StaffID" +
                    " where a.StaffID = '{0}' and datepart(yy,b.Jointime) ={1} and datepart(mm,b.Jointime) ={2}" +
                    " and b.EnterType = '{3}' and a.EnterTypeSmall = '{4}' and a.StaffID = a.AmountManager",
                    sfzStr, jxTime.Year, jxTime.Month, enterType, enterTypeSmall);
            }


            DataTable dtjxCardinalNum = DBHelper.ExecuteQuery(sqljxCardinalNum);
            if (dtjxCardinalNum.Rows.Count == 0) 
            {
                return value;
            }
            return float.Parse(dtjxCardinalNum.Rows[0][0].ToString()) + value;
        }
        /// <summary>
        /// 预定报名类绩效单价计算
        /// </summary>
        /// <param name="sfzStr">绩效人身份证</param>
        /// <param name="jxTime">绩效时间</param>
        /// <param name="enterType">绩效主类型</param>
        /// <param name="enterTypeSmall">绩效子类型</param>
        /// <param name="value">绩效数量</param>
        public static float jxYDBMCaculate(string sfzStr, DateTime jxTime, string enterType, string enterTypeSmall, float value)
        {
            //查出此人的信息
            string[] columNames = new string[] { "CompanyNames", "DepartmentName", "GroupName", "UserType", "AssumedName", "StaffID" };
            string sql = string.Format("StaffID='{0}'", sfzStr);
            DataTable dt1 = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            if (dt1.Rows.Count == 0)
                return 0;
            //找到此类型和子类型一起对应的计算标准
            string sql1 = string.Format("select * from Pub_JXCaculate where CompanyNames = '{0}' and DepartmentName = '{1}'" +
                " and GroupName = '{2}' and UserType = '{3}' and JXType = '{4}' and JXTypeSmall = '{5}' order by UpperLimit asc",
                dt1.Rows[0][0].ToString(), dt1.Rows[0][1].ToString(), dt1.Rows[0][2].ToString(), dt1.Rows[0][3].ToString(),
                enterType, enterTypeSmall);
            DataTable dtJXCaculate = DBHelper.ExecuteQuery(sql1);
            if (dtJXCaculate.Rows.Count == 0)
                return 0;
            string jxCardinalNumStr = dtJXCaculate.Rows[0][7].ToString();//绩效基数 本部门，本组，本人
            float jxCardinalNumThisMonth = 0;//绩效基数-数字
            float jxCardinalNumBeforeMonth = 0;//绩效基数-数字
            string DCCardinalNumStr = dtJXCaculate.Rows[0][8].ToString();//档次基准
            string DCCardinalType = dtJXCaculate.Rows[0][9].ToString();//档次类型
            float oneValue = 0;//单价
            //得到绩效时间月绩效基数和根据档次基准看是否需要得到绩效时间月的上个月的绩效基数
            jxCardinalNumThisMonth = getYDBMJXCardinalNum(dtJXCaculate.Rows[0][1].ToString(), dtJXCaculate.Rows[0][2].ToString(), dtJXCaculate.Rows[0][3].ToString(),
                    jxCardinalNumStr, jxTime, sfzStr, enterType, enterTypeSmall, value);
            if (DCCardinalNumStr == "上月")
            {
                jxCardinalNumBeforeMonth = getYDBMJXCardinalNum(dtJXCaculate.Rows[0][1].ToString(), dtJXCaculate.Rows[0][2].ToString(), dtJXCaculate.Rows[0][3].ToString(),
                    jxCardinalNumStr, jxTime.AddMonths(-1), sfzStr, enterType, enterTypeSmall, 0);
            }
            else //绩效时间月（本月）
            {
                jxCardinalNumBeforeMonth = 0;
            }

            for (int i = 0; i < dtJXCaculate.Rows.Count; i++)
            {
                //查看基数所满足的档次，档次算右不算左
                float lowerLimitNum = 0;
                float UpperLimitNum = 0;

                if (DCCardinalType == "百分比")
                {
                    if (jxCardinalNumBeforeMonth == 0)//上月团队绩效为0
                    {
                        lowerLimitNum = -1;
                        UpperLimitNum = 10000;
                    }
                    else
                    {
                        lowerLimitNum = jxCardinalNumBeforeMonth * (1 + (float.Parse(dtJXCaculate.Rows[i][10].ToString()) / 100));
                        UpperLimitNum = jxCardinalNumBeforeMonth * (1 + (float.Parse(dtJXCaculate.Rows[i][11].ToString()) / 100));
                    }
                }
                else
                {
                    lowerLimitNum = jxCardinalNumBeforeMonth + float.Parse(dtJXCaculate.Rows[i][10].ToString());
                    UpperLimitNum = jxCardinalNumBeforeMonth + float.Parse(dtJXCaculate.Rows[i][11].ToString());
                }
                if (jxCardinalNumThisMonth >= lowerLimitNum && jxCardinalNumThisMonth < UpperLimitNum)
                {
                    //得到绩效时间月此绩效的单价,去最大数字
                    oneValue = float.Parse(dtJXCaculate.Rows[i][12].ToString());
                }
            }
            return oneValue;
        }
        /// <summary>
        /// 插入预定报名类绩效
        /// </summary>
        /// <param name="forid">外键，报名绩效所对应的报名信息</param>
        /// <param name="sfzStr">绩效所属人的身份证</param>
        /// <param name="jxTime">绩效月</param>
        /// <param name="enterType">绩效大类（直接报名，跟踪报名）</param>
        /// <param name="enterTypeSmall">绩效小类（直接报名，跟踪报名）</param>
        /// <param name="value">绩效分值</param>
        /// <param name="remark">绩效备注</param>
        /// <returns>返回插入结果</returns>
        public static bool insert_YDBMJX(int forid, string sfzStr, DateTime jxTime, string enterType, string enterTypeSmall, float value,string remark) 
        {
            //查出此人的信息
            string[] columNames = new string[] { "CompanyNames", "DepartmentName", "GroupName", "UserType", "AssumedName", "StaffID" };
            string sql = string.Format("StaffID='{0}'", sfzStr);
            DataTable dt1 = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            if (dt1.Rows.Count == 0)
                return false;
            //查询出拥有此人管理绩效的人,排除本人
            string sqlPersonManage = string.Format("select b.StaffID,b.CompanyNames from Pub_JXCaculate a left join Users b " +
                " on a.CompanyNames = b.CompanyNames and a.DepartmentName = b.DepartmentName and a.GroupName = b.GroupName and a.UserType = b.UserType" +
                " where a.CompanyNames = '{0}' and a.DepartmentName = '{1}' and a.GroupName = '{2}' " +
                " and JXCardinalNum in ('本部门','本组') and JXType = '{3}' and JXTypeSmall = '{4}' and b.StaffID != '{5}' group by b.StaffID,b.CompanyNames",
                dt1.Rows[0][0].ToString(), dt1.Rows[0][1].ToString(), dt1.Rows[0][2].ToString(), enterType, enterTypeSmall, sfzStr);
            DataTable dtPersonManager = DBHelper.ExecuteQuery(sqlPersonManage);
            int resultNum = 0;
            for (int i = 0; i < dtPersonManager.Rows.Count; i++)
            {
                //计算管理层绩效月对应的类型的单价
                float jxOneValueManager = jxYDBMCaculate(dtPersonManager.Rows[i][0].ToString(), jxTime, enterType, enterTypeSmall, value);
                //插入 管理层绩效
                string sqlInsertManagerJX = string.Format("insert into Pub_YDBM_JXManager values({0},'{1}','{2}',{3},{4},'{5}','{6}',getdate(),'{7}')",
                    forid, sfzStr, enterTypeSmall, value, 0, "管理层预定报名类绩效（系统插入）", dtPersonManager.Rows[i][0].ToString(),frmUTSOFTMAIN.StaffID);
                DBHelper.ExecuteUpdate(sqlInsertManagerJX);
                //更新 管理层绩效金额
                string sqlUpdateManagerJX = string.Format("update Pub_YDBM_JXManager set Amount = {0}*Value where" +
                    " ForID in (select id from Pub_VIPMessage where datepart(yy,Jointime) ={1} and datepart(mm,Jointime) ={2} and EnterType = '{3}')" +
                    " and EnterTypeSmall = '{4}' and AmountManager = '{5}'",
                    jxOneValueManager, jxTime.Year, jxTime.Month, enterType, enterTypeSmall, dtPersonManager.Rows[i][0].ToString());
                resultNum = DBHelper.ExecuteUpdate(sqlUpdateManagerJX);
            }
            //计算 本人绩效月对应的类型的金额
            float jxOneValuePerson = jxYDBMCaculate(sfzStr, jxTime, enterType, enterTypeSmall, value);
            //插入 本人绩效
            string sqlInsertPersonJX = string.Format("insert into Pub_YDBM_JXManager values({0},'{1}','{2}',{3},{4},'{5}','{6}',getdate(),'{7}')",
                forid, sfzStr, enterTypeSmall, value, jxOneValuePerson * value, remark, sfzStr, frmUTSOFTMAIN.StaffID);
            DBHelper.ExecuteUpdate(sqlInsertPersonJX);
            //更新 本人绩效金额
            string sqlUpdatePersonJX = string.Format("update Pub_YDBM_JXManager set Amount = {0}*Value where" +
                    " ForID in (select id from Pub_VIPMessage where datepart(yy,Jointime) ={1} and datepart(mm,Jointime) ={2} and EnterType = '{3}')" +
                    " and EnterTypeSmall = '{4}' and AmountManager = '{5}'",
                    jxOneValuePerson, jxTime.Year, jxTime.Month, enterType, enterTypeSmall, sfzStr);
            resultNum = DBHelper.ExecuteUpdate(sqlUpdatePersonJX);
            if (resultNum > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 删除预定报名类绩效
        /// </summary>
        /// <param name="id">需要删除的绩效对应的id</param>
        /// <param name="forid">外键，报名绩效所对应的报名信息</param>
        /// <param name="sfzStr">绩效所属人的身份证</param>
        /// <param name="jxTime">绩效月</param>
        /// <param name="enterType">绩效大类（直接报名，跟踪报名）</param>
        /// <param name="enterTypeSmall">绩效小类（直接报名，跟踪报名）</param>
        /// <param name="value">绩效分值</param>
        /// <returns>返回删除结果</returns>
        public static bool delete_YDBMJX(int id, int forid, string sfzStr, DateTime jxTime, string enterType, string enterTypeSmall, float value)
        {
            //查出此人的信息
            string[] columNames = new string[] { "CompanyNames", "DepartmentName", "GroupName", "UserType", "AssumedName", "StaffID" };
            string sql = string.Format("StaffID='{0}'", sfzStr);
            DataTable dt1 = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            if (dt1.Rows.Count == 0)
                return false;
            //查询出拥有此人管理绩效的人,排除本人
            string sqlPersonManage = string.Format("select b.StaffID,b.CompanyNames from Pub_JXCaculate a left join Users b " +
                " on a.CompanyNames = b.CompanyNames and a.DepartmentName = b.DepartmentName and a.GroupName = b.GroupName and a.UserType = b.UserType" +
                " where a.CompanyNames = '{0}' and a.DepartmentName = '{1}' and a.GroupName = '{2}' " +
                " and JXCardinalNum in ('本部门','本组') and JXType = '{3}' and JXTypeSmall = '{4}' and b.StaffID != '{5}' group by b.StaffID,b.CompanyNames",
                dt1.Rows[0][0].ToString(), dt1.Rows[0][1].ToString(), dt1.Rows[0][2].ToString(), enterType, enterTypeSmall, sfzStr);
            DataTable dtPersonManager = DBHelper.ExecuteQuery(sqlPersonManage);
            int resultNum = 0;
            for (int i = 0; i < dtPersonManager.Rows.Count; i++)
            {
                //计算管理层绩效月对应的类型的单价(传入负分值)
                float jxOneValueManager = jxYDBMCaculate(dtPersonManager.Rows[i][0].ToString(), jxTime, enterType, enterTypeSmall, -value);
                //删除 管理层绩效
                string sqlDeleteManagerJX = string.Format("delete from Pub_YDBM_JXManager where ForID = {0} "+
                    "and EnterTypeSmall = '{1}' and StaffID = '{2}' and AmountManager = '{3}'",
                    forid, enterTypeSmall, sfzStr, dtPersonManager.Rows[i][0].ToString());
                DBHelper.ExecuteUpdate(sqlDeleteManagerJX);
                //更新 管理层绩效金额
                string sqlUpdateManagerJX = string.Format("update Pub_YDBM_JXManager set Amount = {0}*Value where" +
                    " ForID in (select id from Pub_VIPMessage where datepart(yy,Jointime) ={1} and datepart(mm,Jointime) ={2} and EnterType = '{3}')" +
                    " and EnterTypeSmall = '{4}' and AmountManager = '{5}'",
                    jxOneValueManager, jxTime.Year, jxTime.Month, enterType, enterTypeSmall, dtPersonManager.Rows[i][0].ToString());
                DBHelper.ExecuteUpdate(sqlUpdateManagerJX);
            }
            //计算 本人绩效月对应的类型的金额(传入负分值)
            float jxOneValuePerson = jxYDBMCaculate(sfzStr, jxTime, enterType, enterTypeSmall, -value);
            //删除 本人绩效
            string sqlDeletePersonJX = string.Format("delete from Pub_YDBM_JXManager where id = {0}",id);
            resultNum += DBHelper.ExecuteUpdate(sqlDeletePersonJX);
            //更新 本人绩效金额
            string sqlUpdatePersonJX = string.Format("update Pub_YDBM_JXManager set Amount = {0}*Value where" +
                    " ForID in (select id from Pub_VIPMessage where datepart(yy,Jointime) ={1} and datepart(mm,Jointime) ={2} and EnterType = '{3}')" +
                    " and EnterTypeSmall = '{4}' and AmountManager = '{5}'",
                    jxOneValuePerson, jxTime.Year, jxTime.Month, enterType, enterTypeSmall, sfzStr);
            resultNum += DBHelper.ExecuteUpdate(sqlUpdatePersonJX);
            if (resultNum > 0)
                return true;
            else
                return false;
        }






        /// <summary>
        /// 获取预定报名类基数
        /// </summary>
        /// <param name="CompanyNames">所属公司</param>
        /// <param name="DepartmentName">所属部门</param>
        /// <param name="GroupName">所属组</param>
        /// <param name="jxCardinalNumStr">基数类型</param>
        /// <param name="jxTime">绩效时间</param>
        /// <param name="sfzStr">身份证号码</param>
        /// <param name="enterType">主类型</param>
        /// <param name="enterTypeSmall">子类型</param>
        /// <param name="value">加减值</param>
        /// <returns></returns>
        public static float getTSTKJXCardinalNum(string CompanyNames, string DepartmentName, string GroupName, string jxCardinalNumStr, DateTime jxTime,
            string sfzStr, string enterType, string enterTypeSmall, float value)
        {
            string sqljxCardinalNum = "";
            if (enterTypeSmall.Contains("投诉"))
            {
                if (jxCardinalNumStr == "本部门")
                {
                    sqljxCardinalNum = string.Format("select coalesce(sum(a.Value),0) from Pub_TSTK_JXManager a" +
                        " left join Pub_ComplainRefundInfo b on a.ForID = b.id" +
                        " left join Users c on a.StaffID = c.StaffID" +
                        " where c.CompanyNames = '{0}' and c.DepartmentName = '{1}' and" +
                        " datepart(yy,b.SubmitTime) ={2} and datepart(mm,b.SubmitTime) ={3}"+// and b.SubmitType = '{4}'" +
                        " and a.EnterTypeSmall = '{5}' and a.StaffID = a.AmountManager",
                        CompanyNames, DepartmentName, jxTime.Year, jxTime.Month, enterType, enterTypeSmall);
                }
                else if (jxCardinalNumStr == "本组")
                {
                    sqljxCardinalNum = string.Format("select coalesce(sum(a.Value),0) from Pub_TSTK_JXManager a" +
                        " left join Pub_VIPMessage b on a.ForID = b.id" +
                        " left join Users c on a.StaffID = c.StaffID" +
                        " where c.CompanyNames = '{0}' and c.DepartmentName = '{1}' and EnterTypeSmall = '{2}'" +
                        " datepart(yy,b.SubmitTime) ={3} and datepart(mm,b.SubmitTime) ={4}" +// and b.SubmitType = '{5}'" +
                        " and a.EnterTypeSmall = '{6}' and a.StaffID = a.AmountManager and c.IsPartManager = 0",
                        CompanyNames, DepartmentName, GroupName, jxTime.Year, jxTime.Month, enterType, enterTypeSmall);
                }
                else//本人
                {
                    sqljxCardinalNum = string.Format("select coalesce(sum(a.Value),0) from Pub_TSTK_JXManager a" +
                        " left join Pub_ComplainRefundInfo b on a.ForID = b.id" +
                        " left join Users c on a.StaffID = c.StaffID" +
                        " where a.StaffID = '{0}' and datepart(yy,b.SubmitTime) ={1} and datepart(mm,b.SubmitTime) ={2}" +// and b.SubmitType = '{3}'" +
                        " and a.EnterTypeSmall = '{4}' and a.StaffID = a.AmountManager",
                        sfzStr, jxTime.Year, jxTime.Month, enterType, enterTypeSmall);
                }
            }
            else 
            {
                if (jxCardinalNumStr == "本部门")
                {
                    sqljxCardinalNum = string.Format("select coalesce(sum(a.Value),0) from Pub_TSTK_JXManager a" +
                        " left join Pub_ComplainRefundInfo b on a.ForID = b.id" +
                        " left join Users c on a.StaffID = c.StaffID" +
                        " where c.CompanyNames = '{0}' and c.DepartmentName = '{1}' and" +
                        " datepart(yy,b.SubmitTime) ={2} and datepart(mm,b.SubmitTime) ={3} and b.SubmitType = '{4}'" +
                        " and a.EnterTypeSmall = '{5}' and a.StaffID = a.AmountManager",
                        CompanyNames, DepartmentName, jxTime.Year, jxTime.Month, enterType, enterTypeSmall);
                }
                else if (jxCardinalNumStr == "本组")
                {
                    sqljxCardinalNum = string.Format("select coalesce(sum(a.Value),0) from Pub_TSTK_JXManager a" +
                        " left join Pub_VIPMessage b on a.ForID = b.id" +
                        " left join Users c on a.StaffID = c.StaffID" +
                        " where c.CompanyNames = '{0}' and c.DepartmentName = '{1}' and EnterTypeSmall = '{2}'" +
                        " datepart(yy,b.SubmitTime) ={3} and datepart(mm,b.SubmitTime) ={4} and b.SubmitType = '{5}'" +
                        " and a.EnterTypeSmall = '{6}' and a.StaffID = a.AmountManager and c.IsPartManager = 0",
                        CompanyNames, DepartmentName, GroupName, jxTime.Year, jxTime.Month, enterType, enterTypeSmall);
                }
                else//本人
                {
                    sqljxCardinalNum = string.Format("select coalesce(sum(a.Value),0) from Pub_TSTK_JXManager a" +
                        " left join Pub_ComplainRefundInfo b on a.ForID = b.id" +
                        " left join Users c on a.StaffID = c.StaffID" +
                        " where a.StaffID = '{0}' and datepart(yy,b.SubmitTime) ={1} and datepart(mm,b.SubmitTime) ={2}" +
                        " and b.SubmitType = '{3}' and a.EnterTypeSmall = '{4}' and a.StaffID = a.AmountManager",
                        sfzStr, jxTime.Year, jxTime.Month, enterType, enterTypeSmall);
                }
            }
            
            DataTable dtjxCardinalNum = DBHelper.ExecuteQuery(sqljxCardinalNum);
            return float.Parse(dtjxCardinalNum.Rows[0][0].ToString()) + value;
        }
        /// <summary>
        /// 预定报名类绩效单价计算
        /// </summary>
        /// <param name="sfzStr">绩效人身份证</param>
        /// <param name="jxTime">绩效时间</param>
        /// <param name="enterType">绩效主类型</param>
        /// <param name="enterTypeSmall">绩效子类型</param>
        /// <param name="value">绩效数量</param>
        public static float jxTSTKCaculate(string sfzStr, DateTime jxTime, string enterType, string enterTypeSmall, float value)
        {
            //查出此人的信息
            string[] columNames = new string[] { "CompanyNames", "DepartmentName", "GroupName", "UserType", "AssumedName", "StaffID" };
            string sql = string.Format("StaffID='{0}'", sfzStr);
            DataTable dt1 = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            if (dt1.Rows.Count == 0)
                return 0;
            //找到此类型和子类型一起对应的计算标准
            string sql1 = string.Format("select * from Pub_JXCaculate where CompanyNames = '{0}' and DepartmentName = '{1}'" +
                " and GroupName = '{2}' and UserType = '{3}' and JXType = '{4}' and JXTypeSmall = '{5}'",
                dt1.Rows[0][0].ToString(), dt1.Rows[0][1].ToString(), dt1.Rows[0][2].ToString(), dt1.Rows[0][3].ToString(),
                enterType, enterTypeSmall);
            DataTable dtJXCaculate = DBHelper.ExecuteQuery(sql1);
            if (dtJXCaculate.Rows.Count == 0)
                return 0;
            string jxCardinalNumStr = dtJXCaculate.Rows[0][7].ToString();//绩效基数 本部门，本组，本人
            float jxCardinalNumThisMonth = 0;//绩效基数-数字
            float jxCardinalNumBeforeMonth = 0;//绩效基数-数字
            string DCCardinalNumStr = dtJXCaculate.Rows[0][8].ToString();//档次基准
            string DCCardinalType = dtJXCaculate.Rows[0][9].ToString();//档次类型
            float oneValue = 0;//单价
            //得到绩效时间月绩效基数和根据档次基准看是否需要得到绩效时间月的上个月的绩效基数
            jxCardinalNumThisMonth = getTSTKJXCardinalNum(dtJXCaculate.Rows[0][1].ToString(), dtJXCaculate.Rows[0][2].ToString(), dtJXCaculate.Rows[0][3].ToString(),
                    jxCardinalNumStr, jxTime, sfzStr, enterType, enterTypeSmall, value);
            if (DCCardinalNumStr == "上月")
            {
                jxCardinalNumBeforeMonth = getTSTKJXCardinalNum(dtJXCaculate.Rows[0][1].ToString(), dtJXCaculate.Rows[0][2].ToString(), dtJXCaculate.Rows[0][3].ToString(),
                    jxCardinalNumStr, jxTime.AddMonths(-1), sfzStr, enterType, enterTypeSmall, 0);
            }
            else //绩效时间月（本月）
            {
                jxCardinalNumBeforeMonth = 0;
            }

            for (int i = 0; i < dtJXCaculate.Rows.Count; i++)
            {
                //查看基数所满足的档次，档次算左不算右
                float lowerLimitNum = 0;
                float UpperLimitNum = 0;

                if (DCCardinalType == "百分比")
                {
                    if (jxCardinalNumBeforeMonth == 0)//上月团队绩效为0
                    {
                        lowerLimitNum = -1;
                        UpperLimitNum = 10000;
                    }
                    else
                    {
                        lowerLimitNum = jxCardinalNumBeforeMonth * (1 + (float.Parse(dtJXCaculate.Rows[i][10].ToString()) / 100));
                        UpperLimitNum = jxCardinalNumBeforeMonth * (1 + (float.Parse(dtJXCaculate.Rows[i][11].ToString()) / 100));
                    }
                }
                else
                {
                    lowerLimitNum = jxCardinalNumBeforeMonth + float.Parse(dtJXCaculate.Rows[i][10].ToString());
                    UpperLimitNum = jxCardinalNumBeforeMonth + float.Parse(dtJXCaculate.Rows[i][11].ToString());
                }
                if (jxCardinalNumThisMonth >= lowerLimitNum && jxCardinalNumThisMonth < UpperLimitNum)
                {
                    //得到绩效时间月此绩效的单价
                    oneValue = float.Parse(dtJXCaculate.Rows[i][12].ToString());
                }
            }
            return oneValue;
        }
        /// <summary>
        /// 插入预定报名类绩效
        /// </summary>
        /// <param name="forid">外键，报名绩效所对应的报名信息</param>
        /// <param name="sfzStr">绩效所属人的身份证</param>
        /// <param name="jxTime">绩效月</param>
        /// <param name="enterType">绩效大类（直接报名，跟踪报名）</param>
        /// <param name="enterTypeSmall">绩效小类（直接报名，跟踪报名）</param>
        /// <param name="value">绩效分值</param>
        /// <param name="remark">绩效备注</param>
        /// <returns>返回插入结果</returns>
        public static bool insert_TSTKJX(int forid, string sfzStr, DateTime jxTime, string enterType, string enterTypeSmall, float value, string remark)
        {
            //查出此人的信息
            string[] columNames = new string[] { "CompanyNames", "DepartmentName", "GroupName", "UserType", "AssumedName", "StaffID" };
            string sql = string.Format("StaffID='{0}'", sfzStr);
            DataTable dt1 = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            if (dt1.Rows.Count == 0)
                return false;
            int resultNum = 0;
            //计算 本人绩效月对应的类型的金额
            float jxOneValuePerson = jxTSTKCaculate(sfzStr, jxTime, enterType, enterTypeSmall, value);
            //插入 本人绩效
            string sqlInsertPersonJX = string.Format("insert into Pub_TSTK_JXManager values({0},'{1}','{2}',{3},{4},'{5}','{6}',getdate(),'{7}')",
                forid, sfzStr, enterTypeSmall, value, jxOneValuePerson * value, remark, sfzStr, frmUTSOFTMAIN.StaffID);
            resultNum += DBHelper.ExecuteUpdate(sqlInsertPersonJX);

            if (resultNum > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 删除预定报名类绩效
        /// </summary>
        /// <param name="id">需要删除的绩效对应的id</param>
        /// <param name="forid">外键，报名绩效所对应的报名信息</param>
        /// <param name="sfzStr">绩效所属人的身份证</param>
        /// <param name="jxTime">绩效月</param>
        /// <param name="enterType">绩效大类（直接报名，跟踪报名）</param>
        /// <param name="enterTypeSmall">绩效小类（直接报名，跟踪报名）</param>
        /// <param name="value">绩效分值</param>
        /// <returns>返回删除结果</returns>
        public static bool delete_TSTKJX(int id, int forid, string sfzStr, DateTime jxTime, string enterType, string enterTypeSmall, float value)
        {
            //查出此人的信息
            string[] columNames = new string[] { "CompanyNames", "DepartmentName", "GroupName", "UserType", "AssumedName", "StaffID" };
            string sql = string.Format("StaffID='{0}'", sfzStr);
            DataTable dt1 = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            if (dt1.Rows.Count == 0)
                return false;
            int resultNum = 0;

            //float jxOneValuePerson = jxTSTKCaculate(sfzStr, jxTime, enterType, enterTypeSmall, -value);
            //删除 本人绩效
            string sqlDeletePersonJX = string.Format("delete from Pub_TSTK_JXManager where id = {0}", id);
            resultNum += DBHelper.ExecuteUpdate(sqlDeletePersonJX);
            if (enterTypeSmall.Contains("投诉"))
            {
                //查询出第一档的参数
                string sqlCheckPersonJX = string.Format("select UpperLimit,Value from Pub_JXCaculate where CompanyNames = '{0}' and DepartmentName = '{1}' and GroupName = '{2}' " +
                    "and UserType = '{3}' and JXType = '{4}' and JXTypeSmall = '{5}' order by UpperLimit asc",
                    dt1.Rows[0][0].ToString(), dt1.Rows[0][1].ToString(), dt1.Rows[0][2].ToString(), dt1.Rows[0][3].ToString(), enterType, enterTypeSmall);
                DataTable dtCheckPersonJX = DBHelper.ExecuteQuery(sqlCheckPersonJX);
                if (dtCheckPersonJX.Rows.Count > 0)
                {
                    //更新此人此类的第一档
                    string sqlUpdatePersonJX = string.Format("update Pub_TSTK_JXManager set Amount = Value*{0} where id in"+ 
                        " (select top {1} a.id from Pub_TSTK_JXManager a left join Pub_ComplainRefundInfo b on a.ForID = b.id"+  
                        " where a.StaffID = a.AmountManager and a.AmountManager = '{2}' and b.CompanyNames = '{3}' and"+
                        " a.EnterTypeSmall = '{5}'" +  
                        " order by b.SubmitTime desc)",
                        dtCheckPersonJX.Rows[0][1].ToString(),
                        dtCheckPersonJX.Rows[0][0].ToString(), 
                        sfzStr, dt1.Rows[0][0].ToString(), enterType, enterTypeSmall);
                    DBHelper.ExecuteUpdate(sqlUpdatePersonJX);
                }
            }
            if (resultNum > 0)
                return true;
            else
                return false;
        }



        /// <summary>
        /// 插入拥有退款QQ绩效的退款记录
        /// </summary>
        /// <param name="tkForID">退款记录的id</param>
        /// <param name="qqCompany">退款记录公司</param>
        /// <param name="qqStr">退款QQ</param>
        public static void insert_TSTK_QQManagerJX(string tkForID,string qqCompany,string qqStr) 
        {
            //查询出那些人拥有此QQ报名绩效
            string sqlCheckQQJXManager = string.Format("select b.AmountManager,b.Amount from Pub_VIPMessage a cross join Pub_YDBM_JXManager b where a.id = b.ForID" +
                " and a.CompanyNames = '{0}' and a.qq = '{1}' and a.EnterType like '%报名%'",
                qqCompany, qqStr);
            DataTable dtQQJXManager = DBHelper.ExecuteQuery(sqlCheckQQJXManager);
            
            if(dtQQJXManager.Rows.Count>0)
            {
                string sqlInsertQQJXMamagerTK = "insert into Pub_TSTK_JXManager (ForID,StaffID,EnterTypeSmall,Value,Amount,Remark,AmountManager,EnterTime,EnterPerson)";
                //插入绩效退款金额绩效
                for (int i = 0; i < dtQQJXManager.Rows.Count; i++)
                {
                    sqlInsertQQJXMamagerTK += string.Format(" select {0},'{1}','绩效退款',1,{2},'','{1}',getdate(),'{3}' union all",
                        tkForID, dtQQJXManager.Rows[i][0].ToString(), dtQQJXManager.Rows[i][1].ToString(), frmUTSOFTMAIN.StaffID);
                }
                sqlInsertQQJXMamagerTK = sqlInsertQQJXMamagerTK.Substring(0, sqlInsertQQJXMamagerTK.Length - 10);
                DBHelper.ExecuteUpdate(sqlInsertQQJXMamagerTK);
            }
        }




        /// <summary>
        /// 表格修改限制权限
        /// </summary>
        /// <param name="companyType">公司类型</param>
        /// <param name="tableName">表格名称</param>
        /// <param name="updateTime">被修改对象时间</param>
        public static bool fromUpdatePower(string companyType, string tableName, string updateTimeStr) 
        {
            bool state = true;
            DateTime updateTime = DateTime.Parse(updateTimeStr);
            //被修改对象时间在本月第一天之前
            if (updateTime <= System.DateTime.Now.AddDays(1 - System.DateTime.Now.Day))
            {
                string sql1 = string.Format("select DaysLength from SYS_FromPower where CompanyType = '{0}' and FromName = '{1}'",
                    companyType, tableName);
                DataTable dt1 = DBHelper.ExecuteQuery(sql1);
                if (dt1.Rows.Count != 0)
                {
                    if (int.Parse(dt1.Rows[0][0].ToString()) == 0) 
                    {
                        return true;
                    }
                    if (updateTime.AddDays(int.Parse(dt1.Rows[0][0].ToString())) < System.DateTime.Now) 
                    {
                        state =  false;
                    }
                }
            }
            return state;
        }
    
    }
}
