IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UTSoftUserInfo]') AND type in (N'U'))
--DROP TABLE [dbo].UTSoftUserInfo
go
create table UTSoftUserInfo 
(
	QQ varchar(14) primary key not null,--QQ��
	NickName varchar(30),       --�ǳ�
	Grade varchar(20),--�ȼ�
	Teacher varchar(30),--��ʦ
	Manager varchar(30),--������
	ActorName varchar(30),--��ɫ����
	ActorSN int,--��ɫ���
	Join_time datetime,--����VIpʱ��
	login_time datetime,--����¼ʱ��
	UseNumber int ,--ʹ�ñ��������
	LoginStat int,--��¼״̬
	Name varchar(10),--����
	Age int,--����
	Sex varchar(2),--�Ա�
	Education varchar(10),--ѧ��
	EmploymentTime int ,-- ��ҵʱ�䣨�£�
	YearBalance decimal,--��Ӫҵ��
	category varchar(20),--������Ŀ
	ShopLink varchar(150),--��������
	ShopGrade varchar(10),--���̵ȼ�
	Telephone varchar(20),--�绰����
	HomeAddress varchar(150),--��ͥסַ
	Superiority varchar(2000),--ӵ�е���Դ������
	QunNumber varchar(20),--ѧϰȺ��
	QunCode varchar(200),--��Ⱥ����
	Introduce varchar(30),--������
	ComeFrom varchar(10),--��Դ���Դ����Ѷ��
	province varchar(30),--ʡ
	city varchar(30),--��
	area varchar(30),--��
	Remark varchar(200), --��ע
	CheckIDPhoto varchar(20),--ʵ����֤��Ƭ�ļ�����
	ShowPhoto varchar(20),--��Ƭ��Ƭ�ļ�����
	CheckStat varchar(2),--ʵ����֤״̬ 0 ��Ҫ��֤  1 Ҫ����֤ 2 ��֤ʧ�� 3 ��֤ͨ��
	station varchar(20),--��λ
	AuthorizeFlag varchar (2),--��Ȩ��־ 0 ������Ȩ��1 ������Ȩ
	machineCode varchar(100),--������
	AuthorizeCode varchar(200),--��Ȩ��
	AuthorizeDate datetime,--��Ȩ����
	AuthorizeCount int, --��Ȩ����
	First_LoginDate datetime,   --���ε�¼ʱ��
	Last_LoginIpAddr varchar(200), --����¼IP��ַ
	Passwd varchar(50) --����
)
go
select distinct ActorName from UTSoftUserInfo
--select * into   tmp_UTSoftUserInfo from UTSoftUserInfo
--drop table UTSoftUserInfo
--insert into UTSoftUserInfo select * from tmp_UTSoftUserInfo

select  a.qq, a.nickname,count(distinct b.qq )  as vipcnt, 
sum( case when c.ShouldTraceDate <getdate() then 1 else 0 end ) as shouldcnt,
sum( case when c.TraceDate='1900-1-1' then 0 else 1 end) as actcnt from 
( select  qq,nickname from UTSoftUserInfo where  ActorName ='������ʦ' or ActorName ='����鳤' ) a,
( select qq,nickname,teacher from UTSoftUserInfo where teacher<>'') b,
( select qq,nickname,ShouldTraceDate,TraceDate from TraceRecord ) c 
where b.teacher=a.nickname and c.qq=b.qq
group by a.qq,a.nickname


select 1 from TraceRecord  where TraceContent='' and TraceTimes like '%�ط�%' 
and ShouldTraceDate<=getdate() 
and qq in (select qq from UTSoftUserInfo where Manager = 'evil')


update UTSoftUserInfo set manager='evil',join_time='2016-05-28 16:08:27.033' where qq='2995597626'

select * from TraceRecord where qq in (select qq from UTSoftUserInfo where Manager ='evil')

select 1 from TraceRecord   where TraceContent='' and TraceTimes like '%�ط�%' and ShouldTraceDate<=getdate() 
and qq in (select qq from UTSoftUserInfo where Manager = 'evil') 

--drop table distinct_GroupMemberInfo    

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TraceRecord]') AND type in (N'U'))
--DROP TABLE [dbo].TraceRecord
go
create table TraceRecord 
(
	QQ varchar(14)   not null,--QQ��
	NickName varchar(30) ,       --�ǳ�
	TraceContent varchar(2000),--��������ժҪ
	TraceTimes varchar(10),--�������� 1 һ�� 2 ���� 3 һ�� 4 һ��
	TraceType varchar(10), --���ٷ�ʽ  QQ  �绰 .......
	ShouldTraceDate datetime, --Ӧ��������
	TraceDate datetime, --ʵ�ʸ�������
	CheckDate datetime, --�������
	Checker varchar(30) , --�����
	CheckFlag varchar(10) --����ˣ���Ч����Ч
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
create table ExchangeRecord --������¼
(
	LessonType varchar(14)  not null,--0 ��ʦ�Ƽ� 1 ���տγ�
	ReceiveNickName varchar(30),       --���ܷ��ǳ�
	ReMark varchar(200)  not null,--��ע
	SendNickName varchar(30),       --�����ǳ�
	ExchangeTime datetime, --����ʱ��,����ʱ��
	LessonName varchar(100),--�γ�����
	PlanTime datetime, --�γ����ʱ��
	ExchangeType varchar(10),-- ѧϰ�ƻ�
	Request varchar(500),--��������
	Answer varchar(500),--�ظ�����
	ReadFlag int --ReadFlagѧϰ�ƻ���0 δ�鿴�ʼ� 1 �Ѳ鿴�ʼ� 10 δ�鿴���� 11 �Ѳ鿴���� 20  δ�鿴�Ƽ� 21 �Ѳ鿴�Ƽ�
)
go



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ExchangeRecord]') AND type in (N'U'))
--DROP TABLE [dbo].StudentPass
go
create table StudentPass --ѧԱͨ�ز���
(
	QQ varchar(14)   not null,--QQ��
	NickName varchar(30) ,       --�ǳ�
	PassSn int primary key (QQ,PassSn),--�ؿ����
	PassName varchar(100),--�����������
	ReMark varchar(200)  not null,--������
	PassFileName varchar(100),--ѧ�������ļ�����
	TestTime datetime, --ѧ���ύʱ��
	TeacherName varchar(30),       --������ʦ����
	RePlayTime datetime, --����ʱ��
	PassStat  varchar(10)   not null ,--ͨ��״̬: δ���� δ�ύ  δ����  δͨ�� ��ͨ�� 
	ReadFlag int, --ReadFlag �鿴״̬��0 δ�鿴ѧԱ�Ծ� 1 �Ѳ鿴ѧԱ�Ծ� 10 δ�鿴��ʦ���Ľ�� 11 �Ѳ���ʦ���Ľ��
	Score int --����
)
go

delete StudentPass
select * from StudentPass  where qq='2881763444' and PassSn='1' 
select min(PassSn) from TeacherPass where publish='1'  and PassSn not in (select PassSn from StudentPass where qq='296830244')
select min(PassSn) from TeacherPass where publish='1'  and PassSn not in (select PassSn from StudentPass where qq='296830244') 

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ExchangeRecord]') AND type in (N'U'))
--DROP TABLE [dbo].TeacherPass
go
create table  TeacherPass --ͨ�ز��Թؿ�
(
	PassSn int primary key ,                 --�ؿ����
	PassName varchar(100),--��������
	ReMark varchar(200)  not null,--������
	PassFileName varchar(100),--�ļ�����
	TeacherName varchar(30) ,     --������ʦ
	publish bit --�Ƿ񷢲�
)
go

select  PassSn  from TeacherPass  where publish='1' order by PassSn
select *  from TeacherPass
select PassName,ReMark ,PassFileName from TeacherPass where PassSn='2'

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PassQuestion]') AND type in (N'U'))
--DROP TABLE [dbo].PassQuestion
go
create table  PassQuestion --ͨ�ز�����Ŀ
(
	PassSn int  ,                 --�ؿ����
	QuestionSn int primary key(PassSn,QuestionSn) ,  --��Ŀ���
	QuestionContent varchar(1000),--��������
	QuestionPic1 varchar(50),--���⸽ͼ1
	QuestionPic2 varchar(50),--���⸽ͼ2
	QuestionPic3 varchar(50),--���⸽ͼ3
	TotalScore int,--�������ֵ÷�
)
go

select * from PassQuestion
select  * ,'','','','','',''  from PassQuestion where PassSn='3'  and QuestionSn='1' 

select '3',QuestionSn,QuestionContent,QuestionPic1,QuestionPic2,QuestionPic3,TotalScore,'2881763444','','','','','0'  
from PassQuestion where PassSn='3'  order by QuestionSn

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PassAnswer]') AND type in (N'U'))
--DROP TABLE [dbo].PassAnswer
go
create table  PassAnswer --ѧ��ͨ�ز��Դ�
(
	PassSn int  ,                 --�ؿ����
	QuestionSn int primary key(qq,PassSn,QuestionSn) ,  --��Ŀ���
	QuestionContent varchar(500),--��������
	QuestionPic1 varchar(50),--���⸽ͼ1
	QuestionPic2 varchar(50),--���⸽ͼ2
	QuestionPic3 varchar(50),--���⸽ͼ3
	TotalScore int,--�������ֵ÷�
	
	QQ varchar(14)   not null,--QQ��
	AnswerContent varchar(500),--������
	AnswerPic1 varchar(50),--�𰸸�ͼ1
	AnswerPic2 varchar(50),--�𰸸�ͼ2
	AnswerPic3 varchar(50),--�𰸸�ͼ3
	Score int,--ʵ�ʵ÷�
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
	LessonName varchar(100) primary key not null,--�γ�����
	LessonTime datetime, --�Ͽο�ʼ����ʱ��
	LessonLink varchar(200),--�������ӵ�ַ
	DownloadLink varchar(200),--���ص�ַ
	DownloadPassword varchar(20),--��������
	Teacher varchar(20), --��ʦ
	Remark varchar(300),      --�γ̼��
	LessonType varchar(10), --�γ���� ���� VIP ���� ʵ��
	ClassType varchar (20)  --�༶��� ��Ӣ ���� ����������
)
go



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VIPExchange]') AND type in (N'U'))
--DROP TABLE [dbo].VIPExchange
go
create table VIPExchange
(
	QQ1 varchar(14)  not null,--����QQ��
	NickName1 varchar(30),       --�����ǳ�
	QQ2 varchar(14)  not null,--����QQ��
	NickName2 varchar(30),       --�����ǳ�
	stat varchar(2),            --״̬ 0 ������ 1 ͨ����Ϊ���� 2�ܾ���Ϊ����δ�鿴 3 �ѳ�Ϊ����δ�鿴
	Remark varchar (100)--�ǳ�    --��������
)
go


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Actor]') AND type in (N'U'))
--DROP TABLE [dbo].Actor
go
create table Actor --�˵��ڵ�
(
	Actorsn   int  ,                --��ɫ���
	ActorName varchar(50)  primary key(ActorName,MenuName,UpMenuName) not null,--������ɫ����
	MenuName varchar(50)   not null,--�˵�����
	UpMenuName varchar(50),       --�ϼ��˵�����
	Visiable   bit         --�ɼ��� 0 ���ɼ� 1 �ɼ�
)
go

select * from Actor where ActorName='����Ա' order by upmenuname, actorsn
delete from Actor where ActorName='����Ա' 




IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SoftWare]') AND type in (N'U'))
--DROP TABLE [dbo].SoftWare
go
create table SoftWare --����б�
(
		SoftwareName varchar(50) primary key ,--�������
        SoftwareVersion varchar(50), --����汾
        IcoName varchar(50), --ͼ���ļ���
        Size float ,--�ļ���С
        UpdateTime DateTime  ,--��������
        Remark varchar(200)  ,--���˵�� 44
        Hot int ,--����ȶ� 42
        SoftwareFileName varchar(100),--�����ļ���48
        VideoLink varchar(100),--��Ƶ�̳����� 43
        SoftwareType varchar(10),--������  Free/vip/ut c38
        Release bit              --�Ƿ񷢲� 0 δ���� 1 �ѷ��� c3
)
go

select * from SoftWare

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OperationLog]') AND type in (N'U'))
--DROP TABLE [dbo].OperationLog
go
create table OperationLog --������־
(
		QQ varchar(14) primary key(qq,OperationTime) not null,--QQ��
		NickName varchar(30),       --�ǳ�
        Actor varchar(50), --��ɫ
        OperationTime DateTime  ,--����ʱ��
        IPAddr varchar(200),    --IP��ַ
        OperationName varchar(50), --��������
        OperationObject varchar(50), --��������
        OperationRows int, --��������
        OperationRemark varchar(500) --�������˵��,sql���
)
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VIPMemberShopDiagnosis]') AND type in (N'U'))
--DROP TABLE [dbo].VIPMemberShopDiagnosis
go
create table VIPMemberShopDiagnosis --VIPѧԱ������ϱ�
(
		QQ varchar(14) primary key(qq,DiagnosisDate) not null,--QQ��
		NickName varchar(50),       --�ǳ�
		MemberBaseStat varchar(100),--ѧԱ�������
        ShopLink varchar(100),--��������
        ShopLevel varchar(50),--������ҵ/�ȼ�
        ShopScore varchar(50),--��̬����
        ThirtyServerStat varchar(200),--���30���ڷ������
        LatelyMonthRemarkCnt varchar(100),--���һ������������
        GuestCnt varchar(100),--���̷ÿ���
        MainCorePercent varchar(100),--��Ӫռ��
        ShopGoodsSourceStat varchar(200),--���̻�Դ���
        ShopGoodsNumber varchar(100),--���̱�����
        ShopOptimizeStat varchar(500),--���̲�Ʒ�Ż����
        ShopProblem varchar(500),--��������
        DiagnosisProgramme varchar(500), --��Ϸ���
        teacher varchar(20),--�����ʦ
        DiagnosisDate datetime --�������
)
go

 

 --��ȡ���������������
 select value from  master.dbo.sysconfigures where [config]=103
  --��ȡ��������־
 exec xp_readerrorlog
 --��ȡ��������ǰ������
 select connectnum=count(distinct net_address)-1 from master..sysprocesses
 --��ȡ��������ǰ�û���
use master  select loginame,count(0) from sysprocesses  group by loginame  order by count(0) desc
select * from sysprocesses where dbid= db_id('utcrmdb')
 
 
