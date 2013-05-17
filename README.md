Byteart Retail项目简介
======================
从2007年至今，我一直关注着与领域驱动设计相关的软件开发过程与技术，在这几年中，我坚持不懈地学习、实践，在总结自己实践经验的基础上，设计并开发了一套基于.NET的面向领域驱动的企业应用程序开发框架，沿用我以前开发的一个松耦合架构实验原型，为之取名为Apworks。为了向社区展示Apworks在企业级应用开发上给开发人员带来的便捷，我也针对Apworks框架开发了一套面向CQRS架构的案例程序：Tiny Library CQRS。随着Apworks的不断发展，Tiny Library CQRS也先后更新了两个版本，毋庸置疑，第二版更为成熟，更贴近于实际项目应用。

然而，社区对Tiny Library CQRS的反馈却不是那么积极，分析原因，我觉得有三个方面：首先，基于事件溯源（Event Sourcing）机制的CQRS架构本身就非常复杂，套用世界级软件架构大师Udi Dahan对CQRS架构的总结，就是：“简单，但不容易（Simple, but not easy）”，要让一个对企业级软件架构设计不太熟悉的开发人员掌握好CQRS相关的知识，是一件困难的事情，即使有现成的案例，也会让人感觉无从下手；其次，目前大多数应用程序还远没达到需要使用CQRS架构的规模，在项目中应用CQRS，只能把简单问题复杂化；再次，由于个人时间能力有限，Tiny Library CQRS案例本身也没有提供太多的文档与说明，加上该案例直接使用了Apworks框架，所以很多后台运行机制就变得不那么明朗，这对希望研究CQRS架构的开发人员造成了一定的困难。因此，Tiny Library CQRS感觉就与真实项目的实践脱节，自然关注的人就不多了。所以我打算暂时搁置Tiny Library CQRS的更新，让其也成为一个CQRS架构设计的参考原型，供有兴趣的朋友参观学习。

于是，从今年4月开始，我就着手开发并发展了另一个面向领域驱动的.NET企业级应用架构设计案例：Byteart Retail。与Tiny Library CQRS不同的是，Byteart Retail采用了面向领域驱动的经典分层架构，并且为了展示微软.NET技术在企业级应用开发中的应用，它所使用的第三方组件也几乎都是微软提供的：Entity Framework、ASP.NET MVC、Unity IoC、Unity AOP、Enterprise Library Caching等（用于记录日志的log4net除外，但log4net本身也是众所周知的框架），所以，开发人员只需要打开Byteart Retail的源程序，就能够很清楚地看到系统的各个组件是如何组织在一起并协同工作的。经典分层架构的采用，也为实际项目带来了参考和指导的价值。

![Byteart Retail Version 3](http://images.cnblogs.com/cnblogs_com/daxnet/201211/201211081523197376.png)

Byteart Retail演示体验
============
目前，Byteart Retail已被部署到了Windows Azure，请通过[http://byteartretail.cloudapp.net](http://byteartretail.cloudapp.net "Byteart Retail")访问演示站点。

**郑重声明：在体验的过程中，如需注册新用户，请确保不要用自己的常用密码进行注册，因为为了简化演示和平时的开发调试，Byteart Retail没有对密码进行任何加密处理（数据库明文存储），如果使用自己的常用密码，势必会带来一定的安全隐患。由此导致的密码泄露我本人可不负责哦！另外，我偷懒了，部署的时候直接用的SqlLocalDB，而不是SQL Express。众所周知SqlLocalDB是一个开发版，因此在体验的过程中万一遇到了数据库挂掉的错误，还恳请大家多多包涵！**


Byteart Retail所使用的技术
==========================
Byteart Retail项目使用或涵盖了以下Microsoft技术：
- Microsoft Entity Framework 5 Code First（包括Repository模式的实现、枚举类型的支持以及分页功能的实现）
- ASP.NET MVC 4
- WCF
- Microsoft Patterns & Practices Unity Application Block
- Microsoft Patterns & Practices Unity Policy Injection Extension
- Microsoft Patterns & Practices Caching Application Block
- Microsoft Appfabric Caching
- 使用AutoMapper实现DTO与领域对象映射
- T4自动化代码生成
- 基于Unity的AOP拦截
- 使用log4net记录拦截的Exception详细信息

Byteart Retail所演示的模式和设计思想
==================================
Byteart Retail项目演示或涵盖了以下模式和设计思想：
- 实体、值对象、领域服务
- 规约、仓储、仓储上下文
- 领域事件、事件聚合器、事件总线
- 事务协调器
- 服务定位器模式、工作单元模式、分离接口模式、数据传输对象模式、层超类型模式、传输对象组装器模式


运行Byteart Retail案例
======================
先决条件
--------
从V3开始，本案例使用Visual Studio 2012开发，因此，要编译本案例的源代码程序，则需要首先安装Visual Studio 2012。由于数据库采用了SQL Server Express LocalDB，因此，这部分组件也需要正确安装（如果是选择完整安装Visual Studio 2012，则可以忽略LocalDB的安装）。
另外，Byteart Retail提供了两种事件总线（Event Bus）的实现：一种是面向事件聚合器（Event Aggregator）的，它将把所获得的事件通过聚合器派发到一个或多个事件处理器上；另一种是面向微软MSMQ的，它将把所获得的事件直接派发到MSMQ队列中，如果采用这种事件总线，则需要在机器上安装和配置MSMQ组件，并确保新建的队列是事务型队列。
此外，无需安装其它组件。

编译运行
--------
克隆源代码资源库，或者直接下载zip压缩包，然后在Microsoft Visual Studio 2012中打开ByteartRetail.sln文件，再将ByteartRetail.Web项目设置为启动项目后，直接按F5（或者Debug –> Start Debugging菜单项）运行本案例即可。注意：
- 如果不打算以Debug的方式启动本案例，那就需要首先展开ByteartRetail.Services项目，任选其中一个.svc的服务文件（比如UserService.svc）然后点击右键选择View In Browser菜单项，以便启动服务端的ASP.NET Development Server；最后再直接启动ByteartRetail.Web项目
- 由于Byteart Retail V3的数据库采用的是SQL Server 2012 Express LocalDB（默认实例），在程序连接LocalDB数据库时，LocalDB需要创建/初始化数据库实例，因此在首次启动时有可能会出现数据库连接超时的异常，如果碰到这类问题，则请稍等片刻然后再重试
- 如果以上述第一点的方式运行ByteartRetail.Web项目并出现与WCF绑定相关的错误时，这表示WCF服务并没有完全启动，请重新启动ByteartRetail.Services项目，然后再启动ByteartRetail.Web项目

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
- ByteartRetail.Events：事件相关的事件处理器、事件总线和事件聚合器的定义
- ByteartRetail.Events.Handlers：具体的事件处理器定义
- ByteartRetail.Infrastructure：基础结构层
- ByteartRetail.Infrastructure.Caching：位于基础结构层的缓存实现
- ByteartRetail.ServiceContracts：基于WCF的服务契约
- ByteartRetail.Services：WCF服务
- ByteartRetail.Web：基于ASP.NET MVC的站点程序（表示层） 

以下是各项目之间的依赖关系：
![Byteart Retail Version 3项目间依赖关系](https://github.com/daxnet/ByteartRetail/blob/master/Documents/ByteartRetailAssemblyDependency.png?raw=true)

总结
====
热烈欢迎爱好Microsoft.NET技术以及领域驱动设计的读者朋友对本案例进行深入讨论。同时也欢迎访问我的.NET/DDD架构经验分享交流网站：[http://apworks.org](http://apworks.org "Apworks.ORG")