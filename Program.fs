open System
open System.IO

// Функция для проверки наличия цифры в числе
let containsDigit (number: int) (digit: int) =
    let strNum = number.ToString()
    let strDig = digit.ToString()
    strNum.Contains(strDig)

// Функция для суммирования чисел, содержащих цифру
let sumNumbersWithDigit (numbers: int list) (digit: int) =
    List.fold (fun acc x -> if containsDigit x digit then acc + x else acc) 0 numbers

// Функция для генерации случайных чисел
let generateRandomNumbers (count: int) (maxValue: int) =
    let random = Random()
    let generateNumber () = random.Next(maxValue) + 1
    List.init count (fun _ -> generateNumber ())

// Функция для чтения чисел из файла
let readNumbersFromFile (filePath: string) =
    try
        File.ReadAllLines(filePath) |> Array.toList
        |> List.map (fun line ->
            try
                int line
            with
            | ex -> printfn "Ошибка чтения файла: строка '%s' не является числом" line; 0)
    with
    | ex -> printfn "Ошибка чтения файла: %s" ex.Message; []

// Функция для ввода чисел с клавиатуры
let inputNumbersFromConsole () =
    printfn "Введите числа (введите 'stop' для завершения):"
    let rec inputNumbers () =
        let line = Console.ReadLine()
        if line.ToLower() = "stop" then []
        else
            try
                let number = int line
                number :: inputNumbers ()
            with
            | ex -> printfn "Ошибка: '%s' не является числом. Попробуйте снова." line; inputNumbers ()
    inputNumbers ()

// Основная функция для взаимодействия с пользователем
let main () =
    printfn "Программа для суммирования чисел, содержащих заданную цифру"
    printfn "-------------------------------------------------------"
    printfn "1. Ввести числа с клавиатуры"
    printfn "2. Сгенерировать случайные числа"
    printfn "3. Прочитать числа из файла"
    printfn "-------------------------------------------------------"
    
    let choice = Console.ReadLine() |> int
    
    let numbers =
        match choice with
        | 1 -> inputNumbersFromConsole ()
        | 2 ->
            printfn "Введите количество чисел и максимальное значение через пробел:"
            let input = Console.ReadLine().Split(' ')
            let count = int input.[0]
            let maxValue = int input.[1]
            generateRandomNumbers count maxValue
        | 3 ->
            printfn "Введите путь к файлу:"
            let filePath = Console.ReadLine()
            readNumbersFromFile filePath
        | _ -> printfn "Недопустимый выбор"; []

    if not (List.isEmpty numbers) then
        printfn "Введите цифру для поиска:"
        let digit = Console.ReadLine() |> int
        let sum = sumNumbersWithDigit numbers digit
        printfn "Исходные числа:"
        for n in numbers do printfn "%d" n
        printfn "Сумма чисел, содержащих цифру %d: %d" digit sum
    else
        printfn "Список чисел пуст"

main ()
