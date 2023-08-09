FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /app
EXPOSE 80
COPY Shared/OnlineCourse.Shared/*.csproj Shared/OnlineCourse.Shared/
COPY IdentityServer/OnlineCourse.IdentityServer/*.csproj IdentityServer/OnlineCourse.IdentityServer/ 
RUN dotnet restore  IdentityServer/OnlineCourse.IdentityServer/*.csproj
COPY . .
RUN dotnet publish IdentityServer/OnlineCourse.IdentityServer/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:6.0 as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","OnlineCourse.IdentityServer.dll" ]




