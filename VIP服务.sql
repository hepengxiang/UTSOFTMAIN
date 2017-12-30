IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UTSoftUserInfo]') AND type in (N'U'))
--DROP TABLE [dbo].UTSoftUserInfo
go
create table UTSoftUserInfo 
(
	QQ varchar(14) primary key not null,--QQ号
	NickName varchar(30),       --昵称
	Grade varchar(20),--等级
	Teacher varchar(30),--老师
	Manager varchar(30),--责任人
	ActorName varchar(30),--角色名称
	ActorSN int,--角色编号
	Join_time datetime,--加入VIp时间
	login_time datetime,--最后登录时间
	UseNumber int ,--使用本软件次数
	LoginStat int,--登录状态
	Name varchar(10),--姓名
	Age int,--年龄
	Sex varchar(2),--性别
	Education varchar(10),--学历
	EmploymentTime int ,-- 从业时间（月）
	YearBalance decimal,--年营业额
	category varchar(20),--经验类目
	ShopLink varchar(150),--店铺链接
	ShopGrade varchar(10),--店铺等级
	Telephone varchar(20),--电话号码
	HomeAddress varchar(150),--家庭住址
	Superiority varchar(2000),--拥有的资源或优势
	QunNumber varchar(20),--学习群号
	QunCode varchar(200),--入群代码
	Introduce varchar(30),--介绍人
	ComeFrom varchar(10),--来源（淘大或腾讯）
	province varchar(30),--省
	city varchar(30),--市
	area varchar(30),--区
	Remark varchar(200), --备注
	CheckIDPhoto varchar(20),--实名认证照片文件名称
	ShowPhoto varchar(20),--名片照片文件名称
	CheckStat varchar(2),--实名验证状态 0 不要验证  1 要求验证 2 验证失败 3 验证通过
	station varchar(20),--岗位
	AuthorizeFlag varchar (2),--授权标志 0 不能授权，1 允许授权
	machineCode varchar(100),--机器码
	AuthorizeCode varchar(200),--授权码
	AuthorizeDate datetime,--授权日期
	AuthorizeCount int, --授权次数
	First_LoginDate datetime,   --初次登录时间
	Last_LoginIpAddr varchar(200), --最后登录IP地址
	Passwd varchar(50) --密码
)
go
select distinct ActorName from UTSoftUserInfo
--select * into   tmp_UTSoftUserInfo from UTSoftUserInfo
--drop table UTSoftUserInfo
--insert into UTSoftUserInfo select * from tmp_UTSoftUserInfo

select  a.qq, a.nickname,count(distinct b.qq )  as vipcnt, 
sum( case when c.ShouldTraceDate <getdate() then 1 else 0 end ) as shouldcnt,
sum( case when c.TraceDate='1900-1-1' then 0 else 1 end) as actcnt from 
( select  qq,nickname from UTSoftUserInfo where  ActorName ='助教老师' or ActorName ='解答部组长' ) a,
( select qq,nickname,teacher from UTSoftUserInfo where teacher<>'') b,
( select qq,nickname,ShouldTraceDate,TraceDate from TraceRecord ) c 
where b.teacher=a.nickname and c.qq=b.qq
group by a.qq,a.nickname


select 1 from TraceRecord  where TraceContent='' and TraceTimes like '%回访%' 
and ShouldTraceDate<=getdate() 
and qq in (select qq from UTSoftUserInfo where Manager = 'evil')


update UTSoftUserInfo set manager='evil',join_time='2016-05-28 16:08:27.033' where qq='2995597626'

select * from TraceRecord where qq in (select qq from UTSoftUserInfo where Manager ='evil')

select 1 from TraceRecord   where TraceContent='' and TraceTimes like '%回访%' and ShouldTraceDate<=getdate() 
and qq in (select qq from UTSoftUserInfo where Manager = 'evil') 

--drop table distinct_GroupMemberInfo    

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TraceRecord]') AND type in (N'U'))
--DROP TABLE [dbo].TraceRecord
go
create table TraceRecord 
(
	QQ varchar(14)   not null,--QQ号
	NickName varchar(30) ,       --昵称
	TraceContent varchar(2000),--跟踪内容摘要
	TraceTimes varchar(10),--跟踪类型 1 一天 2 三天 3 一周 4 一月
	TraceType varchar(10), --跟踪方式  QQ  电话 .......
	ShouldTraceDate datetime, --应跟踪日期
	TraceDate datetime, --实际跟踪日期
	CheckDate datetime, --审核日期
	Checker varchar(30) , --审核人
	CheckFlag varchar(10) --待审核，无效，有效
)
go

select * from TraceRecord where qq ='2995597626'
delete from TraceRecord where qq ='2995597626'
--drop table TraceRecord_tmp
select * into TraceRecord_tmp from TraceRecord





select distinct TraceTimes from TraceRecord



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ExchangeRecord]') AND type in (N'U'))
--DROP TABLE [dbo].ExchangeRecord
go
create table ExchangeRecord --交互记录
(
	LessonType varchar(14)  not null,--0 导师推荐 1 当日课程
	ReceiveNickName varchar(30),       --接受方昵称
	ReMark varchar(200)  not null,--备注
	SendNickName varchar(30),       --发起方昵称
	ExchangeTime datetime, --交流时间,发起时间
	LessonName varchar(100),--课程名称
	PlanTime datetime, --课程完成时间
	ExchangeType varchar(10),-- 学习计划
	Request varchar(500),--发起内容
	Answer varchar(500),--回复内容
	ReadFlag int --ReadFlag学习计划：0 未查看笔记 1 已查看笔记 10 未查看点评 11 已查看点评 20  未查看推荐 21 已查看推荐
)
go



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ExchangeRecord]') AND type in (N'U'))
--DROP TABLE [dbo].StudentPass
go
create table StudentPass --学员通关测试
(
	QQ varchar(14)   not null,--QQ号
	NickName varchar(30) ,       --昵称
	PassSn int primary key (QQ,PassSn),--关卡序号
	PassName varchar(100),--考题标题名称
	ReMark varchar(200)  not null,--考题简介
	PassFileName varchar(100),--学生考题文件名称
	TestTime datetime, --学生提交时间
	TeacherName varchar(30),       --批改老师名称
	RePlayTime datetime, --批改时间
	PassStat  varchar(10)   not null ,--通关状态: 未开启 未提交  未批改  未通过 已通过 
	ReadFlag int, --ReadFlag 查看状态：0 未查看学员试卷 1 已查看学员试卷 10 未查看老师批改结果 11 已查老师批改结果
	Score int --评分
)
go

delete StudentPass
select * from StudentPass  where qq='2881763444' and PassSn='1' 
select min(PassSn) from TeacherPass where publish='1'  and PassSn not in (select PassSn from StudentPass where qq='296830244')
select min(PassSn) from TeacherPass where publish='1'  and PassSn not in (select PassSn from StudentPass where qq='296830244') 

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ExchangeRecord]') AND type in (N'U'))
--DROP TABLE [dbo].TeacherPass
go
create table  TeacherPass --通关测试关卡
(
	PassSn int primary key ,                 --关卡编号
	PassName varchar(100),--标题名称
	ReMark varchar(200)  not null,--考题简介
	PassFileName varchar(100),--文件名称
	TeacherName varchar(30) ,     --出题老师
	publish bit --是否发布
)
go

select  PassSn  from TeacherPass  where publish='1' order by PassSn
select *  from TeacherPass
select PassName,ReMark ,PassFileName from TeacherPass where PassSn='2'

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PassQuestion]') AND type in (N'U'))
--DROP TABLE [dbo].PassQuestion
go
create table  PassQuestion --通关测试题目
(
	PassSn int  ,                 --关卡编号
	QuestionSn int primary key(PassSn,QuestionSn) ,  --题目编号
	QuestionContent varchar(1000),--问题内容
	QuestionPic1 varchar(50),--问题附图1
	QuestionPic2 varchar(50),--问题附图2
	QuestionPic3 varchar(50),--问题附图3
	TotalScore int,--本题满分得分
)
go

select * from PassQuestion
select  * ,'','','','','',''  from PassQuestion where PassSn='3'  and QuestionSn='1' 

select '3',QuestionSn,QuestionContent,QuestionPic1,QuestionPic2,QuestionPic3,TotalScore,'2881763444','','','','','0'  
from PassQuestion where PassSn='3'  order by QuestionSn

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PassAnswer]') AND type in (N'U'))
--DROP TABLE [dbo].PassAnswer
go
create table  PassAnswer --学生通关测试答案
(
	PassSn int  ,                 --关卡编号
	QuestionSn int primary key(qq,PassSn,QuestionSn) ,  --题目编号
	QuestionContent varchar(500),--问题内容
	QuestionPic1 varchar(50),--问题附图1
	QuestionPic2 varchar(50),--问题附图2
	QuestionPic3 varchar(50),--问题附图3
	TotalScore int,--本题满分得分
	
	QQ varchar(14)   not null,--QQ号
	AnswerContent varchar(500),--答案内容
	AnswerPic1 varchar(50),--答案附图1
	AnswerPic2 varchar(50),--答案附图2
	AnswerPic3 varchar(50),--答案附图3
	Score int,--实际得分
)
go

insert into PassAnswer 
select '3',QuestionSn,QuestionContent,QuestionPic1,QuestionPic2,QuestionPic3,TotalScore,'2881763444','','','','','0'  from PassQuestion where PassSn='3'  order by QuestionSn
delete PassAnswer where PassSn='1' and qq='2881763444' 
select * from PassAnswer

select max(DiagnosisDate) from VIPMemberShopDiagnosis where qq='282048381' 
select * from VIPMemberShopDiagnosis where qq='282048381' 

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LessonInfo]') AND type in (N'U'))
--DROP TABLE [dbo].LessonInfo
go
create table LessonInfo 
(
	LessonName varchar(100) primary key not null,--课程名称
	LessonTime datetime, --上课开始日期时间
	LessonLink varchar(200),--课堂链接地址
	DownloadLink varchar(200),--下载地址
	DownloadPassword varchar(20),--下载密码
	Teacher varchar(20), --教师
	Remark varchar(300),      --课程简介
	LessonType varchar(10), --课程类别 基础 VIP 美工 实操
	ClassType varchar (20)  --班级类别 精英 孵化 基础。。。
)
go



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VIPExchange]') AND type in (N'U'))
--DROP TABLE [dbo].VIPExchange
go
create table VIPExchange
(
	QQ1 varchar(14)  not null,--申请QQ号
	NickName1 varchar(30),       --申请昵称
	QQ2 varchar(14)  not null,--接受QQ号
	NickName2 varchar(30),       --接受昵称
	stat varchar(2),            --状态 0 申请中 1 通过成为好友 2拒绝成为好友未查看 3 已成为好友未查看
	Remark varchar (100)--昵称    --申请理由
)
go


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Actor]') AND type in (N'U'))
--DROP TABLE [dbo].Actor
go
create table Actor --菜单节点
(
	Actorsn   int  ,                --角色序号
	ActorName varchar(50)  primary key(ActorName,MenuName,UpMenuName) not null,--所属角色名称
	MenuName varchar(50)   not null,--菜单名称
	UpMenuName varchar(50),       --上级菜单名称
	Visiable   bit         --可见性 0 不可见 1 可见
)
go

select * from Actor where ActorName='测试员' order by upmenuname, actorsn
delete from Actor where ActorName='测试员' 




IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SoftWare]') AND type in (N'U'))
--DROP TABLE [dbo].SoftWare
go
create table SoftWare --软件列表
(
		SoftwareName varchar(50) primary key ,--软件名称
        SoftwareVersion varchar(50), --软件版本
        IcoName varchar(50), --图标文件名
        Size float ,--文件大小
        UpdateTime DateTime  ,--更新日期
        Remark varchar(200)  ,--软件说明 44
        Hot int ,--软件热度 42
        SoftwareFileName varchar(100),--运行文件名48
        VideoLink varchar(100),--视频教程链接 43
        SoftwareType varchar(10),--软件类别  Free/vip/ut c38
        Release bit              --是否发布 0 未发布 1 已发布 c3
)
go

select * from SoftWare

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OperationLog]') AND type in (N'U'))
--DROP TABLE [dbo].OperationLog
go
create table OperationLog --操作日志
(
		QQ varchar(14) primary key(qq,OperationTime) not null,--QQ号
		NickName varchar(30),       --昵称
        Actor varchar(50), --角色
        OperationTime DateTime  ,--操作时间
        IPAddr varchar(200),    --IP地址
        OperationName varchar(50), --操作名称
        OperationObject varchar(50), --操作对象
        OperationRows int, --数据行数
        OperationRemark varchar(500) --相关内容说明,sql语句
)
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VIPMemberShopDiagnosis]') AND type in (N'U'))
--DROP TABLE [dbo].VIPMemberShopDiagnosis
go
create table VIPMemberShopDiagnosis --VIP学员店铺诊断表
(
		QQ varchar(14) primary key(qq,DiagnosisDate) not null,--QQ号
		NickName varchar(50),       --昵称
		MemberBaseStat varchar(100),--学员基本情况
        ShopLink varchar(100),--店铺链接
        ShopLevel varchar(50),--店铺行业/等级
        ShopScore varchar(50),--动态评分
        ThirtyServerStat varchar(200),--最近30天内服务情况
        LatelyMonthRemarkCnt varchar(100),--最近一个月评价数量
        GuestCnt varchar(100),--店铺访客数
        MainCorePercent varchar(100),--主营占比
        ShopGoodsSourceStat varchar(200),--店铺货源情况
        ShopGoodsNumber varchar(100),--店铺宝贝数
        ShopOptimizeStat varchar(500),--店铺产品优化情况
        ShopProblem varchar(500),--店铺问题
        DiagnosisProgramme varchar(500), --诊断方案
        teacher varchar(20),--诊断老师
        DiagnosisDate datetime --诊断日期
)
go

 

 --获取服务器最大连接数
 select value from  master.dbo.sysconfigures where [config]=103
  --获取服务器日志
 exec xp_readerrorlog
 --获取服务器当前连接数
 select connectnum=count(distinct net_address)-1 from master..sysprocesses
 --获取服务器当前用户数
use master  select loginame,count(0) from sysprocesses  group by loginame  order by count(0) desc
select * from sysprocesses where dbid= db_id('utcrmdb')
 
 
