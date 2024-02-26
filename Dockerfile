FROM mcr.microsoft.com/dotnet/aspnet:5.0
ADD ./ /
EXPOSE 8080
ENV ASPNETCORE_URLS=http://*:8080
WORKDIR /
ENTRYPOINT ["dotnet", "rent-products-api.dll"]