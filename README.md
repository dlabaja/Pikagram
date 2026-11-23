# Pikagram
Nejpoužívanější Instagram klon v Přerově a Ústeckém kraji

## Co všechno umí?
- Homepage s příspěvky uživatelů
    - filtr podle autora (moje/všechny/olajkované/od lidí co sleduju)
- Tvorba příspěvků (obrázek/video, popisek, tagy)
- Interakce s příspěvky (like, komentář)
- Správa příspěvků (edit, delete) a komentářů (edit)
- Hledání uživatelů
- Uživatelský profil a jeho správa (background, avatar, bio, seznam příspěvků a komentářů)
- Soukromé zprávy (přes tlačítko na profilu)
- Systém sledujících
- Register/Login/Change Password

## 1 AM notes
### Endpointy
- /api používá AJAX (lajky, follow)
- zbytek používá klasický MVC

### Styly
Chybí mi JS bundlery a CSS moduly, ale snažil jsem se poradit i bez toho.

Všechny Komponenty (partial) mají vlastní CSS soubor, který ASP.NET umí bundlovat do hlavičky layoutu -> šetří místo. 
Z nějakého důvodu to ale má problémy se stránkami (některá třída se neaplikuje, je to docela random), takže tam používám inline styly přímo v šabloně.
