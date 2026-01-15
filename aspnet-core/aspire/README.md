## 先决条件

请参考 [Aspire Docs](https://aspire.dev/get-started/prerequisites/) 检查运行所需环境.  

## 安装CLI

请参考 [Aspire Docs](https://aspire.dev/get-started/install-cli/) 安装Aspire CLI.  

## 启动Aspire

```shell
aspire run
```

## 注意事项

- 仅用于学习 [Aspire](https://aspire.dev/docs/) 请勿用于生产环境!  
- 项目默认启用 [Elasticsearch](https://aspire.dev/integrations/databases/elasticsearch/) 作为审计日志及系统运行日志存储, 请视本机情况酌情修改启动参数,避免OOM!  
- 多个项目在启动时初始化Quartz, 就算做了健康检查, 某些情况下还是会造成数据库锁争用, 在仪表盘中重启对应服务即可.  