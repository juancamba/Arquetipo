

## Ejecución

dotnet run dentro de src/Arquetipo.Api


## librerias
https://github.com/martinothamar/Mediator

Importante con esta libreria no meter en mas de un proyecto el source generato, sino falla. Solo debe estar en aquel que hagas el config, en application

## Casos de uso
Los agrupamos en aplication dentro de la entidad que le da nombre por ejemplo si tenemos Users tenemos un caso de uso CreateUser, creamos la carpete Users/CreateUser y ahi dentro metemos el command, el commandHanlder, el validator y los eventHanlder ( los que controlan los eventos de dominio)

## Domain

Cuando se crea un objeto de domino, se recomienda encapsular su creación haciando private su constructor y creando un metodo statico create.

## Domain Events

Cuando ocurre algo en el dominio lo mas común es que se produzca un evento, por ejemplo UserCreated, UserDeleted, UserUpdated, etc. Esto se guardan en el propio domino y luego son capturados en ApplicationDbContext.SaveChangesAsync.

Estos se almacenan en la tabla outbox_messages usando el outbox pattern

Luego con el job InvokeOutboxMessagesJob se intantan disparar si es menester. PUede ser que solo sea enviar un correo electronico, publicar en el bus o lo que sea. 

Es decir se si ocurre un UserCreatedDomainEvent el job intentará lanzar CreateUserDomainEventHandler que implementa la interfaz de Mediator INotification. Esto nos permite separar los concerns una cosa es el evento de dominio, otra la lógica de negocio que se implementa en el CommandHandler CreateUserCommandHandler.

## Base de datos

Usamos un mapeo explicito para facilitar la comprensión y lectura de código. Se crea ademas una clase de configuración dentro de Infrastructure/Configurations por cada entidad de base de datos


### Migracion desde el raiz de la aplicaión:

dotnet ef --verbose migrations add InitialCreate -p src/Arquetipo.Infrastructure/ -s src/Arquetipo.Api

dotnet ef database update -p src/Arquetipo.Infrastructure -s src/Arquetipo.Api

docker run -d   --name postgres -e POSTGRES_PASSWORD=postgres  -e POSTGRES_DB=postgres -p 5432:5432  postgres:16