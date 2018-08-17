1.继承接口 ISTcpActor 比如：SActor
2.用SActor创建server,并调用server.Start  比如：var server = new TcpServer<SActor>(26001);server.Start();
3.客户端新的连接会触发 ISTcpActor.Initialize 断开连接触发UnInitialize
4.ISTcpActor 是多线程驱动的，同一个ISTcpActor永远被某一个线程驱动
5.TcpServer.OnlyOneThread = true 启用单线程驱动，单必须在new TcpServer之前调用