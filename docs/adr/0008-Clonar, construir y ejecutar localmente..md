# [7] - [detalle de producto con foto de la categoría.]

## Clonar el repositorio
git clone https://github.com/usuario/IntcomexProductsApi.git
cd IntcomexProductsApi

## compilar solucion sin docker
-requisitos	
	-.net 8
	-sql server
	-docker desktop
-compilacion
	-dotnet build
	-dotnet run --project Intcomex.ProductsApi.Presentation
-acceder al swagger
	-https://localhost:44350/swagger/index.html
	
## compilar solucion con docker
-contruir imagen
	docker build -t intcomex-api .
-ejecutar contenedor
	docker-compose up -d
-acceder al swagger 
	-http://host.docker.internal:32774/swagger/index.html
	
##creacion db local 
- parado en la raiz del proyecto ejecutar, los archivos de migracion de EF estan dentro de la solucion en 
..\Intcomex.ProductsApi.Infrastructure/Migrations
- ejecutar los siguientes comandos
	-dotnet ef migrations add InitialCreate
	- dotnet ef database update


## ejecucion de test
- parado en la raiz del proyecto ejecutar	
	dotnet test