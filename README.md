# TARge25Shop

Учебный и практический проект каталога товаров и услуг (модули Spaceship и Kindergarten) на базе ASP.NET Core MVC и .NET 10.

## 📖 Документация и инструкции

- [Архитектура и устройство проекта (ARCHITECTURE.md)](ARCHITECTURE.md) — полное описание слоев, диаграммы потоков данных, принципы и паттерны.
- [Руководство по запуску и тестированию (RUN_AND_TEST_GUIDE.md)](RUN_AND_TEST_GUIDE.md) — пошаговая инструкция по развертыванию, чек-лист ручного тестирования CRUD-операций и решение проблем.

## Быстрый старт

### Требования
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- MS SQL Server LocalDB

### Запуск проекта
```powershell
# Восстановление зависимостей и сборка
dotnet build

# Применение миграций базы данных
dotnet ef database update --project TARge25Shop.Data --startup-project TARge25Shop

# Запуск приложения
dotnet run --project TARge25Shop
```
