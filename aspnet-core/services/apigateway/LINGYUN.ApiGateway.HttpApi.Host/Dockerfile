FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
MAINTAINER Colin
WORKDIR /api

COPY . /api

ENV TZ=Asia/Shanghai
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo '$TZ' > /etc/timezone

EXPOSE 80/tcp

ENTRYPOINT ["dotnet", "LINGYUN.ApiGateway.HttpApi.Host.dll"]