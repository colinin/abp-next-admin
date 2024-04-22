**快速启动后端项目：**
1. 使用 `00.auto-config-docker.cmd` 自动配置docker环境
2. 使用 `01.migrate-db.cmd` 迁移数据库
3. 使用 `80.start-host.cmd` 启动后端项目

注：请按自己电脑运行速度调整 `80.start-host.cmd` 文件中的 `stime` 参数。

**快速启动前端项目：**
1. 使用 `91.install-node-module.cmd` 安装npm依赖
2. 使用 `99.start-all.cmd` 启动项目
