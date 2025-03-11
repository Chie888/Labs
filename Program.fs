open System
open System.IO

// Функция для генерации случайных строк
let generateRandomStrings (count: int) =
    let rand = new Random()
    seq {
        for i in 1 .. count do
            yield rand.Next(-100, 100).ToString()          
    }


// Функция для чтения строк из файла
let readStringsFromFile (filePath: string) =
    try
        File.ReadAllLines(filePath) |> Array.toSeq
    with
    | ex -> printfn "Ошибка чтения файла: %s" ex.Message; Seq.empty

// Функция для ввода строк с клавиатуры
let rec inputStringsFromConsole () =
    printfn "Введите строку (введите 'exit' для завершения):"
    let input = Console.ReadLine()
    if input.ToLower() = "exit" then
        Seq.empty
    else
        seq {
            yield input
            yield! inputStringsFromConsole ()
        }

// Функция для получения последовательности длин строк
let getLengths (strings: seq<string>) =
    strings
    |> Seq.map (fun s -> s.Length)

// Основная функция
let main () =
    printfn "Выберите источник данных:"
    printfn "1. Ввести с клавиатуры"
    printfn "2. Сгенерировать случайные строки"
    printfn "3. Прочитать из файла"
    
    let choice = Console.ReadLine() |> int
    
    let strings =
        match choice with
        | 1 -> inputStringsFromConsole ()
        | 2 ->
            printfn "Введите количество строк для генерации:"
            let count = Console.ReadLine() |> int
            let randomStrings = generateRandomStrings count |> Seq.toList // Преобразуем в список
            printfn "Сгенерированные строки:"
            randomStrings
            |> Seq.truncate 10 // Берем первые 10 строк для вывода
            |> Seq.iter (printfn "%s")
            Seq.ofList randomStrings // Возвращаем последовательность
       
        | 3 ->
            printfn "Введите путь к файлу:"
            let filePath = Console.ReadLine()
            readStringsFromFile filePath
        | _ -> Seq.empty

   
    
    let lengths = getLengths strings
    
    // Демонстрация отложенных вычислений
    printfn "Последовательность длин строк:"
    lengths
    |> Seq.truncate 10 // Пример отложенных вычислений, берем первые 10 элементов
    |> Seq.iter (printfn "%d")

main ()
