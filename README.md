# TShock调试小助手

## 介绍

自用的TShock调试小助手，会很不严谨！

输出消息包和hook的参数信息。只是获取调试信息，并不能够实时断点调试。

加载/重载 dll插件。


## 指令
```
/tdb, 调试开关
/tdb handler, handler调试开关（目前仅写了几条调试项）
/tdb debug, 临时打开tshock的debug模式
/tdb clearlog, 清空tshock日志文件
/tdb load <xx.dll>, 加载插件目录下的插件
/tdb rp <key>, 重载指定插件
/tdb rp list, 查询插件的key（来自配置文件）
/tdb reload, 重载配置文件
```


## 权限
```
tdb
```


## 消息包调试备注

参考：[https://tshock.readme.io/edit/multiplayer-packet-structure](https://tshock.readme.io/edit/multiplayer-packet-structure)

泰拉1.4.4.x时改动了一些参数，具体请用 ILSpy 反编译一份泰拉的源码查看

消息包的接收，参考：MessageBuffer.cs

消息包的发送，参考：NetMessage.cs

---

MsgID值

tshock项目：[https://github.com/Pryaxis/TSAPI/blob/general-devel/TerrariaServerAPI/TerrariaApi.Server/PacketTypes.cs](https://github.com/Pryaxis/TSAPI/blob/general-devel/TerrariaServerAPI/TerrariaApi.Server/PacketTypes.cs)

泰拉源码：Terraria.ID.MessageID.cs

---

## 插件重载备注
插件重载仅供调试用，重载后会导致执行`/help`（tshock自带指令）错误。由于tshock没有重启功能，很多插件没有写释放功能，因此插件的释放会不完全。