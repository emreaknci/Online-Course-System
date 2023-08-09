FROM mcr.microsoft.com/dotnet/sdk:7.0 as build
WORKDIR /app
EXPOSE 80
COPY Shared/OnlineCourse.Shared/*.csproj Shared/OnlineCourse.Shared/
COPY Frontends/OnlineCourse.Web/*.csproj Frontends/OnlineCourse.Web/
RUN dotnet restore Frontends/OnlineCourse.Web/*.csproj
COPY . .
RUN dotnet publish Frontends/OnlineCourse.Web/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:7.0 as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","OnlineCourse.Web.dll" ]




