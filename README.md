# Unity-个人简易框架脚本
* Version: 2.0
* 2021年12月16日16:28:13

## MyGUI
* 基于GUI封装的简易调试用UI控件

## 常用脚本
* 单例模式：继承Mono与普通类两种的单例模式基类
* 相机控制：实现相机多种操控方式
* 数组与子对象助手：方便数组计算，子对象处理
* 事件中心：集中处理事件逻辑
* Mono管理器：可以让非继承MonoBehavior的脚本完成相关需求
* 对象池：用于管理场景中的大量对象

## 数据持久化
* XML管理器
* PlayerPrefs管理器
* Json管理器（集合了用于Lua的LitJson插件）

## AB包工具
* AssetBundleManager：自动依赖包加载，从AB包中加载资源
* 编辑器Assets下的AB包一键打包
* LuaPacking：Lua脚本转txt打包

## Lua工具
* 内置了XLua插件（XLua版本：v2.1.15 2020年6月24日）
* LuaManager：集成了C#执行Lua脚本
* SplitTools：字符串分割工具
* 常用类别名与Object基类
