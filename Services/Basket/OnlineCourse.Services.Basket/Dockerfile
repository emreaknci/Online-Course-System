FROM mcr.microsoft.com/dotnet/sdk:7.0 as build
WORKDIR /app
EXPOSE 80
COPY Shared/OnlineCourse.Shared/*.csproj Shared/OnlineCourse.Shared/
COPY Services/Basket/OnlineCourse.Services.Basket/*.csproj Services/Basket/OnlineCourse.Services.Basket/
RUN dotnet restore Services/Basket/OnlineCourse.Services.Basket/*.csproj
COPY . .
RUN dotnet publish Services/Basket/OnlineCourse.Services.Basket/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:7.0 as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","OnlineCourse.Services.Basket.dll" ]