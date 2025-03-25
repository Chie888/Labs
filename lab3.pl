% �������� ��� �������� �������������� �������� ������
member_check(X, [X|_]) :- !.
member_check(X, [_|T]) :- member_check(X, T).

% �������� ��� ���������� �������� ��������
set_difference([], _, []).  % ������� ������: ������ ������
set_difference([H|T], B, Result) :-
    member_check(H, B),       % ���� ������� H ����������� ��������� B
    set_difference(T, B, Result), !.  % ���������� ���� �������
set_difference([H|T], B, [H|Result]) :-
    set_difference(T, B, Result).  % �������� ������� H � ���������

% �������� �������� ��� �������
run :-
    write('��������� ��� ���������� �������� ��������'), nl,
    write('������� ������ ��������� (������): '), read(A),
    write('������� ������ ��������� (������): '), read(B),
    set_difference(A, B, Difference),
    format('�������� ��������: ~w', [Difference]).
