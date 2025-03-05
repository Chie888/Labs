open System
open System.IO

// Функция для получения списка длин строк
let getLengths (strings: string list) =
    List.map (fun (s: string) -> s.Length) strings

// Функция для генерации случайных строк
let generateRandomStrings (count: int) (maxLength: int) =
    let random = Random()
    let generateString () =
        let length = random.Next(maxLength) + 1
        let chars = Array.init length (fun _ -> char(random.Next(97, 123))) // ASCII для маленьких букв
        new string(chars)
    List.init count (fun _ -> generateString ())

// Функция для чтения строк из файла
let readStringsFromFile (filePath: string) =
    try
        File.ReadAllLines(filePath) |> Seq.toList
    with
    | ex -> printfn "Ошибка чтения файла: %s" ex.Message; []

// Основная функция для взаимодействия с пользователем
let main () =
    printfn "Программа для работы со списками строк"
    printfn "------------------------------------"
    printfn "1. Ввести строки с клавиатуры"
    printfn "2. Сгенерировать случайные строки"
    printfn "3. Прочитать строки из файла"
    printfn "------------------------------------"
    
    let choice = Console.ReadLine() |> int
    
    let strings =
        match choice with
        | 1 ->
            printfn "Введите строки (введите 'stop' для завершения):"
            let rec inputStrings () =
                let line = Console.ReadLine()
                if line.ToLower() = "stop" then []
                else line :: inputStrings ()
            inputStrings ()
        | 2 ->
            printfn "Введите количество строк и максимальную длину строки через пробел:"
            let input = Console.ReadLine().Split(' ')
            let count = int input.[0]
            let maxLength = int input.[1]
            generateRandomStrings count maxLength
        | 3 ->
            printfn "Введите путь к файлу:"
            let filePath = Console.ReadLine()
            readStringsFromFile filePath
        | _ -> printfn "Недопустимый выбор"; []

    if not (List.isEmpty strings) then
        let lengths = getLengths strings
        printfn "Исходные строки:"
        for s in strings do printfn "%s" s
        printfn "Длины строк:"
        for l in lengths do printfn "%d" l
    else
        printfn "Список строк пуст"

main ()
