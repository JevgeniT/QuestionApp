FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV ASPNETCORE_ENVIRONMENT=Development
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["QuestionApp/QuestionApp.csproj", "QuestionApp/"]

RUN dotnet restore "QuestionApp/QuestionApp.csproj"
COPY . .
WORKDIR "/src/QuestionApp"
RUN dotnet build "QuestionApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "QuestionApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuestionApp.dll"]
