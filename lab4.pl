% Предикаты для проверки условий
check_condition_1(A, D, N) :-
    (   A = 1, D = 1 -> N = 1
    ;   true
    ).

check_condition_2(D, N, V) :-
    (   D = 0 -> (N = 1, V = 0)
    ;   true
    ).

check_condition_3(A, V) :-
    A \= V.

check_condition_4(D, G) :-
    (   D = 1 -> G = 0
    ;   true
    ).

check_condition_5(N, D, V, G) :-
    (   N = 0 -> ((V = 0 -> D = 1) ; (V = 1 -> (D = 0, G = 1)))
    ;   N = 1 -> (V = 0 ; (V = 1 -> G = 1))
    ).

% Предикат для проверки всех условий
check_all_conditions(A, N, D, G, V) :-
    check_condition_1(A, D, N),
    check_condition_2(D, N, V),
    check_condition_3(A, V),
    check_condition_4(D, G),
    check_condition_5(N, D, V, G).

% Предикат для генерации всех возможных комбинаций
generate_combinations :-
    forall(
        (   between(0, 1, A),
            between(0, 1, N),
            between(0, 1, D),
            between(0, 1, G),
            between(0, 1, V)
        ),
        (   check_all_conditions(A, N, D, G, V)
        ->  format('Состав удовлетворяет условиям: [A=~w, N=~w, D=~w, G=~w, V=~w]\n', [A, N, D, G, V])
        ;   true
        )
    ).

% Запуск генерации и проверки составов
:- write('ПРОГРАММА ДЛЯ РЕШЕНИЯ ЛОГИЧЕСКОЙ ЗАДАЧИ'), nl,
    generate_combinations.
