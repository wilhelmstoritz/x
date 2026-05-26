# HOWTO.md - Komplétní Průvodce ASP.NET Projektům

Tento dokument vysvětluje strukturu a fungování všech projektů v tomto řešení. Je určen pro lidi, kteří nejsou obeznámeni s ASP.NET.

---

## 📋 Obsah
1. [Základní Koncept](#základní-koncept)
2. [Struktura Projektu](#struktura-projektu)
3. [Projekt: people_project](#projekt-people_project)
4. [Projekt: movies_project](#projekt-movies_project)
5. [Projekt: weather_project](#projekt-weather_project)
6. [Projekt: webapp](#projekt-webapp)
7. [Jak to Funguje - Tok Dat](#jak-to-funguje---tok-dat)
8. [Spuštění Projektu](#spuštění-projektu)

---

## 🎯 Základní Koncept

### Co je to ASP.NET?
ASP.NET je **webový framework** (nástroj pro tvorbu webů). Umožňuje vám vytvořit webovou aplikaci, kterou lidé mohou používat v prohlížeči (Chrome, Firefox, Edge).

### Model MVC (Model-View-Controller)
Všechny projekty v tomto řešení používají **MVC architekturu**, která rozděluje kód na tři části:

```
USER (Uživatel)
	↓
CONTROLLER (Řadič)
	↓ 
SERVICE/MODEL (Model, Data)
	↓
VIEW (Zobrazení - HTML)
	↓
BROWSER (Prohlížeč uživatele)
```

**Vysvětlení:**
- **CONTROLLER** (Řadič) - Přijímá požadavky od uživatele, zpracovává je
- **MODEL** (Model) - Datové struktury (třídy) a jejich logika
- **VIEW** (Zobrazení) - HTML šablony, které se zobrazují v prohlížeči

### Příklad: Uživatel si chce prohlédnout seznam lidí

1. Uživatel klikne na "Seznam lidí" v prohlížeči
2. Browser pošle požadavek na server
3. **Controller** se zeptá: "Komu patří tento požadavek?" → Řekne: "HomeController"
4. **Service** přípraví data (seznam lidí)
5. **View** vezme tato data a vytvoří HTML
6. HTML se pošle do prohlížeče a uživatel vidí seznam

---

## 📁 Struktura Projektu

Každý projekt má následující složky (direktoria):

```
projekt_name/
├── Controllers/          ← Řadiče (přijímají požadavky)
├── Models/               ← Datové struktury
├── Services/             ← Služby (logika, API volání, databáze)
├── Views/                ← HTML šablony (zobrazení)
├── wwwroot/              ← Statické soubory (CSS, JS, obrázky)
├── Data/                 ← Datové soubory (JSON, databáze)
├── Properties/           ← Nastavení projektu
├── Program.cs            ← Hlavní soubor - spuštění aplikace
├── appsettings.json      ← Konfigurace aplikace
└── projekt_name.csproj   ← Popis projektu (jaké balíčky potřebuje)
```

### Detaily jednotlivých složek:

#### **Controllers/** (Řadiče)
- Obsahuje soubory typu `*Controller.cs`
- **Úloha**: Přijímá HTTP požadavky od uživatele a rozhoduje, co udělat
- **Příklad**: `HomeController.cs` - obsługuje požadavky pro domovskou stránku
- **Logika**: 
  ```csharp
  // Uživatel chce seznam - controller zavolá service pro data
  public IActionResult Index()
  {
	  var data = service.GetData();  // Žádá data
	  return View(data);              // Pošle data do View
  }
  ```

#### **Models/** (Modely)
- Datové struktury (třídy)
- **Dva typy:**
  1. **Entity Models** - představují data (např. `person_item.cs`, `movie_item.cs`)
  2. **View Models** - speciální modely pro zobrazení (např. `people_index_view_model.cs`)
- **Příklad** - osoba:
  ```csharp
  public class person_item
  {
	  public int id { get; set; }
	  public string jmeno { get; set; }
	  public string prijmeni { get; set; }
	  public DateOnly datum_narozeni { get; set; }
	  // ...
  }
  ```

#### **Services/** (Služby)
- Obsahuje business logiku
- **Příklady:**
  - `people_store.cs` - čte/zapisuje data do JSON souboru
  - `movie_api_service.cs` - volá externí API
  - `weather_api_service.cs` - volá externí API
- **Úloha**: Odděluje logiku od controlleru

#### **Views/** (Zobrazení)
- HTML šablony s C# kódem (`.cshtml` soubory)
- **Struktura:**
  ```
  Views/
  ├── Home/
  │   ├── Index.cshtml
  │   ├── Details.cshtml
  │   └── ...
  ├── Shared/
  │   ├── _Layout.cshtml       ← Hlavní šablona (menu, footer)
  │   └── _ValidationScriptsPartial.cshtml
  └── Error.cshtml
  ```
- **Příklad - Index.cshtml:**
  ```html
  @model people_index_view_model

  <h1>Seznam lidí</h1>
  @foreach (var person in Model.Items)
  {
	  <p>@person.jmeno @person.prijmeni</p>
  }
  ```

#### **wwwroot/** (Web root - veřejné soubory)
- CSS, JavaScript, obrázky, které jsou viditelné pro všechny
- **Složka `lib/`** - obsahuje knihovny (Bootstrap, jQuery)
- **Vlastní CSS/JS** - vlastní styly a skripty

#### **Data/** (Datové soubory)
- JSON soubory s daty (např. `people_data.json`)
- Někdy zde bývá SQLite databáze

#### **Program.cs** (Spouštění)
- **Nejdůležitější soubor!** Zde se aplikace konfiguruje
- Zaregistrují se služby, nastaví se routy, middleware...
- Spustí se web server

---

## 🧑‍💼 Projekt: people_project

### Účel
Aplikace pro správu seznamu lidí. Můžete:
- Prohlížet všechny lidi
- Hledat podle jména/příjmení
- Přidávat nové lidi
- Editovat údaje
- Mazat lidi
- Zobrazit statistiky

### Soubory a struktura

```
people_project/
├── Controllers/
│   └── HomeController.cs          ← Hlavní řadič
├── Models/
│   ├── person_item.cs             ← Osoba (jméno, příjmení, datum narození...)
│   ├── people_index_view_model.cs  ← Model pro seznam lidí
│   └── people_statistics_view_model.cs ← Model pro statistiky
├── Services/
│   └── people_store.cs            ← Logika pro čtení/zápis do JSON
├── Views/
│   └── Home/
│       ├── Index.cshtml            ← Seznam lidí
│       ├── Create.cshtml           ← Formulář pro přidání
│       ├── Edit.cshtml             ← Formulář pro úpravu
│       ├── Details.cshtml          ← Detail jedné osoby
│       ├── Delete.cshtml           ← Potvrzení smazání
│       └── Statistics.cshtml       ← Statistiky
├── Data/
│   └── people_data.json           ← Data se všemi lidmi
└── Program.cs                      ← Konfigurace
```

### Jak funguje:

#### **1. Uživatel otevře `/Home/Index` (seznam lidí)**

```
1. Browser → Server: "Dej mi seznam lidí"
   ↓
2. Controller (HomeController.cs):
   - Zavolá `people_store.get_all()`
   - Vrátí seznam všech lidí
   ↓
3. Service (people_store.cs):
   - Přečte soubor `Data/people_data.json`
   - Parsuje JSON do objektů `person_item`
   - Vrátí seznam controlleru
   ↓
4. Controller:
   - Vezme seznam
   - Připraví ViewModel `people_index_view_model`
   - Pošle do View
   ↓
5. View (Index.cshtml):
   - Vezme data
   - Vytvoří HTML (tabulka se jmény)
   ↓
6. HTML jde do prohlížeče → Uživatel vidí seznam
```

#### **2. Soubor: Controllers/HomeController.cs**

```csharp
public class HomeController : Controller
{
	private readonly people_store m_people_store;

	// Dependency Injection - dostane instanci people_store
	public HomeController(people_store t_people_store)
	{
		m_people_store = t_people_store;
	}

	// /Home/Index - seznam lidí
	public IActionResult Index(string? search_jmeno)
	{
		var l_people = m_people_store.get_all();  // Získá lidi ze služby

		// Filtruje podle vyhledávání
		if (!string.IsNullOrWhiteSpace(search_jmeno))
		{
			l_people = l_people.Where(t => t.jmeno.Contains(search_jmeno)).ToList();
		}

		return View(l_people);  // Pošle do View
	}

	// /Home/Create - vytvoří nového člověka
	[HttpPost]
	public IActionResult Create(person_item t_person)
	{
		m_people_store.add(t_person);  // Přidá do služby
		return RedirectToAction("Index");  // Přesměruje zpět na seznam
	}
}
```

#### **3. Soubor: Models/person_item.cs**

```csharp
public class person_item
{
	public int id { get; set; }                    // ID
	public string jmeno { get; set; } = "";        // Jméno
	public string prijmeni { get; set; } = "";     // Příjmení
	public DateOnly datum_narozeni { get; set; }   // Datum narození
	public int vyska_cm { get; set; }              // Výška v cm
	public decimal vaha_kg { get; set; }           // Váha v kg
	public string mesto { get; set; } = "";        // Město
}
```

#### **4. Soubor: Services/people_store.cs**

```csharp
public class people_store
{
	private readonly string m_data_file_path;  // Cesta k JSON souboru

	public people_store(IWebHostEnvironment t_environment)
	{
		// Najde cestu: projekt/Data/people_data.json
		var l_data_directory = Path.Combine(t_environment.ContentRootPath, "Data");
		Directory.CreateDirectory(l_data_directory);
		m_data_file_path = Path.Combine(l_data_directory, "people_data.json");
	}

	// Přečte všechny lidi z JSON souboru
	public List<person_item> get_all()
	{
		if (!File.Exists(m_data_file_path))
			return new List<person_item>();

		var l_json = File.ReadAllText(m_data_file_path);
		return JsonSerializer.Deserialize<List<person_item>>(l_json) ?? new List<person_item>();
	}

	// Přidá nového člověka a uloží do JSON
	public void add(person_item t_person)
	{
		var l_people = get_all();
		t_person.id = l_people.Max(t => t.id) + 1;  // Nové ID
		l_people.Add(t_person);
		save_all(l_people);
	}

	// Uloží všechny lidi do JSON souboru
	private void save_all(List<person_item> t_people)
	{
		var l_json = JsonSerializer.Serialize(t_people, new JsonSerializerOptions { WriteIndented = true });
		File.WriteAllText(m_data_file_path, l_json);
	}
}
```

#### **5. Soubor: Data/people_data.json**

Vypadá takto:

```json
[
  {
	"id": 1,
	"jmeno": "Jan",
	"prijmeni": "Novák",
	"datum_narozeni": "1990-05-15",
	"vyska_cm": 180,
	"vaha_kg": 75.5,
	"mesto": "Praha"
  },
  {
	"id": 2,
	"jmeno": "Marie",
	"prijmeni": "Svobodová",
	"datum_narozeni": "1995-03-20",
	"vyska_cm": 165,
	"vaha_kg": 60.0,
	"mesto": "Brno"
  }
]
```

#### **6. Soubor: Views/Home/Index.cshtml**

```html
@model List<person_item>

<h1>Seznam lidí</h1>

<form method="get">
	<input name="search_jmeno" placeholder="Hledej podle jména..." />
	<button>Hledat</button>
</form>

<table>
	<tr>
		<th>Jméno</th>
		<th>Příjmení</th>
		<th>Datum narození</th>
		<th>Akce</th>
	</tr>
	@foreach (var person in Model)
	{
		<tr>
			<td>@person.jmeno</td>
			<td>@person.prijmeni</td>
			<td>@person.datum_narozeni</td>
			<td>
				<a href="/Home/Details/@person.id">Detail</a>
				<a href="/Home/Edit/@person.id">Upravit</a>
				<a href="/Home/Delete/@person.id">Smazat</a>
			</td>
		</tr>
	}
</table>

<a href="/Home/Create">Přidat nového člověka</a>
```

---

## 🎬 Projekt: movies_project

### Účel
Aplikace, která zobrazuje filmy z **Ghibli API** (veřejné API s filmy od studia Ghibli).

### Klíčová rozdíl od people_project:
- **Data pochází z externího API**, ne z JSON souboru
- **API** = vzdálený server, který poskytuje data

### Soubory

```
movies_project/
├── Controllers/
│   └── HomeController.cs
├── Models/
│   ├── movie_item.cs              ← Struktura jednoho filmu
│   ├── movies_index_view_model.cs ← Model pro seznam
│   └── movie_statistics_view_model.cs
├── Services/
│   └── movie_api_service.cs       ← Komunikace s API
└── Views/
	└── Home/
		├── Index.cshtml
		├── Details.cshtml
		└── Statistics.cshtml
```

### Jak funguje:

#### **1. Kontrola: Services/movie_api_service.cs**

```csharp
public class movie_api_service
{
	private readonly HttpClient m_http_client;

	// Base URL je "https://ghibliapi.vercel.app/films/"
	public movie_api_service(HttpClient t_http_client)
	{
		m_http_client = t_http_client;
	}

	// Zavolá API a vrátí filmy
	public async Task<List<movie_item>> get_all_movies()
	{
		// HTTP GET na: https://ghibliapi.vercel.app/films/
		var l_response = await m_http_client.GetAsync("");

		if (!l_response.IsSuccessStatusCode)
			return new List<movie_item>();

		var l_json = await l_response.Content.ReadAsStringAsync();

		// Parsuje JSON odpověď
		return JsonSerializer.Deserialize<List<movie_item>>(l_json) ?? new List<movie_item>();
	}
}
```

#### **2. Program.cs - Registrace služby**

```csharp
// Zaregistruje HttpClient s Base URL
builder.Services.AddHttpClient<movie_api_service>(t_client =>
{
	t_client.BaseAddress = new Uri("https://ghibliapi.vercel.app/films/");
});
```

#### **3. Model: Models/movie_item.cs**

```csharp
public class movie_item
{
	public string id { get; set; } = string.Empty;
	public string title { get; set; } = string.Empty;
	public string original_title { get; set; } = string.Empty;
	public int release_date { get; set; }
	public string director { get; set; } = string.Empty;
	// ... další vlastnosti
}
```

#### **4. Controller: Controllers/HomeController.cs**

```csharp
public class HomeController : Controller
{
	private readonly movie_api_service m_api_service;

	public HomeController(movie_api_service t_api_service)
	{
		m_api_service = t_api_service;
	}

	// /Home/Index - seznam filmů
	public async Task<IActionResult> Index()
	{
		// Volá API asynchronně (čeká na odpověď)
		var l_movies = await m_api_service.get_all_movies();
		return View(l_movies);
	}
}
```

**Důležité**: Slovo `async` a `await` znamená, že server čeká na odpověď od API, aniž by blokoval ostatní požadavky.

---

## 🌤️ Projekt: weather_project

### Účel
Aplikace pro zobrazení **aktuálního počasí** z veřejného API (wttr.in).

### Klíčová vlastnost:
- Volá API: `https://wttr.in/{města}` 
- Vrátí JSON s předpověď počasí
- Parsuje složitou JSON strukturu

### Struktura

```
weather_project/
├── Controllers/
│   └── HomeController.cs
├── Models/
│   ├── weather_current.cs              ← Aktuální počasí
│   ├── weather_location.cs             ← Poloha
│   ├── weather_index_view_model.cs     ← ViewModel
│   └── ... (více modelů pro API)
├── Services/
│   └── weather_api_service.cs
└── Views/
	└── Home/
		└── Index.cshtml
```

### Příklad - Services/weather_api_service.cs

```csharp
public class weather_api_service
{
	private readonly HttpClient m_http_client;

	public weather_api_service(HttpClient t_http_client)
	{
		m_http_client = t_http_client;
	}

	// Voláno takto: https://wttr.in/Praha?format=j1
	public async Task<weather_api_response?> get_weather(string t_city)
	{
		var l_url = $"{t_city}?format=j1";  // Přidá parametry
		var l_response = await m_http_client.GetAsync(l_url);

		if (!l_response.IsSuccessStatusCode)
			return null;

		var l_json = await l_response.Content.ReadAsStringAsync();
		return JsonSerializer.Deserialize<weather_api_response>(l_json);
	}
}
```

---

## 💼 Projekt: webapp

### Účel
Klasická **MVC webová aplikace** bez API. Používá vlastní data a logiku.

### Struktura

```
webapp/
├── Controllers/
├── Models/
├── Views/
├── wwwroot/
└── Program.cs
```

**Poznámka**: Tento projekt je základní šablona ASP.NET MVC. Můžete ho rozšířit podle potřeby.

---

## 🔄 Jak to Funguje - Tok Dat

### Scénář 1: people_project - Uživatel přidá nového člověka

```
┌─────────────────────────────────────┐
│  Uživatel otevře Index.cshtml       │
│  Klikne na "Přidat nového člověka"  │
└──────────────┬──────────────────────┘
			   │
			   ↓
┌─────────────────────────────────────┐
│  Browser pošle POST na /Home/Create │
│  s daty: jmeno="Jan", prijmeni="Novák" ...
└──────────────┬──────────────────────┘
			   │
			   ↓
┌─────────────────────────────────────┐
│  HomeController.Create() dostane     │
│  data v parametru person_item       │
└──────────────┬──────────────────────┘
			   │
			   ↓
┌─────────────────────────────────────┐
│  Controller volá:                   │
│  m_people_store.add(person_item)    │
└──────────────┬──────────────────────┘
			   │
			   ↓
┌─────────────────────────────────────┐
│  Services:                          │
│  1. Přečte Data/people_data.json    │
│  2. Přidá nového člověka            │
│  3. Uloží zpět do JSON              │
└──────────────┬──────────────────────┘
			   │
			   ↓
┌─────────────────────────────────────┐
│  Controller vrátí:                  │
│  RedirectToAction("Index")          │
│  (Přesměruje zpět na seznam)        │
└──────────────┬──────────────────────┘
			   │
			   ↓
┌─────────────────────────────────────┐
│  Browser vidí seznam s novým        │
│  člověkem                           │
└─────────────────────────────────────┘
```

### Scénář 2: movies_project - Uživatel si chce prohlédnout filmy

```
┌─────────────────────────────────────┐
│  Uživatel otevře /Home/Index        │
└──────────────┬──────────────────────┘
			   │
			   ↓
┌─────────────────────────────────────┐
│  HomeController.Index() se zavolá   │
│  (async Task<IActionResult>)        │
└──────────────┬──────────────────────┘
			   │
			   ↓
┌─────────────────────────────────────┐
│  Controller volá:                   │
│  await m_api_service.get_all_movies()
└──────────────┬──────────────────────┘
			   │
			   ↓
┌─────────────────────────────────────┐
│  Service volá:                      │
│  https://ghibliapi.vercel.app/films/│
│  (HTTP GET požadavek)               │
└──────────────┬──────────────────────┘
			   │
			   ↓ (čeká na odpověď)
			   │
			   ↓
┌─────────────────────────────────────┐
│  API server vrátí JSON:             │
│  [{"id":"1","title":"Spirited..."}] │
└──────────────┬──────────────────────┘
			   │
			   ↓
┌─────────────────────────────────────┐
│  Service parsuje JSON               │
│  → List<movie_item>                 │
└──────────────┬──────────────────────┘
			   │
			   ↓
┌─────────────────────────────────────┐
│  Controller vrátí:                  │
│  View(movies)                       │
└──────────────┬──────────────────────┘
			   │
			   ↓
┌─────────────────────────────────────┐
│  View (Index.cshtml) zobrazuje      │
│  seznam filmů v HTML                │
└──────────────┬──────────────────────┘
			   │
			   ↓
┌─────────────────────────────────────┐
│  Browser zobrazí seznam s filmy     │
└─────────────────────────────────────┘
```

---

## ▶️ Spuštění Projektu

### 1. V Visual Studio

```
1. Otevřete řešení (Solution)
2. V Solution Exploreru klikněte na projekt (např. people_project)
3. Klikněte na tlačítko Play (▶️ Start Debugging)
   nebo stiskněte F5
4. Aplikace se spustí na http://localhost:5000 (nebo podobná adresa)
5. Browser se otevře a zobrazí aplikaci
```

### 2. Z příkazové řádky

```powershell
# Jděte do adresáře projektu
cd E:\working-area\visual-studio\x\people_project

# Spusťte projekt
dotnet run

# Aplikace bude na http://localhost:5000
```

### 3. URL struktura

```
https://localhost:5000/Home/Index          → Domovská stránka / seznam
https://localhost:5000/Home/Create         → Formulář pro nový záznam
https://localhost:5000/Home/Edit/1         → Editace záznamu s ID=1
https://localhost:5000/Home/Details/1      → Detail záznamu s ID=1
https://localhost:5000/Home/Delete/1       → Smazání záznamu s ID=1
```

**Formát URL:**
```
https://localhost:5000/{Controller}/{Action}/{id}
```

---

## 🔧 Program.cs - Kdy se co zaregistruje?

### people_project/Program.cs

```csharp
var builder = WebApplication.CreateBuilder(args);

// 1. Přidá Controllers + Views
builder.Services.AddControllersWithViews();

// 2. Zaregistruje people_store jako Singleton
//    (bude instance jen jedna pro celou aplikaci)
builder.Services.AddSingleton<people_project.Services.people_store>();

var app = builder.Build();

// Middleware (zpracování požadavků)
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();  // Přesměruj na HTTPS
app.UseRouting();            // Směrování
app.UseAuthorization();      // Autorizace
app.UseStaticFiles();        // Veřejné soubory (CSS, JS)

// Výchozí ruta: /Home/Index
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();  // Spusť aplikaci
```

---

## 📚 Shrnutí

| Soubor | Účel |
|--------|------|
| **Program.cs** | Konfigurace a spuštění |
| **Controllers/** | Obslužné třídy (logika) |
| **Models/** | Datové struktury |
| **Services/** | Business logika (API, data) |
| **Views/** | HTML šablony |
| **wwwroot/** | CSS, JS, obrázky |
| **Data/** | JSON datové soubory |
| **appsettings.json** | Konfigurace |

---

## 🎓 Závěr

Teď byste měli pochopit:
✅ Jak se projekty strukturují
✅ Co dělá Controller, Model, View
✅ Jak data tekou skrz aplikaci
✅ Rozdíly mezi projects (API vs. JSON)
✅ Jak spustit projekt

**Pro více informací:**
- [Microsoft Docs - ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/)
- [Razor Pages - Microsoft](https://learn.microsoft.com/en-us/aspnet/core/razor-pages/)
- [MVC Pattern](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93controller)

