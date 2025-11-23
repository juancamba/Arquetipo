Arquetipo de arquitectura hexagonal .Net

Para generar un nuevo proyecto ejecutar generar.sh "nombre-proyecto" y darle un nombre genérico

## Application
Los casos de uso van en application

    No mezcles la lógica de la aplicación ( que va aquí en el caso de uso) con notificaciones. Por ejemplo CrearAlquiler tendrá su logica de negocio, pero si hay que enviar un correo no irá aqui, sino en las notificaciones. De esto se encarga Mediator.

TODO:
    Seguir documentando    