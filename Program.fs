open System

// Функция для нахождения максимальной цифры в числе
let rec maxDigit (n: int) : int =
    if n = 0 then
        0 // Базовый случай: если число равно 0, возвращаем 0
    else
        let lastDigit = n % 10 // Получаем последнюю цифру числа
        let maxOfRest = maxDigit (n / 10) // Рекурсивно вызываем функцию для оставшейся части числа
        max lastDigit maxOfRest // Возвращаем максимум между последней цифрой и максимальной цифрой оставшейся части

[<EntryPoint>]
let main argv =
    printfn "Введите натуральное число:"
    let input = Console.ReadLine()

    // Проверяем, является ли ввод корректным
    match Int32.TryParse(input) with
    | (true, number) when number > 0 -> 
        let result = maxDigit number // Находим максимальную цифру
        printfn "Максимальная цифра в числе %d: %d" number result
    | _ -> 
        printfn "Пожалуйста, введите корректное натуральное число."

    0 // Код возврата
