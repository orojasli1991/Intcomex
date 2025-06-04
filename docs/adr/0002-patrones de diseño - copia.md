# [2] - [patrones de diseño]

## Estado
[Aceptada]

## Fecha
[20025-05-30]

## Contexto
la prueba exigia no exponer directamente las entidades, adicional de mantener buenas practicas de desarrollo eso implica patrones de diseño
 

## Decisión
se decide utilizar en patron de diseño dto, para exponer los datos que sean necesarios entre la capa de aplicacion y presentacion,
y se implemento repository para extraer el acceso a los datos.

## Consecuencias
- cada entidad del dominio q se utilice tiene su propia interfaz I[Entidad]Repository.
- los controladores usan dto para comunicarse con los servicios.
- AutoMapper se utiliza para mapear entre entidades y DTOs.
- mantiene las capas separadas a la capa de infraestructura.
- es mas facil implemetar el mokequeo de las pruebas unitarias al tener interfaces
- permite evolucion de la api sin depender de l modelo de datos
- se puede llegar a tener redundancia en el codigo
- permite q la solucion sea mas robusta.

## Alternativas consideradas
- N/A

