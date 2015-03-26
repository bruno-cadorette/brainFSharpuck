namespace Brainfuck

module Interpreter = 
    type public ProgramState = 
        { Pointer : int
          Memory : Map<int, int> }
    
    type private Command = 
        | SimpleCommand of char
        | Loop of Command list
    
    let private currentValue state = Map.find state.Pointer state.Memory
    
    let private updateValue state op = 
        let updateMemory = 
            match Map.tryFind state.Pointer state.Memory with
            | Some(x) -> 
                let m = Map.remove state.Pointer state.Memory
                Map.add state.Pointer (op x) m
            | None -> Map.add state.Pointer (op 0) state.Memory
        { state with Memory = updateMemory }
    
    let private parse code = 
        let literals = [ '+'; '-'; '>'; '<'; '.'; ',' ] |> Set.ofList
        
        let rec parsing = 
            function 
            | '[' :: xs -> 
                let innerLoop, innerRemaining = parsing xs
                let commands, remaining = parsing innerRemaining
                let remainingCommands, after = parsing remaining
                Loop(innerLoop) :: commands @ remainingCommands, after
            | ']' :: xs -> [], xs
            | x :: xs when Set.contains x literals -> 
                let commands, remaining = parsing xs
                SimpleCommand(x) :: commands, remaining
            | x :: xs -> parsing xs
            | [] -> [], []
        code
        |> Seq.toList
        |> parsing
        |> fst
    
    let private interpretSimpleCommand state command = 
        match command with
        | '>' -> { state with Pointer = (state.Pointer + 1) }
        | '<' -> { state with Pointer = (state.Pointer - 1) }
        | '+' -> updateValue state (fun x -> x + 1)
        | '-' -> updateValue state (fun x -> x - 1)
        | '.' -> 
            printf "%c" <| (char) (currentValue state)
            state
        | ',' -> updateValue state (fun x -> System.Console.Read())
        | _ -> failwith "invalid command"
    
    let private compute bytecode = 
        let rec cmp state cmd = 
            match cmd with
            | SimpleCommand(x) -> interpretSimpleCommand state x
            | Loop(xs) -> 
                let nextState = computeProgram state xs
                if currentValue (nextState) = 0 then nextState
                else cmp nextState cmd
        
        and computeProgram = List.fold cmp
        computeProgram { Pointer = 0
                         Memory = Map.empty<int, int> } bytecode
    
    let public eval code = 
        code
        |> parse
        |> compute
