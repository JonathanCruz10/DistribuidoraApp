DistribuidoraApp
DistribuidoraApp es una aplicación web desarrollada en ASP.NET Core, diseñada para gestionar productos y proveedores en un entorno de distribución. La aplicación permite realizar operaciones CRUD en productos y proveedores, así como gestionar los proveedores asociados a cada producto.

Características
Gestión de productos y proveedores.
Interfaz de usuario intuitiva para la edición y visualización de datos.
Modal para la edición de productos y proveedores con funcionalidad dinámica.
API RESTful para operaciones CRUD.
Tecnologías
ASP.NET Core: Framework para el desarrollo de la aplicación web.
Entity Framework Core: ORM para la interacción con la base de datos.
Bootstrap: Framework CSS para el diseño responsivo.
jQuery: Biblioteca JavaScript para manipulación del DOM y eventos.
MySQL: Sistema de gestión de base de datos.
Requisitos
.NET 8.0 o superior.
MySQL 8.0 o superior.
IIS 7.5 o superior (para el despliegue).
Instalación
Clonar el repositorio
bash
git clone https://github.com/JonathanCruz10/DistribuidoraApp 
cd DistribuidoraApp
Configurar la base de datos
Crear la base de datos:
Asegúrate de tener MySQL instalado y crea una base de datos llamada DistribuidoraDB.

Actualizar la cadena de conexión:
Modifica el archivo appsettings.json en la raíz del proyecto para incluir la cadena de conexión a tu base de datos:

json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=DistribuidoraDB;User=root;Password=tu-password;"
}
Restaurar dependencias y construir el proyecto

dotnet restore
dotnet build

Ejecutar la aplicación
arduino
dotnet run

Accede a la aplicación en http://localhost:5000.

Despliegue en IIS
Publicar la aplicación:
Publica la aplicación usando el siguiente comando:
bash
Copiar código
dotnet publish -c Release -o ./publish
Configurar IIS:

Abre el Administrador de IIS.
Crea un nuevo sitio web y configura la ruta de la carpeta publish.
Asegúrate de que el pool de aplicaciones esté configurado para usar .NET Core.
Configurar el archivo web.config:
Asegúrate de que el archivo web.config está presente en la carpeta de publicación. Este archivo se genera automáticamente durante el proceso de publicación y contiene la configuración necesaria para IIS.

Uso
API Endpoints:

GET /api/Producto: Obtener una lista de productos.
POST /api/Producto: Crear un nuevo producto.
PUT /api/Producto/{id}: Actualizar un producto existente.
DELETE /api/Producto/{id}: Eliminar un producto.
Modales:
La interfaz de usuario incluye modales para la edición de productos y proveedores, con funcionalidad dinámica para agregar y modificar proveedores.

Licencia
DistribuidoraApp está bajo la Licencia MIT.
