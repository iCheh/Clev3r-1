using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter.DataTemplate
{
    internal static class ErrorsCodeList
    {
        internal static Dictionary<int, string> Errors { get; private set; }

        internal static void SetRU()
        {
            if (Errors == null)
            {
                Errors = new Dictionary<int, string>();
            }
            Errors.Clear();
            // Коды первого прохода
            Errors.Add(1001, "Лишняя скобка");
            Errors.Add(1002, "Неправильно закрыты скобки");
            Errors.Add(1003, "Неправильно закрыта структура IF");
            Errors.Add(1004, "У cтруктура IF нет начала");
            Errors.Add(1005, "Неправильно закрыта структура FOR");
            Errors.Add(1006, "У cтруктура FOR нет начала");
            Errors.Add(1007, "Неправильно закрыта структура WHILE");
            Errors.Add(1008, "У cтруктура WHILE нет начала");
            Errors.Add(1009, "Структура SUB не может содержать в себе другую структуру SUB");
            Errors.Add(1010, "У cтруктуры SUB нет начала");
            Errors.Add(1011, "Структура SUB не закрыта");
            Errors.Add(1012, "Структура IF не закрыта");
            Errors.Add(1013, "Структура FOR не закрыта");
            Errors.Add(1014, "Структура WHILE не закрыта");
            Errors.Add(1015, "Недопустимый код, Должно быть только слово EndIf");
            Errors.Add(1016, "Недопустимый код, Должно быть только слово EndFor");
            Errors.Add(1017, "Недопустимый код, Должно быть только слово EndWhile");
            Errors.Add(1018, "Недопустимый код, Должно быть только слово EndSub");
            Errors.Add(1019, "Недопустимый код, Должно быть только слово Else");
            Errors.Add(1020, "Недопустимый код, Должно быть только слово goto и имя метки перехода (label) без двоеточия");
            Errors.Add(1021, "Недопустимый код, Должно быть только имя метки перехода (label) и двоеточие в конце");
            Errors.Add(1022, "Модуль не найден");
            Errors.Add(1023, "Процедура не найдена");
            Errors.Add(1024, "Метка (label) с таким именем уже определена в данной функции");
            Errors.Add(1025, "Метка (label) с таким именем уже определена в данной программе");
            Errors.Add(1026, "Структура FUNCTION не может содержать в себе другую структуру FUNCTION");
            Errors.Add(1027, "У cтруктуры FUNCTION нет начала");
            Errors.Add(1028, "Структура FUNCTION не закрыта");
            Errors.Add(1029, "Недопустимый код, Должно быть только слово EndFunction");
            Errors.Add(1030, "Структура SUB не может содержать в себе другую структуру FUNCTION");
            Errors.Add(1031, "Структура FUNCTION не может содержать в себе другую структуру SUB");
            Errors.Add(1032, "Строка не распознана");
            Errors.Add(1033, "Тип строки не распознан");
            Errors.Add(1034, "Ошибки в строке");
            Errors.Add(1035, "Метка (label) с таким именем не найдена в функции");
            Errors.Add(1036, "Метка (label) с таким именем не найдена в программе");
            // Коды Include
            Errors.Add(1101, "Файл не найден");
            Errors.Add(1102, "Отсутствует имя подключаемого файла");
            Errors.Add(1103, "Неверное количество параметров");
            Errors.Add(1104, "Имя файла должно быть в виде строки");
            Errors.Add(1105, "Включючаемые файлы не могут содержать своих включений");
            Errors.Add(1106, "Включючаемые файлы не могут содержать ключевое слово folder");
            // Коды Folder
            Errors.Add(1201, "Неверное количество параметров");
            Errors.Add(1202, "Параметры должны быть в виде строки");
            Errors.Add(1203, "Первый параметр должен быть \"prjs\", или \"sd\"");
            Errors.Add(1204, "В имени проекта не может быть больше 32 символов");
            Errors.Add(1205, "Имя проекта не модеть быть пустым");
            Errors.Add(1206, "Имя проекта должно начинаться с буквы A-Z, a - z");
            Errors.Add(1207, "Имя проекта может содержать только буквы A-Z и a-z, цифры 0 - 9 и знак нижнего подчеркивания _");
            Errors.Add(1208, "Ключевое слово folder может быть объявлено только один раз");
            Errors.Add(1209, "Ключевое слово folder нельзя использовать в файлах модулей");
            Errors.Add(1210, "Ключевое слово folder должно быть до начала основного кода");
            // Коды METHOD
            Errors.Add(1301, "Метод не найден");
            Errors.Add(1302, "В вызове метода лишние скобки");
            Errors.Add(1303, "В вызове метода отсутствуют скобки");
            Errors.Add(1304, "Неверное количество параметров");
            Errors.Add(1305, "Переменной не присвоено значение");
            Errors.Add(1306, "Метод в качестве параметра не возвращает значений");
            Errors.Add(1307, "Неверный тип параметра");
            Errors.Add(1308, "Неверное количество параметров, либо отсутствует математический оператор");
            Errors.Add(1309, "Недопустимый математический оператор");
            Errors.Add(1310, "Разные типы данных");
            Errors.Add(1311, "Лишние математические операторы");
            Errors.Add(1312, "Отсутствует параметр");
            Errors.Add(1313, "Метод не возвращает значений");
            Errors.Add(1314, "Неправильное определение метода");
            // Коды Variable
            Errors.Add(1401, "Неправильное определение переменной");
            Errors.Add(1402, "Отсутствует имя переменной");
            Errors.Add(1403, "В определении переменной недопустимые математические опрераторы");
            Errors.Add(1404, "В определении переменной недопустимые выражения");
            Errors.Add(1405, "Переменная не определена");
            Errors.Add(1406, "Переменная не инициализирована");
            Errors.Add(1407, "Переменная имеет другой тип");
            Errors.Add(1408, "После математического оператора ожидается какой-либо операнд (переменная, число, метод)");
            Errors.Add(1409, "Перед математическим оператором ожидается какой-либо операнд (переменная, число, метод)");
            Errors.Add(1410, "После определения имени переменной отсутствует знак =");
            Errors.Add(1411, "После знака = должно быть выражение");
            Errors.Add(1412, "Математическое действие ++ можно применять только к числовым переменным");
            Errors.Add(1413, "Математическое действие -- можно применять только к числовым переменным");
            Errors.Add(1414, "В строке недопустимые выражения");
            Errors.Add(1415, "В переменную, со знаком минус можно записать только число");
            Errors.Add(1416, "Перед следующим операндом ожидается математический оператор");
            Errors.Add(1417, "Строки допустимо складывать только через оператор +");
            Errors.Add(1418, "Математические операции над масствами недопустимы");
            Errors.Add(1419, "Для индексации массива могут использоваться только целые числа");
            Errors.Add(1420, "Для индексации массива могут использоваться только числа");
            Errors.Add(1421, "Данное математическое действие нельзя применять к массивам");
            Errors.Add(1422, "Число индкса массива должно быть целым (без дробной части)");
            Errors.Add(1423, "Для индекса массива можно использовать переменные содержащие только целые числа");
            Errors.Add(1424, "Для индекса массива можно использовать метод который возвращает число");
            Errors.Add(1425, "В элемент массива нельзя записать другой массив");
            Errors.Add(1426, "Между математическими операторами ожидается какой-либо операнд (переменная, число, метод)");
            Errors.Add(1427, "В индексе массива недопустимые значения");
            Errors.Add(1428, "С опрераторами +=, -=, *=, /=, могут использоваться только методы возвращающие числа, или строки");
            // Коды Global
            Errors.Add(1501, "Ключевое слово global нельзя использовать в файлах модулей");
            Errors.Add(1502, "Глобальные переменные нельхя определять в теле процедуры");
            Errors.Add(1503, "Глобальные переменные должны определяться до начала основного кода");
            // Коды Sub
            Errors.Add(1601, "Неверное определение процедуры");
            Errors.Add(1602, "В определении процедуры должно быть ключевое слово Sub и имя процедуры");
            Errors.Add(1603, "В определении процедуры отсутствует имя процедуры");
            Errors.Add(1604, "В определении процедуры недопустимые ключевые слова");
            Errors.Add(1605, "В определении процедуры недопустимые выражения");
            Errors.Add(1606, "Процедура не найдена");
            Errors.Add(1607, "Процедура с таким именем уже определена");
            // Коды User method
            Errors.Add(1701, "Неверный синтаксис вызова процедуры из модуля");
            Errors.Add(1702, "В вызове процедуры отсутствуют скобки");
            Errors.Add(1703, "В параметрах вызова процедуры ошибка");
            Errors.Add(1704, "В параметрах вызова процедуры несколько запятых подряд");
            Errors.Add(1705, "В параметрах вызова процедуры недопустимые выражения");
            Errors.Add(1706, "В параметрах вызова процедуры недопустимые математические опрераторы");
            Errors.Add(1707, "В вызове процедуры после запятой отсутствует параметр");
            Errors.Add(1708, "Модуль не найден");
            Errors.Add(1709, "Модуль должен содержать только определения процедур");
            // Коды Function
            Errors.Add(1801, "Неверное определение функции");
            Errors.Add(1802, "В определении функции отсутствуют скобки");
            Errors.Add(1803, "В определении функции отсутствует имя функции");
            Errors.Add(1804, "В определении функции недопустимые ключевые слова");
            Errors.Add(1805, "В определении функции недопустимые выражения");
            Errors.Add(1806, "Функция не найдена");
            Errors.Add(1807, "Имя не может быть использовано, так как уже определена функция с таким именем");
            Errors.Add(1808, "Имя не может быть использовано, так как уже определена процедура с таким именем");
            Errors.Add(1809, "Функция с таким именем и количеством параметров уже определена");
            Errors.Add(1810, "В определении функции есть переменные с одинаковыми именами");
            Errors.Add(1811, "В определении функции между определениями переменных должна быть запятая");
            Errors.Add(1812, "В определении функции перед запятой должна быть переменная");
            Errors.Add(1813, "В определении функции после ключевого слова in/out должен быть тип переменной");
            Errors.Add(1814, "В определении функции после перед переменной должно быть указан тип переменной");
            Errors.Add(1815, "В определении функции после запятой должно быть ключевое слово in/out");
            Errors.Add(1816, "В определении функции перед типом переменной должно быть ключевое слово in/out");
            Errors.Add(1817, "В определении функции после типа переменной должно быть имя переменной");
            Errors.Add(1818, "В определении функции после перед переменной должно быть указано ключевое слово in/out и тип переменной");
            Errors.Add(1819, "В определении функции не определён тип переменной");
            Errors.Add(1820, "Параметр имеет другой тип");
            Errors.Add(1821, "В вызове функции выходной параметр может быть только переменной");
            Errors.Add(1822, "В определении функции в параметрах нельзя использовать ссылки на глобальные переменные");
            // Коды for if while
            Errors.Add(1901, "Отсутствует ключевое слово To");
            Errors.Add(1902, "В строке инициализации For ошибки");
            Errors.Add(1903, "После слова For должна быть переменная и присвоение ей значения");
            Errors.Add(1904, "В цикле For в переменной должно быть число");
            Errors.Add(1905, "Между операндами должен быть логический оператор");    
            Errors.Add(1906, "Сравнивать можно только числа и строки");
            Errors.Add(1907, "В конце строки должно быть слово Then");
            Errors.Add(1908, "Отсутствует логическое условие");
            Errors.Add(1909, "Нельзя использовать два логических оператора подряд");
            Errors.Add(1910, "В логическом выражении отсутствует левая часть");
            Errors.Add(1911, "В логическом выражении отсутствует правая часть");
            Errors.Add(1912, "В логическом выражении должно быть два операнда");

            Errors.Add(1913, "В строке может быть только одно ключевое слово Break");
            Errors.Add(1914, "Ключевое слово Break можно использовать только внутри For...EndFor и While...EndWhile");
            Errors.Add(1915, "В строке может быть только одно ключевое слово Continue");
            Errors.Add(1916, "Ключевое слово Continue можно использовать только внутри For...EndFor и While...EndWhile");
            // Коды Import module
            Errors.Add(2001, "Файл не найден");
            Errors.Add(2002, "Отсутствует имя импортируемого файла модуля");
            Errors.Add(2003, "Неверное количество параметров");
            Errors.Add(2004, "Имя файла должно быть в виде строки");
            Errors.Add(2005, "Импортируемые файлы модулей не могут содержать включений include");
            Errors.Add(2006, "Импортируемые файлы модулей не могут содержать ключевое слово folder");
            Errors.Add(2007, "В файлах модулей .bpm определение процедур недопустимо");
            Errors.Add(2008, "В файлах модулей .bpm допустимы только определения функций и свойств");
            Errors.Add(2009, "Нельзя использовать ссылки на глобальные переменные в модулях .bpm");
            Errors.Add(2010, "Нельзя использовать ссылки на глобальные метки перехода в модулях .bpm");
            Errors.Add(2011, "Объявление свойства модуля должно состоять только из типа и имени свойства");
            Errors.Add(2012, "Свойство модуля должно состоять из его типа и имени");
            Errors.Add(2013, "Тип свойства может быть только number, number[], string, или string[]");
            Errors.Add(2014, "Свойство с таким именем уже определено в модуле");
            Errors.Add(2015, "Свойство не может быть объявлено внутри метода (функции) модуля");
            Errors.Add(2016, "В строке допустимо только одно ключевое слово - private");
            Errors.Add(2017, "Вызов приватного свойства допустим только в модуле владельце этого свойства");
            Errors.Add(2018, "Вызов приватного метода допустим только в модуле владельце этого метода");
            Errors.Add(2019, "Имя переменной в описании функции модуля совпадает с именем свойства модуля");
            Errors.Add(2020, "Свойство с таким именем не определено в модуле");

            // Отладка
            Errors.Add(4001, "===> 1 <===");
            Errors.Add(4002, "===> 2 <===");
            Errors.Add(4003, "===> 3 <===");
            Errors.Add(4004, "===> 4 <===");
            Errors.Add(4005, "===> 5 <===");
            Errors.Add(4006, "===> 6 <===");
            Errors.Add(4007, "===> 7 <===");
            Errors.Add(4008, "===> 8 <===");
            Errors.Add(4009, "===> 9 <===");
        }

        internal static void SetUA()
        {
            if (Errors == null)
            {
                Errors = new Dictionary<int, string>();
            }
            Errors.Clear();
            // Коди першого проходу
            Errors.Add(1001, "Зайва дужка");
            Errors.Add(1002, "Неправильно закриті дужки");
            Errors.Add(1003, "Неправильно закрита структура IF");
            Errors.Add(1004, "У cтруктури IF немає початку");
            Errors.Add(1005, "Неправильно закрита структура FOR");
            Errors.Add(1006, "У cтруктури FOR немає початку");
            Errors.Add(1007, "Неправильно закрита структура WHILE");
            Errors.Add(1008, "У cтруктури WHILE немає початку");
            Errors.Add(1009, "Структура SUB не може містити у собі іншу структуру SUB");
            Errors.Add(1010, "У cтруктури SUB немає початку");
            Errors.Add(1011, "Структура SUB не є закритою");
            Errors.Add(1012, "Структура IF не є закритою");
            Errors.Add(1013, "Структура FOR не є закритою");
            Errors.Add(1014, "Структура WHILE не є закритою");
            Errors.Add(1015, "Неприпустимий код, має бути тільки слово EndIf");
            Errors.Add(1016, "Неприпустимий код, має бути тільки слово EndFor");
            Errors.Add(1017, "Неприпустимий код, має бути тільки слово EndWhile");
            Errors.Add(1018, "Неприпустимий код, має бути тільки слово EndSub");
            Errors.Add(1019, "Неприпустимий код, має бути тільки слово Else");
            Errors.Add(1020, "Неприпустимий код, має бути тільки слово goto та ім’я мітки переходу (label)");
            Errors.Add(1021, "Неприпустимий код, має бути тільки ім’я мітки переходу (label) та двокрапка у кінці");
            Errors.Add(1022, "Модуль не знайдено");
            Errors.Add(1023, "Процедуру не знайдено");
            Errors.Add(1024, "Мітка (label) з таким ім'ям вже визначена в даній функції");
            Errors.Add(1025, "Метка (label) з таким ім'ям вже визначена в даній програмі");
            Errors.Add(1026, "Структура FUNCTION не може містити у собі іншу структуру FUNCTION");
            Errors.Add(1027, "У cтруктури FUNCTION немає початку");
            Errors.Add(1028, "Структура FUNCTION не є закритою");
            Errors.Add(1029, "Неприпустимий код, має бути тільки слово EndFunction");
            Errors.Add(1030, "Структура SUB не може містити у собі іншу структуру FUNCTION");
            Errors.Add(1031, "Структура FUNCTION не може містити у собі іншу структуру SUB");
            Errors.Add(1032, "Рядок не розпізнаний");
            Errors.Add(1033, "Тип рядка не розпізнаний");
            Errors.Add(1034, "Помилки у рядку");
            Errors.Add(1035, "Мітку (label) з таким ім'ям не знайдено в функції");
            Errors.Add(1036, "Мітку (label) з таким ім'ям не знайдено в програмі");
            // Коди Include
            Errors.Add(1101, "Файл не знайдено");
            Errors.Add(1102, "Відсутнє ім'я файлу, що підключається");
            Errors.Add(1103, "Невірна кількість параметрів");
            Errors.Add(1104, "Им'я файла має бути у вигляді рядка");
            Errors.Add(1105, "Файли, що включаються, не можуть містити своїх включень");
            Errors.Add(1106, "Файли, що включаються, не можуть містити ключове слово folder");
            // Коди Folder
            Errors.Add(1201, "Невірна кількість параметрів");
            Errors.Add(1202, "Параметри мають бути у вигляді рядка");
            Errors.Add(1203, "Першим параметром має бути \"prjs\" або \"sd\"");
            Errors.Add(1204, "Ім’я проекту не може складатися більш ніж з 32 символів");
            Errors.Add(1205, "Ім’я проекту не може бути порожнім");
            Errors.Add(1206, "Ім’я проекту має починатися з літери A-Z, a - z");
            Errors.Add(1207, "Ім'я проекту може містити тільки літери A-Z та a-z, цифри 0 - 9 та знак нижнього підкреслення _");
            Errors.Add(1208, "Ключове слово folder може бути оголошене тільки один раз");
            Errors.Add(1209, "Ключове слово folder не можна використовувати у файлах модулів");
            Errors.Add(1210, "Ключове слово folder має бути до початку основного коду");
            // Коди METHOD
            Errors.Add(1301, "Метод не знайдено");
            Errors.Add(1302, "У виклику методу зайві дужки");
            Errors.Add(1303, "У виклику методу відсутні дужки");
            Errors.Add(1304, "Невірна кількість параметрів");
            Errors.Add(1305, "Змінній не присвоєне значення");
            Errors.Add(1306, "Метод у якості параметру не повертає значень");
            Errors.Add(1307, "Невірний тип параметру");
            Errors.Add(1308, "Невірна кількість аргументів, або відсутній математичний оператор");
            Errors.Add(1309, "Неприпустимий математичний оператор");
            Errors.Add(1310, "Різні типи даних");
            Errors.Add(1311, "Зайві математичні оператори");
            Errors.Add(1312, "Відсутній параметр");
            Errors.Add(1313, "Метод не повертає значень");
            Errors.Add(1314, "Неправильне визначення методу");
            // Коди Variable
            Errors.Add(1401, "Неправильне визначення змінної");
            Errors.Add(1402, "Відсутнє ім'я змінної");
            Errors.Add(1403, "У визначенні змінної неприпустимі математичні оператори");
            Errors.Add(1404, "У визначенні змінної неприпустимі вирази");
            Errors.Add(1405, "Змінну не визначено");
            Errors.Add(1406, "Змінну не ініціалізовано");
            Errors.Add(1407, "Змінна має інший тип");
            Errors.Add(1408, "Після математичного оператора очікується який-небудь операнд (змінна, число, метод)");
            Errors.Add(1409, "Перед математичним оператором очікується який-небудь операнд (змінна, число, метод)");
            Errors.Add(1410, "Після визначення імені змінної відсутній знак =");
            Errors.Add(1411, "Після знаку = має бути вираз");
            Errors.Add(1412, "Математичну дію ++ можна застосовувати тільки до числових змінних");
            Errors.Add(1413, "Математичну дію -- можна застосовувати тільки до числових змінних");
            Errors.Add(1414, "В рядку неприпустимі вирази");
            Errors.Add(1415, "До змінної зі знаком мінус можна записати тільки число");
            Errors.Add(1416, "Перед наступним операндом очікується математичний оператор");
            Errors.Add(1417, "Рядки можна складати тільки через оператор +");
            Errors.Add(1418, "Математичні операції над масивами неприпустимі");
            Errors.Add(1419, "Для індексації масиву можна використовувати тільки цілі числа");
            Errors.Add(1420, "Для індексації масиву можна використовувати тільки числа");
            Errors.Add(1421, "Дану математичну дію не можна застосовувати до масивів");
            Errors.Add(1422, "Число індексу масиву має бути цілим (без дробової частини)");
            Errors.Add(1423, "Для індексу масиву можна використовувати змінні, що містять тільки цілі числа");
            Errors.Add(1424, "Для індексу масиву можна використовувати метод, що повертає число");
            Errors.Add(1425, "До елементу масиву не можна записати інший масив");
            Errors.Add(1426, "Між математичними операторами очікується який-небудь операнд (змінна, число, метод)");
            Errors.Add(1427, "Індекс масиву містить неприпустимі значення");
            Errors.Add(1428, "З операторами +=, -=, *=, /=, можуть використовуватися тільки методи, що повертають числа або рядки");
            // Коди Global
            Errors.Add(1501, "Ключове слово global не можна використовувати в файлах модулів");
            Errors.Add(1502, "Глобальні змінні не можна визначати в тілі процедури");
            Errors.Add(1503, "Глобальні змінні мають бути визначені до початку основного коду");
            // Коди Sub
            Errors.Add(1601, "Невірне визначення процедури");
            Errors.Add(1602, "У визначенні процедури має бути ключове слово Sub та ім'я процедури");
            Errors.Add(1603, "У визначенні процедури відсутнє ім'я процедури");
            Errors.Add(1604, "Визначення процедури містить неприпустимі ключові слова");
            Errors.Add(1605, "Визначення процедури містить неприпустимі вирази");
            Errors.Add(1606, "Процедуру не знайдено");
            Errors.Add(1607, "Процедуру з таким ім'ям вже визначено");
            // Коди User method
            Errors.Add(1701, "Невірний синтаксис виклику процедури з модуля");
            Errors.Add(1702, "У виклику процедури відсутні дужки");
            Errors.Add(1703, "В параметрах виклику процедури є помилка");
            Errors.Add(1704, "В параметрах виклику процедури вказано кілька ком поспіль");
            Errors.Add(1705, "В параметрах виклику процедури містяться неприпустимі вирази");
            Errors.Add(1706, "В параметрах виклику процедури містяться неприпустимі математичні оператори");
            Errors.Add(1707, "У виклику процедури відсутній параметр після коми");
            Errors.Add(1708, "Модуль не знайдено");
            Errors.Add(1709, "Модуль має містити тільки визначення процедур");
            // Коди Function
            Errors.Add(1801, "Невірне визначення функції");
            Errors.Add(1802, "У визначенні функції відсутні дужки");
            Errors.Add(1803, "У визначенні функції відсутнє ім'я функції");
            Errors.Add(1804, "У визначенні функції неприпустимі ключові слова");
            Errors.Add(1805, "У визначенні функції неприпустимі вирази");
            Errors.Add(1806, "Функцію не знайдено");
            Errors.Add(1807, "Не можна використати ім'я, тому що вже визначено функцію з таким ім'ям");
            Errors.Add(1808, "Не можна використати ім'я, тому що вже визначено процедуру з таким ім'ям");
            Errors.Add(1809, "Функцію з таким ім'ям і кількістю параметрів вже визначено");
            Errors.Add(1810, "У визначенні функції є змінні з однаковими іменами");
            Errors.Add(1811, "У визначенні функції між визначеннями змінних має бути кома");
            Errors.Add(1812, "У визначенні функції перед комою має бути змінна");
            Errors.Add(1813, "У визначенні функції після ключового слова in/out має бути тип змінної");
            Errors.Add(1814, "У визначенні функції після перед змінною має бути вказаний тип змінної");
            Errors.Add(1815, "У визначенні функції після коми має бути ключове слово in/out");
            Errors.Add(1816, "У визначенні функції перед типом змінної має бути ключове слово in/out");
            Errors.Add(1817, "У визначенні функції після типу змінної має бути ім'я змінної");
            Errors.Add(1818, "У визначенні функції після змінної має бути ключове слово in/out і тип змінної");
            Errors.Add(1819, "У визначенні функції не визначено тип змінної");
            Errors.Add(1820, "Параметр має інший тип");
            Errors.Add(1821, "У виклику функції вихідний параметр може бути тільки змінною");
            Errors.Add(1822, "У визначенні функції в параметрах не можна використовувати посилання на глобальні змінні");
            // Коди for if while
            Errors.Add(1901, "Відсутнє ключове слово To");
            Errors.Add(1902, "У рядку ініціалізації For є помилки");
            Errors.Add(1903, "Після слова For має бути змінна та значення, що було їй привласнене");
            Errors.Add(1904, "У циклі For у змінній має бути число");
            Errors.Add(1905, "Між операндами має бути логічний оператор");
            Errors.Add(1906, "Порівнювати можна тільки числа і рядки");
            Errors.Add(1907, "В кінці рядка має бути слово Then");
            Errors.Add(1908, "Відсутня логічна умова");
            Errors.Add(1909, "Не можна використовувати два логічних оператора поспіль");
            Errors.Add(1910, "У логічному вираженні відсутня ліва частина");
            Errors.Add(1911, "У логічному вираженні відсутня права частина");

            Errors.Add(1913, "У рядку може бути лише одне ключове слово Break");
            Errors.Add(1914, "Ключове слово Break можна використовувати лише усередині For...EndFor і While...EndWhile");
            Errors.Add(1915, "У рядку може бути лише одне ключове слово Continue");
            Errors.Add(1916, "Ключове слово Continue можна використовувати лише усередині For...EndFor і While...EndWhile");
            // Коди Import module
            Errors.Add(2001, "Файл не знайдено");
            Errors.Add(2002, "Відсутня ім'я файла модуля, що імпортується");
            Errors.Add(2003, "Невірна кількість параметрів");
            Errors.Add(2004, "Им'я файла має бути у вигляді рядка");
            Errors.Add(2005, "Імпортовані файли модулів не можуть містити включень include");
            Errors.Add(2006, "Імпортовані файли модулів не можуть містити ключове слово folder");
            Errors.Add(2007, "У файлах модулів .bpm визначення процедур непримустиме");
            Errors.Add(2008, "У файлах модулів .bpm можуть бути лише визначення функцій і властивостей");
            Errors.Add(2009, "Не можна використовувати посилання на глобальні змінні в модулях .bpm");
			Errors.Add(2010, "Не можна використовувати посилання на глобальні мітки переходу в модулях .bpm");
			Errors.Add(2011, "Оголошення властивості модуля має складатися тільки з типу й імені властивості");
			Errors.Add(2012, "Властивість модуля має складатися з його типу й імені властивості");
			Errors.Add(2013, "Тип властивості може бути тільки number, number[], string, або string[]");
			Errors.Add(2014, "Властивість із таким ім’ям вже визначена в модулі");
			Errors.Add(2015, "Властивість не може бути оголошена всередині методу (функції) модуля");
			Errors.Add(2016, "У рядку може бути тільки одне ключове слово - private");
            Errors.Add(2017, "Виклик приватного властивості допустимо тільки в модулі власника цієї властивості");
            Errors.Add(2018, "Виклик приватного методу допустимо тільки в модулі власника цього методу");
            Errors.Add(2019, "Ім'я змінної в описі функції модуля збігається з ім'ям властивості модуля");
            Errors.Add(2020, "Властивість з такою назвою не визначена в модулі");

            // Отладка
            Errors.Add(4001, "===> 1 <===");
            Errors.Add(4002, "===> 2 <===");
            Errors.Add(4003, "===> 3 <===");
            Errors.Add(4004, "===> 4 <===");
            Errors.Add(4005, "===> 5 <===");
            Errors.Add(4006, "===> 6 <===");
            Errors.Add(4007, "===> 7 <===");
            Errors.Add(4008, "===> 8 <===");
            Errors.Add(4009, "===> 9 <===");
        }

        internal static void SetEN()
        {
            if (Errors == null)
            {
                Errors = new Dictionary<int, string>();
            }
            Errors.Clear();
            // First pass codes
            Errors.Add(1001, "Extra bracket");
            Errors.Add(1002, "Brackets not closed properly");
            Errors.Add(1003, "IF structure not closed properly");
            Errors.Add(1004, "IF structure has no beginning");
            Errors.Add(1005, "FOR structure not closed properly");
            Errors.Add(1006, "FOR structure has no beginning");
            Errors.Add(1007, "WHILE structure not closed properly");
            Errors.Add(1008, "WHILE structure has no beginning");
            Errors.Add(1009, "SUB structure can not contain other SUB structure");
            Errors.Add(1010, "SUB structure has no beginning");
            Errors.Add(1011, "SUB structure is not closed");
            Errors.Add(1012, "IF structure is not closed");
            Errors.Add(1013, "FOR structure is not closed");
            Errors.Add(1014, "WHILE structure is not closed");
            Errors.Add(1015, "Invalid code. ‘EndIf’ must only be indicated");
            Errors.Add(1016, "Invalid code. ‘EndFor’ must only be indicated");
            Errors.Add(1017, "Invalid code. ‘EndWhile’ must only be indicated");
            Errors.Add(1018, "Invalid code. ‘EndSub’ must only be indicated");
            Errors.Add(1019, "Invalid code. ‘Else’ must only be indicated");
            Errors.Add(1020, "Invalid code. ‘Goto’ and jump label name must only be indicated");
            Errors.Add(1021, "Invalid code. Jump label name and two-spot at the end must only be indicated");
            Errors.Add(1022, "Module not found");
            Errors.Add(1023, "Procedure not found");
            Errors.Add(1024, "Label with this name is already defined in this function");
            Errors.Add(1025, "Label with this name is already defined in this program");
            Errors.Add(1026, "FUNCTION structure can not contain other FUNCTION structure");
            Errors.Add(1027, "FUNCTION has no beginning");
            Errors.Add(1028, "FUNCTION structure is not closed");
            Errors.Add(1029, "Invalid code. ‘EndFunction’ must only be indicated");
            Errors.Add(1030, "SUB structure can not contain other FUNCTION structure");
            Errors.Add(1031, "FUNCTION structure can not contain other SUB structure");
            Errors.Add(1032, "Row not recognized");
            Errors.Add(1033, "Row type not recognized");
            Errors.Add(1034, "Errors in the row");
            Errors.Add(1035, "No label with this name found in the function");
            Errors.Add(1036, "No label with this name found in the program");
            // Include codes
            Errors.Add(1101, "File not found");
            Errors.Add(1102, "Missing name of file being included");
            Errors.Add(1103, "Invalid number of parameters");
            Errors.Add(1104, "File name must be a string");
            Errors.Add(1105, "Files being included can not contain own inclusions");
            Errors.Add(1106, "Files being included can not contain keyword ‘folder’");
            // Folder codes
            Errors.Add(1201, "Invalid number of parameters");
            Errors.Add(1202, "Parameters must be indicated as a string");
            Errors.Add(1203, "The first parameter must be \"prjs\" or \"sd\"");
            Errors.Add(1204, "Project name can not contain more than 32 characters");
            Errors.Add(1205, "Project name can not be void");
            Errors.Add(1206, "Project name must begin with A-Z, a - z");
            Errors.Add(1207, "Project name can only contain letters A-Z and a-z, figures 0 - 9 and underscore character _");
            Errors.Add(1208, "Keyword ‘folder’ can only be declared once");
            Errors.Add(1209, "Keyword ‘folder’ can not be used in module files");
            Errors.Add(1210, "Keyword ‘folder’ must be indicated before the beginning of main code");
            // METHOD codes
            Errors.Add(1301, "Method not found");
            Errors.Add(1302, "Extra brackets in method call");
            Errors.Add(1303, "Missing brackets in method call");
            Errors.Add(1304, "Invalid number of parameters");
            Errors.Add(1305, "No value assigned to variable");
            Errors.Add(1306, "Method does not return values ??as a parameter");
            Errors.Add(1307, "Invalid parameter type");
            Errors.Add(1308, "Invalid number of parameters or math operator missing");
            Errors.Add(1309, "Invalid math operator");
            Errors.Add(1310, "Different data types");
            Errors.Add(1311, "Excess math operators");
            Errors.Add(1312, "Missing parameter");
            Errors.Add(1313, "Method does not return values");
            Errors.Add(1314, "Incorrect method definition");
            // Variable codes
            Errors.Add(1401, "Incorrect variable definition");
            Errors.Add(1402, "Missing variable name");
            Errors.Add(1403, "Invalid math operators in variable definition");
            Errors.Add(1404, "Invalid expressions in variable definition");
            Errors.Add(1405, "Variable not defined");
            Errors.Add(1406, "Variable not initialized");
            Errors.Add(1407, "Variable has different type");
            Errors.Add(1408, "An operand (variable, number, method) is expected after math operator");
            Errors.Add(1409, "An operand (variable, number, method) is expected before math operator");
            Errors.Add(1410, "Character ’=’ missing after variable name definition");
            Errors.Add(1411, "Character ’=’ must be followed by an expression");
            Errors.Add(1412, "Mathematical operation ‘++’ can only be applied to numeric variables");
            Errors.Add(1413, "Mathematical operation ‘--’ can only be applied to numeric variables");
            Errors.Add(1414, "Invalid expressions in the row");
            Errors.Add(1415, "Only a number can be written to variable with minus sign");
            Errors.Add(1416, "Math operator is expected after the next operand");
            Errors.Add(1417, "Rows can only be added with ‘+’ operator");
            Errors.Add(1418, "Mathematical operations can not be performed on arrays");
            Errors.Add(1419, "Only integers can be used for array indexing");
            Errors.Add(1420, "Only figures can be used for array indexing");
            Errors.Add(1421, "This mathematical operations can not be applied to arrays");
            Errors.Add(1422, "Array index number must be an integer (without fractional part)");
            Errors.Add(1423, "Variables containing only integers can be used for array indexing");
            Errors.Add(1424, "Method that returns a number can be used for array indexing");
            Errors.Add(1425, "Another array can not be written to an array element");
            Errors.Add(1426, "An operand (variable, number, method) is expected between math operators");
            Errors.Add(1427, "Invalid values in array index");
            Errors.Add(1428, "Only methods that return figures or rows can be used with operators +=, -=, *=, /=");
            // Global codes
            Errors.Add(1501, "Keyword ‘global’ can not be used in module files");
            Errors.Add(1502, "Global variables can not be defined in procedure body");
            Errors.Add(1503, "Global variables must be defined before the beginning of main code.");
            // Sub codes
            Errors.Add(1601, "Invalid procedure definition");
            Errors.Add(1602, "Procedure definition must contain keyword ‘Sub’ and procedure name");
            Errors.Add(1603, "Missing procedure name in procedure definition");
            Errors.Add(1604, "Procedure definition contains invalid keywords");
            Errors.Add(1605, "Procedure definition contains invalid expressions");
            Errors.Add(1606, "Procedure not found");
            Errors.Add(1607, "Procedure with this name is already defined");
            // User method codes
            Errors.Add(1701, "Invalid procedure call from module syntax");
            Errors.Add(1702, "Missing brackets in procedure call");
            Errors.Add(1703, "Error in procedure call parameters");
            Errors.Add(1704, "Several commas in a row in procedure call parameters");
            Errors.Add(1705, "Invalid expressions in procedure call parameters");
            Errors.Add(1706, "Invalid math operators in procedure call parameters");
            Errors.Add(1707, "Missing parameter after the decimal point in procedure call");
            Errors.Add(1708, "Module not found");
            Errors.Add(1709, "Module must only contain procedure definitions");
            // Function codes
            Errors.Add(1801, "Invalid function definition");
            Errors.Add(1802, "Missing brackets in function definition");
            Errors.Add(1803, "Missing function name in function definition");
            Errors.Add(1804, "Invalid keywords in function definition");
            Errors.Add(1805, "Invalid expressions in function definition");
            Errors.Add(1806, "Function not found");
            Errors.Add(1807, "Name can not be used because a function with this name is already defined");
            Errors.Add(1808, "Name can not be used because a procedure with this name is already defined");
            Errors.Add(1809, "Function with this name and number of parameters is already defined");
            Errors.Add(1810, "Function definition contains variables with the same name");
            Errors.Add(1811, "Variable definitions in function definition must be separated with a comma");
            Errors.Add(1812, "There must be a variable before the comma in function definition");
            Errors.Add(1813, "There must be a variable type after keyword ‘in/out’ in function definition");
            Errors.Add(1814, "Variable type must be indicated before the variable in function definition");
            Errors.Add(1815, "Keyword ‘in/out’ must be indicated after the comma in function definition");
            Errors.Add(1816, "Keyword ‘in/out’ must be indicated before the variable type in function definition");
            Errors.Add(1817, "Variable name must be indicated after the variable type in function definition");
            Errors.Add(1818, "Keyword ‘in/out’ and variable type must be indicated before the variable in function definition");
            Errors.Add(1819, "Variable type not defined in function definition");
            Errors.Add(1820, "Parameter has different type");
            Errors.Add(1821, "In function call, output parameter can only be a variable");
            Errors.Add(1822, "Parameters can not contain references to global variables in function definition");
            // for if while codes
            Errors.Add(1901, "Missing keyword ‘To’");
            Errors.Add(1902, "Errrors in For initialization line");
            Errors.Add(1903, "’For’ must be followed by a variable and the value assigned to it");
            Errors.Add(1904, "In For cycle, variable must contain a number");
            Errors.Add(1905, "There must be a logical operator between operands");
            Errors.Add(1906, "Numbers and lines can only be compared");
            Errors.Add(1907, " ’Then’ must be indicated at the end of the line");
            Errors.Add(1908, "Missing logical condition");
            Errors.Add(1909, "You cannot use two logical operators in a row");
            Errors.Add(1910, "Boolean expression missing left side");
            Errors.Add(1911, "Boolean expression missing right side");

            Errors.Add(1913, "There can only be one keyword \"Break\" per line");
            Errors.Add(1914, "Keyword \"Break\" can be used in the middle of For...EndFor and While...EndWhile");
            Errors.Add(1915, "There can only be one keyword \"Continue\" per line");
            Errors.Add(1916, "Keyword \"Continue\" can be used in the middle of For...EndFor and While...EndWhile");
            // Import module codes
            Errors.Add(2001, "File not found");
            Errors.Add(2002, "Missing imported module file name");
            Errors.Add(2003, "Invalid number of parameters");
            Errors.Add(2004, "File name must be a string");
            Errors.Add(2005, "Imported module files can not contain ‘include’ inclusions");
            Errors.Add(2006, "Imported module files can not contain keyword ‘folder’ ");
            Errors.Add(2007, "Invalid procedure definition in .bpm module files");
            Errors.Add(2008, ".bpm module files can only contain function definitions and properties");
            Errors.Add(2009, "References to global variables can not be used in .bpm modules");
			Errors.Add(2010, "References to global goto lables can not be used in .bpm modules");
			Errors.Add(2011, "A module property declaration must only consist of the property type and name");
			Errors.Add(2012, "A module property must consist of its type and name");
			Errors.Add(2013, "A property can only be of number, number[], string, or string[] type");
			Errors.Add(2014, "A property with this name has already been defined in the module");
			Errors.Add(2015, "A property cannot be declared inside a module method (function)");
			Errors.Add(2016, "The ‘private’ keyword is the only one admissible keyword in the line");
            Errors.Add(2017, "Calling a private property is allowed only in the module that owns this property");
            Errors.Add(2018, "Calling a private method is allowed only in the module owner of this method");
            Errors.Add(2019, "The variable name in the module function description is the same as the module property name");
            Errors.Add(2020, "A property with this name is not defined in the module");

            // Отладка
            Errors.Add(4001, "===> 1 <===");
            Errors.Add(4002, "===> 2 <===");
            Errors.Add(4003, "===> 3 <===");
            Errors.Add(4004, "===> 4 <===");
            Errors.Add(4005, "===> 5 <===");
            Errors.Add(4006, "===> 6 <===");
            Errors.Add(4007, "===> 7 <===");
            Errors.Add(4008, "===> 8 <===");
            Errors.Add(4009, "===> 9 <===");
        }

        internal static string GetError(int code)
        {
            if (Errors.ContainsKey(code))
            {
                return Errors[code];
            }
            else
            {
                return "";
            }
        }
    }
}