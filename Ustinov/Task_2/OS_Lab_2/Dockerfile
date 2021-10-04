FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["OS_Lab_2/OS_Lab_2.csproj", "OS_Lab_2/"]
RUN dotnet restore "OS_Lab_2/OS_Lab_2.csproj"
COPY . .
WORKDIR "/src/OS_Lab_2"
RUN dotnet build "OS_Lab_2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OS_Lab_2.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OS_Lab_2.dll"]
