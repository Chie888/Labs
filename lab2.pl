% �������� ��� �������� �����, �������������� �������� ������
count_ending_digit([], _, 0).  % ������� ������: ������ ������
count_ending_digit([H|T], Digit, Count) :-
    number(H),                 % ��������, ��� ������� - �����
    (H mod 10 =:= Digit ->     % ���� ����� ������������ �� �����
        count_ending_digit(T, Digit, C1),
        Count is C1 + 1
    ;
        count_ending_digit(T, Digit, Count)
    ).

% �������� ��� �������������� � �������������
run :-
    write('��������� ��� �������� ���-�� �����, �������������� �� ��������� �����'), nl,
    write('������� ������ ����� (� ������� [1,2,3,...]): '), read(List),
    write('������� ����� (0-9): '), read(Digit),
    between(0, 9, Digit),      % �������� ������������ �����
    count_ending_digit(List, Digit, Count),
    format('���������� �����: ~d', [Count]).
