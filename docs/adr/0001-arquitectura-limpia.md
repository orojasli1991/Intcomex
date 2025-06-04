# [1] - [arquitectura por capas]

## Estado
[Aceptada]

## Fecha
[20025-05-30]

## Contexto
Se desarrolla una API RESTful, la cual debe ser fácilmente testeable, mantenible y preparada para crecer.
 

## Decisión

 **Domain**: contiene entidades de negocio puras.
- **Application**: orquesta casos de uso y contiene servicios.
- **DAO (Infraestructura)**: implementa el acceso a datos usando EF Core.
- **API (Presentación)**: expone controladores con los endpoint
- **Test (Pruebas unitarias y integracion)**: expone los test unitarios

## Consecuencias
- permite prueba unitarias por cada servicio de la aplicacion
- tiene alta cohesion y bajo acoplamiento
- facilita el mantenimiento
- facilita migraciones a otros frmaworks o otro motor de db

## Alternativas consideradas
- monolito = es tradicional pero en el tiempo puede ser dificil de escalar y mantener, segun la prueba debia considerar esos dos escenarios
- microservicios = descartado por el alcance de la prueba

