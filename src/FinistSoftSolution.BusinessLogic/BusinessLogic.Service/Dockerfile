#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FinistSoftSolution.BusinessLogic/BusinessLogic.Service/BusinessLogic.Service.csproj", "FinistSoftSolution.BusinessLogic/BusinessLogic.Service/"]
COPY ["FinistSoftSolution.BusinessLogic/BusinessLogic.Infrastructure/BusinessLogic.Infrastructure.csproj", "FinistSoftSolution.BusinessLogic/BusinessLogic.Infrastructure/"]
COPY ["FinistSoftSolution.BusinessLogic/BusinessLogic.Core/BusinessLogic.Core.csproj", "FinistSoftSolution.BusinessLogic/BusinessLogic.Core/"]
RUN dotnet restore "FinistSoftSolution.BusinessLogic/BusinessLogic.Service/BusinessLogic.Service.csproj"
COPY . .
WORKDIR "/src/FinistSoftSolution.BusinessLogic/BusinessLogic.Service"
RUN dotnet build "BusinessLogic.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BusinessLogic.Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BusinessLogic.Service.dll"]