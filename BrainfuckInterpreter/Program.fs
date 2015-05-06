open Brainfuck
open System
open System.IO
open System.Collections.Generic

let private inputFunc n (input : string) = 
    let str = input.Remove(n)
    let q = new Queue<char>(str)
    fun _ -> 
        if q.Count > 0 then (int) (q.Dequeue())
        else 0

[<EntryPoint>]
let main argv = 
    //the code file we want to parse is in the format of the hackerrank challenge. Maybe I will add a command line argument one day if I have time
    let lines = File.ReadLines(argv.[0])
    //The first line is n and m, n is the length of the programs input and m, the number of line that the code have
    let n, m = (Seq.head lines).Split(' ') |> (fun arr -> (Int32.Parse(arr.[0]), Int32.Parse(arr.[1])))
    //The input is the second line, n characters followed by an '$', which we have to ignore
    let input = inputFunc n <| Seq.nth 1 lines
    //The rest of the lines (m lines) are the lines of the program
    Seq.skip 2 lines
    |> String.Concat
    |> Interpreter.eval input
    |> ignore
    ignore <| Console.ReadKey()
    0
