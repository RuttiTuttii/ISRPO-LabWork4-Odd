# ISRPO LabWork 4 — Команда Нечетных (Модели данных, БД, Фильтрация и Пагинация)

Лабораторная работа №4 по дисциплине **МДК.02.02 Инструментальные средства разработки программного обеспечения (ИСРПО)**.
Тема: **Разработка и интеграция модулей проекта (командная работа)**.

## Участники и задачи (Нечетные задачи по Варианту 1):
* **Участник 1 (Задача 1):** Создание модели данных (`Customer`, `Order`, `Product`, `Category`) и настройка контекста БД (`AppDbContext`, EF Core SQLite, сидирование).
* **Участник 3 (Задача 3):** Разработка логики фильтрации, сортировки и пагинации (`QueryParameters`, `PagedResult<T>`, IQueryable extensions).

## Требования:
* .NET SDK 10.0+ (`net10.0`)
* Entity Framework Core SQLite / InMemory 10.0.x
* xUnit

## Сборка и запуск:
```bash
dotnet restore
dotnet build
dotnet test
dotnet run --project src/LabWork4.Odd/LabWork4.Odd.csproj
```
