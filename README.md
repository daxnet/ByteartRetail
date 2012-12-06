Byteart Retail项目简介
======================
从2007年至今，我一直关注着与领域驱动设计相关的软件开发过程与技术，在这几年中，我坚持不懈地学习、实践，在总结自己实践经验的基础上，设计并开发了一套基于.NET的面向领域驱动的企业应用程序开发框架，沿用我以前开发的一个松耦合架构实验原型，为之取名为Apworks。为了向社区展示Apworks在企业级应用开发上给开发人员带来的便捷，我也针对Apworks框架开发了一套面向CQRS架构的案例程序：Tiny Library CQRS。随着Apworks的不断发展，Tiny Library CQRS也先后更新了两个版本，毋庸置疑，第二版更为成熟，更贴近于实际项目应用。

然而，社区对Tiny Library CQRS的反馈却不是那么积极，分析原因，我觉得有三个方面：首先，基于事件溯源（Event Sourcing）机制的CQRS架构本身就非常复杂，套用世界级软件架构大师Udi Dahan对CQRS架构的总结，就是：“简单，但不容易（Simple, but not easy）”，要让一个对企业级软件架构设计不太熟悉的开发人员掌握好CQRS相关的知识，是一件困难的事情，即使有现成的案例，也会让人感觉无从下手；其次，目前大多数应用程序还远没达到需要使用CQRS架构的规模，在项目中应用CQRS，只能把简单问题复杂化；再次，由于个人时间能力有限，Tiny Library CQRS案例本身也没有提供太多的文档与说明，加上该案例直接使用了Apworks框架，所以很多后台运行机制就变得不那么明朗，这对希望研究CQRS架构的开发人员造成了一定的困难。因此，Tiny Library CQRS感觉就与真实项目的实践脱节，自然关注的人就不多了。所以我打算暂时搁置Tiny Library CQRS的更新，让其也成为一个CQRS架构设计的参考原型，供有兴趣的朋友参观学习。

于是，从今年4月开始，我就着手开发并发展了另一个面向领域驱动的.NET企业级应用架构设计案例：Byteart Retail。与Tiny Library CQRS不同的是，Byteart Retail采用了面向领域驱动的经典分层架构，并且为了展示微软.NET技术在企业级应用开发中的应用，它所使用的第三方组件也几乎都是微软提供的：Entity Framework、ASP.NET MVC、Unity IoC、Unity AOP、Enterprise Library Caching等（用于记录日志的log4net除外，但log4net本身也是众所周知的框架），所以，开发人员只需要打开Byteart Retail的源程序，就能够很清楚地看到系统的各个组件是如何组织在一起并协同工作的。经典分层架构的采用，也为实际项目带来了参考和指导的价值。

![Byteart Retail Version 3](http://images.cnblogs.com/cnblogs_com/daxnet/201211/201211081523197376.png)

Byteart Retail所使用的技术
==========================
Byteart Retail项目使用或涵盖了以下Microsoft技术：
- Microsoft Entity Framework 5 Code First（包括Repository模式的实现、枚举类型的支持以及分页功能的实现）
- ASP.NET MVC 3/4
- WCF
- Microsoft Patterns & Practices Unity Application Block
- Microsoft Patterns & Practices Unity Policy Injection Extension
- Microsoft Patterns & Practices Caching Application Block
- 使用AutoMapper实现DTO与领域对象映射
- T4自动化代码生成
- 基于Unity的AOP拦截
- 使用log4net记录拦截的Exception详细信息

运行Byteart Retail案例
======================
先决条件
--------
从V3开始，本案例使用Visual Studio 2012开发，因此，要编译本案例的源代码程序，则需要首先安装Visual Studio 2012。由于数据库采用了SQL Server Express LocalDB，因此，这部分组件也需要正确安装（如果是选择完整安装Visual Studio 2012，则可以忽略LocalDB的安装）。此外，无需安装其它组件。

编译运行
--------
将下载的ByteartRetail_V3.zip文件解压到一个本地的磁盘目录下，然后在Microsoft Visual Studio 2012中打开ByteartRetail.sln文件，再将ByteartRetail.Web项目设置为启动项目后，直接按F5（或者Debug –> Start Debugging菜单项）运行本案例即可。注意：
1. 如果不打算以Debug的方式启动本案例，那就需要首先展开ByteartRetail.Services项目，任选其中一个.svc的服务文件（比如UserService.svc）然后点击右键选择View In Browser菜单项，以便启动服务端的ASP.NET Development Server；最后再直接启动ByteartRetail.Web项目
2. 由于Byteart Retail V3的数据库采用的是SQL Server 2012 Express LocalDB（默认实例），在程序连接LocalDB数据库时，LocalDB需要创建/初始化数据库实例，因此在首次启动时有可能会出现数据库连接超时的异常，如果碰到这类问题，则请稍等片刻然后再重试
3. 如果以上述第一点的方式运行ByteartRetail.Web项目并出现与WCF绑定相关的错误时，这表示WCF服务并没有完全启动，请重新启动ByteartRetail.Services项目，然后再启动ByteartRetail.Web项目

登录账户
--------
启动成功后，就可以单击页面右上角的“登录”链接进行账户登录。默认的登录账户有（用户名/密码）：
- admin/admin：以管理员角色登录，可以对站点进行管理
- sales/sales：以销售人员角色登录，可以查看系统中订单信息并进行发货等操作
- buyer/buyer：以采购人员角色登录，可以管理商品分类和商品信息
- daxnet/daxnet：普通用户角色，不能对系统进行任何管理操作

解决方案结构
------------
ByteartRetail.sln包含以下项目：
- ByteartRetail.Design：包含一些设计相关的图画文件，仅供参考，没有实际意义
- ByteartRetail.Application：应用层
- ByteartRetail.DataObjects：数据传输对象及其类型扩展
- ByteartRetail.Domain：领域层
- ByteartRetail.Domain.Repositories：仓储的具体实现（目前是基于Entity Framework 5.0的实现）
- ByteartRetail.Infrastructure：基础结构层
- ByteartRetail.Infrastructure.Caching：位于基础结构层的缓存实现
- ByteartRetail.ServiceContracts：基于WCF的服务契约
- ByteartRetail.Services：WCF服务
- ByteartRetail.Web：基于ASP.NET MVC的站点程序（表示层）

以下是各项目之间的依赖关系：
![Byteart Retail Version 3项目间依赖关系](http://images.cnblogs.com/cnblogs_com/daxnet/201211/20121108152334979.png)

总结
====
热烈欢迎爱好Microsoft.NET技术以及领域驱动设计的读者朋友对本案例进行深入讨论。