/* =====================================================================
   SerieA örnek veri dosyası
   ---------------------------------------------------------------------
   Kullanım:
   1) Package Manager Console'da:  Update-Database   (SerieADb ve tablolar oluşur)
   2) Bu dosyayı SQL Server Management Studio'da SerieADb üzerinde çalıştır.
   İçerik: 20 takım, 3 haftalık fikstür (30 maç), 3 detaylı maç (gol, kart, değişiklik)
   Veri zaten varsa tekrar eklemez.
   ===================================================================== */

USE SerieADb;
GO

IF EXISTS (SELECT 1 FROM Teams)
BEGIN
    PRINT 'Teams tablosunda zaten veri var, örnek veri eklenmedi.';
    RETURN;
END

/* ---------- 1) TAKIMLAR (20 takım) ---------- */
SET IDENTITY_INSERT Teams ON;

INSERT INTO Teams (Id, Name, ShortName, LogoUrl, City, Stadium) VALUES
(1,  N'Atalanta',      N'ATA', N'', N'Bergamo',       N'Gewiss Stadium'),
(2,  N'Bologna',       N'BOL', N'', N'Bologna',       N'Renato Dall''Ara'),
(3,  N'Cagliari',      N'CAG', N'', N'Cagliari',      N'Unipol Domus'),
(4,  N'Como',          N'COM', N'', N'Como',          N'Giuseppe Sinigaglia'),
(5,  N'Cremonese',     N'CRE', N'', N'Cremona',       N'Giovanni Zini'),
(6,  N'Fiorentina',    N'FIO', N'', N'Floransa',      N'Artemio Franchi'),
(7,  N'Genoa',         N'GEN', N'', N'Cenova',        N'Luigi Ferraris'),
(8,  N'Hellas Verona', N'VER', N'', N'Verona',        N'Marcantonio Bentegodi'),
(9,  N'Inter',         N'INT', N'', N'Milano',        N'San Siro'),
(10, N'Juventus',      N'JUV', N'', N'Torino',        N'Allianz Stadium'),
(11, N'Lazio',         N'LAZ', N'', N'Roma',          N'Olimpico'),
(12, N'Lecce',         N'LEC', N'', N'Lecce',         N'Via del Mare'),
(13, N'Milan',         N'MIL', N'', N'Milano',        N'San Siro'),
(14, N'Napoli',        N'NAP', N'', N'Napoli',        N'Diego Armando Maradona'),
(15, N'Parma',         N'PAR', N'', N'Parma',         N'Ennio Tardini'),
(16, N'Pisa',          N'PIS', N'', N'Pisa',          N'Arena Garibaldi'),
(17, N'Roma',          N'ROM', N'', N'Roma',          N'Olimpico'),
(18, N'Sassuolo',      N'SAS', N'', N'Reggio Emilia', N'Mapei Stadium'),
(19, N'Torino',        N'TOR', N'', N'Torino',        N'Olimpico Grande Torino'),
(20, N'Udinese',       N'UDI', N'', N'Udine',         N'Bluenergy Stadium');

SET IDENTITY_INSERT Teams OFF;

/* ---------- 2) MAÇLAR (3 hafta x 10 maç = 30 maç) ---------- */
/* Status: 0 = Başlamadı, 1 = Canlı, 2 = Tamamlandı, 3 = Ertelendi */
SET IDENTITY_INSERT Matches ON;

INSERT INTO Matches (Id, HomeTeamId, AwayTeamId, Week, MatchDate, HomeScore, AwayScore, Status, Stadium) VALUES
/* 1. Hafta */
(1,  9,  3,  1, '2026-08-22T18:30:00', 3, 0, 2, N'San Siro'),
(2,  13, 16, 1, '2026-08-22T20:45:00', 2, 1, 2, N'San Siro'),
(3,  10, 15, 1, '2026-08-23T18:30:00', 1, 1, 2, N'Allianz Stadium'),
(4,  14, 18, 1, '2026-08-23T20:45:00', 2, 0, 2, N'Diego Armando Maradona'),
(5,  17, 6,  1, '2026-08-23T20:45:00', 1, 0, 2, N'Olimpico'),
(6,  11, 19, 1, '2026-08-23T18:30:00', 2, 2, 2, N'Olimpico'),
(7,  1,  12, 1, '2026-08-22T15:00:00', 3, 1, 2, N'Gewiss Stadium'),
(8,  2,  8,  1, '2026-08-22T15:00:00', 1, 0, 2, N'Renato Dall''Ara'),
(9,  4,  7,  1, '2026-08-23T15:00:00', 0, 0, 2, N'Giuseppe Sinigaglia'),
(10, 20, 5,  1, '2026-08-23T15:00:00', 2, 1, 2, N'Bluenergy Stadium'),
/* 2. Hafta */
(11, 16, 9,  2, '2026-08-29T18:30:00', 0, 2, 2, N'Arena Garibaldi'),
(12, 13, 10, 2, '2026-08-29T20:45:00', 1, 1, 2, N'San Siro'),
(13, 3,  14, 2, '2026-08-30T20:45:00', 0, 1, 2, N'Unipol Domus'),
(14, 6,  11, 2, '2026-08-30T18:30:00', 1, 2, 2, N'Artemio Franchi'),
(15, 17, 1,  2, '2026-08-30T20:45:00', 2, 2, 2, N'Olimpico'),
(16, 18, 2,  2, '2026-08-29T15:00:00', 1, 3, 2, N'Mapei Stadium'),
(17, 19, 4,  2, '2026-08-29T15:00:00', 1, 0, 2, N'Olimpico Grande Torino'),
(18, 7,  20, 2, '2026-08-30T15:00:00', 2, 1, 2, N'Luigi Ferraris'),
(19, 12, 15, 2, '2026-08-30T15:00:00', 1, 1, 2, N'Via del Mare'),
(20, 8,  5,  2, '2026-08-30T15:00:00', 0, 2, 2, N'Marcantonio Bentegodi'),
/* 3. Hafta */
(21, 9,  13, 3, '2026-09-12T20:45:00', 2, 1, 2, N'San Siro'),
(22, 10, 14, 3, '2026-09-13T20:45:00', 0, 0, 2, N'Allianz Stadium'),
(23, 1,  11, 3, '2026-09-12T18:30:00', 1, 1, 2, N'Gewiss Stadium'),
(24, 17, 2,  3, '2026-09-13T18:30:00', 3, 1, 2, N'Olimpico'),
(25, 6,  19, 3, '2026-09-13T15:00:00', 2, 0, 2, N'Artemio Franchi'),
(26, 3,  7,  3, '2026-09-12T15:00:00', 1, 2, 2, N'Unipol Domus'),
(27, 4,  12, 3, '2026-09-13T15:00:00', 2, 0, 2, N'Giuseppe Sinigaglia'),
(28, 15, 18, 3, '2026-09-12T15:00:00', 0, 1, 2, N'Ennio Tardini'),
(29, 16, 5,  3, '2026-09-13T15:00:00', 1, 1, 2, N'Arena Garibaldi'),
(30, 8,  20, 3, '2026-09-13T15:00:00', 1, 2, 2, N'Marcantonio Bentegodi');

SET IDENTITY_INSERT Matches OFF;

/* ---------- 3) DETAYLI MAÇ 1: Inter 3-0 Cagliari (MatchId = 1) ---------- */
INSERT INTO MatchGoals (MatchId, TeamId, PlayerName, Minute) VALUES
(1, 9, N'Lautaro Martínez', 23),
(1, 9, N'Marcus Thuram',    58),
(1, 9, N'Hakan Çalhanoğlu', 81);

/* CardType: 0 = Sarı, 1 = Kırmızı */
INSERT INTO MatchCards (MatchId, TeamId, PlayerName, Minute, CardType) VALUES
(1, 3, N'Gianluca Gaetano', 34, 0),
(1, 9, N'Nicolò Barella',   72, 0),
(1, 3, N'Yerry Mina',       88, 1);

INSERT INTO Substitutions (MatchId, TeamId, PlayerIn, PlayerOut, Minute) VALUES
(1, 3, N'Zito Luvumbo',    N'Gianluca Gaetano', 46),
(1, 9, N'Pio Esposito',    N'Marcus Thuram',    70),
(1, 9, N'Piotr Zieliński', N'Nicolò Barella',   75),
(1, 3, N'Michel Adopo',    N'Marco Palestra',   78);

/* ---------- 4) DETAYLI MAÇ 2: Milan 2-1 Pisa (MatchId = 2) ---------- */
INSERT INTO MatchGoals (MatchId, TeamId, PlayerName, Minute) VALUES
(2, 16, N'Matteo Tramoni',     12),
(2, 13, N'Rafael Leão',        41),
(2, 13, N'Christian Pulisic',  77);

INSERT INTO MatchCards (MatchId, TeamId, PlayerName, Minute, CardType) VALUES
(2, 16, N'Marius Marin',       29, 0),
(2, 13, N'Tijjani Reijnders',  55, 0),
(2, 16, N'Antonio Caracciolo', 84, 1);

INSERT INTO Substitutions (MatchId, TeamId, PlayerIn, PlayerOut, Minute) VALUES
(2, 13, N'Christopher Nkunku', N'Rafael Leão',       63),
(2, 16, N'Mehdi Léris',        N'Matteo Tramoni',    66),
(2, 13, N'Samuel Chukwueze',   N'Christian Pulisic', 82),
(2, 16, N'Idrissa Touré',      N'Marius Marin',      85);

/* ---------- 5) DETAYLI MAÇ 3: Juventus 1-1 Parma (MatchId = 3) ---------- */
INSERT INTO MatchGoals (MatchId, TeamId, PlayerName, Minute) VALUES
(3, 10, N'Dušan Vlahović',   37),
(3, 15, N'Mateo Pellegrino', 69);

INSERT INTO MatchCards (MatchId, TeamId, PlayerName, Minute, CardType) VALUES
(3, 10, N'Manuel Locatelli', 44, 0),
(3, 15, N'Enrico Delprato',  61, 0),
(3, 10, N'Bremer',           90, 0);

INSERT INTO Substitutions (MatchId, TeamId, PlayerIn, PlayerOut, Minute) VALUES
(3, 15, N'Adrián Bernabé',    N'Hernani',          58),
(3, 10, N'Kenan Yıldız',      N'Teun Koopmeiners', 64),
(3, 15, N'Gabriel Strefezza', N'Mateo Pellegrino', 79),
(3, 10, N'Weston McKennie',   N'Manuel Locatelli', 82);

PRINT 'Örnek veriler eklendi: 20 takım, 30 maç, 3 detaylı maç.';
GO
