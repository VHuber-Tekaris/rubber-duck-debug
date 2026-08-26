# Building needs the sdk image; running needs only the much smaller runtime image.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY DuckApi.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine
WORKDIR /app

# Do not run as root. The data directory has to belong to that user, so both
# lines have to come before USER - afterwards there is no permission to chown.
RUN addgroup -S pond && adduser -S pond -G pond
COPY --from=build /app ./
RUN mkdir -p /app/data && chown pond:pond /app/data
USER pond

ENV ASPNETCORE_HTTP_PORTS=8080
ENV DUCKPOND_TIPS_FILE=/app/tips/duck-tips.json
EXPOSE 8080
ENTRYPOINT ["dotnet", "DuckApi.dll"]
