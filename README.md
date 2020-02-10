# Beezy Prueba técnica
## Alex Crespo - 10/02/2020

### Instrucciones para ejecutar
La prueba está implementada en .NetCore 3.1.1. Para su ejecución, se deberá lanzar el proyecto BeezyBackend.API. Los logs se guardarán en la carpeta /logs

### Consideraciones
 - Diseño en DDD (algo simplificado) respetando la inmutabilidad de las entidades y agregados, a pesar de que se ha reutilizado el modelo extraído de la base de datos. 
 - Para trabajar con la base de datos facilitada se ha partido de un enfoque Database First, pero para respetar una estructura "más DDD", se han modificado los modelos acercándolos a la forma que hubiesen sido concebidos si se hubiese ideado inicialmente como Code First.
 - Dado que la base de datos es de solo lectura, se ha descartado realizar las migraciones Entity Framework de la misma.
 - Se implementa siguiendo TDD en la medida de lo posible, a pesar de que no se han respetado los commits en algunos de los ciclos red-green-refactor.
 - Se siguen las directrices SOLID, YAGNI, KISS, DRY y en general se ha programado el código de la forma más Clean posible, evitando antipatrones y code smells (aplicando TPP, Object Calisthenics...).
 - Se utiliza el patrón Repository para inyectar los accesos a datos. De este modo, el algoritmo de cálculo del IntelligentBillboard es agnóstico del origen de los datos (Base de datos o API externa).
 - Se utiliza Flurl para facilitar en manejo de la API externa.
 - Se utiliza Entity Framework como ORM de acceso a datos.
 - Se utiliza Serilog como capa para implementar ILogger. Permite integrerarse muy fácilmente con plataformas como Graylog. 

### Algorítmo IntelligentBillboard
Cuando obtienen los datos de la base de datos, se ha considerado que un Manager desea proyectar las películas con mayor venta de entradas, separando entre las que se vendieron en salas grandes y pequeñas, en base a su éxito en la ciudad. 

Una película que ha sido la que más ha vendido en salas grandes y también en pequeñas, el algorítmo propondría proyectarlas también en una sala grande y pequeña simultáneamente, ya que entiendo que es la mejor opción para conseguir el máximo aforo posible. Lo que no ocurrirá nunca es que se vuelva a emitir la misma película en el mismo tipo de sala en semanas posteriores. 
  
### Puntos de mejora
 - Hubiese sido interesante utilizar un micro ORM como Dapper para simplificar las Querys contra la base de datos (incluso para mejora del rendimiento). 
 - En un dominio implementado desde cero (sin importarlo desde una base de datos existente), habría sido interesante utilizar ValueObjects en las entidades, en lugar de tipos primitivos.
