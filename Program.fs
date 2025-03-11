open System
open System.IO


let checkDirectory (path: string) =
    if Directory.Exists(path) then
        Some path
    else
        None


let getFiles (path: string) =
    let rec walk (dir: string) =
        seq {
            for file in Directory.GetFiles(dir) do
                yield Path.GetFileName(file)
            for subdir in Directory.GetDirectories(dir) do
                yield! walk subdir
        }
    walk path


let tryMaxBy (projection: 'T -> 'U) (source: seq<'T>) =
    if Seq.isEmpty source then
        None
    else
        Some (Seq.maxBy projection source)


let findLongestFileName (files: seq<string>) =
    match tryMaxBy (fun (file: string) -> file.Length) files with
    | Some file -> file
    | None -> ""

let processInput () =
    printfn "ПРОГРАММА ДЛЯ ПОИСКА ФАЙЛА С САМЫМ ДЛИННЫМ НАЗВАНИЕМ В КАТАЛОГЕ"
    printfn "Введите путь к каталогу (или 'file' для чтения из файла):"
    let input = Console.ReadLine()

    let path =
        if input.ToLower() = "file" then
            // Чтение пути из файла (для простоты считаем, что файл содержит только путь)
            File.ReadAllText("path.txt").Trim()
        else
            input

    match checkDirectory path with
    | Some directoryPath ->
        let files = getFiles directoryPath
        let longestFileName = findLongestFileName files
        printfn "Самое длинное название файла: %s" longestFileName
    | None ->
        printfn "Каталог не найден."


processInput ()
