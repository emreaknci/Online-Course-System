FROM mcr.microsoft.com/dotnet/sdk:7.0 as build
WORKDIR /app
EXPOSE 80
COPY Shared/OnlineCourse.Shared/*.csproj Shared/OnlineCourse.Shared/

COPY Services/Order/OnlineCourse.Services.Order.Application/*.csproj Services/Order/OnlineCourse.Services.Order.Application/

COPY Services/Order/OnlineCourse.Services.Order.Domain/*.csproj Services/Order/OnlineCourse.Services.Order.Domain/

COPY Services/Order/OnlineCourse.Services.Order.Domain.Core/*.csproj Services/Order/OnlineCourse.Services.Order.Domain.Core/

COPY Services/Order/OnlineCourse.Services.Order.Infrastructure/*.csproj Services/Order/OnlineCourse.Services.Order.Infrastructure/


COPY Services/Order/OnlineCourse.Services.Order.API/*.csproj Services/Order/OnlineCourse.Services.Order.API/
RUN dotnet restore Services/Order/OnlineCourse.Services.Order.API/*.csproj
COPY . .
RUN dotnet publish Services/Order/OnlineCourse.Services.Order.API/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:7.0 as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","OnlineCourse.Services.Order.API.dll" ]




