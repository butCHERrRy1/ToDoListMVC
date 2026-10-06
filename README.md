# ToDoListMVC

учебное веб приложение to-do list на ASP.NET Core MVC + Entity Framework Core (SQLite)
реализован CRUD (создание, просмотр, редактирование, удаление задач, отметка о выполнении)

## Стек

- .NET 8 (ASP.NET Core MVC)
- Entity Framework Core (Code First)
- SQLite
- Bootstrap (из стандартного шаблона)

## Функционал

- Список задач на главной странице (`/Tasks`)
- Создание задачи (название, описание)
- Редактирование задачи (название, описание, статус выполнения)
- Удаление задачи с подтверждением
- Быстрая отметка «выполнено / не выполнено» прямо из списка
- Фильтрация: все / выполненные / невыполненные
- Сортировка по дате создания (сначала новые)
- Счётчик невыполненных задач

## Структура проекта


ToDoListMVC/
├── Controllers/
│   ├── HomeController.cs
│   └── TasksController.cs       # CRUD-логика
├── Data/
│   └── AppDbContext.cs          # DbContext для EF Core
├── Models/
│   └── TaskItem.cs              # Сущность задачи
├── Views/
│   ├── Shared/
│   └── Tasks/                   # Index / Create / Edit / Delete / Details
├── Migrations/                  # EF Core миграции
├── appsettings.json             # строка подключения к SQLite
└── Program.cs                   # регистрация DbContext через DI

## Требования

- [.NET SDK 8.0](https://dotnet.microsoft.com/download) или новее
- Инструмент `dotnet-ef`:


dotnet tool install --global dotnet-ef

## Запуск

1. Клонировать/распаковать проект и перейти в папку:

   cd ToDoListMVC


2. Восстановить зависимости:

   dotnet restore

3. Применить миграции и создать файл базы данных `todo.db`:

   dotnet ef database update

   > Если папки `Migrations/` нет (например, её не добавили в репозиторий), сначала:
   > dotnet ef migrations add InitialCreate
   > dotnet ef database update


4. Запустить приложение:

   dotnet run


5. Открыть в браузере адрес из консоли (обычно `https://localhost:7xxx`) и перейти в раздел **«Задачи»** в верхнем меню, либо сразу по адресу:

   https://localhost:7xxx/Tasks


## Строка подключения

Файл `appsettings.json`:

"ConnectionStrings": {
  "DefaultConnection": "Data Source=todo.db"
}


Файл `todo.db` создаётся в корне проекта автоматически при первой миграции.

## Примечания

- `CreatedAt` устанавливается автоматически при создании задачи.
- `IsCompleted` по умолчанию `false`, меняется либо на форме редактирования, либо кнопкой «Выполнено» в списке.
- Все формы защищены `[ValidateAntiForgeryToken]`, удаление — через POST с подтверждением.