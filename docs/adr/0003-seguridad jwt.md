# [3] - [segurida jwt]

## Estado
[Aceptada]

## Fecha
[20025-06-02]

## Contexto
la prueba exigia implementar jwt, aseguradno los end points considerados criticos, para que sea segura y escalable.
 

## Decisión
se implementa jwt firmado con llaves quemadas en la configuracion


## Consecuencias
- no es necesario manejar autenticacion centralizada, lo cual permite mas facil de escalar horizontalmente
- permite interoperabilidad entre otros sistemas
- la clave secreta no se deberia manejar quemada en la configuracion, para deberia usarse algun servicio como 
key vaults que las mantenga en secreto


## Alternativas consideradas
- N/A

