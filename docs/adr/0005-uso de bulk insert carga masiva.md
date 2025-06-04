# [5] - [0005-uso de bulk insert carga masiva]

## Estado
[Aceptada]

## Fecha
[20025-06-03]

## Contexto
la prueba exige realizar una creacion masiva de 100.000 registros de la manera mas eficiente posible. 

## Decisión
se implementa sqlbulkcopy sin usar EF, para evitar lentitud y consumo de recursos, se usa ado.net para tener mayor rendimiento
y permitiendo hacerlo por lotes asi reduce el consumo a la db.


## Consecuencias
- mejora el rendimiento de la carga
- menor consumo de recursos al no usar el ORM
- depender de librerias externas
- inyecta complejidad al codigo
- 

## Alternativas consideradas
- usar insercion con EF de forma tradicional, pero se descarta ya que genera mayor consumo de recursos
- usar libreria externa, pero esto genera mayor complejidad y dependencia
- usar colas, esto implicaba mayor complejidad al momento de integrar y uso de aplicaciones externas y genera dependencia
