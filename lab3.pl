% Предикат для проверки принадлежности элемента списку
member_check(X, [X|_]) :- !.
member_check(X, [_|T]) :- member_check(X, T).

% Предикат для вычисления разности множеств
set_difference([], _, []).  % Базовый случай: пустой список
set_difference([H|T], B, Result) :-
    member_check(H, B),       % Если элемент H принадлежит множеству B
    set_difference(T, B, Result), !.  % Пропускаем этот элемент
set_difference([H|T], B, [H|Result]) :-
    set_difference(T, B, Result).  % Включаем элемент H в результат

% Тестовый предикат для запуска
run :-
    write('ПРОГРАММА ДЛЯ ВЫЧИСЛЕНИЯ РАЗНОСТИ МНОЖЕСТВ'), nl,
    write('Введите первое множество (список): '), read(A),
    write('Введите второе множество (список): '), read(B),
    set_difference(A, B, Difference),
    format('Разность множеств: ~w', [Difference]).
