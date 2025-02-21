open System

module Program =

    // Добавление элемента в начало списка
    let add element list =
        element :: list

    // Удаление первого вхождения элемента из списка
    let rec remove element list =
        match list with
        | [] -> []  // Если список пуст, возвращаем пустой список
        | head :: tail ->
            if head = element then
                tail  // Если элемент найден, пропускаем его
            else
                head :: (remove element tail)  // Иначе добавляем текущий элемент и рекурсивно обрабатываем остальную часть списка

    // Поиск первого вхождения элемента в списке
    let rec find element list =
        match list with
        | [] -> None  // Если список пуст, элемент не найден
        | head :: tail ->
            if head = element then
                Some head  // Если элемент найден, возвращаем Some(элемент)
            else
                find element tail  // Иначе рекурсивно ищем в остальной части списка

    // Сцепка двух списков
    let rec concat list1 list2 =
        match list1 with
        | [] -> list2  // Если первый список пуст, возвращаем второй список
        | head :: tail ->
            head :: (concat tail list2)  // Добавляем текущий элемент и рекурсивно обрабатываем остальную часть первого списка

    // Получение элемента по номеру (индексу)
    let rec getAt index list =
        if index < 0 then
            None  // Если индекс отрицательный, возвращаем None
        else
            match list with
            | [] -> None  // Если список пуст, возвращаем None
            | head :: tail ->
                if index = 0 then
                    Some head  // Если индекс равен 0, возвращаем текущий элемент
                else
                    getAt (index - 1) tail  // Иначе уменьшаем индекс и рекурсивно ищем в остальной части списка

// Пример использования
[<EntryPoint>]
let main argv =
    let myList = [1; 2; 3; 4; 5]

    let newList = Program.add 0 myList
    printfn "Список после добавления 0: %A" newList

    let removedList = Program.remove 3 newList
    printfn "Список после удаления 3: %A" removedList

    match Program.find 4 removedList with
    | Some element -> printfn "Элемент 4 найден: %d" element
    | None -> printfn "Элемент 4 не найден"

    let anotherList = [6; 7; 8]
    let concatenatedList = Program.concat removedList anotherList
    printfn "Сцепленный список: %A" concatenatedList

    match Program.getAt 2 concatenatedList with
    | Some element -> printfn "Элемент с индексом 2: %d" element
    | None -> printfn "Элемент с индексом 2 не найден"

    0
