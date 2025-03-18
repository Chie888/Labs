open System


type Tree =
    | Leaf
    | Node of left: Tree * value: string * right: Tree

let rec insert tree value =
    match tree with
    | Leaf -> Node(Leaf, value, Leaf)
    | Node(left, v, right) ->
        if value <= v then Node(insert left value, v, right)
        else Node(left, v, insert right value)

let rec mapTree f tree =
    match tree with
    | Leaf -> Leaf
    | Node(left, v, right) -> 
        Node(mapTree f left, f v, mapTree f right)

let replaceFirstChar (newChar: char) (s: string) =
    if String.IsNullOrEmpty(s) then s
    else newChar.ToString() + s.[1..]

let rec printTree indent tree =
    match tree with
    | Leaf -> printfn "%sLeaf" indent
    | Node(left, v, right) ->
        printfn "%sNode: %s" indent v
        printTree (indent + "  ") left
        printTree (indent + "  ") right

let rec inputTree tree =
    printf "Введите строку (ENTER для завершения): "
    match Console.ReadLine() with
    | "" -> tree
    | value ->
        let newTree = insert tree value
        inputTree newTree


[<EntryPoint>]
let main argv =
    printfn "ПРОГРАММА ДЛЯ ЗАМЕНЫ ПЕРВОГО СИМВОЛА СТРОКИ"
    printfn "----------------------------------"

    printfn "Введите элементы бинарного дерева"
    printfn "----------------------------------"
    let tree = inputTree Leaf
    
    printfn "\nИсходное дерево:"
    printTree "" tree
    
    printf "\nВведите символ для замены: "
    let inputChar = Console.ReadLine()
    match inputChar with
    | s when s.Length <> 1 ->
        printfn "Ошибка: нужно ввести ровно один символ"
        1
    | _ ->
        let charToReplace = inputChar.[0]
        
        let modifiedTree = 
            tree 
            |> mapTree (replaceFirstChar charToReplace)
        
        printfn "\nМодифицированное дерево:"
        printTree "" modifiedTree
        0
