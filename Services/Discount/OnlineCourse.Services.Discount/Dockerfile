FROM mcr.microsoft.com/dotnet/sdk:7.0 as build
WORKDIR /app
EXPOSE 80
COPY Shared/OnlineCourse.Shared/*.csproj Shared/OnlineCourse.Shared/
COPY Services/Discount/OnlineCourse.Services.Discount/*.csproj Services/Discount/OnlineCourse.Services.Discount/
RUN dotnet restore Services/Discount/OnlineCourse.Services.Discount/*.csproj
COPY . .
RUN dotnet publish Services/Discount/OnlineCourse.Services.Discount/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:7.0 as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","OnlineCourse.Services.Discount.dll" ]