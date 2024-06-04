FROM mcr.microsoft.com/dotnet/aspnet:8.0
LABEL maintainer="colin.in@foxmail.com"
WORKDIR /app

COPY . /app

#东8区
ENV TZ=Asia/Shanghai
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo '$TZ' > /etc/timezone

EXPOSE 80/tcp
VOLUME [ "./app/Logs" ]
VOLUME [ "./app/Modules" ]

RUN apt update
RUN apt install wget -y

ENTRYPOINT ["dotnet", "LY.MicroService.Applications.Single.dll"]
