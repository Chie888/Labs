open System

let isNumber (s: string) =
    match Int32.TryParse(s) with
    | (true, _) -> true
    | _ -> false

let rec readWords (words: string list) =
    printf "Введите слово (или нажмите Enter для завершения): "
    let input = Console.ReadLine()
    
    if input = "" then
        words // Завершаем ввод и возвращаем список слов
    else
        if input.Contains(" ") then
            printfn "Ошибка: Ввод должен содержать только одно слово."
            readWords words // Запрашиваем ввод снова
        elif isNumber input then
            printfn "Ошибка: Ввод не должен быть числом."
            readWords words // Запрашиваем ввод снова
        else
            readWords (input :: words) // Добавляем слово в список и продолжаем ввод

[<EntryPoint>]
let main argv =
    let words = readWords []
    printfn "Список введенных слов:"
    words |> List.rev |> List.iter (printfn "%s") // Выводим слова в том порядке, в котором они были введены
    0 // Возвращаем код завершения


