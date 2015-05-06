namespace Brainfuck

module Interpreter = 
    let parse code = 
        let literals = [ '+'; '-'; '>'; '<'; '.'; ','; '['; ']' ] |> Set.ofList
        
        let rec parsing = 
            function 
            | '[' :: xs -> 
                let innerLoop, innerRemaining = parsing xs
                let commands, remaining = parsing innerRemaining
                Loop(innerLoop) :: commands, remaining
            | ']' :: xs -> [], xs
            | x :: xs -> 
                let commands, remaining = parsing xs
                SimpleCommand(x) :: commands, remaining
            | [] -> [], []
        Seq.filter (fun x -> Set.contains x literals) code
        |> Seq.toList
        |> parsing
        |> fst
    
    let private interpretSimpleCommand inputs state command = 
        match command with
        | '>' -> { State.operation (state) with Pointer = (state.Pointer + 1) }
        | '<' -> { State.operation (state) with Pointer = (state.Pointer - 1) }
        | '+' -> 
            State.updateValue state (fun x -> 
                if x = 255 then 0
                else x + 1)
        | '-' -> 
            State.updateValue state (fun x -> 
                if x = 0 then 255
                else x - 1)
        | '.' -> 
            printf "%c" <| (char) (State.currentValue state)
            State.operation (state)
        | ',' -> State.updateValue state inputs
        | _ -> failwith "invalid command"
    
    let private compute inputs bytecode = 
        let rec cmp state cmd = 
            if state.OpCount > 100000 then 
                if not state.Killed then printfn "\nPROCESS TIME OUT. KILLED!!!"
                { state with Killed = true }
            else 
                match cmd with
                | SimpleCommand(x) -> interpretSimpleCommand inputs state x
                | Loop(xs) -> 
                    let nextState = State.operation (computeProgram (State.operation state) xs)
                    if State.currentValue (nextState) = 0 then nextState
                    else cmp nextState cmd
        
        and computeProgram = List.fold cmp
        computeProgram { Pointer = 0
                         Memory = Map.empty
                         OpCount = 0
                         Killed = false } bytecode
    
    let public eval inputs code = 
        code
        |> parse
        |> compute inputs
