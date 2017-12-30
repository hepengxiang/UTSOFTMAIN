using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MODEL
{

    public class LessonTable
    {
        public string LessonName { get; set; }
        public DateTime LessonTime { get; set; }
        public string LessonLink { get; set; }
        public string DownloadLink { get; set; }
        public string Teacher { get; set; }
        public string Remark { get; set; }
    }

    public class SoftwareTable
    {
        public string SoftwareName { get; set; }//软件名称
        public string Version { get; set; }//软件版本
        public string IcoName { get; set; }//图标文件名
        public float Size { get; set; }//文件大小
        public DateTime UpdateTime { get; set; }//更新日期
        public string Remark { get; set; }//软件说明
        public int Hot { get; set; }//软件热度
        public string FileName { get; set; }//软件运行文件名
        public string VideoLink { get; set; }//视频教程
        public string Type { get; set; }//软件类别  Free/vip/ut
        //public bool Release { get; set; }//是否发布 0 未发布 1 已发布
    }

    public class province
    {
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
    }

    public class city
    {
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string provincecode { get; set; }
    }

    public class area
    {
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string citycode { get; set; }
    }

    public class GlobalMenu//全局菜单
    {
        public string UpMenuName { get; set; }//上级菜单名称
        public string MenuName { get; set; }//菜单名称
        public string pageName{ get; set; }//关联tabpage名称
        public string MenuType { get; set; } //left button ,top button ,tabpage
        public int btnImg { get; set; } //按钮连续三个背景图标的第一个图片排位，从0开始
        public bool Visible { get; set; } //可见性
    }

    public class OperationLog//全局菜单
    {
        public DateTime OperationTime { get; set; }//操作时间
        public string IPAddr { get; set; } //lIP地址
        public string StaffID { get; set; }//身份证

        public string MenuLeft { get; set; } //l左边主界面 （2：弹窗按钮）
        public string MenuRight { get; set; } //l右边子界面（3：弹窗界面）
        public string MenuIn { get; set; } //l按钮名称

        public string OperationObject { get; set; } //操作对象

        public string OperationRemark { get; set; } //相关内容说明
    }

    public class QQGroupMemberInfo
    {
        public string QQ { get; set; }//QQ号
        public string NickName { get; set; }//昵称
        public string GroupNumber { get; set; }//群号
        public string GroupCard { get; set; }//群昵称
        public string GroupGrade { get; set; }//群身份
        public string join_time { get; set; }//加入时间
        public string last_speak_time { get; set; }//最后发言时间
    }
    

}
