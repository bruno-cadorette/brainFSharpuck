namespace Brainfuck

type ProgramState = 
    { Pointer : int
      Memory : Map<int, int>
      OpCount : int
      Killed : bool }

type Command = 
    | SimpleCommand of char
    | Loop of Command list

module State = 
    let currentValue state = defaultArg (Map.tryFind state.Pointer state.Memory) 0
    let operation state = { state with OpCount = state.OpCount + 1 }
    
    let updateValue state op = 
        let updateMemory = 
            match Map.tryFind state.Pointer state.Memory with
            | Some(x) -> 
                let m = Map.remove state.Pointer state.Memory
                Map.add state.Pointer (op x) m
            | None -> Map.add state.Pointer (op 0) state.Memory
        { operation (state) with Memory = updateMemory }
