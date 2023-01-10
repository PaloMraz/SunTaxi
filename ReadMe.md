# Business kontext
Fiktívna taxisluba SunTaxi eviduje svoju flotilu vozidiel v systéme SAP. Poiadavka je v pravidelnıch
intervaloch importova kompletnı zoznam vozidiel zo SAPu do aplikaènej DB. Pre tento úèel
je v SAPe definovanı periodickı report, ktorı aktuálny zoznam vozidiel vygeneruje do textového 
súboru na dohodnutı file share.

# Zadanie
Je potrebné vytvori konzolovú aplikáciu, ktorá bude pravidelne spúšaná (cez Task Scheduler) a má 
má za úlohu naèíta údaje o vozidlách zo súboru vyexportovaného zo SAPu a pod¾a nich aktualizova 
v aplikaènej DB:

* Záznamy o vozidlách v aplikaènej DB sú reprezentované classom `SunTaxi.Core.Data.Vehicle`, unikátny
  identifikátor záznamu je property `PlateNumber`.
* V konfiguraènom súbore aplikácie `appsettings.json` bude uvedená plne kvalifikovaná cesta 
  k exportnému súboru zo SAPu ako aj encoding tohto súboru. Importova sa má evidenèné èíslo EÈV - `PlateNumber`
  a názov `Name` vozidla. Záznamy s prázdnym EÈV majú by pri importe ignorované. Ak sa vyskytnú 
  duplikované EÈV, je potrebné naimportova poslednı naèítanı záznam.
* EÈV je v DB uloené v tzv. normalizovanom tvare bez medzier a pomlèiek (teda ak je napr. v export súbore
  "YZ-043 UI" do DB by malo ís ako "YZ043UI").
* Prístup k aplikaènej DB je reprezentovanı rozhraním `SunTaxi.Core.Services.IVehicleUpdateService`,
  konzolová aplikácia by mala priamo (cez DI konfiguráciu alebo priamo volaním public konštruktora) poui 
  mock implementáciu `SunTaxi.Core.Services.MockVehicleUpdateService`.
* Konzolová aplikácia vráti do OS kód 0 ak všetko prebehlo v poriadku, 1 v prípade vıskytu akejko¾vek chyby.

Vzor exportu zo SAPu je v súbore `SAP_EXPORT_CARS.txt` (encoding UTF-8 without BOM) a je ve¾ká pravdepodobnos,
e formát súboru bude v budúcnosti menenı, take riešenie by malo anticipova túto skutoènos.
