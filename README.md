# TARge25Shop

Многослойный веб-проект интернет-каталога на платформе ASP.NET Core MVC и .NET 10 с реализованными модулями Spaceship (Космические корабли) и Kindergarten (Детские сады), а также наборами модульных (xUnit / InMemory) и сквозных UI (Selenium) тестов.

---

## 📁 Структура решения (`TARge25Shop.slnx`)

Решение состоит из 6 взаимосвязанных проектов:
1. `TARge25Shop` — веб-приложение ASP.NET Core MVC (контроллеры, модели представлений, страницы Razor, стили Bootstrap).
2. `TARge25Shop.Core` — ядро системы (доменные сущности `Spaceship` и `Kindergarten`, DTO-объекты, интерфейсы сервисов).
3. `TARge25Shop.Data` — уровень доступа к данным (Entity Framework Core `TARge25ShopContext`, Code-First миграции).
4. `TARge25Shop.ApplicationServices` — службы бизнес-логики (`SpaceshipServices`, `KindergartenServices`).
5. `TARge25Shop.SpaceshipTest` — модульные тесты сервисов на xUnit с использованием In-Memory базы данных EF Core (22 теста).
6. `TARge25Shop.SeleniumTesting` — автоматизированные UI/E2E тесты в браузере Firefox через Selenium WebDriver.

---

## 📖 Документация и инструкции

- 🏛️ [Архитектура и устройство проекта (ARCHITECTURE.md)](ARCHITECTURE.md) — подробное описание слоев, архитектурная диаграмма Mermaid, модель данных, поток обработки запросов и применяемые паттерны.
- 🧪 [Руководство по запуску и тестированию (RUN_AND_TEST_GUIDE.md)](RUN_AND_TEST_GUIDE.md) — пошаговый запуск проекта, применение миграций, чек-лист ручного тестирования и запуск автоматических тестов.

---

## 🚀 Быстрый старт

### Требования
- [.NET 10 SDK](https://dotnet.microsoft.com/download) (v10.0.400+)
- Microsoft SQL Server LocalDB
- Mozilla Firefox (для запуска Selenium UI тестов)

### 1. Сборка решения
```powershell
dotnet build
```

### 2. Применение миграций базы данных
```powershell
dotnet ef database update --project TARge25Shop.Data --startup-project TARge25Shop
```

### 3. Запуск веб-приложения
```powershell
dotnet run --project TARge25Shop
```
Приложение доступно по адресам:
- HTTPS: [https://localhost:7227](https://localhost:7227)
- HTTP: [http://localhost:5277](http://localhost:5277)

---

## 🧪 Запуск тестов

### Модульные тесты (Unit Tests - 22 теста)
```powershell
dotnet test TARge25Shop.SpaceshipTest
```

### Сквозные UI-тесты (Selenium WebDriver)
> Требуется предварительно запустить приложение в отдельном терминале (`dotnet run --project TARge25Shop`).
```powershell
dotnet test TARge25Shop.SeleniumTesting
```
