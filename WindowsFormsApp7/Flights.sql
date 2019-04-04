-- phpMyAdmin SQL Dump
-- version 4.7.3
-- https://www.phpmyadmin.net/
--
-- Хост: 127.0.0.1:3306
-- Время создания: Апр 05 2019 г., 00:49
-- Версия сервера: 5.6.37
-- Версия PHP: 5.5.38

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- База данных: `Flying`
--

-- --------------------------------------------------------

--
-- Структура таблицы `Flights`
--

CREATE TABLE `Flights` (
  `id_flight` int(11) NOT NULL,
  `id_plane` int(11) NOT NULL,
  `time_start` time NOT NULL,
  `time_end` time NOT NULL,
  `free_count_econom` int(11) NOT NULL,
  `free_count_business` int(11) NOT NULL,
  `punkt_B` varchar(35) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `Flights`
--

INSERT INTO `Flights` (`id_flight`, `id_plane`, `time_start`, `time_end`, `free_count_econom`, `free_count_business`, `punkt_B`) VALUES
(1, 12, '12:30:00', '17:00:00', 12, 3, 'San-Diego'),
(2, 8, '10:30:00', '13:00:00', 0, 13, 'Sumy'),
(3, 117, '02:15:00', '19:20:00', 25, 30, 'Kiev'),
(4, 122, '10:10:00', '16:02:00', 14, 9, 'California'),
(5, 23, '14:00:00', '23:00:00', 2, 3, 'Konotop'),
(6, 244, '13:15:00', '11:00:00', 34, 12, 'London'),
(7, 13, '10:20:00', '13:50:00', 23, 14, 'Paris'),
(8, 114, '17:00:00', '23:15:00', 12, 4, 'Moskva'),
(9, 55, '13:00:00', '22:30:00', 2, 7, 'Berlin'),
(10, 73, '01:20:00', '14:40:00', 0, 5, 'Minsk');

--
-- Индексы сохранённых таблиц
--

--
-- Индексы таблицы `Flights`
--
ALTER TABLE `Flights`
  ADD PRIMARY KEY (`id_flight`,`id_plane`);

--
-- AUTO_INCREMENT для сохранённых таблиц
--

--
-- AUTO_INCREMENT для таблицы `Flights`
--
ALTER TABLE `Flights`
  MODIFY `id_flight` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
