# Руководство по локальному запуску и тестированию проекта TARge25Shop

В данном руководстве описан пошаговый процесс развертывания, запуска, проверки базы данных, ручного тестирования и выполнения автоматизированных Unit и Selenium E2E тестов в локальном окружении.

---

## 1. Системные требования

Перед началом работы убедитесь, что на компьютере установлены следующие компоненты:

1. .NET 10 SDK (версия 10.0.400 или выше):
   ```powershell
   dotnet --version
   ```
2. Microsoft SQL Server LocalDB (или SQL Server Express / Developer):
   - Поставляется в составе Visual Studio (компонент «Хранение и обработка данных» / Data storage and processing).
   - Проверка статуса LocalDB:
     ```powershell
     sqllocaldb info mssqllocaldb
     ```
3. CLI-инструмент Entity Framework Core (`dotnet-ef`):
   - Установка глобального инструмента (если еще не установлен):
     ```powershell
     dotnet tool install --global dotnet-ef
     ```
   - Проверка версии:
     ```powershell
     dotnet ef --version
     ```
   > [!NOTE]
   > Если после установки команда `dotnet ef` не распознается терминалом, убедитесь, что путь к глобальным инструментам .NET добавлен в переменную окружения `PATH`:
   > `$env:USERPROFILE\.dotnet\tools`

4. Mozilla Firefox (требуется для сквозных UI-тестов проекта `TARge25Shop.SeleniumTesting`).
5. Рекомендуемая IDE / Редактор:
   - Visual Studio 2026 / 2022 (версия 17.12+)
   - Visual Studio Code с расширением C# Dev Kit
   - JetBrains Rider 2024.3+

---

## 2. Настройка конфигурации

Строка подключения к базе данных хранится в файле [`TARge25Shop/appsettings.json`](file:///e:/program/ProgrmTARge25/TARge25Shop/appsettings.json):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=TARge25Shop;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

- По умолчанию приложение использует встроенный экземпляр `(localdb)\MSSQLLocalDB`.
- Если используется другой сервер (например, `localhost` или `SQLEXPRESS`), отредактируйте параметр `DefaultConnection`.

---

## 3. Сборка и подготовка базы данных

Откройте терминал PowerShell в корневой папке проекта (`e:\program\ProgrmTARge25`):

### Шаг 3.1. Сборка решения
```powershell
dotnet build
```
Убедитесь, что все 6 проектов решения собрались без ошибок (`0 Ошибок`).

### Шаг 3.2. Применение миграций EF Core к базе данных
Для автоматического создания базы данных `TARge25Shop` и всех таблиц (`Spaceships`, `Kindergartens`, `__EFMigrationsHistory`) выполните:

```powershell
dotnet ef database update --project TARge25Shop.Data --startup-project TARge25Shop
```

> [!TIP]
> Команда автоматически создаст базу данных в LocalDB (если она еще не существовала) и последовательно накатит все миграции:
> 1. `20260908072731_Init` — создание таблицы `Spaceships`.
> 2. `20260917113908_KindergartenInit` — создание таблицы `Kindergartens`.

---

## 4. Локальный запуск веб-приложения

Запустите веб-сервер ASP.NET Core Kestrel с помощью команды:

```powershell
dotnet run --project TARge25Shop
```

В соответствии с [`launchSettings.json`](file:///e:/program/ProgrmTARge25/TARge25Shop/Properties/launchSettings.json) приложение поднимается по адресам:
- HTTPS: [https://localhost:7227](https://localhost:7227)
- HTTP: [http://localhost:5277](http://localhost:5277)

Откройте браузер и перейдите по адресу: [https://localhost:7227](https://localhost:7227)

> [!NOTE]
> При первом запуске браузер может предупредить о самоподписанном HTTPS-сертификате разработки. Чтобы сделать сертификат доверенным в системе, выполните один раз:
> ```powershell
> dotnet dev-certs https --trust
> ```

---

## 5. Чек-лист ручного функционального тестирования (Manual Testing)

В приложении функционируют два модуля: Spaceship и Kindergarten.

### Тест модуля «Spaceship» (Космические корабли)
1. Навигация: В верхнем меню нажмите ссылку Spaceship (URL: `/Spaceship`).
2. Создание (Create):
   - Нажмите синюю кнопку Create.
   - Заполните поля:
     - Name: `Millennium Falcon`
     - Ship Type: `Freighter`
     - Crew: `4`
     - Engine Power: `1500`
   - Нажмите Create.
   - Ожидаемый результат: Происходит перенаправление на страницу списка (`/Spaceship/Index`). Новый корабль отображается в таблице с порядковым номером 1 и корректной датой создания.
3. Проверка бизнес-правил:
   - Попробуйте создать корабль с Crew = `2` и Engine Power = `0`.
   - Ожидаемый результат: В базе сохранится Crew = `4` (минимальное значение) и Engine Power = `1` (минимальная мощность).
4. Детальный просмотр (Details):
   - В строке корабля нажмите кнопку Details.
   - Ожидаемый результат: Открывается карточка со всеми параметрами, включая сгенерированный GUID и метки `CreatedAt` / `UpdatedAt`.
5. Редактирование (Update):
   - Нажмите кнопку Update.
   - Измените `Engine Power` на `2000`, `Crew` на `6`.
   - Нажмите Update.
   - Ожидаемый результат: Данные в таблице обновились. В Details значение `UpdatedAt` стало новее, чем `CreatedAt`.
6. Удаление (Delete):
   - Нажмите кнопку Delete, затем подтвердите удаление.
   - Ожидаемый результат: Запись удалена из таблицы и базы данных.

---

### Тест модуля «Kindergarten» (Детские сады)
1. Навигация: В верхнем меню нажмите ссылку Kindergarten (URL: `/Kindergarten`).
2. Создание (Create):
   - Нажмите кнопку Create.
   - Заполните форму:
     - Group Name: `Pääsuke`
     - Children Count: `18`
     - Kindergarten Name: `Tallinna Päikese Lasteaed`
     - Teacher Name: `Maria Kask`
   - Нажмите Create.
   - Ожидаемый результат: Запись появилась в таблице со всеми полями.
3. Проверка бизнес-правил:
   - Попробуйте создать садик с Children Count = `0` или отрицательным.
   - Ожидаемый результат: Сервис автоматически установит значение `4`.
4. Просмотр деталей (Details):
   - Нажмите Details напротив созданной записи.
   - Ожидаемый результат: Отображается карточка с уникальным `Id`, названием сада, группы, воспитателем и датами.
5. Обновление (Update):
   - Нажмите Update, измените число детей на `20`, нажмите Update.
   - Ожидаемый результат: Изменение сохранено, `UpdatedAt` обновлен.
6. Удаление (Delete):
   - Нажмите Delete, подтвердите действие. Запись удалена.

---

### Граничные случаи и ошибки (Edge Cases)
- Несуществующий Id: Переход на `https://localhost:7227/Kindergarten/Details/00000000-0000-0000-0000-000000000000` возвращает `404 Not Found`.
- Защита от двойной отправки (PRG): Обновление страницы по `F5` после добавления записи не приводит к повторному запросу `POST`.

---

## 6. Автоматизированное тестирование

Решение включает два независимых тестовых проекта: Unit-тесты и E2E UI-тесты.

### 6.1. Модульное тестирование (`TARge25Shop.SpaceshipTest`)

Проект тестирует бизнес-логику сервисов `ISpaceshipServices` и `IKindergartenServices` изолированно, используя In-Memory базу данных Entity Framework Core.

#### Запуск модульных тестов:
```powershell
dotnet test TARge25Shop.SpaceshipTest
```

#### Набор тестов (22 теста):
- Модуль `SpaceshipTest.cs` (11 тестов):
  - `ShouldNot_AddEmptySpaceShip_WhenResultIsReturned` — создание и непустой результат
  - `ShouldNot_GetSpaceShipByID_WhenIDNotEqual` — несовпадение ID при выборке
  - `Should_GetSpaceshipByID_WhenGuidIsEqual` — получение сущности по верному GUID
  - `Should_SpaceshipDeletedByID_WhenReturnedResultIsEqual` — корректное удаление
  - `ShouldNot_DeleteSpaceshipByID_WhenDidNotDeleteSpaceship` — изолированность удаления
  - `Should_UpdateSpaceshipByID_WhenUpdatingData` — успешное обновление
  - `ShouldNot_UpdateSpaceshipByID_WhenUpdatingData` — проверка изменений
  - `Should_SetDefaultCrew4_WhenCrewIs3OrLess` — бизнес-правило минимального экипажа (<= 3 -> 4)
  - `Should_SetEnginePower1_WhenEnginePowerIs0OrNegative` — бизнес-правило минимальной мощности (<= 0 -> 1)
- Модуль `KindergartenTest .cs` (11 тестов):
  - Полное аналогичное покрытие CRUD-операций и бизнес-правила дефолтного количества детей (`ChildrenCount <= 0 -> 4`).

> Все 22 теста выполняются без предварительного поднятия базы данных SQL Server.

---

### 6.2. Сквозное UI-тестирование (`TARge25Shop.SeleniumTesting`)

Проект автоматизирует действия пользователя в браузере с помощью библиотеки Selenium WebDriver и драйвера FirefoxDriver.

#### Предварительные шаги перед запуском Selenium-тестов:
1. Убедитесь, что на компьютере установлен браузер Mozilla Firefox.
2. В отдельном окне терминала запустите веб-сервер приложения:
   ```powershell
   dotnet run --project TARge25Shop
   ```
   Сервер должен быть доступен по адресу `https://localhost:7227`.

#### Запуск UI-тестов:
В основном окне терминала выполните:
```powershell
dotnet test TARge25Shop.SeleniumTesting
```

#### Что проверяют Selenium-тесты (`SpaceShipFrontendTest.cs`):
- `Should_NavigateToCreate_AddSpaceshipWithCorrectData_ReturnToIndex` — открытие страницы списка, клик по кнопке Create (`#CreateInIndex`), ввод данных через селекторы форм (`#CU_NameEntrySpaceShip`, `#CU_ShipTypeEntrySpaceShip`, `#CU_CrewEntrySpaceShip`, `#CU_EnginePowerEntrySpaceShip`), сохранение (`#CU_CreateSpaceShip`), проверка появления корабля в таблице (`#IndexTable`).
- `Should_NavigateToUpdate_OfASpaceShip_WithNewdata` — поиск строки в таблице, переход в форму редактирования, ввод новых значений, сохранение и валидация обновленных данных.
- `Should_NavigateToDetails_OfASpaceShip_WithPreviouslyCorrectData_AndReturnToIndex` — поиск корабля, переход в Details, валидация отображения полей `#id`, `#name`, `#type`, `#crew`, `#power` и формата GUID.

---

## 7. Устранение возможных неполадок (Troubleshooting)

| Проблема | Причина | Решение |
|---|---|---|
| `Cannot open database "TARge25Shop"` | Служба LocalDB остановлена | Запустите LocalDB: `sqllocaldb start mssqllocaldb`. Проверьте статус: `sqllocaldb info mssqllocaldb`. |
| `dotnet-ef: command not found` | Утилита `dotnet-ef` не в `PATH` | Добавьте `$env:USERPROFILE\.dotnet\tools` в переменную `PATH` или выполните: `$env:PATH += ";$env:USERPROFILE\.dotnet\tools"`. |
| Ошибка SSL / HTTPS сертификата | Не настроен доверенный сертификат разработки | Выполните в консоли: `dotnet dev-certs https --trust`. |
| Selenium: `WebDriverException: Cannot find firefox binary` | Не установлен браузер Mozilla Firefox | Установите браузер Firefox или скачайте geckodriver. |
| Selenium: `Connection refused https://localhost:7227` | Веб-приложение не запущено | Запустите приложение перед тестами в отдельном терминале: `dotnet run --project TARge25Shop`. |
