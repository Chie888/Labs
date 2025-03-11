open System
open System.IO


let hasDigit (number: int) (digit: int) =
    let rec check n =
        if n = 0 then false
        else if abs n % 10 = digit then true
        else check (n / 10)
    check number

let sumWithDigit (numbers: seq<int>) (digit: int) =
    numbers
     |> Seq.filter (fun n -> 
        let result = hasDigit n digit
        printfn "Число %d содержит цифру %d: %b" n digit result
        result)
    |> Seq.fold (+) 0


let generateRandomNumbers (count: int) (maxValue: int) =
    let rand = Random()
    seq { for i in 1 .. count -> rand.Next(-maxValue, maxValue) }


let readNumbersFromFile (filePath: string) =
    try
        File.ReadAllLines(filePath)
        |> Array.map int
        |> Seq.ofArray
    with
    | ex -> printfn "Ошибка чтения файла: %s" ex.Message; Seq.empty


let main () =
    printfn "ПРОГРАММА ДЛЯ ВЫСЛЕНИЯ СУММЫ ЧИСЕЛ С ЗАДАННОЙ ЦИФРОЙ"
    printfn "Выберите способ ввода данных:"
    printfn "1. Ввести с клавиатуры"
    printfn "2. Сгенерировать рандомом"
    printfn "3. Прочитать из файла"
    
    let choice = Console.ReadLine() |> int
    
    let numbers =
        match choice with
        | 1 ->
            printfn "Введите числа через пробел:"
            Console.ReadLine().Split([|' '|], StringSplitOptions.RemoveEmptyEntries)
            |> Array.map int
            |> Seq.ofArray
        | 2 ->
            printfn "Введите количество чисел и максимальное значение через пробел:"
            let input = Console.ReadLine().Split([|' '|], StringSplitOptions.RemoveEmptyEntries)
            let count = int input.[0]
            let maxValue = int input.[1]
            generateRandomNumbers count maxValue
        | 3 ->
            printfn "Введите путь к файлу:"
            let filePath = Console.ReadLine()
            readNumbersFromFile filePath
        | _ -> Seq.empty
    
    printfn "Введите цифру для поиска:"
    let digit = Console.ReadLine() |> int
    
    // Проверка ввода
    if digit < 0 || digit > 9 then
        printfn "Цифра должна быть от 0 до 9."
    else  
        let sum = sumWithDigit numbers digit
        printfn "Сумма элементов с цифрой %d: %d" digit sum

main ()
