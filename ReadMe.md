# Business kontext
Fikt�vna taxislu�ba SunTaxi eviduje svoju flotilu vozidiel v syst�me SAP. Po�iadavka je v pravideln�ch
intervaloch importova� kompletn� zoznam vozidiel zo SAPu do aplika�nej DB. Pre tento ��el
je v SAPe definovan� periodick� report, ktor� aktu�lny zoznam vozidiel vygeneruje do textov�ho 
s�boru na dohodnut� file share.

# Zadanie
Je potrebn� vytvori� konzolov� aplik�ciu, ktor� bude pravidelne sp���an� (cez Task Scheduler) a m� 
m� za �lohu na��ta� �daje o vozidl�ch zo s�boru vyexportovan�ho zo SAPu a pod�a nich aktualizova� 
v aplika�nej DB:

* Z�znamy o vozidl�ch v aplika�nej DB s� reprezentovan� classom `SunTaxi.Core.Data.Vehicle`, unik�tny
  identifik�tor z�znamu je property `PlateNumber`.
* V konfigura�nom s�bore aplik�cie `appsettings.json` bude uveden� plne kvalifikovan� cesta 
  k exportn�mu s�boru zo SAPu ako aj encoding tohto s�boru. Importova� sa m� eviden�n� ��slo E�V - `PlateNumber`
  a n�zov `Name` vozidla. Z�znamy s pr�zdnym E�V maj� by� pri importe ignorovan�. Ak sa vyskytn� 
  duplikovan� E�V, je potrebn� naimportova� posledn� na��tan� z�znam.
* E�V je v DB ulo�en� v tzv. normalizovanom tvare bez medzier a poml�iek (teda ak je napr. v export s�bore
  "YZ-043 UI" do DB by malo �s� ako "YZ043UI").
* Pr�stup k aplika�nej DB je reprezentovan� rozhran�m `SunTaxi.Core.Services.IVehicleUpdateService`,
  konzolov� aplik�cia by mala priamo (cez DI konfigur�ciu alebo priamo volan�m public kon�truktora) pou�i� 
  mock implement�ciu `SunTaxi.Core.Services.MockVehicleUpdateService`.
* Konzolov� aplik�cia vr�ti do OS k�d 0 ak v�etko prebehlo v poriadku, 1 v pr�pade v�skytu akejko�vek chyby.

Vzor exportu zo SAPu je v s�bore `SAP_EXPORT_CARS.txt` (encoding UTF-8 without BOM) a je ve�k� pravdepodobnos�,
�e form�t s�boru bude v bud�cnosti menen�, tak�e rie�enie by malo anticipova� t�to skuto�nos�.
