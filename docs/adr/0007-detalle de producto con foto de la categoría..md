# [7] - [detalle de producto con foto de la categoría.]

## Estado
[Aceptada]

## Fecha
[20025-06-04]

## Contexto
cargar una foto y exponerla en el end point de categorias
 
## Decisión
se decide manejar en base64 guardada en la db, pensando a futuro que un cliente consuma dicho endponit y pueda a nivel de front redenrizar la imagen

## Consecuencias
- manejo interno e integridad refererecial garantizada
- no depende de terceros
- ideal y simple para el tamaño de la prueba
- consumo y portabilidad facil 

## Alternativas consideradas
- implementar una ruta local dentro del proyecto que guarde imagenes, se descarta por temas de configuracion de rutas y genera que 
la solucion sea mas pesada en el caso que se tuvieran muchas categorias
- impementar un localstorague en cloud, se descarta debido a acceso gratuido de cloud y mayor configuracion
- consumir una url externa donde esten alojadas las imagenes, se descarta para no generar dependencias de terceros

