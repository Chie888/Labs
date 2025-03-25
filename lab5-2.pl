% Предикат для подсчёта чисел, оканчивающихся заданной цифрой
count_ending_digit([], _, 0).  % Базовый случай: пустой список
count_ending_digit([H|T], Digit, Count) :-
    number(H),                 % Проверка, что элемент - число
    (H mod 10 =:= Digit ->     % Если число оканчивается на цифру
        count_ending_digit(T, Digit, C1),
        Count is C1 + 1
    ;
        count_ending_digit(T, Digit, Count)
    ).

% Предикат для взаимодействия с пользователем
run :-
    write('ПРОГРАММА ДЛЯ ПОДСЧЕТА КОЛ-ВА ЧИСЕЛ, ОКАНЧИВАЮЩИХСЯ НА ВЫБРАННУЮ ЦИФРУ'), nl,
    write('Введите список чисел (в формате [1,2,3,...]): '), read(List),
    write('Введите цифру (0-9): '), read(Digit),
    between(0, 9, Digit),      % Проверка корректности цифры
    count_ending_digit(List, Digit, Count),
    format('Количество чисел: ~d', [Count]).
