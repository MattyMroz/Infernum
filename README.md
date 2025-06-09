# PoliŁPG 🎓 - Symulator Studenta Politechniki

Witaj w oficjalnym repozytorium gry **PoliŁPG**! Jest to unikalny, dwuosobowy symulator życia studenta, w którym gracze rywalizują o przetrwanie kolejnego semestru na pełnej wyzwań politechnice.

## 🎯 Założenia i Główne Mechaniki Gry

Projekt opiera się na następujących założeniach, które definiują rozgrywkę:

*   **🎮 Rozgrywka 1vs1:** Gra jest przeznaczona dla dokładnie dwóch graczy w trybie rywalizacji na podzielonym ekranie (split-screen).
*   **🗺️ Eksploracja Świata:** Gracze poruszają się po mapach inspirowanych terenem politechniki (m.in. budynki WEEIA, CTI), obserwując akcję z perspektywy top-down (z góry).
*   **🧠 Rozwój Postaci:** Kluczem do sukcesu jest rozwijanie statystyk postaci (wiedza z matematyki, informatyki, itp.) poprzez granie w dedykowane minigry.
*   **🎲 Mechanika Egzaminów:** Zdanie egzaminu opiera się na rzucie kostką, a szanse na sukces są wprost proporcjonalne do poziomu wiedzy zdobytej w minigrach.
*   **🌍 Interaktywny Świat:** Mapa jest pełna obiektów, z którymi można wejść w interakcję, takich jak teleporty, kosze na śmieci, czy jedzenie wpływające na statystyki.
*   **🏆 Cel Gry:** Celem każdego gracza jest zaliczenie jak największej liczby egzaminów i przetrwanie semestru.

## 🔧 Wymagania Techniczne

Do uruchomienia i dalszego rozwoju projektu wymagane jest następujące oprogramowanie:

*   **Silnik Gry:** Unity 6 LTS (wersja `6000.0.42f1`)
*   **Język Programowania:** C#

## 🏛️ Architektura Systemu - Diagram Klas UML

Poniższy diagram przedstawia kluczowe klasy systemu oraz relacje między nimi (dziedziczenie, kompozycja, asocjacja). Został on stworzony w celu logicznego zobrazowania struktury gry.

Możesz wygenerować wizualizację diagramu, kopiując poniższy kod i wklejając go na stronie:
**[https://yuml.me/diagram/scruffy/class/draw](https://yuml.me/diagram/scruffy/class/draw)**

✍️ **Kod Diagramu:**
```yuml
[Gra]1 ++- 2[Gracz],
[Gra]1 ++- 1[Mapa],
[Gra]1 ++- 2[Kamera],
[Gra]1 ++- 1[UI],
[Mapa]1 ++- 1..*[ObiektInteraktywny],
[Gracz]1 ++- 1[KartaStatystyk],
[Gracz]1 ++- 1[KartaOcen],
[KartaOcen]1 ++- 0..*[Ocena],
[Ocena]1 - 1[Przedmiot],
[Egzamin]1 - 1[Przedmiot],
[Kamera]1 - 1[Gracz],
[Gracz]1 - 0..1[ObiektInteraktywny],
[ObiektInteraktywny]^[Egzamin],
[ObiektInteraktywny]^[Jedzenie],
[ObiektInteraktywny]^[Teleport],
[ObiektInteraktywny]^[Kosz],
[ObiektInteraktywny]^[Minigra],
[Minigra]^[MinigraInf],
[Minigra]^[MinigraElek],
[Minigra]^[MinigraMat],
[Minigra]^[MinigraProg],
[Minigra]^[MinigraGraf]
```

![Diagram UML Architektury Gry](https://yuml.me/3031a431.jpg)


## 🕹️ Jak Grać?

1.  Uruchom grę.
2.  Dwóch graczy steruje swoimi postaciami za pomocą przypisanych klawiszy.
3.  Eksploruj mapę w poszukiwaniu minigier i obiektów interaktywnych.
4.  Graj w minigry, aby podnosić statystyki wiedzy.
5.  Gdy poczujesz się na siłach, podejdź do obiektu `Egzamin`, aby spróbować go zaliczyć.
6.  Zarządzaj swoimi statystykami i przetrwaj semestr!

---
*Dokumentacja stworzona na potrzeby projektu z Podstaw Inżynierii Oprogramowania.*


