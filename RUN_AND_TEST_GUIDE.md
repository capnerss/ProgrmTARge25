# Руководство по локальному запуску и тестированию проекта TARge25Shop

В данном руководстве описан пошаговый процесс развертывания, запуска, проверки базы данных и тестирования веб-приложения **TARge25Shop** в локальном окружении.

---

## 1. Системные требования

Перед началом работы убедитесь, что на компьютере установлены следующие компоненты:

1. **.NET 10 SDK** (версия 10.0.400 или выше):
   ```powershell
   dotnet --version
   ```
2. **Microsoft SQL Server LocalDB** (или SQL Server Express / Developer):
   - Поставляется в составе Visual Studio (компонент «Хранение и обработка данных» / Data storage and processing).
   - Проверка статуса LocalDB:
     ```powershell
     sqllocaldb info mssqllocaldb
     ```
3. **CLI-инструмент Entity Framework Core (`dotnet-ef`)**:
   - Установка глобального инструмента (если еще не установлен):
     ```powershell
     dotnet tool install --global dotnet-ef
     ```
   - Если инструмент уже установлен, проверка версии:
     ```powershell
     dotnet ef --version
     ```
   > [!NOTE]
   > Если после установки команда `dotnet ef` не распознается терминалом, убедитесь, что путь к глобальным инструментам .NET добавлен в переменную окружения `PATH`:
   > `$env:USERPROFILE\.dotnet\tools`

4. **Рекомендуемая IDE / Редактор**:
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
Убедитесь, что сборка завершилась без ошибок (`0 Ошибок`).

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

## 4. Локальный запуск приложения

Запустите веб-сервер ASP.NET Core Kestrel с помощью команды:

```powershell
dotnet run --project TARge25Shop
```

В консоли появятся URL-адреса локального сервера:
- **HTTPS:** `https://localhost:7198`
- **HTTP:** `http://localhost:5242`

Откройте браузер и перейдите по адресу: [https://localhost:7198](https://localhost:7198)

> [!NOTE]
> При первом запуске браузер может предупредить о самоподписанном HTTPS-сертификате разработки. Чтобы сделать сертификат доверенным в системе, выполните один раз:
> ```powershell
> dotnet dev-certs https --trust
> ```

---

## 5. Чек-лист ручного функционального тестирования (Manual Testing)

В приложении реализовано два основных модуля: **Spaceship** и **Kindergarten**. Пройдите следующие шаги для валидации их корректной работы.

### Тест модуля «Spaceship» (Космические корабли)
1. **Навигация:** В верхнем меню нажмите ссылку **Spaceship** (URL: `/Spaceship`).
2. **Создание (Create):**
   - Нажмите синюю кнопку **Create**.
   - Заполните поля:
     - Name: `Millennium Falcon`
     - Ship Type: `Freighter`
     - Crew: `4`
     - Engine Power: `1500`
   - Нажмите **Create**.
   - **Ожидаемый результат:** Происходит перенаправление на страницу списка (`/Spaceship/Index`). Новый корабль отображается в таблице с порядковым номером 1 и корректной датой создания.
3. **Детальный просмотр (Details):**
   - В строке созданного корабля нажмите кнопку **Details**.
   - **Ожидаемый результат:** Открывается карточка со всеми параметрами, включая сгенерированный GUID и метки `CreatedAt` / `UpdatedAt`.
   - Нажмите кнопку **Back** для возврата к списку.
4. **Редактирование (Update):**
   - Нажмите кнопку **Update**.
   - Измените `Engine Power` на `2000`, `Crew` на `6`.
   - Нажмите **Update**.
   - **Ожидаемый результат:** Данные в таблице обновились. При переходе в Details значение `UpdatedAt` стало новее, чем `CreatedAt`.
5. **Удаление (Delete):**
   - Нажмите кнопку **Delete**.
   - **Ожидаемый результат:** Открывается страница подтверждения с характеристиками удаляемого корабля.
   - Нажмите красную кнопку **Delete**.
   - **Ожидаемый результат:** Запись удалена из таблицы и базы данных.

---

### Тест модуля «Kindergarten» (Детские сады)
1. **Навигация:** В верхнем меню нажмите ссылку **Kindergarten** (URL: `/Kindergarten`).
2. **Создание (Create):**
   - Нажмите кнопку **Create**.
   - Заполните форму:
     - Group Name: `Pääsuke` (Ласточка)
     - Children Count: `18`
     - Kindergarten Name: `Tallinna Päikese Lasteaed`
     - Teacher Name: `Maria Kask`
   - Нажмите **Create**.
   - **Ожидаемый результат:** Запись появилась в таблице со всеми полями.
3. **Просмотр деталей (Details):**
   - Нажмите **Details** напротив созданной записи.
   - **Ожидаемый результат:** Отображается карточка с уникальным `Id`, названием сада, группы, воспитателем и датами.
4. **Обновление (Update):**
   - Нажмите **Update**.
   - Измените количество детей `Children Count` с `18` на `20`.
   - Нажмите **Update**.
   - **Ожидаемый результат:** Изменение отображается в списке, `UpdatedAt` обновлен.
5. **Удаление (Delete):**
   - Нажмите **Delete**, проверьте данные на странице подтверждения, нажмите кнопку **Delete**.
   - **Ожидаемый результат:** Запись удалена.

---

### Тестирование граничных случаев и ошибок (Edge Cases)
- **Запрос несуществующего Id:**
  - Введите в адресную строку: `https://localhost:7198/Kindergarten/Details/00000000-0000-0000-0000-000000000000`
  - **Ожидаемый результат:** Сервер возвращает статус `404 Not Found`.
- **Повторная отправка формы (PRG-паттерн):**
  - Создайте запись и после возврата на страницу Index нажмите `F5` (Refresh) в браузере.
  - **Ожидаемый результат:** Запись не дублируется, форма повторно не отправляется (благодаря паттерну Post-Redirect-Get).

---

## 6. Автоматизированное тестирование (Unit Testing)

Для добавления модульных тестов в решение рекомендуется использовать тестовый проект на базе **xUnit** и библиотеки **Moq** / **Microsoft.EntityFrameworkCore.InMemory**.

### 6.1. Команда создания тестового проекта (опционально)
```powershell
# Создание проекта xUnit
dotnet new xunit -n TARge25Shop.Test -f net10.0

# Добавление зависимостей
dotnet add TARge25Shop.Test/TARge25Shop.Test.csproj package Moq
dotnet add TARge25Shop.Test/TARge25Shop.Test.csproj package Microsoft.EntityFrameworkCore.InMemory
dotnet add TARge25Shop.Test/TARge25Shop.Test.csproj reference TARge25Shop.Core/TARge25Shop.Core.csproj
dotnet add TARge25Shop.Test/TARge25Shop.Test.csproj reference TARge25Shop.ApplicationServices/TARge25Shop.ApplicationServices.csproj
dotnet add TARge25Shop.Test/TARge25Shop.Test.csproj reference TARge25Shop.Data/TARge25Shop.Data.csproj

# Запуск тестов
dotnet test
```

---

## 7. Устранение возможных неполадок (Troubleshooting)

| Проблема | Причина | Решение |
|---|---|---|
| `Cannot open database "TARge25Shop"` или ошибка подключения | Служба LocalDB остановлена или не установлена | Запустите LocalDB командой: `sqllocaldb start mssqllocaldb`. Проверьте строку подключения в `appsettings.json`. |
| `dotnet-ef: command not found` | Утилита `dotnet-ef` не в `PATH` | Добавьте путь `$env:USERPROFILE\.dotnet\tools` в переменную `PATH` или вызовите утилиту напрямую: `& "$env:USERPROFILE\.dotnet\tools\dotnet-ef.exe" ...` |
| Ошибка доверия HTTPS сертификата в браузере | Не настроен доверенный сертификат ASP.NET Core | Выполните в консоли: `dotnet dev-certs https --trust` |
| Ошибки после изменения моделей | База данных не синхронизирована с миграциями | Выполните `dotnet ef database update --project TARge25Shop.Data --startup-project TARge25Shop` |
