namespace Brainfuck
    module Program =
        [<EntryPoint>]
        let main argv = 
            Interpreter.eval "++++++++++[>+++++++>++++++++++>+++>+<<<<-]>++.>+.+++++++..+++.>++.<<+++++++++++++++.>.+++.------.--------.>+.>."
            System.Console.ReadLine()
            0 // return an integer exit code
