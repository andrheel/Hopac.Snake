module Keyboard

open Hopac
open Infixes

let [<Literal>] private ESC     = '\x1B'

let private loop = 
  job {
    match System.Console.ReadKey true |> _.KeyChar with
    | ESC -> State Quit
    | 'w' -> Direction (0,-1)
    | 'a' -> Direction (-1,0)
    | 's' -> Direction (0,1) 
    | 'd' -> Direction (1,0) 
    | 'r' -> State Init 
    | _   -> Skip
    |> event
  }

let initialize = Job.foreverServer loop |> start 