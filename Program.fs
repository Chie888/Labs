open System

type Tree =
    | Leaf
    | Node of left: Tree * value: int * right: Tree

let rec insert tree value =
    match tree with
    | Leaf -> Node(Leaf, value, Leaf)
    | Node(left, v, right) ->
        if value <= v then Node(insert left value, v, right)
        else Node(left, v, insert right value)


let rec printTree indent tree =
    match tree with
    | Leaf -> printfn "%sLeaf" indent
    | Node(left, v, right) ->
        printfn "%sNode: %d" indent v
        printTree (indent + "  ") left
        printTree (indent + "  ") right


let isLeaf = function
    | Leaf -> true
    | Node(Leaf, _, Leaf) -> true
    | _ -> false

// fold
let rec collectPositiveLeaves tree =
    match tree with
    | Leaf -> []
    | Node(Leaf, v, Leaf) when v > 0 -> [v]
    | Node(left, _, right) -> collectPositiveLeaves left @ collectPositiveLeaves right



let rec inputTree tree =
    printf "Введите целое число (ENTER для завершения): "
    match Console.ReadLine() with
    | "" -> tree
    | value ->
        match Int32.TryParse(value) with
        | true, v -> let newTree = insert tree v
                     inputTree newTree
        | _ -> printfn "Ошибка: нужно ввести целое число"
               inputTree tree


[<EntryPoint>]
let main argv =
    printfn "ПРОГРАММА ДЛЯ ПОИСКА ПОЛОЖИТЕЛЬНЫХ ЛИСТЬЕВ"
    printfn "----------------------------------"
    printfn "Введите элементы бинарного дерева"
    printfn "----------------------------------"

    let tree = inputTree Leaf
    
    printfn "\nИсходное дерево:"
    printTree "" tree
    
    let positiveLeaves = collectPositiveLeaves tree
    
    printfn "\nСписок положительных листьев:"
    printfn "%A" positiveLeaves
    
    0
