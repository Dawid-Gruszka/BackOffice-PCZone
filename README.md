# 🖥️ BackOffice - PC Zone

Sistema de administración de productos, categorías y pedidos para una tienda de tecnología, desarrollado con **WPF (.NET 9)**. Este sistema permite gestionar los elementos de un catálogo digital mediante una interfaz gráfica moderna, conectándose a un backend RESTful.

---

## 🚀 Tecnologías Utilizadas

- **WPF (Windows Presentation Foundation)**  
- **.NET 9**
- **MVVM (Model-View-ViewModel)**
- **HTTPClient (REST API Client)**
- **XAML** para el diseño de UI
- **C#** para lógica de negocio
- **MultipartFormDataContent** para envío de imágenes

---

## 🧱 Estructura del Proyecto

```
BackOffice/
├── Assets/                  # Recursos gráficos como íconos e imágenes
├── Models/                 # Clases que representan los datos (Product, Order, Category...)
├── Rest/                   # Cliente HTTP que se comunica con la API REST
├── Services/               # Lógica de negocio desacoplada mediante interfaces genéricas
├── ViewModels/             # Binding con la UI y comandos para acciones
├── Views/                  # Pantallas visuales en XAML (Productos, Categorías, Pedidos)
├── MainWindow.xaml         # Ventana principal con navegación lateral
└── README.md               # Este archivo
```

---

## 🧠 Principales Características

### 📦 Gestión de Productos
- Crear, editar, eliminar productos
- Carga de imágenes asociadas
- Visualización de detalles

### 🗂️ Gestión de Categorías
- CRUD completo de categorías
- Carga de imágenes por categoría

### 📑 Gestión de Pedidos
- Visualización de pedidos realizados

### 🔁 Comunicación API REST
- Implementada en `RestClient.cs`
- Serialización automática con `System.Text.Json`
- Manejo de errores y respuestas HTTP

### ♻️ Diseño Modular
- Uso de interfaz genérica `IService<TRead, TCreate, TUpdate>`
- Separación de responsabilidades por capas
- Arquitectura MVVM para facilitar mantenibilidad

---

## 🖼️ Vista de Interfaz

La ventana principal (`MainWindow.xaml`) está dividida en dos secciones:
- **Sidebar Izquierda**: navegación por botones (Productos, Categorías, Pedidos, Salir)
- **Panel Principal**: contenido dinámico que se cambia con `ContentControl` y binding a `CurrentView`

---

## ⚙️ Configuración

1. Clona este repositorio:
```bash
git clone https://github.com/tuusuario/BackOffice.git
```

2. Abre el proyecto en Visual Studio 2022 o superior.

3. Asegúrate de configurar la URL de tu API REST en:  
   `Properties/Settings.settings` → `Uri`

4. Ejecuta la solución (`Ctrl + F5`)

---

## 📸 Requisitos de Imágenes

- Las imágenes de productos y categorías deben estar en formato `.jpg`.
- Se cargan mediante `MultipartFormDataContent` al momento de crear o actualizar.

---

## 📝 Autor

Desarrollado por **Dawid Gruszka** como parte de un proyecto educativo del ciclo DAM 🧑‍💻.
