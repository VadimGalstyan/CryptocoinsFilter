ENG/ARM/RUS

**Cryptocoins Filter**

This is a web application built using **C#** and **ASP.NET MVC** that allows users to filter and explore cryptocurrencies across multiple exchanges. Users can select different exchanges (Binance, Bybit, Bitget, Gate.io, MEXC) and market types (Spot or Futures) to generate a customized list of coins available for trading.

The application automatically imports the latest symbol information from each exchange’s public API, storing it in a **Microsoft SQL Server** database. It supports real-time updates of available trading pairs and ensures that users can easily see which coins are available on multiple exchanges and market types.

**Key Features:**

* Filter coins by exchange and market type (Spot/Futures).
* Dynamically fetches trading pairs from multiple exchange APIs.
* Handles missing or delisted coins gracefully.
* User-friendly interface with cancel buttons to reset selections.

**Technologies Used:**

* **C#** – Main programming language for the backend logic.
* **ASP.NET MVC** – Framework used to build the web application with models, views, and controllers.
* **Microsoft SQL Server** – Database to store exchange data and symbols.
* **ADO.NET (Microsoft.Data.SqlClient)** – Used for database access and executing SQL queries directly.
* **JavaScript** – Client-side logic for handling user interactions (e.g., cancel buttons, radio selection).
* **HTML/CSS** – Frontend markup and styling.
* **Newtonsoft.Json (Json.NET)** – Used to parse JSON data from exchange APIs.

This project is ideal for cryptocurrency traders, enthusiasts, or developers who want to analyze trading pairs across multiple exchanges in one unified interface.


**Cryptocoiins Filter**

Սա վեբ հավելված է, որը կառուցված է **C#** և **ASP.NET MVC** լեզուներով և թույլ է տալիս օգտատերերին ֆիլտրել և ուսումնասիրել կրիպտոարժույթները բազմաթիվ բորսաներում: Օգտատերերը կարող են ընտրել տարբեր բորսաներ (Binance, Bybit, Bitget, Gate.io, MEXC) և շուկայի տեսակներ (Spot կամ Futures)՝ առևտրի համար հասանելի մետաղադրամների անհատականացված ցանկ ստեղծելու համար:

Հավելվածը ավտոմատ կերպով ներմուծում է վերջին խորհրդանիշների տեղեկատվությունը յուրաքանչյուր բորսայի հանրային API-ից՝ այն պահելով **Microsoft SQL Server** տվյալների բազայում: Այն աջակցում է առկա առևտրային զույգերի իրական ժամանակի թարմացումներին և ապահովում է, որ օգտատերերը հեշտությամբ տեսնեն, թե որ մետաղադրամներն են հասանելի բազմաթիվ բորսաներում և շուկաների տեսակներում:

*Հիմնական առանձնահատկություններ՝**

* Ֆիլտրում է մետաղադրամները ըստ բորսայի և շուկայի տեսակի (Spot/Futures):
* Դինամիկ կերպով վերցնում է առևտրային զույգերը բազմաթիվ բորսայի API-ներից:
* Նրբորեն մշակում է բացակայող կամ ցուցակից հանված մետաղադրամները:
* Օգտագործողի համար հարմար ինտերֆեյս՝ չեղարկման կոճակներով՝ ընտրությունները վերականգնելու համար:

*Օգտագործվող տեխնոլոգիաներ՝**

* **C#** – Հիմնական ծրագրավորման լեզու backend տրամաբանության համար:
* **ASP.NET MVC** – Շրջանակ, որն օգտագործվում է վեբ հավելվածը մոդելներով, տեսակետներով և կառավարիչներով կառուցելու համար։
* **Microsoft SQL Server** – Տվյալների բազա՝ փոխանակման տվյալներն ու խորհրդանիշները պահելու համար։
* **ADO.NET (Microsoft.Data.SqlClient)** – Օգտագործվում է տվյալների բազային մուտք գործելու և SQL հարցումներն անմիջապես կատարելու համար։
* **JavaScript** – Հաճախորդի կողմից տրվող տրամաբանություն՝ օգտատիրոջ փոխազդեցությունները կարգավորելու համար (օրինակ՝ չեղարկման կոճակներ, ռադիո ընտրություն)։
* **HTML/CSS** – Frontend-ի նշագրում և ոճավորում։
* **Newtonsoft.Json (Json.NET)** – Օգտագործվում է JSON տվյալները փոխանակման API-ներից վերլուծելու համար։

Այս նախագիծը իդեալական է կրիպտոարժույթային առևտրականների, էնտուզիաստների կամ մշակողների համար, ովքեր ցանկանում են վերլուծել առևտրային զույգերը բազմաթիվ բորսաներում մեկ միասնական ինտերֆեյսում։



**Фильтр криптовалют**

Это веб-приложение, созданное с использованием **C#** и **ASP.NET MVC**, позволяет пользователям фильтровать и просматривать криптовалюты на нескольких биржах. Пользователи могут выбирать различные биржи (Binance, Bybit, Bitget, Gate.io, MEXC) и типы рынков (спот или фьючерс), чтобы сформировать индивидуальный список монет, доступных для торговли.

Приложение автоматически импортирует актуальную информацию о символах из публичного API каждой биржи, сохраняя её в базе данных **Microsoft SQL Server**. Оно поддерживает обновление доступных торговых пар в режиме реального времени и позволяет пользователям легко просматривать доступные монеты на разных биржах и типах рынков.

**Ключевые особенности:**

* Фильтрация монет по бирже и типу рынка (спот/фьючерс).
* Динамическое извлечение торговых пар из API нескольких бирж.
* Корректная обработка отсутствующих или исключенных из листинга монет.
* Удобный интерфейс с кнопками отмены для сброса настроек.

**Использованные технологии:**

* **C#** — Основной язык программирования для внутренней логики.
* **ASP.NET MVC** — Фреймворк, используемый для создания веб-приложения с моделями, представлениями и контроллерами.
* **Microsoft SQL Server** — База данных для хранения данных и символов биржи.
* **ADO.NET (Microsoft.Data.SqlClient)** — Используется для доступа к базе данных и непосредственного выполнения SQL-запросов.
* **JavaScript** — Клиентская логика для обработки действий пользователя (например, кнопки отмены, выбор переключателей).
* **HTML/CSS** — Разметка и стилизация интерфейса.
* **Newtonsoft.Json (Json.NET)** — Используется для анализа JSON-данных из API бирж.

Этот проект идеально подходит для криптовалютных трейдеров, энтузиастов и разработчиков, которым необходимо анализировать торговые пары на нескольких биржах в одном унифицированном интерфейсе.
