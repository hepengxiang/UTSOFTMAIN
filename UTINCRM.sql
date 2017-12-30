create table UTStaffInfo
(
	Name  varchar(10) primary key not null,--����
	Grade varchar(20),--��λ����
	
	Sex varchar(2),--�Ա�
	BirthDay datetime ,-- ����
	Nation varchar(10),--����
	Education varchar(10),--ѧ��
	HalfBodyPhoto varchar(30),       --������
	IDphoto1 varchar(30),       --���֤����
	IDphoto2 varchar(30),       --���֤����
	Married varchar(2),--���
	HomeAddress varchar(150),--��ͥסַ
	QQ varchar(14) not null,--QQ��
	Telephone varchar(20),--�绰����
	WeChat varchar(20),--΢�ź�
	HomeTelephone varchar(20),--��ͥ���õ绰
	Resumes varchar(500),--��������
	Favor varchar(100), --����
	EmploymentDate datetime ,-- ��ְ����
	gularizationDate datetime ,-- ת������
	Internship int ,-- ʵϰʱ�����£�
	ContractInfo varchar(20),--��ͬ��Ϣ
	ReMark varchar(200),     --��ע
	AssumedName varchar(20), --����
	onjob bit                --1 ��ְ 0 ��ְ
)
go
--drop table UTStaffInfo

select *  from UTStaffInfo where qq='392946638'
select *  from UTStaffInfo where AssumedName ='����'
select * from UTStaffInfo where AssumedName='����' and qq<>'296830244' 

select distinct ProjectName from UTPerformance where Submitter='����' or Submitter='�ƹ㲿QQ����Ա' 
or CHARINDEX(ProjectName, (select PerformanceStr from UTGradeInfo  where Grade=  (select grade from UTStaffInfo where AssumedName ='����')) )>0 and Submitter='����' 

create table UTGradeInfo
(
	Grade varchar(20) primary key not null,--ְλ����
	Department varchar(20),--��������
	UpGrade varchar(20),--�ϼ��������
	PerformanceStr varchar(300)--��Ч��������
)
select * from UTGradeInfo
--update UTGradeInfo set PerformanceStr=''
--drop table UTGradeInfo
select name,grade from UTStaffInfo

select PerformanceStr from UTGradeInfo  where Grade=  (select grade from UTStaffInfo where AssumedName ='evil')
select distinct ProjectName from UTPerformance where    Submitter='����' 
or (Submitter='' and ProjectName in ())

select distinct ProjectName from UTPerformance where Submitter='����' or
CHARINDEX(ProjectName, (select PerformanceStr from UTGradeInfo  where Grade=  (select grade from UTStaffInfo where AssumedName ='����')) )>0
and Submitter=''



create table UTPerformance--��Ч���˹����
(
	ProjectName varchar(100)  not null,--��Ч����������
	ValueType varchar(20) ,--���������� ����ֵ������������ֵ��ƽ��ֵ
	Cycle varchar(2),--����
	ReferProject varchar(100),--���û���
	Formula varchar(1000),--���㹫ʽ
	ScorePrice decimal(18,2),  --������ֵ��ÿ��ֵ��������ң�
	Submitter varchar(50),--�ύ��
	Checker varchar(50),--�����
	Accepter  varchar(50) ,--��Ч��
	Remark varchar(200)--��ע
)
--drop table UTPerformance
select * from UTPerformance


create table PerformanceCheck --��Ч��
(
	ProjectName varchar(100)  not null,--��Ч������
	QQ varchar(20) primary key(ProjectName,AcceptName,OccurDate) not null,--��Ч��QQ
	AcceptName  varchar(50)  not null,--��Ч��
	CheckValue decimal,--��Чֵ
	OccurDate datetime ,--�ύ����
	AddFile varchar(30),--�����ļ�
	Submitter varchar(20),--�ύ��
	Checker varchar(20),--�����
	CheckStat varchar(10),  --��˽��(����� ��ͨ�� δͨ��)
	Remark varchar(200), --��ע
	SettleFlag bit --�����־ 1 �ѽ��� 0 δ����
)
--drop table PerformanceCheck
select *  from PerformanceCheck where ProjectName='QQ�鱨������һ������' order by occurdate

select *  from PerformanceCheck where ProjectName='QQ�鱨������' order by occurdate
--delete  from PerformanceCheck where ProjectName='QQ�鱨������һ������'
--delete  from PerformanceCheck where ProjectName='QQ�鱨������һ������'
--delete  from PerformanceCheck where checkvalue=0
--update PerformanceCheck set OccurDate='2016-06-23 19:32:35.337'  where ProjectName='QQ�鱨������' and OccurDate='2016-07-02 14:56:59.013'

select * from 
(select AcceptName,sum(CheckValue) as totalvalue ,row_number() over(order by sum(CheckValue) desc) as pm from PerformanceCheck 
where ProjectName ='QQ�鱨������' and OccurDate >='2016/6/27 0:00:00'  and OccurDate<'2016/7/4 0:00:00' group by AcceptName  having sum(CheckValue)>0 ) T 
where AcceptName='evil' 

select   max(OccurDate)  from PerformanceCheck where ProjectName='QQ�鱨������һ������' and AcceptName='evil' and CheckStat='��ͨ��'  
and OccurDate >='2016/6/1 0:00:00'  and OccurDate<'2016/8/1 0:00:00' 

select * from PerformanceCheck where ProjectName='QQ�鱨������һ������'
--delete from PerformanceCheck where ProjectName='QQ�鱨������һ������'

create table ChangeStationRecord --��λ�����¼
(
	Name  varchar(10)  not null,--����
	Grade varchar(20),--��λ����
	ChangeStationDate datetime ,-- ��������
	Remark varchar(200)
)



create table UTReport
(
	ReportType varchar(20),--��������
	ReportName varchar(20),--������������
	Content varchar(500),--����
	AddFile1 varchar(30),--����1
	AddFile2 varchar(30),--����2
	AddFile3 varchar(30),--����3
)

create table UTReportType
(
	ReportType varchar(20)--��������
)


select * from Advice  where  advicetype='�ܾ�������'  order by TraceDate
select top 10 * from OperationLog 