FROM mcr.microsoft.com/dotnet/sdk:7.0 as build
WORKDIR /app
EXPOSE 80
COPY Shared/OnlineCourse.Shared/*.csproj Shared/OnlineCourse.Shared/
COPY Services/PhotoStock/OnlineCourse.Services.PhotoStock/*.csproj Services/PhotoStock/OnlineCourse.Services.PhotoStock/
RUN dotnet restore Services/PhotoStock/OnlineCourse.Services.PhotoStock/*.csproj
COPY . .
RUN dotnet publish Services/PhotoStock/OnlineCourse.Services.PhotoStock/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:7.0 as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","OnlineCourse.Services.PhotoStock.dll" ]