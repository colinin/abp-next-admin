# 基于ABP CLI的扩展工具集

提供快速创建模板项目、生成JavaScript库命令等更多功能。

## 开始使用

```shell
dotnet tool install --global LINGYUN.Abp.Cli
```

## 使用方法

```shell
使用方法:

    labp <command> <target> [options]

命令列表:

    > help: 显示命令行帮助。使用 ` labp help <command> ` 获取详细帮助
    > create: 基于自定义的ABP启动模板生成新的解决方案
    > generate-proxy: 生成客户端服务代理和DTO以消费HTTP API
    > generate-view: 从HTTP API代理生成视图代码

获取命令的详细帮助:

    labp help <command>
```

## 功能特性

* 支持生成TypeScript客户端代理代码
  - Axios HTTP客户端
  - Vben Admin集成
  - UniApp集成
* 支持生成Flutter客户端代理代码
  - Dio HTTP客户端
  - REST服务集成
* 支持生成视图代码
  - Vben Admin视图模板
  - Flutter GetX视图模板
* 自定义ABP启动模板

## 反馈

有问题需要反馈？

- [Github问题](https://github.com/colinin/abp-next-admin/issues)

[English](./README.md)
