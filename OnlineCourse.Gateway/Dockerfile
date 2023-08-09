FROM mcr.microsoft.com/dotnet/sdk:7.0 as build
WORKDIR /app
EXPOSE 80

COPY  OnlineCourse.Gateway/*.csproj  OnlineCourse.Gateway/
RUN dotnet restore OnlineCourse.Gateway/*.csproj
COPY . .
RUN dotnet publish OnlineCourse.Gateway/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:7.0 as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","OnlineCourse.Gateway.dll" ]




