# dotnet-shop-cart
.NET 6 Shop Cart Sample API
[![Docker Image CI](https://github.com/schivei/dotnet-shop-cart/actions/workflows/docker-image.yml/badge.svg)](https://github.com/schivei/dotnet-shop-cart/actions/workflows/docker-image.yml)


## Build and Run
Compile docker image from root folder using the following command:
```bash
docker build -f src/Schivei.Shop.Cart/Dockerfile -t schivei.shop .
docker run --name schivei.shop -p 18080:80 schivei.shop
```


## API Documentation
Open http://localhost:18080/swagger on your favorite browser
