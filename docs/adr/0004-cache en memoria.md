# [4] - [cache en memoria]

## Estado
[Aceptada]

## Fecha
[20025-06-03]

## Contexto
la prueba exige realizar una creacion masiva de 100.000 registros entre los cuales debe consultar previamente el end point de categorias exietentes,
por lo cual se genera un cuello de botella, y hace que el cargue sea demorado. 

## Decisión
se implementa cache en memoria el cual nos ayuda a tener durante un minuto el tiepo de categorias exietntes previamente consultadas por medio
del endpoint expuesto para eso, y asi no generar consultas innecesarias  a la api y a la db.


## Consecuencias
- mejora el rendimiento de la api
- la cache no tiene persistencia, es decir se pierde al reiniciar la api
- no genera sobrecarga de recursos externos.

## Alternativas consideradas
- manejar cache en un componente externo, implicaba mas configuracion, y un poco excesivo para la prueba 
- usas dictionary pero se descarta por que no maneja expiracion, la idea es que si cambian las categorias este cached
se actualice cada cierto tiempo.
