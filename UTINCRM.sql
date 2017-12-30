create table UTStaffInfo
(
	Name  varchar(10) primary key not null,--姓名
	Grade varchar(20),--岗位名称
	
	Sex varchar(2),--性别
	BirthDay datetime ,-- 生日
	Nation varchar(10),--民族
	Education varchar(10),--学历
	HalfBodyPhoto varchar(30),       --半身照
	IDphoto1 varchar(30),       --身份证正面
	IDphoto2 varchar(30),       --身份证反面
	Married varchar(2),--婚否
	HomeAddress varchar(150),--家庭住址
	QQ varchar(14) not null,--QQ号
	Telephone varchar(20),--电话号码
	WeChat varchar(20),--微信号
	HomeTelephone varchar(20),--家庭备用电话
	Resumes varchar(500),--个人履历
	Favor varchar(100), --爱好
	EmploymentDate datetime ,-- 入职日期
	gularizationDate datetime ,-- 转正日期
	Internship int ,-- 实习时长（月）
	ContractInfo varchar(20),--合同信息
	ReMark varchar(200),     --备注
	AssumedName varchar(20), --化名
	onjob bit                --1 在职 0 离职
)
go
--drop table UTStaffInfo

select *  from UTStaffInfo where qq='392946638'
select *  from UTStaffInfo where AssumedName ='刘工'
select * from UTStaffInfo where AssumedName='刘工' and qq<>'296830244' 

select distinct ProjectName from UTPerformance where Submitter='刘工' or Submitter='推广部QQ组组员' 
or CHARINDEX(ProjectName, (select PerformanceStr from UTGradeInfo  where Grade=  (select grade from UTStaffInfo where AssumedName ='刘工')) )>0 and Submitter='本人' 

create table UTGradeInfo
(
	Grade varchar(20) primary key not null,--职位名称
	Department varchar(20),--所属部门
	UpGrade varchar(20),--上级主管身份
	PerformanceStr varchar(300)--绩效考核内容
)
select * from UTGradeInfo
--update UTGradeInfo set PerformanceStr=''
--drop table UTGradeInfo
select name,grade from UTStaffInfo

select PerformanceStr from UTGradeInfo  where Grade=  (select grade from UTStaffInfo where AssumedName ='evil')
select distinct ProjectName from UTPerformance where    Submitter='刘工' 
or (Submitter='' and ProjectName in ())

select distinct ProjectName from UTPerformance where Submitter='刘工' or
CHARINDEX(ProjectName, (select PerformanceStr from UTGradeInfo  where Grade=  (select grade from UTStaffInfo where AssumedName ='刘工')) )>0
and Submitter=''



create table UTPerformance--绩效考核规则表
(
	ProjectName varchar(100)  not null,--绩效考核项名称
	ValueType varchar(20) ,--考核项类型 输入值，排名，汇总值，平均值
	Cycle varchar(2),--周期
	ReferProject varchar(100),--引用基数
	Formula varchar(1000),--计算公式
	ScorePrice decimal(18,2),  --分数价值（每分值多少人民币）
	Submitter varchar(50),--提交人
	Checker varchar(50),--审核人
	Accepter  varchar(50) ,--绩效人
	Remark varchar(200)--备注
)
--drop table UTPerformance
select * from UTPerformance


create table PerformanceCheck --绩效单
(
	ProjectName varchar(100)  not null,--绩效项名称
	QQ varchar(20) primary key(ProjectName,AcceptName,OccurDate) not null,--绩效人QQ
	AcceptName  varchar(50)  not null,--绩效人
	CheckValue decimal,--绩效值
	OccurDate datetime ,--提交日期
	AddFile varchar(30),--附加文件
	Submitter varchar(20),--提交人
	Checker varchar(20),--审核人
	CheckStat varchar(10),  --审核结果(待审核 已通过 未通过)
	Remark varchar(200), --备注
	SettleFlag bit --结算标志 1 已结算 0 未结算
)
--drop table PerformanceCheck
select *  from PerformanceCheck where ProjectName='QQ组报名人数一周排名' order by occurdate

select *  from PerformanceCheck where ProjectName='QQ组报名人数' order by occurdate
--delete  from PerformanceCheck where ProjectName='QQ组报名人数一月排名'
--delete  from PerformanceCheck where ProjectName='QQ组报名人数一周排名'
--delete  from PerformanceCheck where checkvalue=0
--update PerformanceCheck set OccurDate='2016-06-23 19:32:35.337'  where ProjectName='QQ组报名人数' and OccurDate='2016-07-02 14:56:59.013'

select * from 
(select AcceptName,sum(CheckValue) as totalvalue ,row_number() over(order by sum(CheckValue) desc) as pm from PerformanceCheck 
where ProjectName ='QQ组报名人数' and OccurDate >='2016/6/27 0:00:00'  and OccurDate<'2016/7/4 0:00:00' group by AcceptName  having sum(CheckValue)>0 ) T 
where AcceptName='evil' 

select   max(OccurDate)  from PerformanceCheck where ProjectName='QQ组报名人数一周排名' and AcceptName='evil' and CheckStat='已通过'  
and OccurDate >='2016/6/1 0:00:00'  and OccurDate<'2016/8/1 0:00:00' 

select * from PerformanceCheck where ProjectName='QQ组报名人数一周排名'
--delete from PerformanceCheck where ProjectName='QQ组报名人数一周排名'

create table ChangeStationRecord --岗位变更记录
(
	Name  varchar(10)  not null,--姓名
	Grade varchar(20),--岗位名称
	ChangeStationDate datetime ,-- 换岗日期
	Remark varchar(200)
)



create table UTReport
(
	ReportType varchar(20),--报表类型
	ReportName varchar(20),--报表主题名称
	Content varchar(500),--内容
	AddFile1 varchar(30),--附件1
	AddFile2 varchar(30),--附件2
	AddFile3 varchar(30),--附件3
)

create table UTReportType
(
	ReportType varchar(20)--报表类型
)


select * from Advice  where  advicetype='总经理信箱'  order by TraceDate
select top 10 * from OperationLog 