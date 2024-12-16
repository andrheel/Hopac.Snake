[<AutoOpen>]
module Globals

open Hopac

type XY = int * int
let inline ( ++ ) ((x0,y0):XY) ((x1,y1):XY) : XY = (x0+x1,y0+y1)


type GameState = 
  | Init
  | Playing
  | GameOver
  | Quit
   
type GameEvent = 
  | Direction of XY
  | Tick
  | GrowingFood
  | State of GameState 
  | Skip


let exitcode  = IVar<int>()
let events    = Mailbox<GameEvent>()

let rnd     = System.Random() 

let event x = Mailbox.send events x |> start 


module Block = 
  let [<Literal>] Fence = '#'
  let [<Literal>] Head  = 'O'
  let [<Literal>] Body  = '+'
  let [<Literal>] Food  = '*'
  let [<Literal>] Space = ' '

