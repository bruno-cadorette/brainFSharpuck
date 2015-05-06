namespace Brainfuck

open System

module Debug = 
    let rec parser = 
        function 
        | SimpleCommand(c) :: xs -> 
            printf "%c" c
            parser xs
        | Loop(x) :: xs -> 
            printf "\n["
            parser x
            printf "]\n"
            parser xs
        | [] -> ()
    
    let memory state = 
        let size = 
            Map.toSeq state.Memory
            |> Seq.last
            |> fst
            |> (+) 1
        Array.init size (fun i -> defaultArg (Map.tryFind i state.Memory) 0)
        |> Seq.map (fun x -> x.ToString())
        |> String.concat ","
        |> printfn "\nPointer: %i \nOperation count: %i\nMemory: %s" state.Pointer state.OpCount
    
    let code str = 
        let literals = [ '+'; '-'; '>'; '<'; '.'; ','; '['; ']' ] |> Set.ofList
        Seq.filter (fun x -> Set.contains x literals) str
        |> String.Concat
        |> printfn "%s"
