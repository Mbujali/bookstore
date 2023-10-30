FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY BookstoreDemo.csproj .
RUN dotnet restore "BookstoreDemo.csproj"
COPY . .
RUN dotnet publish "BookstoreDemo.csproj" -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0 as final
WORKDIR /app
COPY --from=build /publish .

EXPOSE 5268

ENTRYPOINT ["dotnet", "BookstoreDemo.dll"]